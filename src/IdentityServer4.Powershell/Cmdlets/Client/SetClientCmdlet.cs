using IdentityServer4.Models;
using IdentityServer4.Powershell.Controllers;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Set", Nouns.ClientNoun, SupportsShouldProcess = true)]
    public class SetClientCmdlet : EFCmdletBase
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public Client Client { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string NewClientId { get; set; }

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

            _controller.UpdateClient(Client, NewClientId, _passThrough);
        }
    }
}
