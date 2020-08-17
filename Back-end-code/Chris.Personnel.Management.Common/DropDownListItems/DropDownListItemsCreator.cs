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
                    Value = ((int)Gender.Male).ToString(),
                    Text = "男"
                },
                new GenderDropDownListItem
                {
                    Value = ((int)Gender.Female).ToString(),
                    Text = "女"
                }
            };
        }

        public static IEnumerable<IsEnabledDropDownListItem> GetEnabledDropDownListItems()
        {
            return new[]
            {
                new IsEnabledDropDownListItem
                {
                    Value = ((int)IsEnabled.Enabled).ToString(),
                    Text = "启用"
                },
                new IsEnabledDropDownListItem
                {
                    Value = ((int)IsEnabled.Disabled).ToString(),
                    Text = "禁用"
                }
            };
        }
    }
}
