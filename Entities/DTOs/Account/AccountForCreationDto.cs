using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DTOs.Account;

public class AccountForCreationDto
{
    [Required(ErrorMessage = "Account type is required")]
    public string? AccountType { get; set; }

    [ForeignKey(nameof(Owner))]
    public Guid OwnerId { get; set; }
}
