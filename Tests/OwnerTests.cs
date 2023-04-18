using API.Controllers;
using AutoMapper;
using Contracts;
using Entities.Models;
using Moq;

namespace Tests;

public class OwnerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly IMapper _mockMapper;
    private readonly OwnerController _ownerController;
    public OwnerTests()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _ownerController = new OwnerController(_mockRepo.Object, _mockMapper);
    }

    [Fact]
    public void GetOwners_ResponseTest()
    {
        // Arrange
        var ownersParameter = new OwnerParameters()
        {

        };

        // Act
        var result = _ownerController.GetAllOwners(ownersParameter);

        // Assert
        Assert.NotNull(result);
    }
}

