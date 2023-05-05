using Application.Interfaces;
using Application.Models;

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

        public async Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, bool LikeUser1)
        {
            //id esta en User2?
            //Si
            //Match
            //No Match
            //id esta en User1?
            //si -> Update
            //no -> add
            return new UserMatchResponse();
        }

    }
}
