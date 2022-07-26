using AutoMapper;
using Identity.Dto;
using Identity.Models;
using Identity.Repository.Contract;
using Identity.ServiceResponder;
using Identity.Services.Contract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using XSystem.Security.Cryptography;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        private readonly IConfiguration _configuration;

        private readonly IMapper _map;
        public AccountService(IAccountRepository repo, IConfiguration configuration, IMapper map)
        {
            _repo = repo;
            _configuration = configuration;
            _map = map;
        }

        public string createTokenFromAccount(Account acc)
        {
            if (acc != null)
            {
                //create claims details based on the user information
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", acc.Id.ToString()),
                        new Claim("FullName", acc.FullName),
                        new Claim("UserName", acc.UserName),  
                        new Claim("Email", acc.Email)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires : DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "";
        }

        public AccountDTO? ValidateJwtToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                AccountDTO acc = new AccountDTO();
                
                var jwtToken = (JwtSecurityToken)validatedToken;
                acc.Id = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
                acc.UserName = jwtToken.Claims.First(x => x.Type == "UserName").Value;
                acc.FullName = jwtToken.Claims.First(x => x.Type == "FullName").Value;
                acc.Email = jwtToken.Claims.First(x => x.Type == "Email").Value;

                
                // return account id from JWT token if validation successful
                return acc;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public async Task<int> addNewAccount(RegisterAccoutDTO registerAccount)
        {
            try
            {
                

                Account acc = _map.Map<Account>(registerAccount);
                acc.Id = await _repo.createIdForAccountId();
                bool isSuccess = await _repo.addNewAccount(acc);

                if (isSuccess)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return -1;
            }

        }
    }
}
