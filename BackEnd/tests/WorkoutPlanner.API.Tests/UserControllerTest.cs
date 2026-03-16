using FluentAssertions;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class UserControllerTest
{
    Mock<IUserLogic> _userLogicMock = null!;
    UserController _controller = null!; 
    
    [TestInitialize]
    public void SetUp()
    {
        _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
        _controller = new UserController(_userLogicMock.Object);
    }

    [TestMethod]
    public async Task Create_ReturnsCreatedUser()
    {
        // Arrange
        var name = "John";
        var surname = "Doe";
        var email = "j@gmail.com";
        var password = "password123";

        var createdUser = new User { Name = name, Email = email, Role = Enums.UserRole.Player };
        _userLogicMock.Setup(logic => logic.CreateUser(password, name, surname, email, Enums.UserRole.Player))
            .ReturnsAsync(createdUser);
        var request = new Models.CreateUserRequestDto
            { Password = password, Name = name, Surname = surname, Email = email, Role = Enums.UserRole.Player };

        // Act
        var result = await _controller.Create(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Email.Should().Be(email);
        result.Role.Should().Be(Enums.UserRole.Player);
        _userLogicMock.Verify(logic => logic.CreateUser(password, name, surname, email, Enums.UserRole.Player),
            Times.Once);
    }
    
    [TestMethod]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        var name = "John";
        _userLogicMock.Setup(logic => logic.DeleteUser(name)).Returns(Task.CompletedTask);
        var request = new DeleteUserRequestDto { Name = name };

        // Act
        var result = await _controller.Delete(request);

        // Assert
        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.NoContentResult>();
        _userLogicMock.Verify(logic => logic.DeleteUser(name), Times.Once);
    }

    [TestMethod]
    public async Task GetByName_ReturnsUser()
    {
        // Arrange
        var name = "John";
        var email = "j@gmail.com";
        var user = new User { Name = name, Email = email, Role = Enums.UserRole.Player };
        _userLogicMock.Setup(logic => logic.GetUserByName(name)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetByName(name);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);  
        result.Email.Should().Be(email);
        result.Role.Should().Be(Enums.UserRole.Player);
        _userLogicMock.Verify(logic => logic.GetUserByName(name), Times.Once);
    }

    [TestMethod]
    public async Task GetAll_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Name = "John", Email = "j@gmail.com", Role = Enums.UserRole.Player },
            new User { Name = "Jane", Email = "s@gmail.com", Role = Enums.UserRole.Trainer }
        };
        _userLogicMock.Setup(logic => logic.GetAllUsers()).ReturnsAsync(users);
        
        // Act
        var result = await _controller.GetAll();
        
        // Assert
        result.Should().NotBeNull();
        result.Users.Should().HaveCount(2);
        result.Users[0].Name.Should().Be("John");
        result.Users[0].Email.Should().Be("j@gmail.com");
        result.Users[0].Role.Should().Be(Enums.UserRole.Player);
        result.Users[1].Name.Should().Be("Jane");
        result.Users[1].Email.Should().Be("s@gmail.com");
        result.Users[1].Role.Should().Be(Enums.UserRole.Trainer);
        _userLogicMock.Verify(logic => logic.GetAllUsers(), Times.Once);
    }

    [TestMethod]
    public async Task Update_ReturnsUpdatedUser()
    {
        // Arrange
        var name = "John";
        var updatedEmail = "s@gmail.com";
        var updatedUser = new User { Name = name, Email = updatedEmail, Role = Enums.UserRole.Player };
        _userLogicMock.Setup(logic => logic.UpdateUser(name, null, null, null, updatedEmail, null))
            .ReturnsAsync(updatedUser);
        var request = new UpdateUserRequestDto { Email = updatedEmail };
        
        // Act
        var result = await _controller.Update(name, request);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Email.Should().Be(updatedEmail);
        result.Role.Should().Be(Enums.UserRole.Player);
        _userLogicMock.Verify(logic => logic.UpdateUser(name, null, null, null, updatedEmail, null), Times.Once);
    }

}