import { SortOrder, ExportFormat } from './common';

export interface PageItem {
  id: number;
  date: string;
  countNotes: number;
  countCalories: number;
}

export interface PagesFilter {
  sortOrder: SortOrder;
  showCount?: number;
}

export interface PageCreateEdit {
  date: string;
}

export interface PageEditRequest {
  id: number;
  page: PageCreateEdit;
}

export interface PagesExportRequest {
  startDate: string;
  endDate: string;
  format: ExportFormat;
}