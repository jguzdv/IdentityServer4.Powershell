using IdentityServer4.EntityFramework.Interfaces;
using System;

namespace IdentityServer4.Powershell.Controllers
{
    class ControllerBase
    {
        protected Cmdlets.ICmdlet Cmdlet { get; set; }

        public ControllerBase(Cmdlets.ICmdlet cmdlet)
        {
            Cmdlet = cmdlet ?? throw new ArgumentNullException(nameof(cmdlet));
        }
    }

    class EFControllerBase : ControllerBase
    {
        protected IConfigurationDbContext DbContext { get; private set; }

        public EFControllerBase(IConfigurationDbContext dbContext, Cmdlets.ICmdlet cmdlet)
            :base(cmdlet)
        {
            DbContext = dbContext;
        }
    }
}
