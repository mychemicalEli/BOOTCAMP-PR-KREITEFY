export interface RegisterRequest {

}export interface RegisterRequest {
    id: number | undefined;
    name: string;
    lastName: string;
    email: string;
    password: string;
    roleId: number;
    roleName: string | undefined;
    token: string;
}
