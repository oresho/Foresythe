using System;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foresythe.ActionFilters
{
    public class ValidateBookExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;

        public ValidateBookExistsAttribute(IRepositoryManager repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var BookId = (Guid)context.ActionArguments["BookId"];
            var book = await _repository.bookRepository.GetBookAsync(BookId);

            if (book == null)
            {
                _logger.LogInfo($"Book with id: {BookId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            else
            {
                context.HttpContext.Items.Add("book", book);
                await next();
            }
        }
    }
}
