
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands
{
    public class MatchCommands : IMatchCommands
    {
        private readonly AppDbContext _context;

        public MatchCommands(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Match> CreateMatch(Match match)
        {
            _context.Matches.Add(match);

            await _context.SaveChangesAsync();

            return match;
        }

        public async Task DeleteMatch(int id)
        {
            Match toDelete = await _context.Matches.FirstOrDefaultAsync(x => x.MatchId == id);

            _context.Matches.Remove(toDelete);

            await _context.SaveChangesAsync();
        }
    }
}
