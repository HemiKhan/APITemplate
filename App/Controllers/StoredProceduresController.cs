using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Services.StoredProcedures;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredProceduresController : ControllerBase
    {
        private readonly IStoredProcedureService storedProcedureService ;
        public StoredProceduresController(IStoredProcedureService storedProcedureService)
        {
            this.storedProcedureService = storedProcedureService;
        }


        [HttpGet("SPGetEmployee")]
        public async Task<IActionResult> SPGetEmployee()
        {
            if (ModelState.IsValid)
            {
                var result = await storedProcedureService.SPGetEmployeeAsync();
                if (result.Status)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("SPAddEmployee")]
        public async Task<IActionResult> SPAddEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                var result = await storedProcedureService.SPAddEmployeeAsync(model);
                if (result.Status)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPut("SPEditEmployee")]
        public async Task<IActionResult> SPEditEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                var result = await storedProcedureService.SPEditEmployeeAsync(model);
                if (result.Status)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpDelete("SPDeleteEmployee")]
        public async Task<IActionResult> SPDeleteEmployee(int EmpId)
        {
            var result = await storedProcedureService.SPDeleteByIdEmployeeAsync(EmpId);
            if (result.Status)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("SPGetEmployeeById")]
        public async Task<IActionResult> SPGetEmployeeById(int EmpId)
        {
            var result = await storedProcedureService.SPEmployeeGetByIdAsync(EmpId);
            if (result.Status)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
