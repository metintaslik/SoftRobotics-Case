using softrobotics.auth.domain.Entity;
using softrobotics.shared.Models;

namespace softrobotics.auth.application.Common.Interface
{
    public interface ITokenHelper
    {
        TokenDto CreateToken(User user);
        string GenerateRefreshToken();
    }
}
