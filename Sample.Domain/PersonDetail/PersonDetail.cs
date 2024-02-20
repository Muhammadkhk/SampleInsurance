using Sample.Framework.Domain;

namespace Sample.Domain.PersonDetail
{
    public class PersonDetail : AggregateRoot<PersonDetailId>
    {
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Country Country { get; private set; }
        public City City { get; private set; }
        public InsurancePremiumDental? InsurancePremiumDental { get; private set; }
        public InsurancePremiumHospitalization? InsurancePremiumHospitalization { get; private set; }
        public InsurancePremiumOpration? InsurancePremiumOpration { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public PersonDetail(PersonDetailId PersonDetailId, FirstName firstName, LastName lastName, Country country, City city, InsurancePremiumOpration? insurancePremiumOpration, InsurancePremiumHospitalization? insurancePremiumHospitalization, InsurancePremiumDental? insurancePremiumDental)
        {
            Id = PersonDetailId;
            LastName = lastName;
            FirstName = firstName;
            Country = country;
            City = city;
            InsurancePremiumDental = insurancePremiumDental;
            InsurancePremiumHospitalization = insurancePremiumHospitalization;
            InsurancePremiumOpration = insurancePremiumOpration;
        }

        protected PersonDetail() { }
        protected override void EnsureValidState()
        {
            throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        } 
    }
}
