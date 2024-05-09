using AutoMapper;
using HealthCare.Core.Entities;
using HealthCare.PL.DTOs;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.PL.Controllers
{

    public class HistoryController : APIBaseController
    {
        private readonly HealthCareContext _dbContext;
        private readonly IMapper _mapper;

        public HistoryController(HealthCareContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("AddHistory")]
        public async Task<ActionResult<HistoryToReturnDto>> AddHistory(HistoryDto model)
        {
            // Check If Hardware Is Used
            var hardware = await _dbContext.Hardware.FirstOrDefaultAsync(h => h.Id == model.HardwareId);
            if (hardware == null || hardware.IsUsed == false)
            {
                return BadRequest(new { message = "This Hardware Isn't Used!" });
            }

            // Get Ptient With This HardwareId
            var patient = await _dbContext.Patient.FirstOrDefaultAsync(p => p.HardwareId == model.HardwareId);
            if(patient == null)
            {
                return BadRequest(new { message = "This Hardware Isn't Used!" });
            }

            var history = _mapper.Map<History>(model);
            history.HistoryPatientId = patient.Id;
            history.HistoryDoctorId = patient.PatientDoctorId;

            _dbContext.UserHistory.Add(history);
            if(await _dbContext.SaveChangesAsync() > 0)
                return Ok(new { message = "Added Successfully!" });

            return BadRequest(new { message = "Failed To Add History!" });

        }
    }
}
