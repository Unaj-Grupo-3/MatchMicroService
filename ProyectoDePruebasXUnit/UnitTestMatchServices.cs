using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestMatchServices
    {
        [Fact]
        public async void CreateMatchTestCase1() //if(create != null) & if(chatResp != null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>(); 
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);
            int userId1 = 1;
            int userId2 = 2;
            MatchRequest request = new()
            {
                User1 = userId1,
                User2 = userId2,
            };
            Domain.Entities.Match match = new()
            {
                MatchId = 1,
                User1Id = request.User1,
                User2Id = request.User2,
                CreatedAt = DateTime.UtcNow,
                View1 = false,
                View2 = false
            };
            mockCommands.Setup(c => c.CreateMatch(It.IsAny<Domain.Entities.Match>())).Returns(Task.FromResult(match));

            ChatResponse chatResp = new()
            {
                ChatId = 1,
                User1Id = userId1,
                User2Id = userId2
            };
            mockChatApiServices.Setup(chat => chat.CreateChat(userId1, userId2)).Returns(Task.FromResult(chatResp));

            //ACT
            var result = await matchServices.CreateMatch(request);

            //ASSERT
            result.ChatId.Should().Be(chatResp.ChatId);
            result.Id.Should().Be(match.MatchId);
        }

        [Fact]
        public async void CreateMatchTestCase2() //if(create != null) & if(chatResp == null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);
            int userId1 = 1;
            int userId2 = 2;
            MatchRequest request = new()
            {
                User1 = userId1,
                User2 = userId2,
            };
            Domain.Entities.Match match = new()
            {
                MatchId = 1,
                User1Id = request.User1,
                User2Id = request.User2,
                CreatedAt = DateTime.UtcNow,
                View1 = false,
                View2 = false
            };
            mockCommands.Setup(c => c.CreateMatch(It.IsAny<Domain.Entities.Match>())).Returns(Task.FromResult(match));

            mockChatApiServices.Setup(chat => chat.CreateChat(userId1, userId2)).Returns(Task.FromResult<ChatResponse>(null));

            //ACT
            var result = await matchServices.CreateMatch(request);

            //ASSERT
            result.Id.Should().Be(match.MatchId);
            result.ChatId.Should().Be(-1);
        }

        [Fact]
        public async void CreateMatchTestCase3() //if(create = null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);
            int userId1 = 1;
            int userId2 = 2;
            MatchRequest request = new()
            {
                User1 = userId1,
                User2 = userId2,
            };

            mockCommands.Setup(c => c.CreateMatch(It.IsAny<Domain.Entities.Match>())).Returns(Task.FromResult<Domain.Entities.Match>(null));

            //ACT
            var result = await matchServices.CreateMatch(request);

            //ASSERT
            result.Should().Be(null);
        }

        [Fact]
        public async void GetByIdTestNull() //if(match == null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);
            
            int matchId = 1;

            mockQueries.Setup(q => q.GetById(It.IsAny<int>())).Returns(Task.FromResult<Domain.Entities.Match>(null));

            //ACT
            var result = await matchServices.GetById(matchId);

            //ASSERT
            result.Should().Be(null);
        }
        [Fact]
        public async void GetByIdTest() //if(match != null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int matchId = 1;
            int userId1 = 1;
            int userId2 = 2;

            Domain.Entities.Match match = new()
            {
                MatchId = matchId,
                User1Id = userId1,
                User2Id = userId2,
            };

            mockQueries.Setup(q => q.GetById(It.IsAny<int>())).Returns(Task.FromResult(match));

            //ACT
            var result = await matchServices.GetById(matchId);

            //ASSERT
            result.Id.Should().Be(match.MatchId);
            result.User1.Should().Be(match.User1Id);
            result.User2.Should().Be(match.User2Id);
            //Assert.Equal(match.MatchId, result.Id);
        }
    }
}