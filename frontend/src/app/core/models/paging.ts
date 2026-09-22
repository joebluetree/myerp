/** Mirrors MyErp.Common.Models.PagedRequest on the server. */
export interface PagedRequest {
  page: number;
  pageSize: number;
  search?: string;
  sortBy?: string;
  sortDescending?: boolean;
}

/** Mirrors MyErp.Common.Models.PagedResult<T> on the server. */
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export const defaultPagedRequest: PagedRequest = {
  page: 1,
  pageSize: 25,
};

export function emptyPage<T>(request: PagedRequest = defaultPagedRequest): PagedResult<T> {
  return {
    items: [],
    totalCount: 0,
    page: request.page,
    pageSize: request.pageSize,
    totalPages: 0,
    hasPrevious: false,
    hasNext: false,
  };
}
