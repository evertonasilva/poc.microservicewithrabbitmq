using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Actio.Common.Auth
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SecurityKey _issueSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidatorParameters;

        public JwtHandler(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _issueSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(_issueSigningKey,SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidatorParameters =new TokenValidationParameters
            {
            ValidateAudience = false,
            ValidIssuer = _options.Issuer,
            IssuerSigningKey = _issueSigningKey             
            };
        }        
        public JsonWebToken Create(Guid userId)
        {
            var nowUTC = DateTime.UtcNow;
            var expires = nowUTC.AddMinutes(_options.ExpiryMinutes);
            var centuryBegin  = new DateTime(1970,1,1).ToUniversalTime();
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUTC.Ticks - centuryBegin.Ticks).TotalSeconds);
            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss",_options.Issuer},
                {"iat",now},
                {"exp", exp},
                {"unique_name",userId}
            };
            var jwt = new JwtSecurityToken(_jwtHeader,payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new  JsonWebToken
            {
                Expires = exp,
                Token = token
            };
        }
    }
}