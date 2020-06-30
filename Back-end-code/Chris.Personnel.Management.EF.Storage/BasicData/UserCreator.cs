using System;
using System.Collections.Generic;
using System.Text;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.EF.Storage.BasicData
{
    internal class UserCreator
    {
        public static User[] Create()
        {
            var user = new User
            {
                Name = "Admin",
                Password = "admin123",
                TrueName = "施晓勇",
                Gender = Gender.Male,
                CardId = "140226199401294051",
                Phone = "13259769759",
                IsEnabled = IsEnabled.Enabled,
                CreatedUserId = new Guid("32ec1e37-fe6d-4606-902e-6705beb0afc0"),
                CreatedTime = DateTime.Now,
            };

            return new[]
            {
                user
            };
        }
    }
}
