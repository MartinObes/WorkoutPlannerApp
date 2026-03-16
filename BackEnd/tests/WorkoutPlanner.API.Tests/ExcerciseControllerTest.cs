using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutPlanner.API.Controllers;
using WorkoutPlanner.API.Models;
using WorkoutPlanner.Application.Excercises;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Tests;

[TestClass]
public class ExcerciseControllerTest
{
        private Mock<IExcerciseLogic> _excerciseLogicMock = null!;
        private ExerciseController _controller = null!;
    
        [TestInitialize]
        public void SetUp()
        {
            _excerciseLogicMock = new Mock<IExcerciseLogic>(MockBehavior.Strict);
            _controller = new ExerciseController(_excerciseLogicMock.Object);
        }
        
        [TestMethod]
        public async Task GetAll_ReturnsAllExcercises()
        {
            // Arrange
            var excercises = new List<Domain.Excercise>
            {
                new Excercise { Name = "Excercise1" },
                new Excercise { Name = "Excercise2" }
            };
            _excerciseLogicMock.Setup(logic => logic.GetAllExcercises()).ReturnsAsync(excercises);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Excercises.Count);
            Assert.AreEqual("Excercise1", result.Excercises[0].Name);
            Assert.AreEqual("Excercise2", result.Excercises[1].Name);
            _excerciseLogicMock.Verify(logic => logic.GetAllExcercises(), Times.Once);
        }
        
        [TestMethod]
        public async Task Create_ReturnsCreatedExcercise()
        {
            // Arrange
            var excerciseName = "NewExcercise";
            var createdExcercise = new Excercise { Name = excerciseName };
            _excerciseLogicMock.Setup(logic => logic.CreateExcercise(excerciseName)).ReturnsAsync(createdExcercise);
            var request = new CreateExcerciseRequestDto { Name = excerciseName };

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(excerciseName, result.Name);
            _excerciseLogicMock.Verify(logic => logic.CreateExcercise(excerciseName), Times.Once);
        }

        [TestMethod]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var excerciseName = "ExcerciseToDelete";
            _excerciseLogicMock.Setup(logic => logic.DeleteExcercise(excerciseName)).Returns(Task.CompletedTask);
            
            // Act
            var result = await _controller.Delete(excerciseName);
            
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _excerciseLogicMock.Verify(logic => logic.DeleteExcercise(excerciseName), Times.Once);
        }

        [TestMethod]
        public async Task GetByName_ReturnsExcercise()
        {
            // Arrange
            var excerciseName = "Excercise1";
            var excercise = new Excercise { Name = excerciseName };
            _excerciseLogicMock.Setup(logic => logic.GetExcerciseByName(excerciseName)).ReturnsAsync(excercise);
            
            // Act
            var result = await _controller.GetByName(excerciseName);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(excerciseName, result.Name);
            _excerciseLogicMock.Verify(logic => logic.GetExcerciseByName(excerciseName), Times.Once);
        }
}
