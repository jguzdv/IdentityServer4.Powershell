namespace IdentityServer4.Powershell.Cmdlets
{
    public interface ICmdlet
    {
        bool ShouldContinue(string query, string caption);
        bool ShouldProcess(string target, string action);

        void WriteObject(object sendToPipeline);
    }
}
