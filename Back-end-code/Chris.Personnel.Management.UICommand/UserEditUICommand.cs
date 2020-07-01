using System;
using Chris.Personnel.Management.UICommand.DTO;

namespace Chris.Personnel.Management.UICommand
{
    public class UserEditUICommand
    {
        public Guid Id { get; set; }

        public UserDTO User { get; set; }
    }
}