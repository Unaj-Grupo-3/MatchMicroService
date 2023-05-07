using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands
{
    public class UserMatchCommands : IUserMatchCommands
    {
        private readonly AppDbContext _context;

        public UserMatchCommands(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserMatchResponse> Like(UserMatch userMatch)
        {
            _context.UserMatches.Add(userMatch);

            await _context.SaveChangesAsync();

            return new UserMatchResponse
            {
                User1 = userMatch.User1,
                User2 = userMatch.User2,
                IsMatch = null,
            };
        }

        public async Task<UserMatchResponse> UpdateRow(int UserMatchId, int LikeUser1, int LikeUser2)
        {
            var row = await _context.UserMatches.FindAsync(UserMatchId);
            row.LikeUser1 = LikeUser1;
            row.LikeUser2 = LikeUser2;

            await _context.SaveChangesAsync();

            return new UserMatchResponse
            {
                User1 = row.User1,
                User2 = row.User2,
                IsMatch = LikeUser1 == 1 && LikeUser2 == 1,
            };
        }
    }
}
