using System;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foresythe.ActionFilters
{
    public class ValidateAuthorExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;

        public ValidateAuthorExistsAttribute(IRepositoryManager repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var AuthorId = (Guid)context.ActionArguments["AuthorId"];
            var author = await _repository.AuthorRepository.GetAuthorAsync(AuthorId);

            if (author == null)
            {
                _logger.LogInfo($"Author with id: {AuthorId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            else
            {
                context.HttpContext.Items.Add("author", author);
                await next();
            }
        }
    }
}
