namespace Application.DTOs.Response.Account
{
    public class GetUserWithRoleResponseDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
    }
}
