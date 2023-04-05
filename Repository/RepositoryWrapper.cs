using Contracts;
using Entities;

namespace Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private RepositoryContext _repositoryContext;
    private IOwnerRepository _owner;
    private IAccountRepository _account;
    public RepositoryWrapper(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }
    public IOwnerRepository OwnerRepository
    {
        get
        {
            if (_owner == null)
            {
                _owner = new OwnerRepository(_repositoryContext);
            }
            return _owner;
        }
    }

    public IAccountRepository AccountRepository
    {
        get
        {
            if (_account == null)
            {
                _account = new AccountRepository(_repositoryContext);
            }
            return _account;
        }
    }

    public void Save()
    {
        _repositoryContext.SaveChanges();
    }
}
