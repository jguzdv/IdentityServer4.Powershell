using IdentityServer4.Powershell.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IdentityServer4.Powershell.Tests.Controllers
{
    public class SecretControllerTests
    {
        [Theory,
            InlineData(1),
            InlineData(10),
            InlineData(64),
            InlineData(512)]
        public void Generates_Key(int length)
        {
            var result = SecretController.GenerateKey(length);

            Assert.Equal(length, result.Length);
        }

        [Theory,
            InlineData(0),
            InlineData(-1)]
        public void Throws_for_invalid_length(int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretController.GenerateKey(length));
        }
    }
}
