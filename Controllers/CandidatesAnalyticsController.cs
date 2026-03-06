using Assignment.DTOs;
using Assignment.Models;
using Assignment.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AppPolicies.RequireUserRole)]
    public class CandidatesAnalyticsController : ControllerBase
    {
        private readonly ICandidatesAnalyticsRepository _repository;

        public CandidatesAnalyticsController(ICandidatesAnalyticsRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var candidatesAnalytics = await _repository.GetCandidatesAnalyticsAsync();
                return Ok(candidatesAnalytics.Adapt<List<CandidatesAnalyticsDTO>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET BY ID
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            try
            {
                var candidatesAnalytics = await _repository.GetCandidatesAnalyticsByIdAsync(id);
                return Ok(candidatesAnalytics.Adapt<CandidatesAnalytics>());
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


        // POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CandidatesAnalyticsDTO analyticsDTO)
        {
            try
            {
                var analytics = analyticsDTO.Adapt<CandidatesAnalytics>();
                if (ModelState.IsValid)
                {
                    int Id = await _repository.AddCandidatesAnalyticsAsync(analytics);
                    var resultDto = analytics.Adapt<CandidatesAnalyticsDTO>();

                    return CreatedAtAction(
                        nameof(GetId),
                        new { Id },
                        resultDto
                    );
                }
                else
                {
                    return BadRequest(ModelState);
                }
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

        // PUT
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CandidatesAnalyticsDTO analyticsDTO)
        {
            try
            {
                var analytics = analyticsDTO.Adapt<CandidatesAnalytics>();
                await _repository.UpdateCandidatesAnalyticsAsync(id, analytics);
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

        // DELETE

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _repository.DeleteCandidatesAnalyticsAsync(id);
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
