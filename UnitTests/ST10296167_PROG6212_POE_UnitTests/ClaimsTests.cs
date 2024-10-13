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

            context = new AppDbContext(options); 

            // Initialize the controller
            controller = new ClaimController(context);

            // Create and mock session
            var mockSession = new Mock<ISession>();
            var mockHttpContext = new Mock<HttpContext>();

            // Setup session to return lecturer ID
            var mockLecturerID = 1; 
            byte[] byteLecturerID = BitConverter.GetBytes(mockLecturerID); 
            mockSession.Setup(s => s.TryGetValue(It.Is<string>(key => key == "AccountID"), out byteLecturerID))
                       .Returns(true); // Always returns true and sets the byteLecturerID as the session value

            // Setup session to return AccountType, defaulting to Lecturer (you can override this in each test)
            var accountType = "Lecturer"; // Default AccountType
            byte[] accountTypeBytes = System.Text.Encoding.UTF8.GetBytes(accountType);
            mockSession.Setup(s => s.TryGetValue(It.Is<string>(key => key == "AccountType"), out accountTypeBytes))
                       .Returns(true);

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

        // Tests for Claim Verification and Claim Status updates
        //------------------------------------------------------------------------------------------------------------------------------------------//
        [TestMethod]
        public async Task ProcessClaim_UpdateClaimApprovalStatus_PCApproval()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 3,
                LecturerID = 1,
                HourlyRate = 100,  
                HoursWorked = 10,  
                ClaimMonth = "October",  
                ClaimAmount = 1000,  
                Description = "Testing",
                ApprovalPC = 0,  
                ApprovalAM = 0   
            };

            // Add the claim to the in-memory database
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            // Override session for "Programme Coordinator"
            var mockSession = new Mock<ISession>();
            var accountType = "Programme Coordinator";
            byte[] accountTypeBytes = System.Text.Encoding.UTF8.GetBytes(accountType);
            mockSession.Setup(s => s.TryGetValue("AccountType", out accountTypeBytes)).Returns(true);

            // Create a mock HttpContext and set the session
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            // Set the controller's HttpContext with the mock session
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await controller.ProcessClaim(claim.ClaimID, "approve");

            // Assert
            var updatedClaim = await context.Claims.FindAsync(claim.ClaimID); 

            Assert.AreEqual(1, updatedClaim?.ApprovalPC);  
            Assert.AreEqual("Pending (1/2)", updatedClaim?.Status);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task ProcessClaim_UpdateClaimApprovalStatus_PCDenial()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 4,
                LecturerID = 1,
                HourlyRate = 100,
                HoursWorked = 10,
                ClaimMonth = "October",
                ClaimAmount = 1000,
                Description = "Testing",
                ApprovalPC = 0,
                ApprovalAM = 0
            };

            // Add the claim to the in-memory database
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            // Override session for "Programme Coordinator"
            var mockSession = new Mock<ISession>();
            var accountType = "Programme Coordinator";
            byte[] accountTypeBytes = System.Text.Encoding.UTF8.GetBytes(accountType);
            mockSession.Setup(s => s.TryGetValue("AccountType", out accountTypeBytes)).Returns(true);

            // Create a mock HttpContext and set the session
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            // Set the controller's HttpContext with the mock session
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await controller.ProcessClaim(claim.ClaimID, "deny");

            // Assert
            var updatedClaim = await context.Claims.FindAsync(claim.ClaimID); 

            Assert.AreEqual(2, updatedClaim?.ApprovalPC);
            Assert.AreEqual("Rejected", updatedClaim?.Status);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task ProcessClaim_UpdateClaimApprovalStatus_AMApproval()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 5,
                LecturerID = 1,
                HourlyRate = 100,
                HoursWorked = 10,
                ClaimMonth = "October",
                ClaimAmount = 1000,
                Description = "Testing",
                ApprovalPC = 1,
                ApprovalAM = 0
            };

            // Add the claim to the in-memory database
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            // Override session for "Programme Coordinator"
            var mockSession = new Mock<ISession>();
            var accountType = "Academic Manager";
            byte[] accountTypeBytes = System.Text.Encoding.UTF8.GetBytes(accountType);
            mockSession.Setup(s => s.TryGetValue("AccountType", out accountTypeBytes)).Returns(true);

            // Create a mock HttpContext and set the session
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            // Set the controller's HttpContext with the mock session
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await controller.ProcessClaim(claim.ClaimID, "approve");

            // Assert
            var updatedClaim = await context.Claims.FindAsync(claim.ClaimID); 

            Assert.AreEqual(1, updatedClaim?.ApprovalAM);  
            Assert.AreEqual("Approved", updatedClaim?.Status);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task ProcessClaim_UpdateClaimApprovalStatus_AMDenial()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 6,
                LecturerID = 1,
                HourlyRate = 100,
                HoursWorked = 10,
                ClaimMonth = "October",
                ClaimAmount = 1000,
                Description = "Testing",
                ApprovalPC = 1,
                ApprovalAM = 0
            };

            // Add the claim to the in-memory database
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            // Override session for "Programme Coordinator"
            var mockSession = new Mock<ISession>();
            var accountType = "Academic Manager";
            byte[] accountTypeBytes = System.Text.Encoding.UTF8.GetBytes(accountType);
            mockSession.Setup(s => s.TryGetValue("AccountType", out accountTypeBytes)).Returns(true);

            // Create a mock HttpContext and set the session
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            // Set the controller's HttpContext with the mock session
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await controller.ProcessClaim(claim.ClaimID, "deny");

            // Assert
            var updatedClaim = await context.Claims.FindAsync(claim.ClaimID);

            Assert.AreEqual(2, updatedClaim?.ApprovalAM);
            Assert.AreEqual("Rejected", updatedClaim?.Status);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}