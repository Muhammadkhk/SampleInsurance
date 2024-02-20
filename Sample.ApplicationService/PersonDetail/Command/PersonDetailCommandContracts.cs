
using Sample.Domain.PersonDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ApplicationService.PersonDetailCommand.Command
{
    public static class PersonDetailCommandContracts
    {
        public class Create
        {
            public string FirstName { get;  set; }
            public string LastName { get;  set; }
            public string Country { get;  set; }
            public string City { get;  set; }
            public long? InsurancePremiumDental { get; set; }
            public long? InsurancePremiumHospitalization { get; set; }
            public long? InsurancePremiumOpration { get; set; }
        }
    }
}
