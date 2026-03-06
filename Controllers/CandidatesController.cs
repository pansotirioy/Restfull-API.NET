using Assignment.DTOs;
using Assignment.Models;
using Assignment.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidatesRepository _candidatesRepository;

        public CandidatesController(ICandidatesRepository candidatesRepository)
        {
            _candidatesRepository = candidatesRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var candidates = await _candidatesRepository.GetCandidatesAsync();
                return Ok(candidates.Adapt<List<CandidateRUDDTO>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> GetCandidateById([FromRoute] string id)
        {
            try
            {
                var candidate = await _candidatesRepository.GetCandidateByIdAsync(id);
                return Ok(candidate.Adapt<CandidateRUDDTO>());
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

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> GetCandidateByUserName([FromRoute] string username)
        {
            try
            {
                var candidate = await _candidatesRepository.GetCandidateByUserNameAsync(username);
                return Ok(candidate.Adapt<CandidateRUDDTO>());
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

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> UpdateCandidate([FromRoute] string id, [FromBody] CandidateRUDDTO candidateDTO)
        {
            try
            {
                var candidate = candidateDTO.Adapt<Candidate>();
                await _candidatesRepository.UpdateCandidateAsync(id, candidate);
                return NoContent();
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
    }
}