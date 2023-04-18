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
		var owners = FindByCondition(o => o.DateOfBirth >= ownerParameters.MinYearOfBirth &&
				o.DateOfBirth <= ownerParameters.MaxYearOfBirth)
				.OrderBy(on => on.Name);

		return PagedList<Owner>.ToPagedList(owners,
			ownerParameters.PageNumber,
			ownerParameters.PageSize);
	}
	public Owner GetOwnerById(Guid ownerId)
	{
		return FindByCondition(ow => ow.OwnerId.Equals(ownerId))
			.FirstOrDefault();
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

	private void ApplySort(ref IQueryable<Owner> owners, string orderByQueryString)
	{
		if (!owners.Any())
			return;
		if (string.IsNullOrWhiteSpace(orderByQueryString))
		{
			owners = owners.OrderBy(x => x.Name);
		}

		var orderParams = orderByQueryString.Trim().Split(',');
	}
}
