
using Dapper;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Sample.Framework.ApplicationService;
using Sample.Core.Infrastructure;

namespace Sample.ApplicationService.PersonDetail.Query
{
    public static class Queries
    {
        private static int Offset(int page, int pageSize) => (page - 1) * pageSize;

        public static async Task<QueryResult<PersonDetailReadContracts.GetAllPersonDetailsByPagination>> Query(this SampleDbContext context, PersonDetailQueryContracts.GetAllPersonDetailsByPagination query)
        {

            IQueryable<Sample.Domain.PersonDetail.PersonDetail> rawQuery = context.PersonDetails
            .AsNoTracking()
             .Where(w => w.IsDeleted == false);

            var totalCount = await rawQuery.CountAsync();
            var dataRows = await rawQuery
               .Skip(Offset(query.Page, query.PageSize))
             .Take(query.PageSize)
              .OrderByDescending(s => s.Id)
              .Select(s =>
              new PersonDetailReadContracts.GetAllPersonDetailsByPagination
              {
                  Country = s.Country,
                  FirstName = s.FirstName,
                  City = s.City,
                  LastName = s.LastName,
                  InsurancePremiumOpration = s.InsurancePremiumOpration,
                  InsurancePremiumHospitalization = s.InsurancePremiumHospitalization,
                  InsurancePremiumDental = s.InsurancePremiumDental
              })
              .ToListAsync();

            return new QueryResult<PersonDetailReadContracts.GetAllPersonDetailsByPagination>(dataRows, dataRows.Count(), totalCount);

        }
    }
}



