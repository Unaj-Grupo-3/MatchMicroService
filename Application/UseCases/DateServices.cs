using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Application.Helpers;
using Microsoft.IdentityModel.Abstractions;
using System.Runtime.Intrinsics.X86;

namespace Application.UseCases
{
    public class DateServices : IDateServices
    {
        private readonly IDateCommands _commands;
        private readonly IDateQueries _queries;
        private readonly IUserApiServices _queriesUserApi;

        public DateServices(IDateCommands commands, IDateQueries queries, IUserApiServices queriesUserApi)
        {
            _commands = commands;
            _queries = queries;
            _queriesUserApi = queriesUserApi;
        }

        public async Task<DateResponse> CreateDate(DateRequest2 req)
        {
            Date date = new Date
            {
                MatchId = req.MatchId,
                Location = req.Location,
                Description = req.Description,
                Time = req.Time,
                ProposedUserId = req.ProposedUserId,
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

        public async Task<IList<DatesDetailsResponse>> GetDatesDetails(int userId)
        {
            List<DatesDetailsResponse> response = new List<DatesDetailsResponse>();
            IList<DateResponse> dates = await _queries.GetDatesByUserId(userId); // Citas del usuario buscado
            List<int> usersForAPI = new List<int>();
            foreach (DateResponse item in dates)
            {
                var user1 = item.Match.User1;
                var user2 = item.Match.User2;

                if (!usersForAPI.Contains(user1))
                {
                    usersForAPI.Add(user1);
                }
                if (!usersForAPI.Contains(user2))
                {
                    usersForAPI.Add(user2);
                }
            }
            var usersDetails = await _queriesUserApi.GetUsersApi(usersForAPI); // Datos sensibles de los usuarios


            foreach (DateResponse item in dates)
            {
                int proposedUser_Id = item.Match.User1 == item.ProposedUserId ? item.Match.User1 : item.Match.User2;
                int receivingUser_Id = item.Match.User2 != item.ProposedUserId ? item.Match.User2 : item.Match.User1;

                var proposedUser = usersDetails.FirstOrDefault(f => f.UserId == proposedUser_Id);
                var receivingUser = usersDetails.FirstOrDefault(f => f.UserId == receivingUser_Id);
                var latitudeDate = Convert.ToDouble(item.Location.Split(';')[0].Replace('.', ','));
                var longitudeDate = Convert.ToDouble(item.Location.Split(';')[1].Replace('.', ','));

                DatesDetailsResponse obj = new DatesDetailsResponse()
                {
                    DateId = item.DateId,
                    Time = item.Time,
                    Latitude = latitudeDate, // latitud de cita
                    Longitude = longitudeDate, // longitud de cita
                    Description = item.Description,
                    DistanceBetweenUsers = CalculateDistance.Calculate(proposedUser.Location.Longitude, receivingUser.Location.Longitude, proposedUser.Location.Latitude, receivingUser.Location.Latitude),
                    DistanceBetweenUserDate1 = CalculateDistance.Calculate(proposedUser.Location.Longitude, longitudeDate, proposedUser.Location.Latitude, latitudeDate),
                    DistanceBetweenUserDate2 = CalculateDistance.Calculate(receivingUser.Location.Longitude, longitudeDate, receivingUser.Location.Latitude, latitudeDate),
                    User1 = new UserResponse2()
                    {
                        UserId = proposedUser.UserId,
                        Name = proposedUser.Name,
                        LastName = proposedUser.LastName,
                        Age = DateTime.Today.AddTicks(-proposedUser.Birthday.Ticks).Year - 1,
                        Images = proposedUser.Images.Count.Equals(0) ? null : proposedUser.Images[0].Url
                    },
                    User2 = new UserResponse2()
                    {
                        UserId = receivingUser.UserId,
                        Name = receivingUser.Name,
                        LastName = receivingUser.LastName,
                        Age = DateTime.Today.AddTicks(-receivingUser.Birthday.Ticks).Year - 1,
                        Images = receivingUser.Images.Count.Equals(0) ? null : receivingUser.Images[0].Url
                    },
                    State = item.State
                };

                response.Add(obj);
            }

            // ordenamos la lista por fecha mas proxima de cita
            return response.OrderByDescending(o => o.Time).ToList();
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
                ProposedUserId = date.ProposedUserId,
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
