﻿using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IOwnerRepository Owner
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_repositoryContext);
                }
                return _owner;
            }
        }
        public IAccountRepository Account
        {
            get
            {
                return _account;
            }
        }
        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
