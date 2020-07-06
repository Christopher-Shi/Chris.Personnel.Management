using Chris.Personnel.Management.Common.Enums;

namespace Chris.Personnel.Management.QueryService.Enums
{
    public static class EnumDescription
    {
        public static string GetEnabledDescription(this IsEnabled isEnabled)
        {
            switch (isEnabled)
            {
                case IsEnabled.Disabled:
                    return "停用";
                case IsEnabled.Enabled:
                    return "启用";
                default:
                    return "";
            }
        }

        public static string GetGenderDescription(this Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "女";
                case Gender.Male:
                    return "男";
                default:
                    return "";
            }
        }
    }
}
