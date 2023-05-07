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
        
        public async Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, int like)
        {
            UserMatchResponse response = new();

            var entry = await _queries.WasSeen(userId1, userId2);
            
            if(entry != null)
            {
                response = await _commands.UpdateRow(entry.UserMatchId, entry.LikeUser1, like); //Agregar Like en la row del User2
                
            }
            else 
            {
                var previousRow = await _queries.Saw(userId1, userId2);

                if (previousRow != null)
                {
                    response = await _commands.UpdateRow(previousRow.UserMatchId, like,previousRow.LikeUser2 ); //Modificar Like Anterior
                }
                else
                {
                    response = await _commands.Like(new UserMatch //AddLike
                    {
                        User1 = userId1,
                        User2 = userId2,
                        LikeUser1 = like,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    });
                }

            }
            return response;
        }

    }
}
