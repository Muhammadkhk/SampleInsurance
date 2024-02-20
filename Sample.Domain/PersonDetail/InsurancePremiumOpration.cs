
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class InsurancePremiumOpration : Value<InsurancePremiumOpration>
    {

        public long Value { get; internal set; }
        protected InsurancePremiumOpration() { }
        public InsurancePremiumOpration(long num) {
            if (num < 5000 || num > 500000000)
            {
                throw new Exception("InsurancePremiumOpration Not Ok");
            }
            else
            {
                Value = num;
            }
             }


        public static InsurancePremiumOpration Fromlong(long num) =>
            new InsurancePremiumOpration(num);

        public static implicit operator long(InsurancePremiumOpration num) =>
            num.Value;


        public static InsurancePremiumOpration NoTitle =>
            new InsurancePremiumOpration();
    }
}
