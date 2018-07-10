using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Add", Nouns.ApiResource, SupportsShouldProcess = true)]
    public class AddApiResourceCmdlet : EFCmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public ApiResource ApiResource { get; set; }

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

            _controller.AddApiResource(ApiResource, _passThrough);
        }
    }
}
