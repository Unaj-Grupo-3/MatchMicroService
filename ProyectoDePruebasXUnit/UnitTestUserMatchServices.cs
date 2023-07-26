using Application.UseCases;
using System.Security.Claims;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestUserMatchServices
    {
        [Fact]
        public void Test1()
        {
            //ARRANGE
            UserMatchServices userMatchServices = new();
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims, "Bearer");

            int userId = 1;
            bool resultExpected = true;

            //ACT

            //ASSERT

        }
    }
}