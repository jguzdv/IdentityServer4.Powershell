using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Powershell.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.Powershell.Tests.Controllers
{
    public class ApiResourceControllerTest : IClassFixture<DbContextFixture>
    {
        private DbContextFixture _dbContextFixture;

        public ApiResourceControllerTest(DbContextFixture fixture)
        {
            _dbContextFixture = fixture;
        }

        [Fact]
        public async Task Adds_to_database()
        {
            var cmdletMock = CmdletMock.CreateMock();
            var ctxName = Guid.NewGuid().ToString();

            var newApiResource = new Models.ApiResource
            {
                Name = "MyResource",
                Scopes = new Models.Scope[] {
                    new Models.Scope { Name = "Scope1" },
                    new Models.Scope { Name = "Scope2" }
                }
            };

            using(var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var controller = new ApiResourceController(ctx, cmdletMock.Object);
                controller.AddApiResource(newApiResource, true);
            }

            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var resources = ctx.ApiResources.ToList();
                Assert.Single(resources);

                var store = new ResourceStore(ctx, NullLogger<ResourceStore>.Instance);
                var resource = await store.FindApiResourceAsync("MyResource");

                Assert.NotNull(resource);
                Assert.Equal(2, resource.Scopes.Count);
            }

            cmdletMock.Verify(cmdlet => cmdlet.WriteObject(newApiResource), Times.Exactly(1));
        }

        [Fact]
        public async Task Updates_in_database()
        {
            var ctxName = Guid.NewGuid().ToString();
            var cmdletMock = CmdletMock.CreateMock();

            var newResource = new Models.ApiResource
            {
                Name = "MyResource",
                Scopes = new[] { new Models.Scope { Name = "Scope1" } }
            };
            
            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var controller = new ApiResourceController(ctx, cmdletMock.Object);
                controller.AddApiResource(newResource, false);
            }

            Models.ApiResource setResource;
            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var store = new ResourceStore(ctx, NullLogger<ResourceStore>.Instance);
                setResource = await store.FindApiResourceAsync("MyResource");
            }

            setResource.Scopes = new[] { new Models.Scope { Name = "Scope1" }, new Models.Scope { Name = "Scope2" } };
            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var controller = new ApiResourceController(ctx, cmdletMock.Object);
                controller.UpdateApiResource(setResource, "MyOtherResource", false);
            }

            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var store = new ResourceStore(ctx, NullLogger<ResourceStore>.Instance);
                var resource = await store.FindApiResourceAsync("MyOtherResource");

                Assert.NotNull(resource);
                Assert.Equal(2, resource.Scopes.Count);
            }
        }

        [Fact]
        public void Deletes_from_database()
        {
            Models.ApiResource passThrougResult = null;

            var cmdletMock = CmdletMock.CreateMock();
            cmdletMock.Setup(cmdlet => cmdlet.WriteObject(It.IsAny<Models.ApiResource>()))
                .Callback<object>(p => passThrougResult = p as Models.ApiResource);

            var ctxName = Guid.NewGuid().ToString();

            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                ctx.ApiResources.Add(new Models.ApiResource
                {
                    Name = "MyResource"
                }.ToEntity());
                ctx.SaveChanges();
            }

            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var controller = new ApiResourceController(ctx, cmdletMock.Object);
                controller.DeleteApiResource("MyResource", true);
            }

            using (var ctx = _dbContextFixture.GetContext(ctxName))
            {
                var resources = ctx.ApiResources.ToList();
                Assert.Empty(resources);

                Assert.Equal("MyResource", passThrougResult?.Name);
            }
        }
    }
}
