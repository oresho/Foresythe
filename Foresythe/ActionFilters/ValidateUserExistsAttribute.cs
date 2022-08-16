using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Foresythe.ActionFilters
{
    public class ValidateUserExistsAttribute : IAsyncActionFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerService _logger;

        public ValidateUserExistsAttribute(ILoggerService logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var UserId = (Guid)context.ActionArguments["UserId"];
            var user = await _userManager.Users.Where(u => UserId.Equals(u.Id)).SingleOrDefaultAsync();

            if (user == null)
            {
                _logger.LogInfo($"User with id: {UserId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            else
            {
                context.HttpContext.Items.Add("user", user);
                await next();
            }
        }
    }
}