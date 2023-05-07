using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDateServices
    {
        Task<DateResponse> CreateDate(DateRequest req);
        public Task<IList<DateResponse>> GetDatesByUserId(int userId);

    }
}
