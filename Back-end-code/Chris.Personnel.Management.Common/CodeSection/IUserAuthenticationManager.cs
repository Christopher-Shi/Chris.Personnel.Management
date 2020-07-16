namespace Chris.Personnel.Management.Common.CodeSection
{
    public interface IUserAuthenticationManager
    {
        LoginUserInformationForCodeSection CurrentUser { get; }
    }
}