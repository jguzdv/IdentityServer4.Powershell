using IdentityServer4.Powershell.Model;
using System;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    [Cmdlet("New", "SharedSecret")]
    public class NewSecretCmdlet : Cmdlet
    {
        [Parameter]
        public int Length { get; set; } = 48;

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public DateTime? Expiration { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            WriteObject(new PlainTextSecret(Length)
            {
                Description = Description,
                Expiration = Expiration
            });
        }
    }
}
