﻿using System;
using System.ComponentModel;

namespace Chris.Personnel.Management.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return customAttribute == null ? val.ToString() : ((DescriptionAttribute) customAttribute).Description;
        }
    }
}
