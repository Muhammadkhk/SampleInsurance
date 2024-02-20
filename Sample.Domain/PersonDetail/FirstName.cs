
using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class FirstName : Value<FirstName>
    {

        public string Value { get; internal set; }
        protected FirstName() { }
        public FirstName(string text) => Value = text;


        public static FirstName FromString(string text) =>
            new FirstName(text);

        public static implicit operator string(FirstName text) =>
            text.Value;


        public static FirstName NoTitle =>
            new FirstName();
    }
}
