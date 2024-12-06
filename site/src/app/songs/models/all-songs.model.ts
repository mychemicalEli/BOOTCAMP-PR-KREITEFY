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
    albumId: number;
    albumCover: string;
    artistId: number;
    artistName: string;
    genreId: number;
}
