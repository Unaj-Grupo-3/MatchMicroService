using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class DateQueries : IDateQueries
    {
        private readonly AppDbContext _context;

        public DateQueries(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Date> GetDateById(int id)
        {
            var query = await _context.Dates.Include(e => e.Match)
                                            .FirstOrDefaultAsync(e => e.DateId.Equals(id));

            return query;
        }

        public async Task<IList<DateResponse>> GetDatesByUserId(int userId)
        {//aun no probe
            IList<DateResponse> matches = await _context.Dates
                .Include(date => date.Match)
                .Where(x => x.Match.User1Id == userId || x.Match.User2Id == userId)
                .Select(d => new DateResponse
                    {
                     DateId = d.DateId,
                     Description = d.Description,
                     Location = d.Location, 
                     Match = new MatchResponse
                     {
                         Id = d.MatchId,
                         User1 = d.Match.User1Id,
                         User2 = d.Match.User2Id,
                     },
                     State = d.State,
                     Time = d.Time,

                })
                .ToListAsync();
            return matches;
        }

        public async Task<IList<DateResponse>> GetDatesByMatchId(int macthId)
        {
            IList<DateResponse> dates = await _context.Dates
                                                .Include(e => e.Match)
                                                .Where(e => e.Match.MatchId == macthId)
                                                .Select(d => new DateResponse
                                                {
                                                    DateId = d.DateId,
                                                    Description = d.Description,
                                                    Location = d.Location,
                                                    Match = new MatchResponse
                                                    {
                                                        Id = d.MatchId,
                                                        User1 = d.Match.User1Id,
                                                        User2 = d.Match.User2Id,
                                                    },
                                                    State = d.State,
                                                    Time = d.Time
                                                }).ToListAsync();

            return dates;
        }
    }
}
