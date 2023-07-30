using Application.Interfaces;
using Application.Models;

namespace Application.UseCases
{
    public class DateValidations : IDateValidations
    {
        private readonly IDateQueries _queries;

        public DateValidations(IDateQueries queries)
        {
            _queries = queries;
        }

        public async Task<bool> IsInDate(int userId, DateEditRequest req)
        {
            var date = await _queries.GetDateById(req.DateId);

            if(date.Match.User1Id == userId || date.Match.User2Id == userId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
