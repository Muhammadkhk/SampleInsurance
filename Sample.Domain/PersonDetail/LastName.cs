
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class LastName : Value<LastName>
    {

        public string Value { get; internal set; }
        protected LastName() { }
        public LastName(string text) => Value = text;


        public static LastName FromString(string text) =>
            new LastName(text);

        public static implicit operator string(LastName text) =>
            text.Value;


        public static LastName NoTitle =>
            new LastName();
    }
}
