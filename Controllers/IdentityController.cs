using AutoMapper;
using Identity.Dto;
using Identity.Models;
using Identity.Repository.Contract;
using Identity.ServiceResponder;
using Identity.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using XSystem.Security.Cryptography;

namespace Identity.Controllers
{
    
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IAccountRepository _repo;
        private readonly IAccountService _serv;
        private readonly IMapper _map;

        public IdentityController(IAccountRepository repo, IAccountService serv, IMapper map)
        {
            _repo = repo;
            _serv = serv;
            _map = map;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> registerAccount(RegisterAccoutDTO registerAccount)
        {
            string userName = registerAccount.UserName;
            string passWord = registerAccount.PassWord;
            string fullName = registerAccount.FullName;
            string email = registerAccount.Email;

            ServiceResponse<AccountDTO> res = new ServiceResponse<AccountDTO>();
            var accountResponse = _map.Map<AccountDTO>(registerAccount);

            res.Data = accountResponse;
            res.Success = false;
            res.Message = "Error";
            res.StatusCode = StatusCodes.Status400BadRequest.ToString();

            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(passWord) || String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(email))
            {
                res.ErrorMessages = new List<string>() { "Some field is null or Empty. Please fill out before requesting" };
                return BadRequest(res);
            }

            
            int messageResponse = await _serv.addNewAccount(registerAccount);

            if (messageResponse == 1)
            {
                res.Data = accountResponse;
                res.Success = true;
                res.Message = "Success";
                res.StatusCode = StatusCodes.Status201Created.ToString();
                return StatusCode(201, res);
            }
            else
            {
                res.ErrorMessages = new List<string>() { "Account already exists. Please enter new username && password" };
                return BadRequest(res);
            }
            
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> checkLogin([FromBody] UserInforDto userInfo)
        {
            string userName = userInfo.UserName;
            string passWord = userInfo.PassWord;

            ServiceResponse<UserInforDto> res = new ServiceResponse<UserInforDto>();

            if(String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(passWord))
            {
                res.Data = userInfo;
                res.Success = false;
                res.Message = "Error";
                res.StatusCode = StatusCodes.Status400BadRequest.ToString();
                res.ErrorMessages = new List<string>() { "UserName or PassWord is empty"};
                return BadRequest(userInfo);
            }

            Account user = await _repo.checkUserByUserName(userName);
            if(user != null)
            {
                var md5 = new MD5CryptoServiceProvider();
                var data = Encoding.ASCII.GetBytes(passWord);
                var hashedPassWord = md5.ComputeHash(data);


                if (hashedPassWord.SequenceEqual(user.PassWord))
                {
                    string token = _serv.createTokenFromAccount(user);
                    return Ok(token);
                }
            }

            res.Data = userInfo;
            res.Success = false;
            res.Message = "Error";
            res.StatusCode = StatusCodes.Status401Unauthorized.ToString();
            res.ErrorMessages = new List<string>() { "UserName or PassWord is empty" };
            return StatusCode(401, res);
        }

        [HttpPost]
        [Route("token")]
        public IActionResult checkToken([FromHeader(Name = "Authorization")] string authValue)
        {
            string token = authValue.Split(' ')[1];

            AccountDTO? acc = _serv.ValidateJwtToken(token);

            ServiceResponse<AccountDTO> res = new ServiceResponse<AccountDTO>();
            res.Data = acc;
            if (acc != null)
            {
                res.Success = true;
                res.Message = "Success";
                res.StatusCode = StatusCodes.Status200OK.ToString();
                return Ok(res);
            }
            else
            {
                res.Success = false;
                res.Message = "Error";
                res.ErrorMessages = new List<string>() { "Not Authorized" };
                return StatusCode(401,res);
            }

        }
    }
}
