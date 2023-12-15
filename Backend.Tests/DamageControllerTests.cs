using Backend.Controllers;
using Backend.Data.Interfaces;
using Backend.Data.Repositories;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Backend.Tests;

public class DamageControllerTests
{
    private readonly Mock<IDamageRepository> _mockRepo;
    private readonly DamageController _controller;

    public DamageControllerTests() {
        _mockRepo = new Mock<IDamageRepository>();
        _controller = new DamageController(_mockRepo.Object);
    }

    [Fact]
    public void GetAllDamages_ReturnsAllDamages() {
        // Arrange
        var mockDamages = new List<Damage> { new Damage(), new Damage() };
        _mockRepo.Setup(repo => repo.GetAllDamages()).Returns(mockDamages);

        // Act
        var result = _controller.GetAllDamages();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Damage>>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(mockDamages, returnValue.Value);
    }

    [Fact]
    public void GetAllDamages_ReturnsEmptyListWhenNoDamages() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllDamages()).Returns(new List<Damage>());

        // Act
        var result = _controller.GetAllDamages();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Damage>>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Empty((IEnumerable<Damage>)returnValue.Value);
    }

    [Fact]
    public void GetDamageById_ReturnsDamageWhenFound() {
        // Arrange
        var damage = new Damage { ID = 1 };
        _mockRepo.Setup(repo => repo.GetDamageById(1)).Returns(damage);

        // Act
        var result = _controller.GetDamageById(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Damage>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(damage, returnValue.Value);
    }

    [Fact]
    public void GetDamageById_ReturnsNotFoundWhenDamageDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetDamageById(It.IsAny<int>())).Returns((Damage)null);

        // Act
        var result = _controller.GetDamageById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Post_CreatesDamageAndReturnsCreatedAtAction() {
        // Arrange
        var newDamage = new Damage();
        _mockRepo.Setup(repo => repo.CreateDamage(newDamage)).Returns(newDamage);

        // Act
        var result = _controller.Post(newDamage);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Damage>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnValue = Assert.IsType<Damage>(createdAtActionResult.Value);
        Assert.Equal(newDamage, returnValue);
    }


    [Fact]
    public void Post_ReturnsBadRequestWhenModelInvalid() {
        // Arrange
        _controller.ModelState.AddModelError("error", "some error"); // Simulate model state error
        var newDamage = new Damage();

        // Act
        var result = _controller.Post(newDamage);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Damage>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }


    [Fact]
    public void Delete_ReturnsNoContentWhenDamageDeleted() {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteDamageById(1)).Returns(true);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNotFoundWhenDamageDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteDamageById(It.IsAny<int>())).Returns(false);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}