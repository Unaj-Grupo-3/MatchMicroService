using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using FluentAssertions;
using Moq;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestMatchServices
    {
        [Fact]
        public async void Test()
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
    }
}