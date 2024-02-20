using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.ApplicationService
{
    public interface IApplicationQueryService<T>
    {
        Task<QueryResult<T>> Handle(object query);
    }
}
