﻿

using Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.UseCases
{
    public class TokenServices : ITokenServices
    {
        public bool ValidateUserId(ClaimsIdentity identity,int userId)
        {
            try
            {
                var id = identity.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

                if (id != userId.ToString())
                {
                    return false;
                }

                return true;
            }

            catch (ArgumentException)
            {
                return false;
            }
        }
        public int GetUserId(ClaimsIdentity identity)
        {
            var id = int.Parse(identity.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            return id;
        }
    }
}
