using IdentityServer4.Powershell.Controllers;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Add", Nouns.ClientNoun, SupportsShouldProcess = true)]
    public class AddClientCmdlet : EFCmdletBase
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public Models.Client Client { get; set; }

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

            _controller.AddClient(Client, _passThrough);
        }
    }
}
