using IdentityServer4.Powershell.Controllers;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("Get", Nouns.ClientNoun, ConfirmImpact = ConfirmImpact.None)]
    public class GetClientCmdlet : EFCmdletBase
    {
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0)]
        public string ClientId { get; set; }

        private ClientController _controller;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _controller = new ClientController(DbContext, this);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (!string.IsNullOrEmpty(ClientId))
            {
                _controller.GetClient(ClientId);
            } else
            {
                _controller.GetClients();
            }
        }
    }
}
