using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

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

        [Fact]
        public async void GetByUserIdTest() //if(match != null)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId = 1;

            Domain.Entities.Match match1 = new()
            {
                MatchId = 1,
                User1Id = userId,
                User2Id = 2,
            };
            Domain.Entities.Match match2 = new()
            {
                MatchId = 2,
                User1Id = 3,
                User2Id = userId,
            };
            IList<Domain.Entities.Match> matches = new List<Domain.Entities.Match> { match1, match2 };

            MatchResponse matchResponse1 = new()
            {
                Id = match1.MatchId,
                User1 = match1.User1Id,
                User2 = match1.User2Id,
            };
            MatchResponse matchResponse2 = new()
            {
                Id = match2.MatchId,
                User1 = match2.User2Id,
                User2 = match2.User1Id,
            };
            IList<MatchResponse> matchResponsesExpected = new List<MatchResponse>() { matchResponse1, matchResponse2 };

            mockQueries.Setup(q => q.GetByUserId(It.IsAny<int>())).Returns(Task.FromResult(matches));

            //ACT
            var result = await matchServices.GetByUserId(userId);

            //ASSERT
            result.Count.Equals(matchResponsesExpected.Count);
            for(int i = 0; i < result.Count; i++)
            {
                result[i].Id.Should().Be(matchResponsesExpected[i].Id);
                result[i].User1.Should().Be(matchResponsesExpected[i].User1);
                result[i].User2.Should().Be(matchResponsesExpected[i].User2);
            }
        }

        [Fact]
        public async void GetByUserIdZeroTest() //if(match.count ==0)
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId = 1;

            IList<Domain.Entities.Match> matches = new List<Domain.Entities.Match>();

            IList<MatchResponse> matchResponsesExpected = new List<MatchResponse>();

            mockQueries.Setup(q => q.GetByUserId(It.IsAny<int>())).Returns(Task.FromResult(matches));

            //ACT
            var result = await matchServices.GetByUserId(userId);

            //ASSERT
            result.Count.Equals(matchResponsesExpected.Count);

        }

        [Fact]
        public async void GetByUsersIdsTestIsNull() //
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId1 = 1;
            int userId2 = 2;

            mockQueries.Setup(q => q.GetByUsersIds(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult<Domain.Entities.Match>(null));

            //ACT
            var result = await matchServices.GetByUsersIds(userId1, userId2);

            //ASSERT
            result.Should().Be(null);
        }

        [Fact]
        public async void GetByUsersIdsTest() //
        {
            //ARRANGE
            //mocks de dependencia
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId1 = 1;
            int userId2 = 2;

            Domain.Entities.Match match = new()
            {
                MatchId = 1,
                User1Id = userId1,
                User2Id = userId2,
            };

            mockQueries.Setup(q => q.GetByUsersIds(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult<Domain.Entities.Match>(match));

            MatchResponse matchResponseExpected = new()
            {
                Id = match.MatchId,
                User1 = match.User1Id,
                User2 = match.User2Id,
            };

            //ACT
            var result = await matchServices.GetByUsersIds(userId1, userId2);

            //ASSERT
            result.Id.Should().Be(matchResponseExpected.Id);
            result.User1.Should().Be(matchResponseExpected.User1);
            result.User2.Should().Be(matchResponseExpected.User2);
        }

        [Fact]
        public async void ExistMatchTestTrue() //
        {
            //ARRANGE
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId1 = 1;
            int userId2 = 2;

            mockQueries.Setup(q => q.Exist(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(true));

            //ACT
            var result = await matchServices.ExistMatch(userId1, userId2);

            //ASSERT
            result.Should().BeTrue();
        }

        [Fact]
        public async void ExistMatchTestFalse() //
        {
            //ARRANGE
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            int userId1 = 1;
            int userId2 = 2;

            mockQueries.Setup(q => q.Exist(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(false));

            //ACT
            var result = await matchServices.ExistMatch(userId1, userId2);

            //ASSERT
            result.Should().BeFalse();
        }

        [Fact]
        public async void UpdateMatchTestTrue() //
        {
            //ARRANGE
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            MatchRequestUpdate matchRequestUpdate = new ();

            mockCommands.Setup(q => q.UpdateMatch(It.IsAny<Domain.Entities.Match>())).Returns(Task.FromResult(new Domain.Entities.Match()));

            //ACT
            var result = await matchServices.UpdateMatch(matchRequestUpdate);

            //ASSERT
            result.Should().BeTrue();
        }

        [Fact]
        public async void UpdateMatchTestFalse() //
        {
            //ARRANGE
            var mockCommands = new Mock<IMatchCommands>();
            var mockQueries = new Mock<IMatchQueries>();
            var mockChatApiServices = new Mock<IChatApiServices>();
            MatchServices matchServices = new(mockCommands.Object, mockQueries.Object, mockChatApiServices.Object);

            MatchRequestUpdate matchRequestUpdate = new();

            mockCommands.Setup(q => q.UpdateMatch(It.IsAny<Domain.Entities.Match>())).ThrowsAsync(new Exception());

            //ACT
            var result = await matchServices.UpdateMatch(matchRequestUpdate);

            //ASSERT
            result.Should().BeFalse();
        }
    }
}