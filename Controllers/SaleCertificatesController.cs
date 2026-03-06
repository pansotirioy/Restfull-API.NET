using Assignment.DTOs;
using Assignment.Models;
using Assignment.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleCertificatesController : ControllerBase
    {
        private readonly ISaleCertificatesRepository _context;

        public SaleCertificatesController(ISaleCertificatesRepository context)
        {
            _context = context;
        }

        // GET: api/SaleCertificates
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCertifiicate()
        {
            try
            {
                var SaleCertificates = await _context.GetSaleCertificatesAsync();
                return Ok(SaleCertificates.Adapt<List<SaleCertificateDTO>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSaleCertificateById([FromRoute] int id)
        {
            try
            {
                var SaleCertificate = await _context.GetSaleCertificateByIdAsync(id);
                return Ok(SaleCertificate.Adapt<SaleCertificateDTO>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> PutSaleCertificate([FromRoute]int id, [FromBody] SaleCertificateDTO SaleCertificateDTO)
        {
            try
            {
                var SaleCertificate = SaleCertificateDTO.Adapt<SaleCertificate>();

                await _context.UpdateSaleCertificateAsync(id, SaleCertificate);

                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> PostSaleCertificate([FromBody] SaleCertificateDTO SaleCertificateDTO)
        {
            try
            {
                var SaleCertificate = SaleCertificateDTO.Adapt<SaleCertificate>();

                int Id = await _context.AddSaleCertificateAsync(SaleCertificate);
                var resultDto = SaleCertificate.Adapt<SaleCertificateDTO>();

                return CreatedAtAction(
                    nameof(GetSaleCertificateById),
                    new { Id },
                    resultDto
                );
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/SaleCertificates/id
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> DeleteSaleCertificate([FromRoute] int id)
        {
            try
            {
                await _context.DeleteSaleCertificateAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
    }

}