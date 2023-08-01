namespace softrobotics.auth.application.Common.Model;

public class TokenDto
{
    public TokenDto(
            string accessToken,
            TimeSpan expiresIn,
            string refreshToken
        )
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public TimeSpan ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}