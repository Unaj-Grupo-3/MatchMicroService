using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class DateServices : IDateServices
    {
        private readonly IDateCommands _commands;
        private readonly IDateQueries _queries;

        public DateServices(IDateCommands commands, IDateQueries queries)
        {
            _commands = commands;
            _queries = queries;
        }

        public async Task<DateResponse> CreateDate(DateRequest req)
        {
            Date date = new Date
            {
                DateId = req.DateId,
                MatchId = req.MatchId,
                Location = req.Location,
                Description = req.Description,
                Time = DateTime.Now,
                State = 0
            };

            await _commands.InsertDate(date);

            var query = await _queries.GetDateById(req.DateId);

            DateResponse response = new DateResponse
            {
                DateId = query.DateId,
                Location = query.Location,
                Description = query.Description,
                Time = query.Time,
                State = query.State,
                Match = new MatchResponse
                {
                    Id = query.MatchId,
                    User1 = query.Match.User1Id,
                    User2 = query.Match.User2Id
                }
            };

            return response;
        }
    }
}
