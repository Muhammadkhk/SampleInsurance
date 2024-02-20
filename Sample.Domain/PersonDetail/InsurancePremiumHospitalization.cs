
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class InsurancePremiumHospitalization : Value<InsurancePremiumHospitalization>
    {

        public long Value { get; internal set; }
        protected InsurancePremiumHospitalization() { }
        public InsurancePremiumHospitalization(long num) {
            if (num < 4000 || num > 400000000)
            {
                throw new Exception("InsurancePremiumHospitalization Not Ok");
            }
            else
            {
                Value = num;
            }
             }


        public static InsurancePremiumHospitalization Fromlong(long num) =>
            new InsurancePremiumHospitalization(num);

        public static implicit operator long(InsurancePremiumHospitalization num) =>
            num.Value;


        public static InsurancePremiumHospitalization NoTitle =>
            new InsurancePremiumHospitalization();
    }
}
