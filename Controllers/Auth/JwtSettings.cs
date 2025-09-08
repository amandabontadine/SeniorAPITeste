namespace SeniorAPITeste.Auth;

public sealed class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiresMinutes { get; set; } = 60;
}