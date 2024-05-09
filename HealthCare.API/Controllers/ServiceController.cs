using AutoMapper;
using HealthCare.Core;
using HealthCare.Core.Entities.Data;
using HealthCare.Core.Specifications.ServiceSpecification;
using HealthCare.PL.DTOs;
using HealthCare.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.PL.Controllers
{
    [Authorize]
    public class ServiceController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ServiceToReturnDto>>> GetServicAddress([FromQuery] ServiceSpecParams Params)
        {
            var Spec = new ServiceSpec(Params);
            var ServiceAddress = await _unitOfWork.CreateRepository<Services>().GetAllWithSpecAsync(Spec);
            var MappedService = _mapper.Map<IReadOnlyList<Services>, IReadOnlyList<ServiceToReturnDto>>(ServiceAddress);
            var Count = Spec.Count;
            return (new Pagination<ServiceToReturnDto>(Params.PageSize, Params.index, MappedService, Count));
        }
    }
}
