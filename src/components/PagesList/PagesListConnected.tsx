import { connect } from 'react-redux';
import PagesList from './PagesList';
import { ThunkDispatch } from 'redux-thunk';
import { PagesFilter, PageItem } from '../../models';
import { AnyAction } from 'redux';
import { getPages } from '../../action-creators';
import { FoodDiaryState } from '../../store';
import { GetPagesListSuccessAction, GetPagesListErrorAction } from '../../action-types';

export interface StateToPropsMapResult {
  loading: boolean;
  loaded: boolean;
  errorMessage?: string;
  visiblePages: PageItem[];
  currentDraftPageId: number;
  pagesFilter: PagesFilter;
}

export interface DispatchToPropsMapResult {
  getPages: (filter: PagesFilter) => Promise<GetPagesListSuccessAction | GetPagesListErrorAction>;
}

const mapStateToProps = (state: FoodDiaryState): StateToPropsMapResult => {
  return {
    visiblePages: state.pages.list.visiblePages ?? [],
    loaded: state.pages.list.loaded ?? false,
    loading: state.pages.list.loading ?? false,
    errorMessage: state.pages.list.errorMessage,
    pagesFilter: state.pages.filter,
    currentDraftPageId: state.pages.list.currentDraftPageId,
  };
};

const mapDispatchToProps = (dispatch: ThunkDispatch<PageItem[], PagesFilter, AnyAction>): DispatchToPropsMapResult => {
  return {
    getPages: (filter: PagesFilter): Promise<GetPagesListSuccessAction | GetPagesListErrorAction> => {
      return dispatch(getPages(filter));
    },
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(PagesList);
