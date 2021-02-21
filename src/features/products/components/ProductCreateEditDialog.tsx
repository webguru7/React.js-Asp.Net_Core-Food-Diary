import React, { useEffect } from 'react';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogProps,
  DialogTitle,
  TextField,
} from '@material-ui/core';
import { ProductCreateEdit, ProductItem } from '../models';
import { DialogCustomActionProps } from '../../__shared__/types';
import { useInput, useInputAutocomplete } from '../../__shared__/hooks';
import { CategoryAutocomplete } from '../../categories/components';

interface ProductCreateEditDialogProps
  extends DialogProps,
    DialogCustomActionProps<ProductCreateEdit> {
  product?: ProductItem;
}

const ProductCreateEditDialog: React.FC<ProductCreateEditDialogProps> = ({
  product,
  onDialogCancel,
  onDialogConfirm,
  ...dialogProps
}: ProductCreateEditDialogProps) => {
  const { title, submitText, initialProductName, initialCaloriesCost, initialCategory } = product
    ? {
        title: 'Edit product',
        submitText: 'Save',
        initialProductName: product.name,
        initialCaloriesCost: product.caloriesCost,
        initialCategory: {
          id: product.categoryId,
          name: product.categoryName,
        },
      }
    : {
        title: 'New product',
        submitText: 'Create',
        initialProductName: '',
        initialCaloriesCost: 100,
        initialCategory: null,
      };

  const productNameInput = useInput(initialProductName);
  const caloriesCostInput = useInput(initialCaloriesCost);
  const categoryInput = useInputAutocomplete(initialCategory);

  useEffect(() => {
    if (dialogProps.open) {
      categoryInput.setValue(initialCategory);
    }

    return () => {
      productNameInput.setValue(initialProductName);
      caloriesCostInput.setValue(initialCaloriesCost);
      categoryInput.setValue(initialCategory);
    };
  }, [dialogProps.open]);

  const handleSubmitClick = (): void => {
    if (categoryInput.value) {
      onDialogConfirm({
        name: productNameInput.value,
        caloriesCost: caloriesCostInput.value,
        categoryId: categoryInput.value.id,
      });
    }
  };

  return (
    <Dialog maxWidth="xs" fullWidth {...dialogProps}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <TextField
          {...productNameInput.binding}
          label="Product name"
          placeholder="Enter product name"
          margin="normal"
          fullWidth
          autoFocus
        ></TextField>
        <TextField
          {...caloriesCostInput.binding}
          type="number"
          label="Calories cost"
          placeholder="Enter calories cost"
          margin="normal"
          fullWidth
        ></TextField>
        <CategoryAutocomplete {...categoryInput.binding}></CategoryAutocomplete>
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" onClick={handleSubmitClick}>
          {submitText}
        </Button>
        <Button variant="text" onClick={onDialogCancel}>
          Cancel
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ProductCreateEditDialog;
