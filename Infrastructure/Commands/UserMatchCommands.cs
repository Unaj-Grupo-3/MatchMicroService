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

        public async Task<UserMatchResponse> UpdateRow(UserMatch userMatch)
        {
            _context.UserMatches.Find(userMatch);

            await _context.SaveChangesAsync();

            return new UserMatchResponse
            {
                User1 = userMatch.User1,
                User2 = userMatch.User2,
                IsMatch = userMatch.LikeUser1 == 1 && userMatch.LikeUser2 == 1,
            };
        }
    }
}
