
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class Country : Value<Country>
    {

        public string Value { get; internal set; }
        protected Country() { }
        public Country(string text) => Value = text;


        public static Country FromString(string text) =>
            new Country(text);

        public static implicit operator string(Country text) =>
            text.Value;


        public static Country NoTitle =>
            new Country();
    }
}
