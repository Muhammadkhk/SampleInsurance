using Sample.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.PersonDetail
{
    public class PersonDetailId : Value<PersonDetailId>
    {
        public Guid Value { get; internal set; }

        public PersonDetailId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "PersonDetail id cannot be empty");

            Value = value;
        }
        protected PersonDetailId() { }

        public static implicit operator Guid(PersonDetailId self) => self.Value;

        public static implicit operator PersonDetailId(string value)
            => new PersonDetailId(Guid.Parse(value));

        public override string ToString() => Value.ToString();
    }
}
