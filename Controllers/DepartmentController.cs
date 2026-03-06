using Assignment.DTOs;
using Assignment.Models;
using Assignment.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Assignment.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDepartment()
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentAsync();
                return Ok(department.Adapt<List<DepartmentDTO>>());
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
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(id);
                return Ok(department.Adapt<DepartmentDTO>());
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

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            try
            {
                var department = departmentDTO.Adapt<Department>();
                int Id = await _departmentRepository.AddDepartmentAsync(department);
                var resultDto = department.Adapt<DepartmentDTO>();

                return CreatedAtAction(
                    nameof(GetDepartmentById),
                    new { Id },
                    resultDto
                );
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] DepartmentDTO departmentDTO)
        {
            try
            {
                var department = departmentDTO.Adapt<Department>();
                await _departmentRepository.UpdateDepartmentAsync(id, department);
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DepartmentAddress([FromRoute] int id)
        {
            try
            {
                await _departmentRepository.DeleteDepartmentAsync(id);
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
