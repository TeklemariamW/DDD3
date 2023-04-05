using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
{
	public OwnerRepository(RepositoryContext repositoryContext)
		: base(repositoryContext)
	{ }

	public PagedList<Owner> GetAllOwners(OwnerParameters ownerParameters)
	{
		return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
			ownerParameters.PageNumber,
			ownerParameters.PageSize);
	}
	public Owner GetOwnerById(Guid ownerId)
	{
		return FindByCondition(ow => ow.OwnerId.Equals(ownerId)).FirstOrDefault();
	}
	public Owner GetOwnerWithDetails(Guid ownerId)
	{
		return FindByCondition(ow => ow.OwnerId.Equals(ownerId))
			.Include(ac => ac.Accounts)
			.FirstOrDefault();
	}
	public void CreateOwner(Owner owner)
	{
		Create(owner);
	}
	public void UpdateOwner(Owner owner)
	{
		Update(owner);
	}
	public void DeleteOwner(Owner owner)
	{
		Delete(owner);
	}
}
