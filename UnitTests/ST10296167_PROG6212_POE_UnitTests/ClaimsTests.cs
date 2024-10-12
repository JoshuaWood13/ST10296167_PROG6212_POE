using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ST10296167_PROG6212_POE.Controllers;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Models;

namespace ST10296167_PROG6212_POE_UnitTests
{
    [TestClass]
    public class ClaimsTests
    {
        private ClaimController controller;
        private AppDbContext context;

        [TestInitialize]
        public void Initialize()
        {
            // Setup an InMemory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            context = new AppDbContext(options); // Using InMemory database for real behavior

            // Initialize the controller
            controller = new ClaimController(context);

            // Create and mock session
            var mockSession = new Mock<ISession>();
            var mockHttpContext = new Mock<HttpContext>();

            // Setup session to return lecturer ID
            var mockLecturerID = 1; // Set a valid lecturer ID
            byte[] byteLecturerID = BitConverter.GetBytes(mockLecturerID); // Convert to byte[]
            mockSession.Setup(s => s.TryGetValue(It.Is<string>(key => key == "AccountID"), out byteLecturerID))
                       .Returns(true); // Always returns true and sets the byteLecturerID as the session value

            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            controller = new ClaimController(context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

        }

        // Tests for Submit Claim functionality 
        //------------------------------------------------------------------------------------------------------------------------------------------//
        [TestMethod]
        public async Task SubmitClaim_ValidClaim_AddsClaimToDb()
        {
            // Arrange
            // Create a valid claim with necessary data
            var validClaim = new Claims
            {
                HourlyRate = 100,
                HoursWorked = 10,
                ClaimMonth = "May",
                Description = "Test",
                ClaimAmount = 1000
            };


            // Act
            var result = await controller.SubmitClaim(validClaim);

            // Assert 
            var dbClaim = context.Claims.FirstOrDefault(c => c.Description == "Test");

            Assert.IsNotNull(dbClaim);
            Assert.AreEqual(validClaim.Description, dbClaim.Description);
            Assert.AreEqual(validClaim.HourlyRate, dbClaim.HourlyRate);
            Assert.AreEqual(validClaim.HoursWorked, dbClaim.HoursWorked);
            Assert.AreEqual(validClaim.ClaimMonth, dbClaim.ClaimMonth);
            Assert.AreEqual(validClaim.ClaimAmount, dbClaim.ClaimAmount);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task SubmitClaim_InvalidClaim_ReturnsViewWithValidationErrors()
        {
            // Arrange
            // Missing HourlyRate, HoursWorked
            var invalidClaim = new Claims
            {
                ClaimMonth = "May",
                Description = "Test"
            };

            // Manually add a validation error for missing HourlyRate field
            controller.ModelState.AddModelError("HourlyRate", "Please enter an hourly rate");
            controller.ModelState.AddModelError("HoursWorked", "Please enter hours worked");

            // Act
            var result = await controller.SubmitClaim(invalidClaim);

            // Assert
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey("HourlyRate"));
            Assert.AreEqual("Please enter an hourly rate", controller.ModelState["HourlyRate"].Errors[0].ErrorMessage);
            Assert.IsTrue(controller.ModelState.ContainsKey("HoursWorked"));
            Assert.AreEqual("Please enter hours worked", controller.ModelState["HoursWorked"].Errors[0].ErrorMessage);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}