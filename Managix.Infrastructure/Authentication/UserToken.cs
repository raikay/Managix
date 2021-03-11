using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Managix.Infrastructure.Configuration;
using Managix.Infrastructure.Extensions;

namespace Managix.Infrastructure.Authentication
{
    public class UserToken
    {

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetToken(TokenUserDto user)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configs.JwtConfig.SecurityKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var timestamp = DateTime.Now.AddMinutes(Configs.JwtConfig.Expires + Configs.JwtConfig.RefreshExpires).ToTimestamp().ToString();
            var claims = new[]
 {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserNickName, user.NickName),
                new Claim(ClaimAttributes.RefreshExpires, timestamp)
            };

            var token = new JwtSecurityToken(
                issuer: Configs.JwtConfig.Issuer,
                audience: Configs.JwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(Configs.JwtConfig.Expires),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Claim[] Decode(string jwtToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
            return jwtSecurityToken?.Claims?.ToArray();
        }

    }

    public class TokenUserDto
    {
        public long Id { set; get; }

        public string NickName { set; get; }
        public string UserName { set; get; }
    }
}

