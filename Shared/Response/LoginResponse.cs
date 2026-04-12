namespace Shared.Response
{
    public record LoginResponse(string? Id, string Email, string RefreshToken, string AccessToken);
}
