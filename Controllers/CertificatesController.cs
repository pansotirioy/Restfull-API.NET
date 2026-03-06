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
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificatesRepository _context;

        public CertificatesController(ICertificatesRepository context)
        {
            _context = context;
        }

        // GET: api/Certificates/Candidate/"CandidateId"
        [HttpGet("Candidate/{CandidateId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> GetAllCertificatesByCandidateIdAsync([FromRoute]string CandidateId)
        {
            try
            {
                var certificates = await _context.GetAllCertificatesByCandidateIdAsync(CandidateId);
                return Ok(certificates.Adapt<List<CertificateDTO>>());
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
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> GetCertificateById([FromRoute] int id)
        {
            try
            {
                var certificate = await _context.GetCertificateByIdAsync(id);
                return Ok(certificate.Adapt<CertificateDTO>());
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
        public async Task<IActionResult> PutCertificate([FromRoute]int id, [FromBody] CertificateDTO certificateDTO)
        {
            try
            {
                var certificate = certificateDTO.Adapt<Certificate>();

                await _context.UpdateCertificateAsync(id, certificate);

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
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> PostCertificate([FromBody] CertificateDTO certificateDTO)
        {
            try
            {
                var certificate = certificateDTO.Adapt<Certificate>();

                int Id = await _context.AddCertificateAsync(certificate);
                var resultDto = certificate.Adapt<CertificateDTO>();

                return CreatedAtAction(
                    nameof(GetCertificateById),
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


        // DELETE: api/certificates/id
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> DeleteCertificate([FromRoute] int id)
        {
            try
            {
                await _context.DeleteCertificateAsync(id);
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


        [HttpPut("{CId:int}/{CAId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> AddCertificateCandidateAnalitycsAsync([FromRoute] int CId, [FromRoute] int CAId)
        {
            try
            {
                await _context.AddCertificateCandidateAnalitycsAsync(CId, CAId);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("remove/{CId:int}/{CAId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> RemoveCandidatesCertificateAnalitycs([FromRoute] int CId, [FromRoute] int CAId)
        {
            try
            {
                await _context.RemoveCertificateCandidateAnalitycsAsync(CId, CAId);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("NotObtained/{candidateId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> NotObtainedCertificatesByCandidateIdAsync([FromRoute] string candidateId)
        {
            try
            {
                var output = await _context.NotObtainedCertificatesByCandidateIdAsync(candidateId);
                return Ok(output);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("MarksPerTopic/{candidateId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> MarksPerTopicPerCertificateAsync([FromRoute] string candidateId)
        {
            try
            {
                var output =  _context.MarksPerTopicPerCertificateAsync(candidateId);
                return Ok(output);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("CertificatesFromSale/{candidateId:guid}/{saleCertificateId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> AddCertificatesFromSaleAsync([FromRoute] string candidateId, [FromRoute] int saleCertificateId)
        {
            try
            {
                var output = _context.AddCertificatesFromSaleAsync(candidateId, saleCertificateId);
                return Ok(output);
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