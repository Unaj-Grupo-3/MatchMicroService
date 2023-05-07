using Application.Interfaces;
using Application.Models;
using Domain.Entities;

namespace Application.UseCases
{
    public class UserMatchServices : IUserMatchServices
    {
        private readonly IUserMatchCommands _commands;
        private readonly IUserMatchQueries _queries;

        public UserMatchServices(IUserMatchCommands commands, IUserMatchQueries queries)
        {
            _commands = commands;
            _queries = queries;
        }
        public async Task<IList<UserMatch>> GetAll()
        {
            IList<UserMatch> matches = await _queries.GetAllMatch();
            return matches;
        }
        public async Task<IList<UserMatch>> GetMatchesByUserId(int userId)
        {
            IList<UserMatch> matches = await _queries.GetMatchByUserId(userId);
            return matches;
        }
        
        public async Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, int LikeUser2)
        {
            UserMatchResponse response = new();

            var entry = await _queries.WasSeen(userId1, userId2);
            
            if(entry != null)
            {
                response = await _commands.UpdateRow(entry.UserMatchId, LikeUser2, entry.LikeUser2); //Agregar Like en la row del User2
                
            }
            else 
            {
                var previousRow = await _queries.Saw(userId1, userId2);

                if (previousRow != null)
                {
                    response = await _commands.UpdateRow(previousRow.UserMatchId, previousRow.LikeUser2, LikeUser2); //Modificar Like Anterior
                }
                else
                {
                    response = await _commands.Like(new UserMatch //AddLike
                    {
                        User1 = userId1,
                        User2 = userId2,
                        LikeUser2 = LikeUser2,
                        CreatedAt = DateTime.Now,
                    });
                }

            }
            return response;
        }

    }
}
