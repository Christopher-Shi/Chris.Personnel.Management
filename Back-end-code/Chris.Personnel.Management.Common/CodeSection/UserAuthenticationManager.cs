using System;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Http;

namespace Chris.Personnel.Management.Common.CodeSection
{
    public class UserAuthenticationManager : IUserAuthenticationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static LoginUserInformationForCodeSection _currentUser;

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

                var jwtStr = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                    .Replace("Bearer ", "");

                if (jwtStr.IsNullOrEmpty())
                {
                    throw new UnauthorizedException();
                }
                var tokenModelJwt = JwtHelper.SerializeJwt(jwtStr);
                var userId = tokenModelJwt.Uid;
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
