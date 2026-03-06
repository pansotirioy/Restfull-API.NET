using Assignment.DTOs;
using Assignment.Models;
using Assignment.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AppPolicies.RequireUserRole)]
    public class PhotoIdController : ControllerBase
    {
        private readonly IPhotoIdRepository _repository;

        public PhotoIdController(IPhotoIdRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPgotoIds()
        {
            try
            {
                var photoId = await _repository.GetPhotoIdsAsync();
                return Ok(photoId.Adapt<List<PhotoIdDTO>>());
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
        public async Task<IActionResult> GetPhotoIdById([FromRoute] int id)
        {
            try
            {
                var result = await _repository.GetPhotoIdByIdAsync(id);
                return Ok(result.Adapt<PhotoIdDTO>());
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
        public async Task<IActionResult> PostPhotoId([FromBody] PhotoIdDTO photoIdDTO)
        {
            try
            {
                var photoId = photoIdDTO.Adapt<PhotoId>();

                int Id = await _repository.AddPhotoIdAsync(photoId);
                var resultDto = photoId.Adapt<PhotoIdDTO>();

                return CreatedAtAction(
                    nameof(GetPhotoIdById),
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutPhotoId([FromRoute] int id, [FromBody] PhotoIdDTO photoIdDTO)
        {
            try
            {
                var photoId = photoIdDTO.Adapt<PhotoId>();

                await _repository.UpdatePhotoIdAsync(id, photoId);
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePhotoId([FromRoute] int id)
        {
            try
            {
                await _repository.DeletePhotoIdAsync(id);
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

