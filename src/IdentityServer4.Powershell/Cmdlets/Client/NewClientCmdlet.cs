using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("New", Nouns.ClientNoun, ConfirmImpact = ConfirmImpact.None)]
    public class NewClientCmdlet : Cmdlet
    {
        private bool _allowAccessTokensViaBrowser;

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string ClientId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 1)]
        public string ClientName { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string[] RedirectUris { get; set; } = new string[0];

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string[] AllowedScopes { get; set; } = new string[0];

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string[] AllowedGrantTypes { get; set; } = new string[0];

        public SwitchParameter AllowAccessTokensViaBrowser
        {
            get => _allowAccessTokensViaBrowser;
            set => _allowAccessTokensViaBrowser = value;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var client = new Client
            {
                ClientId = ClientId,
                ClientName = ClientName,

                RedirectUris = RedirectUris.ToList(),
                AllowedScopes = AllowedScopes.ToList(),
                AllowedGrantTypes = AllowedGrantTypes.ToList(),

                AllowAccessTokensViaBrowser = _allowAccessTokensViaBrowser,
            };
            client.AllowedCorsOrigins = ClientController.BuildCorsUris(RedirectUris);

            WriteObject(client);
        }
    }
}
