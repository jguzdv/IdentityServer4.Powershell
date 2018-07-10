using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Powershell.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.Powershell.Tests.Controllers
{
    public class ClientControllerTests
    {
        [Fact]
        public void Constructs_CORS_Uris_FromHttpUrls()
        {
            var redirectUris = new[] { "https://www.uni-mainz.de/path/", "https://www.uni-mainz.de/path/and/path/", "http://www.uni-mainz.de", "http://www.zdv.uni-mainz.de/path", "something://urn" };
            var expectedCorsUris = new[] { "https://www.uni-mainz.de", "http://www.uni-mainz.de", "http://www.zdv.uni-mainz.de" };

            var corsUris = ClientController.BuildCorsUris(redirectUris);

            Assert.Equal(expectedCorsUris, corsUris);
        }

        public class AddClientTests : IClassFixture<DbContextFixture>
        {
            private DbContextFixture _dbContextFixture;

            public AddClientTests(DbContextFixture fixture)
            {
                _dbContextFixture = fixture;
            }

            [Fact]
            public async Task Adds_to_database()
            {
                var cmdletMock = CmdletMock.CreateMock();
                var ctxName = Guid.NewGuid().ToString();

                var newClient = new Models.Client
                {
                    ClientId = "MyClientId",
                    AllowedScopes = new[] { "Scope1" },
                    AllowedGrantTypes = new[] { "Grant1" },
                    AllowedCorsOrigins = new[] { "Origin1" },
                    Claims = new[] { new Claim("ClaimType1", "Claim1"), },
                    Properties = new Dictionary<string, string> { { "Key1", "Value1" } },
                    RedirectUris = new[] { "Redirect1" }
                };

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var controller = new ClientController(ctx, cmdletMock.Object);
                    controller.AddClient(newClient, false);
                }

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var clients = ctx.Clients.ToList();
                    Assert.Single(clients);

                    var store = new ClientStore(ctx, NullLogger<ClientStore>.Instance);
                    var client = await store.FindClientByIdAsync("MyClientId");

                    Assert.NotNull(client);
                    Assert.Equal(1, client.AllowedScopes.Count);
                    Assert.Equal(1, client.AllowedGrantTypes.Count);
                    Assert.Equal(1, client.AllowedCorsOrigins.Count);
                    Assert.Equal(1, client.Claims.Count);
                    Assert.Equal(1, client.Properties.Count);
                    Assert.Equal(1, client.RedirectUris.Count);
                }
            }

            [Fact]
            public void Add_passes_through()
            {
                var cmdletMock = CmdletMock.CreateMock();
                var ctxName = Guid.NewGuid().ToString();

                var newClient = new Models.Client
                {
                    ClientId = "MyClientId"
                };

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var controller = new ClientController(ctx, cmdletMock.Object);
                    controller.AddClient(newClient, true);
                }

                cmdletMock.Verify(cmdlet => cmdlet.WriteObject(newClient), Times.Exactly(1));
            }
        }

        public class UpdateClientTests : IClassFixture<DbContextFixture>
        {
            private DbContextFixture _dbContextFixture;

            public UpdateClientTests(DbContextFixture fixture)
            {
                _dbContextFixture = fixture;
            }

            [Fact]
            public async Task Updates_in_database()
            {
                var ctxName = Guid.NewGuid().ToString();
                var cmdletMock = CmdletMock.CreateMock();

                var setClient = new Models.Client
                {
                    ClientId = "MyClientId",
                    AllowedScopes = new List<string> { "Scope1" },
                    AllowedCorsOrigins = new List<string> { "Origin1" },
                    AllowedGrantTypes = new List<string> { "GrantType1" },
                    Claims = new List<Claim> { new Claim("ClaimType1", "Claim1"), },
                    Properties = new Dictionary<string, string> { { "Key1", "Value1" } },
                    RedirectUris = new List<string> { "Redirect1" }
                };

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    ctx.Clients.Add(setClient.ToEntity());
                    ctx.SaveChanges();
                }

                setClient.AllowedScopes = new List<string> { "Scope1", "Scope2" };
                setClient.AllowedCorsOrigins = new List<string>();
                setClient.AllowedGrantTypes = new List<string> { "GrantType1", "GrantType2" };
                setClient.Claims.Add(new Claim("ClaimType2", "Claim2"));
                setClient.Properties.Add("Key2", "Value2");
                setClient.RedirectUris = new List<string> { "Redirect2", "Redirect1" };

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var controller = new ClientController(ctx, cmdletMock.Object);
                    controller.UpdateClient(setClient, "MyNewClientId", true);
                }

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var store = new ClientStore(ctx, NullLogger<ClientStore>.Instance);
                    var client = await store.FindClientByIdAsync("MyNewClientId");

                    Assert.NotNull(client);
                    Assert.Equal(2, client.AllowedScopes.Count);
                    Assert.Equal(0, client.AllowedCorsOrigins.Count);
                    Assert.Equal(2, client.AllowedGrantTypes.Count);
                    Assert.Equal(2, client.Claims.Count);
                    Assert.Equal(2, client.Properties.Count);
                    Assert.Equal(2, client.RedirectUris.Count);
                }

                cmdletMock.Verify(cmdlet => cmdlet.WriteObject(setClient), Times.Exactly(1));
            }
        }

        public class DeleteClientTests : IClassFixture<DbContextFixture>
        {
            private DbContextFixture _dbContextFixture;

            public DeleteClientTests(DbContextFixture fixture)
            {
                _dbContextFixture = fixture;
            }

            [Fact]
            public void Deletes_from_database()
            {
                Models.Client passThrougResult = null;

                var cmdletMock = CmdletMock.CreateMock();
                cmdletMock.Setup(cmdlet => cmdlet.WriteObject(It.IsAny<Models.Client>()))
                    .Callback<object>(p => passThrougResult = p as Models.Client);

                var ctxName = Guid.NewGuid().ToString();

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    ctx.Clients.Add(new Models.Client
                    {
                        ClientId = "MyClientId",
                        AllowedScopes = new[] { "Scope1" }
                    }.ToEntity());
                    ctx.SaveChanges();
                }

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var controller = new ClientController(ctx, cmdletMock.Object);
                    controller.DeleteClient("MyClientId", false);
                }

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var clients = ctx.Clients.ToList();
                    Assert.Empty(clients);
                }
            }

            [Fact]
            public void Delete_passes_through()
            {
                Models.Client passThrougResult = null;

                var cmdletMock = CmdletMock.CreateMock();
                cmdletMock.Setup(cmdlet => cmdlet.WriteObject(It.IsAny<Models.Client>()))
                    .Callback<object>(p => passThrougResult = p as Models.Client);

                var ctxName = Guid.NewGuid().ToString();

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    ctx.Clients.Add(new Models.Client
                    {
                        ClientId = "MyClientId",
                        AllowedScopes = new[] { "Scope1" }
                    }.ToEntity());
                    ctx.SaveChanges();
                }

                using (var ctx = _dbContextFixture.GetContext(ctxName))
                {
                    var controller = new ClientController(ctx, cmdletMock.Object);
                    controller.DeleteClient("MyClientId", true);
                }

                Assert.Equal("MyClientId", passThrougResult?.ClientId);
            }
        }
    }
}
