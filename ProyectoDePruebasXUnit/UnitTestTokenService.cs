using Application.UseCases;
using System.Security.Claims;

namespace ProyectoDePruebasXUnit
{
    public class UnitTestTokenService
    {
        [Fact]
        public void Test1_ValidateUserIdTrue()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims, "Bearer");

            int userId = 1;
            bool resultExpected = true;

            //ACT
            var result = services.ValidateUserId(identity, userId);

            //ASSERT
            Assert.Equal(resultExpected, result);

        }
        [Fact]
        public void Test1_ValidateUserIdFalse()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims, "Bearer");

            int userId = 2;
            bool resultExpected = false;

            //ACT
            var result = services.ValidateUserId(identity, userId);

            //ASSERT
            Assert.Equal(resultExpected, result);
        }
        [Fact]
        public void Test1_ValidateUserIdException()
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
        public void Test2_GetUserId()
        {
            //ARRANGE
            TokenServices services = new();
            var claims = new List<Claim> { new Claim("UserId", "1") };
            var identity = new ClaimsIdentity(claims, "Bearer");

            int userIdExpected = 1;

            //ACT
            int userIdResponse = services.GetUserId(identity);

            //ASSERT
            Assert.Equal(userIdExpected, userIdResponse);
        }
        
    }
}