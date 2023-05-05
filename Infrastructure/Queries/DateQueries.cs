using Application.Interfaces;
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
    }
}
