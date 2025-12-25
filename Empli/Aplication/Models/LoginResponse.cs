namespace Empli.Aplication.Models
{
    public record LoginResponse(string? Id, string Email, string RefreshToken, string AccessToken);
}
