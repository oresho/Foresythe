using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Entities;
using Entities.DTOs;
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
            var books = await _repositoryManager.bookRepository.GetAllBooksAsync();
            //var bookResponse = _mapper.Map<IEnumerable<BookOutputDto>>(books); didnt work

            return Ok(books);
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
            await _repositoryManager.bookRepository.CreateAsync(book);
            await _repositoryManager.SaveAsync();

            var bookResponse = _mapper.Map<BookOutputDto>(book);

            return CreatedAtAction("GetBook", new { BookId = book.Id}, bookResponse);
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
            var book = await _repositoryManager.bookRepository.GetBookAsync(BookId);
            _repositoryManager.bookRepository.Delete(book);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }

        #region Update
        //[HttpPut("{BookId}", Name = "UpdateBook")] //ISBN index causing issues with updates
        //public async Task<IActionResult> UpdateBook(Guid BookId, [FromBody] Book book1)
        //{
        //    var book = await _repositoryManager.bookRepository.GetBookAsync(BookId);
        //    _repositoryManager.bookRepository.Update(book1);
        //    await _repositoryManager.SaveAsync();

        //    return NoContent();
        //}
        #endregion


    }
}
