using System;
using System.Linq;
using Chris.Personnel.Management.Common.CodeSection;
using Microsoft.AspNetCore.Http;

namespace Chris.Personnel.Management.Common
{
    public class UserAuthenticationManager : IUserAuthenticationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private LoginUserInformationForCodeSection _currentUser;
        public UserAuthenticationManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public LoginUserInformationForCodeSection CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }

                if (_httpContextAccessor.HttpContext.User == null)
                {
                    throw new UnauthorizedException();
                }
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedException();
                }
                _currentUser = new LoginUserInformationForCodeSection(new Guid(userId), userName);

                return _currentUser;
            }
        }
    }
}
