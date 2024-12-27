using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace APIClient.Helper
{
    public class Utilities
    {
        public static string GetRsaToken()
        {
            var _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var RSA_PRIVATE_KEY = String.Join('\n', _config.GetSection("DataService:jwt_rsa_private_key").Get<string[]>());
            string jwtToken = "";

            byte[] RSAprivateKey = Convert.FromBase64String(RSA_PRIVATE_KEY);

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(RSAprivateKey, out _);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                //IssuedAt = DateTime.UtcNow.AddMinutes(-1),
                //Expires = DateTime.UtcNow.AddMinutes(3),

                IssuedAt = DateTime.UtcNow.AddMinutes(-120),
                Expires = DateTime.UtcNow.AddMinutes(120),

                Subject = new ClaimsIdentity(new[] {

                    new Claim("scope", "read:service write:service"),
                }),

                SigningCredentials = new SigningCredentials(
                            key: new RsaSecurityKey(rsa),
                            algorithm: SecurityAlgorithms.RsaSha256)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(descriptor);
            jwtToken = tokenHandler.WriteToken(token);

            //Remove the nbf claim
            tokenHandler.SetDefaultTimesOnTokenCreation = false;

            return jwtToken;
        }
    }
}
