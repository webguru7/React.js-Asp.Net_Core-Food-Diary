import React from 'react';
import { useDispatch } from 'react-redux';
import { TablePagination } from '@mui/material';
import { useAppSelector } from '../../__shared__/hooks';
import { pageNumberChanged, pageSizeChanged } from '../slice';

const PagesTablePagination: React.FC = () => {
  const pageNumber = useAppSelector(state => state.pages.filter.pageNumber);
  const pageSize = useAppSelector(state => state.pages.filter.pageSize);
  const totalPagesCount = useAppSelector(state => state.pages.totalPagesCount);
  const dispatch = useDispatch();

  const handleChangePage = (
    event: React.MouseEvent<HTMLButtonElement, MouseEvent> | null,
    pageIndex: number,
  ): void => {
    dispatch(pageNumberChanged(pageIndex + 1));
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const newPageSize = Number(event.target.value);
    dispatch(pageSizeChanged(newPageSize));
  };

  return (
    <TablePagination
      component="div"
      count={totalPagesCount}
      page={pageNumber - 1}
      rowsPerPage={pageSize}
      onPageChange={handleChangePage}
      onRowsPerPageChange={handleChangeRowsPerPage}
    />
  );
};

export default PagesTablePagination;
