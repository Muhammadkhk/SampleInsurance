using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ApplicationService.PersonDetail.Query
{
    public static class PersonDetailQueryContracts
    {
        public class GetAllPersonDetailsByPagination
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 25;
        }

    }
}
