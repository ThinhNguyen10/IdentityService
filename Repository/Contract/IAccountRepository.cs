using Identity.Models;

namespace Identity.Repository.Contract
{
    public interface IAccountRepository
    {
        Task<Account> checkUserByUserName(string userName);

        Task<bool> addNewAccount(Account acc);

        Task<int> createIdForAccountId();
    }
}
