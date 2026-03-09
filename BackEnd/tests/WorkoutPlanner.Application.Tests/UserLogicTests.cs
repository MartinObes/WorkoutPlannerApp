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
        Guid userId = Guid.NewGuid();

        
        // Act
        var result = _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("Jdoe@gmail.com");
        result.Role.Should().Be(UserRole.Player);
        result.PasswordHash.Should().Be(password);
        result.Id.Should().NotBeEmpty();
        result.Id.Should().NotBe(userId);
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
        Guid userId = Guid.NewGuid();

        
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
        Guid userId = Guid.NewGuid();
        
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
        Guid userId = Guid.NewGuid();

        
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
        Guid userId = Guid.NewGuid();


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
        Guid userId = Guid.NewGuid();

        
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
        Guid userId = Guid.NewGuid();

        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Email cannot be empty.");
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
        
        // Act
        Action act = () => _userLogic.CreateUser(password, name, surname, email, role);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid user role.");
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
        
        // Act
        var result = _userLogic.UpdateUser(user, "NewPassword123!", "Jane", "Smith", "js@gmail.com", UserRole.Trainer);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Jane Smith");
        result.Email.Should().Be("js@gmail.com");
        result.Role.Should().Be(UserRole.Trainer);
        result.PasswordHash.Should().Be("NewPassword123!");
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
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, null, null, "invalid-email", null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
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
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, "Jane@", "Smith", null, null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
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
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, "Jane", "Smith@", null, null);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name and surname cannot contain special characters.");
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
        
        // Act
        Action act = () => _userLogic.UpdateUser(user, null, null, null, null, (UserRole)999);
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid user role.");
    }

    [TestMethod]
    public void DeleteUser_WhenNullUser_ThrowsArgumentNullException()
    {
        // Arrange
        User user = null!;

        // Act
        Action act = () => _userLogic.DeleteUser(user);
        
        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("User cannot be null. (Parameter 'user')");
    }
}
