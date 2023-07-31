using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestDateValidation
    {
        [Fact]
        public async void IsInDateTestTrue()
        {
            //ARRANGE
            var mockQueries = new Mock<IDateQueries>();
            DateValidations dateValidationsService = new (mockQueries.Object);

            int userId = 1;
            int anotherUserId = 100;

            DateEditRequest req = new() 
            { 
                DateId = 1,
                State = 1  // aceptado
            };
            Date date = new()
            {
                Match = new()
                {
                    User1Id = userId,
                    User2Id = anotherUserId,
                }
            };

            mockQueries.Setup(query => query.GetDateById(It.IsAny<int>())).Returns(Task.FromResult(date));

            //ACT
            var result = await dateValidationsService.IsInDate(userId,req);

            //ASSERT
            result.Should().BeTrue();
        }
        [Fact]
        public async void IsInDateTestFalse()
        {
            //ARRANGE
            var mockQueries = new Mock<IDateQueries>();
            DateValidations dateValidationsService = new(mockQueries.Object);

            int userId = 1;

            DateEditRequest req = new() { DateId = 1 };
            Date date = new()
            {
                Match = new()
                {
                    User1Id = 3,
                    User2Id = 4,
                }
            };
            mockQueries.Setup(query => query.GetDateById(It.IsAny<int>())).Returns(Task.FromResult(date));

            //ACT
            var result = await dateValidationsService.IsInDate(userId, req);

            //ASSERT
            result.Should().BeFalse();
        }

    }
}
