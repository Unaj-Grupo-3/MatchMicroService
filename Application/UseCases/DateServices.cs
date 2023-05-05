using Application.Interfaces;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class DateServices
    {
        private readonly IDateCommands _commands;

        public DateServices(IDateCommands commands)
        {
            _commands = commands;
        }

        public async Task<DateResponse> CreateDate()
    }
}
