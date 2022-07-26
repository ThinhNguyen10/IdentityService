using Identity.Models;
using Identity.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AuthenticationContext _context;
        public AccountRepository(AuthenticationContext context)
        {
            _context = context;
        }

        public async Task<Account> checkUserByUserName(string userName)
        {
            return await _context.Accounts.Where(account => account.UserName == userName).FirstOrDefaultAsync(); 
        }

        public async Task<bool>  addNewAccount(Account acc)
        {
            await _context.Accounts.AddAsync(acc);
            return await Save(); 
        }

        private async Task<bool> Save()
        {
            int entry = await _context.SaveChangesAsync();
            return entry > 0? true: false;
        }

        public async Task<int> createIdForAccountId()
        {
            int accountId = 0;
            List<int> IdList = await _context.Accounts.Select(acc => acc.Id).ToListAsync();
            IdList.Sort();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (IdList[0] != 0) { accountId = 0; break; }
                if (IdList[i] == IdList.Count - 1) { accountId = IdList.Count; break; }
                if (IdList[i + 1] != IdList[i] + 1) { accountId = IdList[i] + 1; break; }

            }
            return accountId;
        }
    }
}
