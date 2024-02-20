using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.ApplicationService
{
    public interface IApplicationCommandService
    {
        Task<CommandResult> Handle(object command);
    }
}
