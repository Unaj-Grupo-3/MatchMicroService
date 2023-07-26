using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Security.Claims;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestUserMatchServices
    {
        [Fact]
        public async void Test1()
        {
            //ARRANGE
            //UserMatchServices(IUserMatchCommands commands, IUserMatchQueries queries)
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);

            IList<UserMatch> resultExpected = new List<UserMatch>();
            mockQueries.Setup(query => query.GetAllMatch()).Returns(Task.FromResult(resultExpected));

            //ACT
            var result = await userMatchServices.GetAll();

            //ASSERT
            result.GetType().Should().Be(resultExpected.GetType());

        }
    }
}