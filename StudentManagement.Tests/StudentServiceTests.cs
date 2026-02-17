using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Helpers;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Tests
{
    public class StudentServiceTests
    {

        private async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            await databaseContext.Database.EnsureCreatedAsync();
            return databaseContext;
        }

        [Fact]
        public async Task GetAllStudents_ReturnsCorrectCount()
        {
            // 1. Arrange
            var dbContext = await GetDatabaseContext();
            var mockMapper = new Mock<IMapper>();

            // Seed data
            dbContext.Students.Add(new Student { Id = 1, FirstName = "John", LastName = "Doe", Email = "j@test.com" });
            dbContext.Students.Add(new Student { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@test.com" });
            await dbContext.SaveChangesAsync();

            var service = new StudentService(dbContext, mockMapper.Object);
            var query = new StudentQueryObject { PageNumber = 1, PageSize = 10 };

            // Mock the Mapper return value
            mockMapper.Setup(m => m.Map<List<StudentDto>>(It.IsAny<List<Student>>()))
                      .Returns(new List<StudentDto> { new StudentDto(), new StudentDto() });

            // 2. Act
            var result = await service.GetAllStudents(query);

            // 3. Assert
            Assert.Equal(2, result.Count);
        }
    }
}
