using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.Domain
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}
