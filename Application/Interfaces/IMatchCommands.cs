﻿
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMatchCommands
    {
        Task<Match> CreateMatch(Match match);
        Task<Match> UpdateMatch(Match match);
        Task DeleteMatch(int id);
    }
}
