using Application.UseCases;
using System.Security.Claims;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestTokenService
    {
        [Fact]
        public void ValidateUserIdTrue()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims);

            int userId = 1;
            bool resultExpected = true;

            //ACT
            var result = services.ValidateUserId(identity, userId);

            //ASSERT
            Assert.Equal(resultExpected, result);

        }
        [Fact]
        public void ValidateUserIdFalse()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims);

            int userId = 2;
            bool resultExpected = false;

            //ACT
            var result = services.ValidateUserId(identity, userId);

            //ASSERT
            Assert.Equal(resultExpected, result);
        }
        [Fact]
        public void ValidateUserIdException()
        {
            //ARRANGE
            TokenServices services = new();
            var identity = new ClaimsIdentity(new List<Claim>());

            int userId = 1;
            bool resultExpected = false;

            //ACT: si busco un usuario que no existe devuelve un false
            var result = services.ValidateUserId(identity, userId);

            //ASSERT
            Assert.Equal(resultExpected, result);

            //ACT: services.ValidateUserId(identity, userId);

            //ASSERT //Func<object?> testCode
            //Assert.Throws<NullReferenceException>(() => services.ValidateUserId(identity, userId));
            //no espera una exception, return false;
        }

        [Fact]
        public void GetUserId()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") }; //type, value
            var identity = new ClaimsIdentity(claims);

            int userIdExpected = 1;

            //ACT
            int userIdResponse = services.GetUserId(identity);

            //ASSERT
            Assert.Equal(userIdExpected, userIdResponse);
        }
        
    }
}