using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands
{
    public class DateCommands : IDateCommands
    {
        private readonly AppDbContext _context;

        public DateCommands(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertDate(Date date)
        {
            _context.Add(date);

            await _context.SaveChangesAsync();
        }

        public async Task<Date> AlterDate(DateEditRequest req)
        {
            var date = await _context.Dates.Include(e => e.Match)
                                           .FirstOrDefaultAsync( e => e.DateId == req.DateId );

            if (date == null)
            {
                return null;
            }

            date.State = req.State;

            await _context.SaveChangesAsync();

            return date;
        }
    }
}
