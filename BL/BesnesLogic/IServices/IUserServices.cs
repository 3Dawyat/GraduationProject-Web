
namespace BL.BesnesLogic.IServices
{
    public interface IUserServices
    {
        Task<AutheModel> RegisterAsync(RegisterModel model);
        Task<AutheModel> GetTokenAsync(LoginModel model);
        Task<string> AddRoleToUserAsync(AddRoleModel model);
        Task<AutheModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
