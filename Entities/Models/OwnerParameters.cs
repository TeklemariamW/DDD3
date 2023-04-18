namespace Entities.Models;

public class OwnerParameters : QueryStringParameters
{
    public DateTime MinYearOfBirth { get; set; }
    public DateTime MaxYearOfBirth { get; set; } = DateTime.UtcNow;

    public bool ValidYearRange => MaxYearOfBirth > MinYearOfBirth;

    public string Name { get; set; }
}
