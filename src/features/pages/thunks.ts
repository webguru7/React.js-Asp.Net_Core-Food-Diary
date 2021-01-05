import config from '../__shared__/config';
import { SortOrder } from '../__shared__/models';
import { createApiCallAsyncThunk, createUrl, handleEmptyResponse } from '../__shared__/utils';
import { PageCreateEdit, PageItem } from './models';

export type GetPagesRequest = {
  startDate?: string;
  endDate?: string;
  sortOrder: SortOrder;
};

export type EditPageRequest = {
  id: number;
  page: PageCreateEdit;
};

export const getPages = createApiCallAsyncThunk<PageItem[], GetPagesRequest>(
  'pages/getPages',
  params => createUrl(`${config.apiUrl}/v1/pages`, params),
  response => response.json(),
  'Failed to get pages',
);

export const createPage = createApiCallAsyncThunk<number, PageCreateEdit>(
  'pages/createPage',
  () => `${config.apiUrl}/v1/pages`,
  async response => Number(await response.text()),
  'Failed to create page',
  {
    method: 'POST',
    bodyCreator: page => JSON.stringify(page),
  },
);

export const editPage = createApiCallAsyncThunk<void, EditPageRequest>(
  'pages/editPage',
  ({ id }) => `${config.apiUrl}/v1/pages/${id}`,
  handleEmptyResponse,
  'Failed to update page',
  {
    method: 'PUT',
    bodyCreator: ({ page }) => JSON.stringify(page),
  },
);
