
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class City : Value<City>
    {

        public string Value { get; internal set; }
        protected City() { }
        public City(string text) => Value = text;


        public static City FromString(string text) =>
            new City(text);

        public static implicit operator string(City text) =>
            text.Value;


        public static City NoTitle =>
            new City();
    }
}
