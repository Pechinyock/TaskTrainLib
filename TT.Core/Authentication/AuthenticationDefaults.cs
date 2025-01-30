using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TT.Core;

public static class AuthenticationDefaults
{
    public static SymmetricSecurityKey GetSymmetricKey(string key) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

    public static TokenValidationParameters GetValidationParameters(AuthOptions options) 
    {
        ArgumentNullException.ThrowIfNull(nameof(options));
        if (String.IsNullOrEmpty(options.Key))
            throw new ArgumentNullException(nameof(options.Key));

        var key = GetSymmetricKey(options.Key);

        return new TokenValidationParameters() 
        {
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,

            ValidateAudience = true,
            ValidAudience = options.Audience,

            ValidateLifetime = true,
        };
    }

    public static AuthOptions GetDefaultOptions() 
    {
        return new AuthOptions()
        {
            Issuer = "TaskTrain",
            Audience = "TTClients",
            Key = "mysupersecret_secretsecretsecretkey!123",
            Lifetime = 5
        };
    }
}
