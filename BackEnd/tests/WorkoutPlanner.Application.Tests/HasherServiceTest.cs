using FluentAssertions;
using WorkoutPlanner.Application.Services.HasherService;

namespace WorkoutPlanner.Application.Tests;

[TestClass]
public class HasherServiceTest
{
    private HasherService _hasherService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _hasherService = new HasherService();
    }

    // Hash tests

    [TestMethod]
    public void Hash_WhenValidPassword_ReturnsHashedString()
    {
        // Arrange
        string password = "Password123!";

        // Act
        var result = _hasherService.Hash(password);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().NotBe(password);
        result.Split('.').Should().HaveCount(3);
    }

    [TestMethod]
    public void Hash_WhenCalledTwiceWithSamePassword_ReturnsDifferentHashes()
    {
        // Arrange
        string password = "Password123!";

        // Act
        var hash1 = _hasherService.Hash(password);
        var hash2 = _hasherService.Hash(password);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [TestMethod]
    public void Hash_WhenHashedStringFormat_ContainsIterationsSaltAndHash()
    {
        // Arrange
        string password = "Password123!";

        // Act
        var result = _hasherService.Hash(password);
        var parts = result.Split('.');

        // Assert
        parts.Should().HaveCount(3);
        int.TryParse(parts[0], out var iterations).Should().BeTrue();
        iterations.Should().BeGreaterThan(0);
        Convert.FromBase64String(parts[1]).Should().HaveCount(16); // SaltSize
        Convert.FromBase64String(parts[2]).Should().HaveCount(32); // HashSize
    }

    [TestMethod]
    public void Hash_WhenPasswordIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        string password = "";

        // Act
        Action act = () => _hasherService.Hash(password);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Password cannot be empty. (Parameter 'password')");
    }

    [TestMethod]
    public void Hash_WhenPasswordIsWhitespace_ThrowsArgumentException()
    {
        // Arrange
        string password = "   ";

        // Act
        Action act = () => _hasherService.Hash(password);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Password cannot be empty. (Parameter 'password')");
    }

    // Verify tests

    [TestMethod]
    public void Verify_WhenCorrectPassword_ReturnsTrue()
    {
        // Arrange
        string password = "Password123!";
        var hashed = _hasherService.Hash(password);

        // Act
        var result = _hasherService.Verify(hashed, password);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Verify_WhenWrongPassword_ReturnsFalse()
    {
        // Arrange
        string password = "Password123!";
        string wrongPassword = "WrongPassword!";
        var hashed = _hasherService.Hash(password);

        // Act
        var result = _hasherService.Verify(hashed, wrongPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenHashedPasswordIsEmpty_ReturnsFalse()
    {
        // Arrange
        string hashedPassword = "";
        string providedPassword = "Password123!";

        // Act
        var result = _hasherService.Verify(hashedPassword, providedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenProvidedPasswordIsEmpty_ReturnsFalse()
    {
        // Arrange
        string password = "Password123!";
        var hashed = _hasherService.Hash(password);

        // Act
        var result = _hasherService.Verify(hashed, "");

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenHashedPasswordIsMalformed_ReturnsFalse()
    {
        // Arrange
        string malformedHash = "not-a-valid-hash";
        string providedPassword = "Password123!";

        // Act
        var result = _hasherService.Verify(malformedHash, providedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenHashedPasswordHasInvalidBase64_ReturnsFalse()
    {
        // Arrange
        string invalidBase64Hash = "100000.not!!valid==base64.alsoinvalid!!";
        string providedPassword = "Password123!";

        // Act
        var result = _hasherService.Verify(invalidBase64Hash, providedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenHashedPasswordHasWrongNumberOfParts_ReturnsFalse()
    {
        // Arrange
        string twoPartHash = "100000.onlytwoparts";
        string providedPassword = "Password123!";

        // Act
        var result = _hasherService.Verify(twoPartHash, providedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Verify_WhenIterationsPartIsNotANumber_ReturnsFalse()
    {
        // Arrange
        string password = "Password123!";
        var hashed = _hasherService.Hash(password);
        var parts = hashed.Split('.');
        string corruptedHash = $"notanumber.{parts[1]}.{parts[2]}";

        // Act
        var result = _hasherService.Verify(corruptedHash, password);

        // Assert
        result.Should().BeFalse();
    }
}