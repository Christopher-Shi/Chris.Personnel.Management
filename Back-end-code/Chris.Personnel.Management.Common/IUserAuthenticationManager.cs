using Chris.Personnel.Management.Common.CodeSection;

namespace Chris.Personnel.Management.Common
{
    public interface IUserAuthenticationManager
    {
        LoginUserInformationForCodeSection CurrentUser { get; }
    }
}