using Application.Interfaces;
using Application.Models;
using Domain.Entities;


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

        public async Task<DateResponse> CreateDate(DateRequest2 req)
        {
            Date date = new Date
            {
                MatchId = req.MatchId,
                Location = req.Location,
                Description = req.Description,
                Time = DateTime.Now,
                State = 0
            };

            await _commands.InsertDate(date);

            var dates = await _queries.GetDatesByMatchId(req.MatchId);
            var response = dates.FirstOrDefault(e => e.Time.Equals(date.Time));

            
            return response;
        }

        public async Task<IList<DateResponse>> GetDatesByUserId(int userId)
        {
            IList<DateResponse> dates = await _queries.GetDatesByUserId(userId);
            return dates;
        }

        public async Task<IList<DateResponse>> GetDatesByMatchId(int matchId)
        {
            // Devuelve la listta de citas que planifico con ese usuario matcheado.
            var response = await _queries.GetDatesByMatchId(matchId);
            return response;
        }

        public async Task<DateResponse> EditDate(DateEditRequest req)
        {
            var date = await _commands.AlterDate(req);

            DateResponse resp = new DateResponse
            {
                DateId = date.DateId,
                Location = date.Location,
                Description = date.Description,
                Time = date.Time,
                State = date.State,
                Match = new MatchResponse
                {
                    Id = date.Match.MatchId,
                    User1 = date.Match.User1Id,
                    User2 = date.Match.User2Id
                }
            };

            return resp;
        }
    }
}
