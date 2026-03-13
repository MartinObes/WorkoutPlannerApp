using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Infraestructure.Repositories;
using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class UserLogicTests
{
    private UserLogic _userLogic = null!;
    private Mock<UserRepository> _userRepositoryMock = null!;
    private readonly DbContextOptions<AppDbContext> _dummyOptions = new();

    [TestInitialize]
    public void Initialize()
    {
        var mockDbContext = new Mock<AppDbContext>(_dummyOptions);
        _userRepositoryMock = new Mock<UserRepository>(MockBehavior.Strict, mockDbContext.Object);
        _userLogic = new UserLogic(_userRepositoryMock.Object);
    }
    
    [TestMethod]
    public async Task CreateUser_WhenValidInput_ReturnsUser()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe";
        string email = "Jdoe@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        var result = await _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("Jdoe@gmail.com");
        result.Role.Should().Be(UserRole.Player);
        result.PasswordHash.Should().Be(password);
        result.Id.Should().NotBeEmpty();
        result.Id.Should().NotBe(userId);
        _userRepositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<User>(u => u.Name == result.Name && u.Email == result.Email && u.Role == result.Role && u.PasswordHash == result.PasswordHash)), Times.Once);
    }
    
    [TestMethod]
    public void CreateUser_WhenInvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe";
        string email = "invalid-email";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        Action act = () =>  _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenNameContainsSpecialCharacters_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John@";
        string surname = "Doe";
        string email = "kk@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenSurnameContainsSpecialCharacters_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe@";
        string email = "l@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenEmptyPassword_ThrowsArgumentException()
    {
        // Arrange
        string password = "";
        string name = "John";
        string surname = "Doe";
        string email = "l@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Password cannot be empty.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenEmptyName_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "";
        string surname = "Doe";
        string email = "l@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);


        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenEmptySurname_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "";
        string email = "a@gmail.com";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Surname cannot be empty.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }
    
    [TestMethod]
    public void CreateUser_WhenEmptyEmail_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe";
        string email = "";
        UserRole role = UserRole.Player;
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Email cannot be empty.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void CreateUser_WhenInvalidRole_ThrowsArgumentException()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe";
        string email = "j@gmail.com";
        UserRole role = (UserRole)999; // Invalid role
        Guid userId = Guid.NewGuid();
        
        _userRepositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid user role.");
        _userRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void UpdateUser_WhenValidInput_ReturnsUpdatedUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "j@gmail.com",
            Role = UserRole.Player,
            PasswordHash = "Password123!"
        };
        
        _userRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .Verifiable();
        
        // Act
        var result = _userLogic.UpdateUser(user, "NewPassword123!", "Jane", "Smith", "js@gmail.com", UserRole.Trainer);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Jane Smith");
        result.Email.Should().Be("js@gmail.com");
        result.Role.Should().Be(UserRole.Trainer);
        result.PasswordHash.Should().Be("NewPassword123!");
        _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u => u.Id == user.Id && u.Name == result.Name && u.Email == result.Email && u.Role == result.Role && u.PasswordHash == result.PasswordHash)), Times.Once);
    }

    [TestMethod]
    public void UpdateUser_WhenNullUser_ThrowsArgumentException()
    {
        // Arrange
        User user = null!;

        // Act
        Action act = () => _userLogic.UpdateUser(null, null, null, null, null, null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("User cannot be null.");
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }
    
    [TestMethod]
    public void UpdateUser_WhenInvalidEmail_ThrowsArgumentException()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "jd@gmail.com",
            Role = UserRole.Player,
            PasswordHash = "Password123!"
        };
        
        _userRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .Verifiable();
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, null, null, "invalid-email", null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void UpdateUser_WhenNameContainsSpecialCharacters_ThrowsArgumentException()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "jd@gmail.com",
            Role = UserRole.Player,
            PasswordHash = "Password123!"
        };
        
        _userRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .Verifiable();
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, "Jane@", "Smith", null, null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void UpdateUser_WhenSurnameContainsSpecialCharacters_ThrowsArgumentException()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "jd@gmail.com",
            Role = UserRole.Player,
            PasswordHash = "Password123!"
        };
        
        _userRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .Verifiable();
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, "Jane", "Smith@", null, null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void UpdateUser_WhenInvalidRole_ThrowsArgumentException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "jd@gmail.com",
            Role = UserRole.Player,
            PasswordHash = "Password123!"
        };
        
        _userRepositoryMock
            .Setup(repo => repo.Update(It.IsAny<User>()))
            .Verifiable();
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, null, null, null, (UserRole)999);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid user role.");
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public void DeleteUser_WhenValidUser_CallsRepositoryDelete()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "j@gmail.com", Role = UserRole.Player, PasswordHash = "Password123!" };
        _userRepositoryMock.Setup(repo => repo.Delete(user)).Verifiable();
        
        // Act
        _userLogic.DeleteUser(user);
        
        // Assert
        _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Once);
    }

    [TestMethod]
    public void DeleteUser_WhenNullUser_ThrowsArgumentNullException()
    {
        // Arrange
        User user = null!;
        _userRepositoryMock.Setup(repo => repo.Delete(user)).Verifiable();

        // Act
        Action act = () => _userLogic.DeleteUser(user);
        
        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("User cannot be null. (Parameter 'user')");
        _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Never);
    }
    
    [TestMethod]
    public async Task UserExists_WhenUserExists_ReturnsTrue()
    {
        // Arrange
        string name = "John Doe";
        _userRepositoryMock.Setup(repo => repo.ExistsAsync(u => u.Name == name)).ReturnsAsync(true);
        
        // Act
        var result = await _userLogic.UserExists(name);
        
        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(u => u.Name == name), Times.Once);
    }

    [TestMethod]
    public async Task UserExists_WhenUserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string name = "John Doe";
        _userRepositoryMock.Setup(repo => repo.ExistsAsync(u => u.Name == name)).ReturnsAsync(false);
        
        // Act
        var result = await _userLogic.UserExists(name);
        
        // Assert
        result.Should().BeFalse();
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(u => u.Name == name), Times.Once);
    }

    [TestMethod]
    public async Task GetUserByName_WhenUserExists_ReturnsUser()
    {
        // Arrange
        string name = "John Doe";
        var user = new User { Id = Guid.NewGuid(), Name = name, Email = "j@gmail.com", Role = UserRole.Player, PasswordHash = "Password123!" };
        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);   
        
        // Act
        var result = await _userLogic.GetUserByName(name);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), null), Times.Once);
    }

    [TestMethod]
    public void GetUserByName_WhenUserDoesNotExist_ThrowsArgumentNullException()
    {
        // Arrange
        string name = "John Doe";
        _userRepositoryMock.Setup(repo => repo.GetAsync(u => u.Name == name, null)).ReturnsAsync((User)null!);   
        
        // Act
        Action act = () => _userLogic.GetUserByName(name).GetAwaiter().GetResult();
        
        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("User cannot be null. (Parameter 'user')");
        _userRepositoryMock.Verify(repo => repo.GetAsync(u => u.Name == name, null), Times.Once);
    }

    [TestMethod]
    public async Task GetAllUsers_ReturnsListOfUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(), Name = "John Doe", Email = "j@gmail.com", Role = UserRole.Player,
                PasswordHash = "Password123!"
            },
            new User
            {
                Id = Guid.NewGuid(), Name = "Jane Smith", Email = "d@gmail.com", Role = UserRole.Trainer,
                PasswordHash = "Password123!"
            }
        };
        _userRepositoryMock.Setup(repo => repo.GetAllAsync(null, null)).ReturnsAsync(users);
        
        // Act
        var result = await _userLogic.GetAllUsers();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Jane Smith");
        result[1].Name.Should().Be("John Doe");
        _userRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }
    
    [TestMethod]
    public async Task GetAllUsers_WhenNoUsers_ReturnsEmptyList()
    {
        // Arrange
        var users = new List<User>();
        _userRepositoryMock.Setup(repo => repo.GetAllAsync(null, null)).ReturnsAsync(users);
        
        // Act
        var result = await _userLogic.GetAllUsers();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _userRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
    }
}
