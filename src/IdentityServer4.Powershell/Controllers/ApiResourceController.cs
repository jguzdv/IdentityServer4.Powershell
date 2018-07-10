using System;
using System.Linq;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using IdentityServer4.Powershell.Cmdlets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace IdentityServer4.Powershell.Controllers
{
    class ApiResourceController : EFControllerBase
    {
        private Lazy<AutoMapper.IMapper> _lazyMapper;
        private AutoMapper.IMapper Mapper { get => _lazyMapper.Value; }

        internal ApiResourceController(IConfigurationDbContext dbContext, ICmdlet cmdlet)
            : base(dbContext, cmdlet)
        {
            _lazyMapper = new Lazy<AutoMapper.IMapper>(() => 
                new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                    .CreateMapper()
                , true);
        }

        internal ApiResource GetApiResource(string apiName)
        {
            var store = new ResourceStore(DbContext, NullLogger<ResourceStore>.Instance);
            var apiResource = store.FindApiResourceAsync(apiName).Result;

            Cmdlet.WriteObject(apiResource);
            return apiResource;
        }

        internal ApiResource[] GetApiResources(string[] apiScopes)
        {
            var store = new ResourceStore(DbContext, NullLogger<ResourceStore>.Instance);
            var apiResources = store.FindApiResourcesByScopeAsync(apiScopes).Result;

            var result = apiResources.ToArray();
            Cmdlet.WriteObject(result);

            return result;
        }

        internal ApiResource[] GetApiResources()
        {
            var store = new ResourceStore(DbContext, NullLogger<ResourceStore>.Instance);
            var resources = store.GetAllResourcesAsync().Result;

            var result = resources.ApiResources.ToArray();
            Cmdlet.WriteObject(result);

            return result;
        }

        internal void AddApiResource(ApiResource apiResource, bool passThrough)
        {
            if (Cmdlet.ShouldProcess(apiResource.Name, "Add"))
            {
                var resourceEntity = apiResource.ToEntity();
                DbContext.ApiResources.Add(resourceEntity);
                DbContext.SaveChanges();
            }

            if (passThrough)
                Cmdlet.WriteObject(apiResource);
        }

        internal void UpdateApiResource(ApiResource apiResource, string newResourceName, bool passThrough)
        {
            if (Cmdlet.ShouldProcess(apiResource.Name, "Set"))
            {
                var resourceEntity = DbContext.ApiResources
                    .Include(x => x.Secrets)
                    .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                    .Include(x => x.UserClaims)
                    .First(x => x.Name == apiResource.Name);
                
                Mapper.Map(apiResource, resourceEntity);
                if (!string.IsNullOrEmpty(newResourceName))
                    resourceEntity.Name = newResourceName;

                DbContext.SaveChanges();
            }

            if (passThrough)
                Cmdlet.WriteObject(apiResource);
        }

        internal void DeleteApiResource(string apiResourceName, bool passThrough)
        {
            var apiResource = DbContext.ApiResources.FirstOrDefault(x => x.Name == apiResourceName);
            if (apiResource == null)
                return;

            if (!Cmdlet.ShouldContinue($"Soll die API-Ressource ${apiResource.Name} ({apiResource.DisplayName}) wirklick gelöscht werden?", "Ressource löschen?"))
                return;

            if (passThrough)
                Cmdlet.WriteObject(apiResource.ToModel());

            if (Cmdlet.ShouldProcess(apiResource.Name, "Delete"))
            {
                DbContext.ApiResources.Remove(apiResource);
                DbContext.SaveChanges();
            }
        }
    }
}
