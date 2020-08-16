using System.Collections.Generic;
using Chris.Personnel.Management.Common.Enums;

namespace Chris.Personnel.Management.Common.DropDownListItems
{
    public class DropDownListItemsCreator
    {
        public static IEnumerable<GenderDropDownListItem> GetGenderDropDownListItems()
        {
            return new[]
            {
                new GenderDropDownListItem
                {
                    Key = ((int)Gender.Male).ToString(),
                    Value = "男"
                },
                new GenderDropDownListItem
                {
                    Key = ((int)Gender.Female).ToString(),
                    Value = "女"
                }
            };
        }

        public static IEnumerable<IsEnabledDropDownListItem> GetEnabledDropDownListItems()
        {
            return new[]
            {
                new IsEnabledDropDownListItem
                {
                    Key = ((int)IsEnabled.Enabled).ToString(),
                    Value = "启用"
                },
                new IsEnabledDropDownListItem
                {
                    Key = ((int)IsEnabled.Disabled).ToString(),
                    Value = "禁用"
                }
            };
        }
    }
}
