using Entities.Models;

namespace Contracts;

public interface IAccountRepository
{
    PagedList<Account> GetAllAccounts(AccountParameters accountParameters);
    IEnumerable<Account> AccountsByOwner(Guid ownerId);
    Account GetAccountById(Guid id);
    void CreateAccount(Account account);
}