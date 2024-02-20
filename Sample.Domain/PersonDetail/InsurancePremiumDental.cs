
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class InsurancePremiumDental : Value<InsurancePremiumDental>
    {

        public long Value { get; internal set; }
        protected InsurancePremiumDental() { }
        public InsurancePremiumDental(long num) {
            if (num < 2000 || num > 200000000)
            {
                throw new Exception("InsurancePremiumDental Not Ok");
            }
            else
            {
                Value = num;
            }
             }


        public static InsurancePremiumDental Fromlong(long num) =>
            new InsurancePremiumDental(num);

        public static implicit operator long(InsurancePremiumDental num) =>
            num.Value;


        public static InsurancePremiumDental NoTitle =>
            new InsurancePremiumDental();
    }
}
