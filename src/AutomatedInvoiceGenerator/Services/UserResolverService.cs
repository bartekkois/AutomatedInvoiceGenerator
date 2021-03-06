﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AutomatedInvoiceGenerator.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUserId()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch (System.NullReferenceException)
            {
                return string.Empty;
            }
        }
    }
}
