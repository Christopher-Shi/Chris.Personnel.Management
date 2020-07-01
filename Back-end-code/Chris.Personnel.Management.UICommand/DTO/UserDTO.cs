using Chris.Personnel.Management.Common.Enums;

namespace Chris.Personnel.Management.UICommand.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public string TrueName { get; set; }

        public Gender Gender { get; set; }

        public string CardId { get; set; }

        public string Phone { get; set; }

        public IsEnabled IsEnabled { get; set; }
    }
}