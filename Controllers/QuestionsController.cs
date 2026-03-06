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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _repository;

        public QuestionsController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> GetQuestions()
        {
            try
            {
                var questions = await _repository.GetQuestionsAsync();
                return Ok(questions.Adapt<List<QuestionDTO>>());
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
        public async Task<IActionResult> GetQuestionsById([FromRoute] int id)
        {
            try
            {
                var result = await _repository.GetQuestionsByIdAsync(id);
                return Ok(result.Adapt<QuestionDTO>());
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
        public async Task<IActionResult> PostQuestions([FromBody] QuestionDTO QuestionDTO)
        {
                var question = QuestionDTO.Adapt<Question>();

                int Id = await _repository.AddQuestionsAsync(question);
                var resultDto = question.Adapt<QuestionDTO>();

                return CreatedAtAction(
                    nameof(GetQuestionsById),
                    new { Id },
                    resultDto
                );

            
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> PutQuestions([FromRoute] int id, [FromBody] QuestionDTO questionDTO)
        {
            try
            {
                var question = questionDTO.Adapt<Question>();

                await _repository.UpdateQuestionsAsync(id, question);
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
        [Authorize(Policy = AppPolicies.RequireAdministratorRole)]
        public async Task<IActionResult> DeleteQuestions([FromRoute] int id)
        {
            try
            {
                await _repository.DeleteQuestionsAsync(id);
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


        [HttpGet("Random/{number:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = AppPolicies.RequireUserRole)]
        public async Task<IActionResult> GetRandomQuestions(int number)
        {
            try
            {
                var questions = await _repository.GetRandomQuestionsAsync(number);
                return Ok(questions.Adapt<List<QuestionDTO>>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

