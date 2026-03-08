using FluentAssertions;
using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Domain.Tests;

[TestClass]
public class DomainValidations
{
    //Tests for WorkoutExcercise Domain Validations (abbreviated to WE for shorter tests names)
    [TestMethod] 
    public void ValidateWE_WithValidAttributes_ShouldNotThrowException()
    {
        //Arrange
        int sets = 3;
        int reps = 10;
        LoadType loadType = LoadType.Weight;
        int weight = 100;
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, weight, null);
        
        //Assert
        act.Should().NotThrow();
    }
    
    [TestMethod]
    public void ValidateWE_WithZeroSets_ShouldThrowArgumentException()
    {
        //Arrange
        int sets = 0;
        int reps = 10;
        LoadType loadType = LoadType.Weight;
        int weight = 100;
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, weight, null);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Sets must be greater than 0");
    }
    
    [TestMethod]
    public void ValidateWE_WithZeroReps_ShouldThrowArgumentException()
    {
        //Arrange
        int sets = 3;
        int reps = 0;
        LoadType loadType = LoadType.Weight;
        int weight = 100;
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, weight, null);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Reps must be greater than 0");
    }
    
    [TestMethod]
    public void ValidateWE_WithWeightLoadTypeAndNullWeight_ShouldThrowArgumentException()
    {
        //Arrange
        int sets = 3;
        int reps = 10;
        LoadType loadType = LoadType.Weight;
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, null, null);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Weight must be provided");
    }
    
    [TestMethod]
    public void ValidateWE_WithPercentageLoadTypeAndNullPercentage_ShouldThrowArgumentException()
    {
        //Arrange
        int sets = 3;
        int reps = 10;
        LoadType loadType = LoadType.Percentage;
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, null, null);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Percentage must be provided");
    }
    
    [TestMethod]
    public void ValidateWE_WrongEnumValue_ShouldThrowArgumentException()
    {
        //Arrange
        int sets = 3;
        int reps = 10;
        LoadType loadType = (LoadType)999; // Invalid enum value
        
        //Act
        Action act = () => WorkoutExcercise.Validate(sets, reps, loadType, null, null);
        
        //Assert
        act.Should().Throw<ArgumentException>("invalid load type.");
    }
    
    //User Validation
    [TestMethod]
    public void ValidateUserRole_WithValidRole_ShouldNotThrowException()
    {
        //Arrange
        UserRole role = UserRole.Player;
        
        //Act
        Action act = () => User.ValidateRole(role);
        
        //Assert
        act.Should().NotThrow();
    }
    
    [TestMethod]
    public void ValidateUserRole_WithInvalidRole_ShouldThrowArgumentException()
    {
        //Arrange
        UserRole role = (UserRole)999; // Invalid enum value
        
        //Act
        Action act = () => User.ValidateRole(role);
        
        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid user role.");
    }
}
