using Entities.Models;

namespace Contracts;

public interface IOwnerRepository
{
    PagedList<Owner> GetAllOwners(OwnerParameters ownerParameters);
    Owner GetOwnerById(Guid ownerId);
    Owner GetOwnerWithDetails(Guid ownerId);
    void CreateOwner(Owner owner);
    void UpdateOwner(Owner owner);
    void DeleteOwner(Owner owner);
}
