using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;
using Foresythe.ActionFilters;
using Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Foresythe.Controllers
{
    [Route("api/v1/Authors")]
    public class AuthorController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorController(IRepositoryManager repositoryManager,
            ILoggerService logger,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _repositoryManager.AuthorRepository.GetAllAuthorsAsync();
            var authorResponse = _mapper.Map<IEnumerable<AuthorOutputDto>>(authors);

            return Ok(authorResponse);
        }

        [HttpGet("{AuthorId}")]
        [ServiceFilter(typeof(ValidateAuthorExistsAttribute))]
        public IActionResult GetAuthor(Guid AuthorId)
        {
            var author = HttpContext.Items["author"] as Author;
            var authorResponse = _mapper.Map<AuthorOutputDto>(author);

            return Ok(authorResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorInputDto authorInputDto)
        {
            var author = _mapper.Map<Author>(authorInputDto);
            await _repositoryManager.AuthorRepository.CreateAsync(author);
            await _repositoryManager.SaveAsync();

            var authorResponse = _mapper.Map<AuthorOutputDto>(author);

            return CreatedAtAction("GetAuthor", new { AuthorId = author.Id }, authorResponse);
        }

        [HttpPatch("{AuthorId}")]
        [ServiceFilter(typeof(ValidateAuthorExistsAttribute))]
        public async Task<IActionResult> UpdatePartialAuthor(Guid AuthorId, [FromBody] JsonPatchDocument<AuthorInputDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogError("patchDocument object sent from client is null.");
                return BadRequest("patchDocument object is null");
            }

            var author = HttpContext.Items["author"] as Author;
            var patchedAuthor = _mapper.Map<AuthorInputDto>(author);

            patchDocument.ApplyTo(patchedAuthor, ModelState);

            TryValidateModel(patchDocument);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(patchedAuthor, author);
            await _repositoryManager.SaveAsync();

            var authorResponse = _mapper.Map<AuthorOutputDto>(author);

            return Ok(authorResponse);
        }

        [HttpDelete("{AuthorId}")]
        [ServiceFilter(typeof(ValidateAuthorExistsAttribute))]
        public async Task<IActionResult> DeleteAuthor(Guid AuthorId)
        {
            var author = HttpContext.Items["author"] as Author;
            _repositoryManager.AuthorRepository.Delete(author);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
