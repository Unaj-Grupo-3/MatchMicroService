using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class UserMatchQueries : IUserMatchQueries
    {
        private readonly AppDbContext _context;

        public UserMatchQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<UserMatch>> GetAllMatch()
        {
            IList<UserMatch> matches = await _context.UserMatches
                .Where(m => m.LikeUser1 && m.LikeUser2)
                .ToListAsync();
            return matches;
        }
    }
}
