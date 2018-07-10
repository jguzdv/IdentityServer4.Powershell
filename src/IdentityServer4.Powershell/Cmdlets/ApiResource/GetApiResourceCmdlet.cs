using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Powershell.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Get", Nouns.ApiResource)]
    public class GetApiResourceCmdlet : EFCmdletBase
    {
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ByName")]
        public string ApiName { get; set; }

        [Parameter(ParameterSetName = "ByScopes")]
        public string[] ApiScopes { get; set; }

        private ApiResourceController _controller;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _controller = new ApiResourceController(DbContext, this);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if(!string.IsNullOrEmpty(ApiName))
            {
                _controller.GetApiResource(ApiName);
            }
            else if (ApiScopes?.Any() ?? false)
            {
                _controller.GetApiResources(ApiScopes);
            }
            else
            {
                _controller.GetApiResources();
            }
        }
    }
}
