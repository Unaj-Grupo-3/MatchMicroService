
using Application.Interfaces;
using Application.Models;
using Domain.Entities;

namespace Application.UseCases
{
    public class MatchServices : IMatchServices
    {
        private readonly IMatchCommands _commands;
        private readonly IMatchQueries _queries;
        private readonly IChatApiServices _chatApiServices;

        public MatchServices(IMatchCommands commands, IMatchQueries queries, IChatApiServices chatApiServices)
        {
            _commands = commands;
            _queries = queries;
            _chatApiServices = chatApiServices;
        }

        public async Task<MatchResponse2> CreateMatch(MatchRequest request)
        {
            Match match = new Match()
            {
                User1Id = request.User1,
                User2Id = request.User2,
                CreatedAt = DateTime.UtcNow,
            };

            Match create = await _commands.CreateMatch(match);

            if(create != null)
            {
                //Creo una instancia de Chat -> Chat API
                var chatResp = await _chatApiServices.CreateChat(match.User1Id, match.User2Id);

                if(chatResp != null)
                {
                    MatchResponse2 response = new MatchResponse2()
                    {
                        Id = create.MatchId,
                        User1 = create.User1Id,
                        User2 = create.User2Id,
                        ChatId = chatResp.ChatId
                    };

                    return response;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        public async Task DeleteMatch(int id)
        {
            await _commands.DeleteMatch(id);
        }

        public async Task<MatchResponse> GetById(int id)
        {
            Match match = await _queries.GetById(id);

            if (match == null)
            {
                return null;
            }

            MatchResponse response = new MatchResponse()
            {
                Id = match.MatchId,
                User1 = match.User1Id,
                User2 = match.User2Id,
            };

            return response;
        }

        public async Task<IList<MatchResponse>> GetByUserId(int userId)
        {
            IList<Match> matches = await _queries.GetByUserId(userId);

            IList<MatchResponse> matchResponses = new List<MatchResponse>();

            if (matches.Count == 0)
            {
                return matchResponses;
            }

            foreach (Match match in matches)
            {
                MatchResponse response = new MatchResponse()
                {
                    Id = match.MatchId,
                    User1 = match.User1Id,
                    User2 = match.User2Id,
                };

                matchResponses.Add(response);
            }

            return matchResponses;
        }

        public async Task<MatchResponse> GetByUsersIds(int userId1, int userId2)
        {
            Match match = await _queries.GetByUsersIds(userId1, userId2);

            if (match == null)
            {
                return null;
            }


            MatchResponse response = new MatchResponse()
            {
                Id = match.MatchId,
                User1 = match.User1Id,
                User2 = match.User2Id,
            };

            return response;
        }
        public async Task<IList<MatchResponse>> GetAll()
        {
            IList<MatchResponse> matches = await _queries.GetAllMatch();
            return matches;
        }
    }
}
