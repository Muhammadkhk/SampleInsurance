using Sample.Domain.PersonDetail;
using Sample.Framework.ApplicationService;
using Sample.Framework.Infrastructure;
using Sample.Framework.Utils;
using Microsoft.AspNetCore.Http;
using Sample.ApplicationService.PersonDetailCommand.Command;

namespace Sample.ApplicationService.PersonDetail.Command
{
    public class PersonDetailCommandApplicationService : IApplicationCommandService
    {
        private readonly IPersonDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonDetailCommandApplicationService(IPersonDetailRepository repository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<CommandResult> Handle(object command) =>

            command switch
            {
                PersonDetailCommandContracts.Create cmd => HandleCreate(cmd),
                _ => Task.FromResult(new CommandResult(isSuccess: false, message: null, errorMessage: "UnsupPersonDetailed command"))
            };

        public async Task<CommandResult> HandleCreate(PersonDetailCommandContracts.Create command)
        {
            var Id = new PersonDetailId(Guid.NewGuid());
            var firstName = new FirstName(command.FirstName);
            var lastName = new LastName(command.LastName);
            var city = new City(command.City);
            var country = new Country(command.Country);
            InsurancePremiumDental? insurancePremiumDental = command.InsurancePremiumDental != null? new InsurancePremiumDental(command.InsurancePremiumDental??default) : null;
            InsurancePremiumHospitalization? insurancePremiumHospitalization = command.InsurancePremiumHospitalization != null ?  new InsurancePremiumHospitalization(command.InsurancePremiumHospitalization??default) : null;
            InsurancePremiumOpration? insurancePremiumOpration = command.InsurancePremiumOpration != null ?  new InsurancePremiumOpration(command.InsurancePremiumOpration??default) : null;
            var PersonDetail = new Domain.PersonDetail.PersonDetail(Id,firstName,lastName,country,city, insurancePremiumOpration, insurancePremiumHospitalization, insurancePremiumDental);
            await _repository.Add(PersonDetail);
            await _unitOfWork.Commit();
            return new CommandResult(isSuccess: true, message: "PersonDetail added successfully", errorMessage: null);
        }
        
    }
}
