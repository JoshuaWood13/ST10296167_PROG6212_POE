using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;
using ST10296167_PROG6212_POE.Data;
using ST10296167_PROG6212_POE.Controllers;
using ST10296167_PROG6212_POE.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ST10296167_PROG6212_POE_UnitTests
{
    [TestClass]
    public class DocumentsTests
    {
        private AppDbContext context;
        private DocumentController controller;

        [TestInitialize]
        public void Initialize()
        {
            // InMemory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            context = new AppDbContext(options);

            controller = new DocumentController(context);

            // Create and mock session
            var mockSession = new Mock<ISession>();
            var mockHttpContext = new Mock<HttpContext>();

            // Mock session for AccountID
            mockSession.Setup(s => s.TryGetValue(It.Is<string>(key => key == "AccountID"), out It.Ref<byte[]>.IsAny))
                .Returns((string key, out byte[] value) =>
                {
                    if (key == "AccountID")
                    {
                        value = BitConverter.GetBytes(1).Reverse().ToArray(); 
                        return true;
                    }
                    value = null;
                    return false;
                });

            // Ensure session methods return and set values correctly
            var sessionStorage = new Dictionary<string, byte[]>();
            mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession.Object);

            controller = new DocumentController(context)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            controller.HttpContext.Session.SetInt32("AccountID", 1);
        }

        // Tests for uploading Valid and Invalid files
        //------------------------------------------------------------------------------------------------------------------------------------------//
        [TestMethod]
        public async Task UploadDocs_UploadValidFile_DocLinkedToClaimAndAdddedToDb()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 1,
                LecturerID = 1,
                HourlyRate = 50,
                HoursWorked = 10,
                ClaimMonth = "October",
                ClaimAmount = 500,
                Description = "Test claim"
            };
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            var document = new Documents
            {
                ClaimID = 1,
                FileName = "testfile.pdf"
            };

            var fileContent = Encoding.UTF8.GetBytes("This is a test file content");
            var fileName = "testfile.pdf";

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(fileContent.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileContent));
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((stream, token) => {
                    new MemoryStream(fileContent).CopyTo(stream);
                })
                .Returns(Task.CompletedTask);

            // Act
            await controller.UploadDocs(document, fileMock.Object);

            // Assert
            var uploadedDocument = await context.Documents.FirstOrDefaultAsync(d => d.FileName == fileName);

            // Verify that file is succesfully uploaded
            Assert.IsNotNull(uploadedDocument);
            Assert.AreEqual(1, uploadedDocument.ClaimID);
            Assert.AreEqual(fileName, uploadedDocument.FileName);
            Assert.AreEqual(fileContent.Length, uploadedDocument.FileData.Length);
            Assert.IsTrue(uploadedDocument.FileData.SequenceEqual(fileContent));
        }

        [TestMethod]
        public async Task UploadDocs_UploadInvalidFile_DocNotUploadedAndReturnedToView()
        {
            // Arrange
            var claim = new Claims
            {
                ClaimID = 2,
                LecturerID = 1,
                HourlyRate = 50,
                HoursWorked = 100,
                ClaimMonth = "October",
                ClaimAmount = 5000,
                Description = "Test claim"
            };
            await context.Claims.AddAsync(claim);
            await context.SaveChangesAsync();

            var document = new Documents
            {
                ClaimID = 2,
                FileName = "testfile.docx"
            };

            var fileContent = new byte[0];
            var fileName = "testfile.docx";

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(fileContent.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileContent));
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((stream, token) => {
                    new MemoryStream(fileContent).CopyTo(stream);
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UploadDocs(document, fileMock.Object);

            // Assert
            var viewResult = result as ViewResult;

            // Verify user is returned to view for error information
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("UploadDocuments", viewResult.ViewName);

            // Verify that document was not added to the DB
            var uploadedDocument = await context.Documents.FirstOrDefaultAsync(d => d.FileName == fileName);
            Assert.IsNull(uploadedDocument);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
