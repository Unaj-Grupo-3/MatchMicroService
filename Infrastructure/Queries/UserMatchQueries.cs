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
                .Where(m => m.LikeUser1 == 1 && m.LikeUser2 == 1)
                .ToListAsync();
            return matches;
        }
        public async Task<IList<UserMatch>> GetMatchByUserId(int userId)
        {
            IList<UserMatch> matches = await _context.UserMatches
                .Where(m => 
                        (m.User1 == userId || m.User2 == userId) 
                        && m.LikeUser1 == 1 && m.LikeUser2 == 1)
                .ToListAsync();
            return matches;
        }
        public async Task<UserMatch> WasSeen(int userId1, int userId2)
        {
            var userMatch = await _context.UserMatches
                .FirstOrDefaultAsync(m => m.User2 == userId1 && m.User1 == userId2);
            return userMatch;
        }
        public async Task<UserMatch> Saw(int userId1, int userId2)
        {
            var userMatch = await _context.UserMatches
                .FirstOrDefaultAsync(m => m.User1 == userId1 && m.User2 == userId2);
            return userMatch;
        }
    }
}
