using System.Text;
using System.Security.Claims;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// JwtAuth扩展
    /// </summary>
    public class JwtAuthExtension
    {
        private readonly JwtOptions _option;
        private readonly string? _keyText = null;
        private readonly SigningCredentials? _signingCredentials = null;

        private readonly TimeZoneInfo _zoneInfo;
        private JwtSecurityTokenHandler? _getTokenHdl = null;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="options"></param>
        public JwtAuthExtension(IOptionsMonitor<JwtOptions> options)
        {
            _option = options.CurrentValue; startTime = new DateTime(1970, 1, 1);

            var keyBts = Convert.FromBase64String(_option.SecurityKey!); _keyText = Encoding.UTF8.GetString(keyBts);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyText));

            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            validateParameter = new TokenValidationParameters()
            {
                ValidateIssuer = true, // 是否验证 Issuer（发行商）
                ValidateAudience = true, // 是否验证 Audience（受众者）
                ValidateLifetime = true, // 是否验证失效时间
                ValidateIssuerSigningKey = true, // 是否验证 Issuer 的签名键
                ValidIssuer = _option.Issuer, // ValidAudience,ValidIssuer这两项的值要和验证中心的值保持一致
                ValidAudience = _option.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyText))
            };

            _getTokenHdl = new JwtSecurityTokenHandler(); // _checkTokenHdler = new JwtSecurityTokenHandler();
            _zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
        }

        /// <summary>
        /// 生成token
        /// </summary>
        public (string, string?) GetToken(string? userName, string? userId)
        {
            #region 有效载荷

            var claims = new[] { // new Claim(ClaimTypes.Name, userName),
                new Claim("nm", userName),
                new Claim("rl", "admin"),
                new Claim("ky", userId)
            };

            #endregion

            var token = new JwtSecurityToken(
                issuer: _option.Issuer!,
                audience: _option.Audience!,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_option.Timeout),
                signingCredentials: _signingCredentials);
            // var tokenHandler = new JwtSecurityTokenHandler();
            var acceToken = _getTokenHdl.WriteToken(token);
            return (acceToken, null);
        }

        private DateTime startTime;  // 常量
        private TokenValidationParameters validateParameter; // 固定配置参数对象

        /// <summary>
        /// token校验
        /// </summary>
        public async Task<(DateTime, string?, string?, bool)> CheckToken(string token)
        {
            var res = await _getTokenHdl.ValidateTokenAsync(token, validateParameter);
            var timeNow = DateTime.Now;
            if (res.Claims != null) // res.IsValid && 
            {
                var timeStamp = res.Claims.FirstOrDefault(c => c.Key == "exp").Value?.ToString();
                var uid = res.Claims.FirstOrDefault(c => c.Key == "ky").Value?.ToString();
                var username = res.Claims.FirstOrDefault(c => c.Key == "nm").Value?.ToString();

                if (timeStamp != null)
                {
                    var start = startTime + TimeZoneInfo.Local.GetUtcOffset(timeNow);
                    var tempTime = TimeZoneInfo.ConvertTime(start, _zoneInfo);
                    var endTime = tempTime.AddSeconds(int.Parse(timeStamp));
                    return (endTime, username, uid, endTime.Subtract(timeNow).TotalSeconds >= 0);
                }
            }
            return (timeNow.AddDays(-1), null, null, false);
        }
    }
}
