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
    [Route("api/v1/Books")]
    public class BookController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookController(IRepositoryManager repositoryManager,
            ILoggerService logger,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repositoryManager.BookRepository.GetAllBooksAsync();
            var bookResponse = _mapper.Map<IEnumerable<BookOutputDto>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{BookId}")]
        [ServiceFilter(typeof(ValidateBookExistsAttribute))]
        public IActionResult GetBook(Guid BookId)
        {
            var book = HttpContext.Items["book"] as Book;
            var bookResponse = _mapper.Map<BookOutputDto>(book);

            return Ok(bookResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddBook([FromBody] BookInputDto bookInputDto)
        {
            var book = _mapper.Map<Book>(bookInputDto);
            await _repositoryManager.BookRepository.CreateAsync(book);
            await _repositoryManager.SaveAsync();

            var bookResponse = _mapper.Map<BookOutputDto>(book);

            return CreatedAtAction("GetBook", new { BookId = book.Id}, bookResponse);
        }        

        [HttpPost("AddToAuthor/{AuthorId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateAuthorExistsAttribute))]
        public async Task<IActionResult> AddBookToAuthor(Guid AuthorId, [FromBody] BookInputDto bookInputDto)
        {
            var book = _mapper.Map<Book>(bookInputDto);
            await _repositoryManager.BookRepository.CreateAsync(book);

            var author = HttpContext.Items["author"] as Author;
            author.Books.Add(book);

            await _repositoryManager.SaveAsync();
            
            var bookResponse = _mapper.Map<BookOutputDto>(book);

            return CreatedAtAction("GetBook", new { BookId = book.Id }, bookResponse);
        }


        [HttpPatch("{BookId}")]
        [ServiceFilter(typeof(ValidateBookExistsAttribute))]
        public async Task<IActionResult> UpdatePartialBook(Guid BookId, [FromBody] JsonPatchDocument<BookInputDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogError("patchDocument object sent from client is null.");
                return BadRequest("patchDocument object is null");
            }

            var book = HttpContext.Items["book"] as Book;
            var patchedBook = _mapper.Map<BookInputDto>(book);

            patchDocument.ApplyTo(patchedBook,ModelState);

            TryValidateModel(patchDocument);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(patchedBook, book);
            await _repositoryManager.SaveAsync();

            var bookResponse = _mapper.Map<BookOutputDto>(book);

            return Ok(bookResponse);
        }

        [HttpDelete("{BookId}")]
        [ServiceFilter(typeof(ValidateBookExistsAttribute))]
        public async Task<IActionResult> DeleteBook(Guid BookId)
        {
            var book = HttpContext.Items["book"] as Book;
            _repositoryManager.BookRepository.Delete(book);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}
