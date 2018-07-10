using AutoMapper;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Set", Nouns.ApiResource, SupportsShouldProcess = true)]
    public class SetApiResourceCmdlet : EFCmdletBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public ApiResource ApiResource { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string NewName { get; set; }

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

            _controller.UpdateApiResource(ApiResource, NewName, _passThrough);
        }
    }
}
