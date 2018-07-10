using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System.Management.Automation;

namespace IdentityServer4.Powershell.Cmdlets
{
    public abstract class EFCmdletBase : PSCmdlet, ICmdlet
    {
        [Parameter(Mandatory = true)]
        public string ConnectionString { get; set; }
        
        [Parameter]
        public string DefaultSchema { get; set; } = "Conf";

        internal IConfigurationDbContext DbContext { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            //especially during testing there might already be an DbContext. If that's the case, we wont replace it.
            if (DbContext == null)
            {
                var dbCtxBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>()
                    .UseSqlServer(ConnectionString);

                DbContext = new ConfigurationDbContext(dbCtxBuilder.Options, new ConfigurationStoreOptions { DefaultSchema = DefaultSchema });
            }
        }

        //static EFCmdletBase()
        //{
        //    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve; ;
        //}

        //private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    var assemblies = AppDomain.CurrentDomain
        //        .GetAssemblies()
        //        .ToList();

        //    var resolvedAssembly = assemblies
        //        .FirstOrDefault(a => a.FullName == args.Name);

        //    if (resolvedAssembly != null)
        //        return resolvedAssembly;

        //    char[] separator = new[] { ',' };

        //    resolvedAssembly = assemblies
        //        .FirstOrDefault(a => args.Name.StartsWith(a.FullName.Split(separator, 2)[0] + separator));

        //    return resolvedAssembly;
        //}
    }
}
