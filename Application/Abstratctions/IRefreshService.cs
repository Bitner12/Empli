using Shared.Response;

namespace Application.Services;

public interface IRefreshService
{
    Task<LoginResponse> Refresh(string token);
}