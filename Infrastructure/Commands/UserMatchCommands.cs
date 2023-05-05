using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Commands
{
    public class UserMatchCommands : IUserMatchCommands
    {
        private readonly AppDbContext _context;

        public UserMatchCommands(AppDbContext context)
        {
            _context = context;
        }
    }
}
