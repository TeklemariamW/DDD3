namespace Contracts;

public interface IRepositoryWrapper
{
    IOwnerRepository OwnerRepository { get; }
    IAccountRepository AccountRepository { get; }
    void Save();
}
