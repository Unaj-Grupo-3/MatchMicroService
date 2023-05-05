using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Queries
{
    public class UserMatchQueries : IUserMatchQueries
    {
        private readonly AppDbContext _context;

        public UserMatchQueries(AppDbContext context)
        {
            _context = context;
        }
    }
}
