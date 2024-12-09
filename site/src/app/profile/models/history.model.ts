export interface PaginatedResponse<HistorySongsDto> {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    data: HistorySongsDto[];
}

export interface HistorySongsDto {
    songId: number;
    title: string;
    artist: string;
    humanizedPlayedAt: string;
}