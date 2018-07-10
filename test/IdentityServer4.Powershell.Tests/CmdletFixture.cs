using IdentityServer4.Powershell.Cmdlets;
using Moq;
using System;

namespace IdentityServer4.Powershell.Tests
{
    static class CmdletMock
    {
        internal static Mock<ICmdlet> CreateMock()
        {
            var mock = new Mock<ICmdlet>();
            mock.Setup(cmdlet => cmdlet.ShouldContinue(It.IsAny<string>(), It.IsAny<String>()))
                .Returns(true);
            mock.Setup(cmdlet => cmdlet.ShouldProcess(It.IsAny<string>(), It.IsAny<String>()))
                .Returns(true);

            return mock;
        }
    }
}
