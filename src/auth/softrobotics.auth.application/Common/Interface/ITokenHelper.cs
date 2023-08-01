using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.Common.Interface
{
    public interface ITokenHelper
    {
        TokenDto CreateToken(User user);
        string GenerateRefreshToken();
    }
}
