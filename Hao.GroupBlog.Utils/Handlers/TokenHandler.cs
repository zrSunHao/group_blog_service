using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hao.GroupBlog.Utils.Handlers
{
    public class TokenHandler
    {
        public string BuilderToken(TokenMsg info)
        {
            var claims = new List<Claim>();
            if (info.Pairs != null)
            {
                foreach (var p in info.Pairs) { claims.Add(new Claim($"scope{p.Key}", p.Value)); }
            }

            claims.Add(new Claim(ClaimTypes.Sid, info.Id));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, info.Name));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKeyBt = Encoding.UTF8.GetBytes(info.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = info.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = info.ExpiredAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKeyBt), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class TokenMsg
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Key { get; set; }
        public string Issuer { get; set; }
        public DateTime ExpiredAt { get; set; }
        public Dictionary<string, string> Pairs { get; set; }
    }
}
