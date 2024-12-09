export interface UserProfileDto {
    id: number | undefined;
    name: string;
    lastName: string;
    email: string;
    password: string;
    roleId: number | undefined;
    roleName: string | undefined;
}