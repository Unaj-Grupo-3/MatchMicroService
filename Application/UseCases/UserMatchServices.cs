using Application.Interfaces;

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
    }
}
