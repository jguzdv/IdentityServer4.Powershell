using IdentityServer4.Powershell.Controllers;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Remove", Nouns.ClientNoun, ConfirmImpact = ConfirmImpact.High, SupportsShouldProcess = true)]
    public class RemoveClientCmdlet : EFCmdletBase
    {
        
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public string ClientId { get; set; }

        private bool _passThrough;

        public SwitchParameter PassThrough
        {
            get => _passThrough;
            set => _passThrough = value;
        }

        private ClientController _controller;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _controller = new ClientController(DbContext, this);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            _controller.DeleteClient(ClientId, _passThrough);
        }
    }
}
