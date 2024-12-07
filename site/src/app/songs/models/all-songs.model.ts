export interface PaginatedResponse<SongListDto> {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    data: SongListDto[];
}

export interface SongListDto {
    id: number;
    title: string;
    albumName: string;
    albumCover: string;
    artistName: string;
    genreName: string;
}
