using FluentAssertions;
using WorkoutPlanner.Application.Users;
using WorkoutPlanner.Domain;
using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class UserLogicTests
{
    private UserLogic _userLogic = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userLogic = new UserLogic();
    }
    [TestMethod]
    public void CreateUser_WhenValidInput_ReturnsUser()
    {
        // Arrange
        string password = "Password123!";
        string name = "John";
        string surname = "Doe";
        string email = "Jdoe@gmail.com";
        UserRole role = UserRole.Player;
        
        // Act
        var result = _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("Jdoe@gmail.com");
        result.Role.Should().Be(UserRole.Player);
        result.PasswordHash.Should().Be(password);
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Password cannot be empty.");
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

        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be empty.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Surname cannot be empty.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Email cannot be empty.");
    }
    
}
