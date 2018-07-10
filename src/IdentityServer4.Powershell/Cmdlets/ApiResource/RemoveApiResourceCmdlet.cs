using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;
using System.Linq;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Remove", Nouns.ApiResource, ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveApiResourceCmdlet : EFCmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string Name { get; set; }

        private bool _passThrough;
        public SwitchParameter PassThrough
        {
            get => _passThrough;
            set => _passThrough = value;
        }

        private ApiResourceController _controller;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _controller = new ApiResourceController(DbContext, this);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            _controller.DeleteApiResource(Name, _passThrough);
        }
    }
}
