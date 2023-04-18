using Contracts;
using Entities;
using Entities.Models;

namespace Repository;

public class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    { }
    public IEnumerable<Account> GetAllAccounts()
    {
        return FindAll().OrderBy(ac => ac.AccountType).ToList();
    }
    public IEnumerable<Account> AccountsByOwner(Guid ownerId)
    {
        return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
    }

    public PagedList<Account> GetAllAccounts(AccountParameters accountParameters)
    {
        var accounts = FindAll().OrderBy(ac => ac.AccountType);

        return PagedList<Account>.ToPagedList(accounts,
            accountParameters.PageNumber, accountParameters.PageSize);
    }
    public Account GetAccountById(Guid acountId)
    {
        return FindByCondition(acId => acId.AccountId.Equals(acountId)).FirstOrDefault();
    }

    public void CreateAccount(Account account)
    {
        Create(account);
    }
}
