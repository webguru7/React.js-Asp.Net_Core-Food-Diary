import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { IconButton, Toolbar, Tooltip, Typography } from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import FilterListIcon from '@material-ui/icons/FilterList';
import DeleteIcon from '@material-ui/icons/Delete';
import { useTypedSelector } from '../../__shared__/hooks';
import { useToolbarStyles } from '../../__shared__/styles';
import ProductCreateEditDialog from './ProductCreateEditDialog';
import ProductsFilterDialog from './ProductsFilterDialog';
import { ProductCreateEdit } from '../models';
import { createProduct, deleteProduct } from '../thunks';
import { ConfirmationDialog } from '../../__shared__/components';

const ProductsTableToolbar: React.FC = () => {
  const classes = useToolbarStyles();
  const [productCreateEditDialogOpen, setProductCreateEditDialogOpen] = useState(false);
  const [productsFilterDialogOpen, setProductsFilterDialogOpen] = useState(false);
  const [productDeleteDialogOpen, setProductDeleteDialogOpen] = useState(false);

  const selectedProductId = useTypedSelector(state => state.products.selectedProductId);

  const dispatch = useDispatch();

  const handleAddClick = (): void => {
    setProductCreateEditDialogOpen(true);
  };

  const handleFilterClick = (): void => {
    setProductsFilterDialogOpen(true);
  };

  const handleDeleteClick = (): void => {
    setProductDeleteDialogOpen(true);
  };

  const handleCreateEditDialogConfirm = (product: ProductCreateEdit): void => {
    setProductCreateEditDialogOpen(false);
    dispatch(createProduct(product));
  };

  const handleFilterDialogConfirm = (): void => {
    setProductsFilterDialogOpen(false);
  };

  const handleDeleteDialogConfirm = (): void => {
    setProductDeleteDialogOpen(false);

    if (selectedProductId !== null) {
      dispatch(deleteProduct(selectedProductId));
    }
  };

  const handleCreateEditDialogClose = (): void => {
    setProductCreateEditDialogOpen(false);
  };

  const handleFilterDialogClose = (): void => {
    setProductsFilterDialogOpen(false);
  };

  const handleDeleteDialogClose = (): void => {
    setProductDeleteDialogOpen(false);
  };

  return (
    <Toolbar className={classes.root}>
      <ProductCreateEditDialog
        open={productCreateEditDialogOpen}
        onClose={handleCreateEditDialogClose}
        onDialogConfirm={handleCreateEditDialogConfirm}
        onDialogCancel={handleCreateEditDialogClose}
      ></ProductCreateEditDialog>
      <ProductsFilterDialog
        open={productsFilterDialogOpen}
        onClose={handleFilterDialogClose}
        onDialogConfirm={handleFilterDialogConfirm}
        onDialogCancel={handleFilterDialogClose}
      ></ProductsFilterDialog>
      <ConfirmationDialog
        open={productDeleteDialogOpen}
        dialogTitle="Delete product confirmation"
        dialogMessage="Do you really want to delete selected product?"
        onDialogConfirm={handleDeleteDialogConfirm}
        onDialogCancel={handleDeleteDialogClose}
      ></ConfirmationDialog>
      <Typography variant="h1" className={classes.title}>
        Products
      </Typography>
      {selectedProductId === null ? (
        <React.Fragment>
          <Tooltip title="Add new product">
            <span>
              <IconButton onClick={handleAddClick}>
                <AddIcon></AddIcon>
              </IconButton>
            </span>
          </Tooltip>
          <Tooltip title="Filter products">
            <span>
              <IconButton onClick={handleFilterClick}>
                <FilterListIcon></FilterListIcon>
              </IconButton>
            </span>
          </Tooltip>
        </React.Fragment>
      ) : (
        <React.Fragment>
          <Tooltip title="Delete product">
            <span>
              <IconButton onClick={handleDeleteClick}>
                <DeleteIcon></DeleteIcon>
              </IconButton>
            </span>
          </Tooltip>
        </React.Fragment>
      )}
    </Toolbar>
  );
};

export default ProductsTableToolbar;