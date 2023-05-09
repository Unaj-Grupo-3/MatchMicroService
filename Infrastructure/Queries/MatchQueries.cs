using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class MatchQueries : IMatchQueries
    {
        private readonly AppDbContext _context;

        public MatchQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Match>> GetAllMatch()
        {
            IList<Match> matches = await _context.Matches
                .ToListAsync();
            return matches;
        }

        public async Task<Match> GetById(int id)
        {
            Match match = await _context.Matches.FirstOrDefaultAsync(x => x.MatchId == id);

            return match;
        }

        public async Task<IList<Match>> GetByUserId(int userId)
        {
            IList<Match> matches = await _context.Matches.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
            return matches;
        }

        public async Task<Match> GetByUsersIds(int userId1, int userId2)
        {
            //Match match = await _context.Matches.FirstOrDefaultAsync(x => (x.User1Id == userId1 || x.User2Id == userId1) && (x.User1Id == userId2 || x.User2Id == userId2));

            Match match = await _context.Matches
                                        .Where(x => x.User1Id == userId1 || x.User2Id == userId1)
                                        .Where(x => x.User1Id == userId2 || x.User2Id == userId2)
                                        .FirstOrDefaultAsync();
            return match;
        }
    }
}
