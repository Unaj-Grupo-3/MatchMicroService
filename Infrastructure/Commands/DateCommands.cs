using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
