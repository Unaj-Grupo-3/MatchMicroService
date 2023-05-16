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
    }
}
