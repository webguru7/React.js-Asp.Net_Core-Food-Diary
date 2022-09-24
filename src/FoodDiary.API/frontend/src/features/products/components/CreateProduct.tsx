import AddIcon from '@mui/icons-material/Add';
import React, { useEffect, useState } from 'react';
import { AppFab } from 'src/components';
import { useAppSelector } from 'src/store';
import { useCreateProductMutation, useProductsQuery } from '../api';
import { selectProductsQueryArg } from '../selectors';
import { ProductFormData } from '../types';
import ProductInputDialog from './ProductInputDialog';

const CreateProduct: React.FC = () => {
  const [isDialogOpened, setIsDialogOpened] = useState(false);
  const productsQueryArg = useAppSelector(selectProductsQueryArg);

  const { isLoading: isLoadingProducts, refetch: refetchProducts } =
    useProductsQuery(productsQueryArg);

  const [createProduct, { isLoading: isProductCreating, isSuccess: isProductCreated }] =
    useCreateProductMutation();

  useEffect(() => {
    if (isProductCreated) {
      refetchProducts();
      setIsDialogOpened(false);
    }
  }, [isProductCreated, refetchProducts]);

  function handleCreate() {
    setIsDialogOpened(true);
  }

  function handleDialogSubmit({ name, caloriesCost, category }: ProductFormData) {
    createProduct({
      name,
      caloriesCost,
      categoryId: category?.id,
    });
  }

  return (
    <React.Fragment>
      <AppFab
        aria-label="Open create product dialog"
        color="primary"
        onClick={handleCreate}
        disabled={isLoadingProducts || isProductCreating}
      >
        <AddIcon />
      </AppFab>

      <ProductInputDialog
        isOpened={isDialogOpened}
        setIsOpened={setIsDialogOpened}
        title="Create product"
        submitText="Create"
        onSubmit={handleDialogSubmit}
        isLoading={isProductCreating}
      />
    </React.Fragment>
  );
};

export default CreateProduct;