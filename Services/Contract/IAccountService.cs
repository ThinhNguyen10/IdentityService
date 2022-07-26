using Identity.Dto;
using Identity.Models;

namespace Identity.Services.Contract
{
    public interface IAccountService
    {
        string createTokenFromAccount(Account acc);

        AccountDTO? ValidateJwtToken(string token);

        Task<int> addNewAccount(RegisterAccoutDTO registerAccount);


    }
}
