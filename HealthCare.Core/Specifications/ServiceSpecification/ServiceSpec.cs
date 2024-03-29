using HealthCare.Core.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Specifications.ServiceSpecification
{
    public class ServiceSpec : Specifications<Entities.Data.Services>
    {
        public ServiceSpec(ServiceSpecParams Params) : 
            base(p=>
            (string.IsNullOrEmpty(Params.name) || p.Name.ToLower().Contains(Params.name.ToLower()))
            &&
            (string.IsNullOrEmpty(Params.type) || p.Type.ToLower().Contains($"{Params.type.ToLower()[0]}"))
            &&
            (string.IsNullOrEmpty(Params.street) || p.Address.Street.ToLower().Contains(Params.street.ToLower()))
            &&
            (string.IsNullOrEmpty(Params.region) || p.Address.Region.ToLower().Contains(Params.region.ToLower()))
            &&
            (string.IsNullOrEmpty(Params.city) || p.Address.City.ToLower().Contains(Params.city.ToLower()))
            &&
            (string.IsNullOrEmpty(Params.country) || p.Address.Country.ToLower().Contains(Params.country.ToLower()))
            )
        {

            if (!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "NameAsc":
                         OrderBy = P => P.Name;
                        break;
                    case "NameDesc":
                        OrderByDesc = P => P.Name;
                        break;
                    case "CityAsc":
                         OrderBy = P => P.Address.City;
                        break;
                    case "CityDesc":
                        OrderByDesc = P => P.Address.City;
                        break;
                    default:
                        OrderBy = P => P.Name;
                        break;
                }
            }

            ApplyPagination(Params.PageSize * (Params.index - 1), Params.PageSize);

            CountEnable = true;
        }
    }
}
