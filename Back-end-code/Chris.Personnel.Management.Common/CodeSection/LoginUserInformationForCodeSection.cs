using System;

namespace Chris.Personnel.Management.Common.CodeSection
{
    public class LoginUserInformationForCodeSection
    {
        public LoginUserInformationForCodeSection(Guid userId, string loginUser)
        {
            UserId = userId;
            LoginUser = loginUser;
        }
        public Guid UserId { get; set; }
        public string LoginUser { get; set; }
    }
}
