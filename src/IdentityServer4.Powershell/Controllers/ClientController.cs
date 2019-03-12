using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Powershell.Cmdlets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Powershell.Controllers
{
    class ClientController : EFControllerBase
    {
        private Lazy<AutoMapper.IMapper> _lazyMapper;
        private AutoMapper.IMapper Mapper { get => _lazyMapper.Value; }

        internal ClientController(IConfigurationDbContext dbContext, ICmdlet cmdlet) 
            : base(dbContext, cmdlet)
        {
            _lazyMapper = new Lazy<AutoMapper.IMapper>(() => CreateMapper(), true);
        }

        internal AutoMapper.IMapper CreateMapper()
        {
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>());
            //mapperConfig.FindTypeMapFor<>

            return mapperConfig.CreateMapper();
        }

        internal void GetClient(string clientId)
        {
            var store = new ClientStore(DbContext, NullLogger<ClientStore>.Instance);
            var client = store.FindClientByIdAsync(clientId).Result;

            Cmdlet.WriteObject(client);
        }

        internal void GetClients()
        {
            var clients = DbContext.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .AsEnumerable()
                .Select(c => c.ToModel())
                .ToArray();

            foreach(var client in clients)
                Cmdlet.WriteObject(client);
        }

        internal void AddClient(Models.Client client, bool passThrough)
        {
            if (Cmdlet.ShouldProcess(client.ClientName, "add"))
            {
                var clientEntity = client.ToEntity();
                DbContext.Clients.Add(clientEntity);
                DbContext.SaveChanges();
            }

            if (passThrough)
                Cmdlet.WriteObject(client);
        }

        internal void UpdateClient(Models.Client client, string newClientId, bool passThrough)
        {
            if (Cmdlet.ShouldProcess(client.ClientName, "Set"))
            {
                var clientEntity = DbContext.Clients
                    .Include(x => x.AllowedGrantTypes)
                    .Include(x => x.RedirectUris)
                    .Include(x => x.PostLogoutRedirectUris)
                    .Include(x => x.AllowedScopes)
                    .Include(x => x.ClientSecrets)
                    .Include(x => x.Claims)
                    .Include(x => x.IdentityProviderRestrictions)
                    .Include(x => x.AllowedCorsOrigins)
                    .Include(x => x.Properties)
                    .First(c => c.ClientId == client.ClientId);
                
                Mapper.Map(client, clientEntity);
                if (!string.IsNullOrEmpty(newClientId))
                    clientEntity.ClientId = newClientId;

                DbContext.SaveChanges();
            }

            if (passThrough)
                Cmdlet.WriteObject(client);
        }

        internal void DeleteClient(string clientId, bool passThrough)
        {
            var client = DbContext.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if (client == null)
                return;

            if (!Cmdlet.ShouldContinue($"Soll der Client ${client.ClientId} ({client.ClientName}) wirklick gelöscht werden?", "Client löschen?"))
                return;

            if (passThrough)
                Cmdlet.WriteObject(client.ToModel());

            if (Cmdlet.ShouldProcess(client.ClientName, "delete"))
            {
                DbContext.Clients.Remove(client);
                DbContext.SaveChanges();
            }
        }

        internal static ICollection<string> BuildCorsUris(string[] redirectUris)
        {
            return redirectUris
                .Where(redirectUri => System.Uri.IsWellFormedUriString(redirectUri, System.UriKind.Absolute))
                .Where(redirectUri =>
                    redirectUri.StartsWith("http:", System.StringComparison.OrdinalIgnoreCase) ||
                    redirectUri.StartsWith("https:", System.StringComparison.OrdinalIgnoreCase))
                .Select(redirectUri =>
                {
                    var uri = new System.Uri(redirectUri);
                    return $"{uri.Scheme}://{uri.Host}";
                })
                .Distinct()
                .ToList();
        }
    }
}
