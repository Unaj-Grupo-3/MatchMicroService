
using Application.Interfaces;
using Azure.Core;
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

        public async Task<Match> UpdateMatch(Match match)
        {
            try
            {
                var updateMatch = (from s in _context.Matches
                                   where (s.User1Id.Equals(match.User1Id) && s.User2Id.Equals(match.User2Id))
                                   select s).FirstOrDefault();
                //if (updateMatch != null)
                //{
                //    updateMatch.View1 = match.View1;
                //}
                //else
                //{
                //    updateMatch = (from s in _context.Matches
                //                   where (s.User2Id.Equals(match.User1Id) && s.User1Id.Equals(match.User2Id))
                //                   select s).FirstOrDefault();
                //    updateMatch.View2 = match.View1;
                //}

                updateMatch.View1 = match.View1;
                updateMatch.View2 = match.View2;

                _context.Matches.Update(updateMatch);
                await _context.SaveChangesAsync();

                return updateMatch;
            }
            catch (Exception ex)
            {
                return null;
            }
            
            
        }
    }
}
