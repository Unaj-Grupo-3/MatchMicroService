using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestUserMatchServices
    {
        [Fact]
        public async void GetAllMatchTest()
        {
            //ARRANGE
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
        //AddOrUpdateMethod --> UserMatchResponse
        //CASE 1: es la primer interaccion entre user 1 y user 2, agrego un registro a la bd user1, user2, ±1
        [Fact]
        public async void AddOrUpdateTestCase1()
        {
            //ARRANGE
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);
            int userId1 = 1;
            int userId2 = 2;
            int like = 1;

            mockQueries.Setup(q => q.WasSeen(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult<UserMatch>(null)); //U1 no fue likeado por 2
            mockQueries.Setup(q => q.Saw(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult<UserMatch>(null)); //ni tiene que modificar su propio like
            mockCommands.Setup(c => c.Like(It.IsAny<UserMatch>())).ReturnsAsync(new UserMatchResponse()
            {
                User1 = userId1,
                User2 = userId2,
                IsMatch = false,
            }); //guardo el registro del like en la bd

            //ACT
            var result = await userMatchServices.AddOrUpdate(userId1, userId2, like);

            //ASSERT
            result.User1.Should().Be(userId1);
            result.User2.Should().Be(userId2);
            result.IsMatch.Should().BeFalse();
        }
        //AddOrUpdateTest --> UserMatchResponse
        //CASE 2: el user 1 fue visto por el user 2, lo agrego, hay un registro en la bd user2, user1, ±1
        [Fact]
        public async void AddOrUpdateTestCase2()
        {
            //ARRANGE
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);
            int userId1 = 1;
            int userId2 = 2;
            int like = 1;

            mockQueries.Setup(q => q.WasSeen(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new UserMatch()
            {
                User1 = userId2,
                User2 = userId1,
                LikeUser1 = 0,
                LikeUser2 = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            })); //U1 fue likeado por 2
            mockCommands.Setup(c => c.UpdateRow(It.IsAny<int>(), It.IsAny<int>(), like)).ReturnsAsync(new UserMatchResponse() 
            {
                User1 = userId2,
                User2 = userId1,
                IsMatch = true,
            }); //guardo el registro del like en la bd

            //ACT
            var result = await userMatchServices.AddOrUpdate(userId1, userId2, like);

            //ASSERT
            result.User1.Should().Be(userId2);
            result.User2.Should().Be(userId1);
            result.IsMatch.Should().BeTrue();
        }
        //AddOrUpdateTest --> UserMatchResponse
        //CASE 3: el user 1 ya likeo al user2, hay que modificar el registro en la bd user1, user2, ±1
        [Fact]
        public async void AddOrUpdateTestCase3()
        {
            //ARRANGE
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);
            int userId1 = 1;
            int userId2 = 2;
            int like = 1;

            mockQueries.Setup(q => q.WasSeen(userId1, userId2)).Returns(Task.FromResult<UserMatch>(null)); //U1 no fue likeado por 2
            mockQueries.Setup(q => q.Saw(userId1, userId2)).Returns(Task.FromResult(new UserMatch()
            {
                User1 = userId1,
                User2 = userId2,
                LikeUser1 = 0,
                LikeUser2 = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }));//tiene que modificar su propio like
            mockCommands.Setup(c => c.UpdateRow(It.IsAny<int>(), It.IsAny<int>(), like)).ReturnsAsync(new UserMatchResponse() 
            {
                User1 = userId1,
                User2 = userId2,
                IsMatch = false,
            }); //guardo el cambio del like en la bd

            //ACT
            var result = await userMatchServices.AddOrUpdate(userId1, userId2, like);

            //ASSERT
            result.User1.Should().Be(userId1);
            result.User2.Should().Be(userId2);
            result.IsMatch.Should().BeFalse();
        }

        [Fact]
        public async void GetAllWorkerTest()
        {
            //ARRANGE
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);

            IList<UserMatch> resultExpected = new List<UserMatch>();
            mockQueries.Setup(query => query.GetWorkerAllMatch()).Returns(Task.FromResult(resultExpected));

            //ACT
            var result = await userMatchServices.GetAllWorker();

            //ASSERT
            result.GetType().Should().Be(resultExpected.GetType());
        }

        [Fact]
        public async void GetMatchByUserIdTest()
        {
            //ARRANGE
            var mockCommands = new Mock<IUserMatchCommands>();
            var mockQueries = new Mock<IUserMatchQueries>(); //mock de dependencia
            UserMatchServices userMatchServices = new(mockCommands.Object, mockQueries.Object);
            int userId = 1;
            IList<UserMatch> resultExpected = new List<UserMatch>();
            mockQueries.Setup(query => query.GetMatchByUserId(userId)).Returns(Task.FromResult(resultExpected));

            //ACT
            var result = await userMatchServices.GetMatchesByUserId(userId);

            //ASSERT
            result.GetType().Should().Be(resultExpected.GetType());
        }
    }
}