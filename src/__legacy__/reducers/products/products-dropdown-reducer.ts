import { ProductsDropdownState } from '../../store';
import { ProductsDropdownActions, ProductsDropdownActionTypes } from '../../action-types';

export const initialState: ProductsDropdownState = {
  productDropdownItems: [],
  productDropdownItemsFetchState: {
    loading: false,
    loaded: false,
  },
};

const productsDropdownReducer = (
  state: ProductsDropdownState = initialState,
  action: ProductsDropdownActions,
): ProductsDropdownState => {
  switch (action.type) {
    case ProductsDropdownActionTypes.Request:
      return {
        ...state,
        productDropdownItemsFetchState: {
          loading: true,
          loaded: false,
          loadingMessage: action.requestMessage,
        },
      };
    case ProductsDropdownActionTypes.Success:
      return {
        ...state,
        productDropdownItems: action.data,
        productDropdownItemsFetchState: {
          loading: false,
          loaded: true,
        },
      };
    case ProductsDropdownActionTypes.Error:
      return {
        ...state,
        productDropdownItemsFetchState: {
          loading: false,
          loaded: false,
          error: action.errorMessage,
        },
      };
    default:
      return state;
  }
};

export default productsDropdownReducer;