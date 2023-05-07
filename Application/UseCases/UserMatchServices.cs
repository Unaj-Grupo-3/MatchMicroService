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
        public async Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, int LikeUser2)
        {
            UserMatchResponse response = new();

            var entry = await _queries.WasSeen(userId1, userId2);
            
            if(entry != null)
            {
                response = await _commands.UpdateRow(new UserMatch //Agregar Like en la row del User2
                {
                    User1 = entry.User1,
                    User2 = entry.User2,
                    LikeUser1 = LikeUser2,
                    LikeUser2 = entry.LikeUser2
                });
            }
            else 
            {
                var previousRow = await _queries.Saw(userId1, userId2);

                if (previousRow != null)
                {
                    response = await _commands.UpdateRow(new UserMatch //Modificar Like Anterior
                    {
                        User1 = previousRow.User1,
                        User2 = previousRow.User2,
                        LikeUser1 = previousRow.LikeUser1,
                        LikeUser2 = LikeUser2
                    });
                }
                else
                {
                    response = await _commands.Like(new UserMatch //AddLike
                    {
                        User1 = userId1,
                        User2 = userId2,
                        LikeUser2 = LikeUser2
                    });
                }

            }
            return response;
        }

    }
}
