using IdentityServer4.Models;
using System.Linq;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("New", Nouns.ApiResource)]
    public class NewApiResourceCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Scope[] Scopes { get; set; } = new Scope[0];

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string[] UserClaims { get; set; } = new string[0];

        
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var apiResource = new ApiResource(Name)
            {
                Scopes = Scopes.ToList(),
                UserClaims = UserClaims.ToList()
            };

            if(!apiResource.Scopes.Any())
            {
                apiResource.Scopes.Add(new Scope(Name));
            }

            WriteObject(apiResource);
        }
    }
}
