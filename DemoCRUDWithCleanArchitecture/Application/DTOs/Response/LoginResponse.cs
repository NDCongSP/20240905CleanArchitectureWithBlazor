namespace Application.DTOs.Response
{
    public record LoginResponse(bool flag = false, string message = null!, string token = null!, string refreshToken = null!, string expiration = null!);
}
