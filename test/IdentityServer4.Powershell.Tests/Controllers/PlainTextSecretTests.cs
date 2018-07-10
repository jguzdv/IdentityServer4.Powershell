using IdentityServer4.Powershell.Model;
using Xunit;

namespace IdentityServer4.Powershell.Tests.Controllers
{
    public class PlainTextSecretTests
    {
        [Fact]
        public void Ctor_CreatesValidSecret()
        {
            var sut = new PlainTextSecret();

            Assert.True(sut.PlainText?.Length > 16); //Assert the secret has a good default length
            Assert.Equal(44, sut.Value?.Length); //Assert the Hash has been set and is 44 chars long (base64 of Hash).
        }

        [Fact]
        public void Value_IsSha256Hash()
        {
            var sut = new PlainTextSecret
            {
                PlainText = "Hello World!"
            };

            Assert.Equal("f4OxZX/x/FO5LcGBSKHWXfwtSx+j1ncoSt3SABJtkGk=", sut.Value, ignoreCase: true);
        }
    }
}
