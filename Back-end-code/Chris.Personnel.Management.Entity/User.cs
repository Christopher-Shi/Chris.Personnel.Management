using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chris.Personnel.Management.Common.EntityModel;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.Common.Exceptions;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;

namespace Chris.Personnel.Management.Entity
{
    public class User : Aggregate
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        public string Salt { get; private set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string CardId { get; private set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public IsEnabled IsEnabled { get; private set; }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        public Guid? RoleId { get; private set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatedUserId { get; private set; }

        [NotMapped]
        public AggregateReference<User> CreatedUser => CreatedUserId;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }

        /// <summary>
        /// 最后编辑人
        /// </summary>
        public Guid? LastModifiedUserId { get; private set; }

        [NotMapped]
        public AggregateReference<User> LastModifiedUser => LastModifiedUserId;

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime? LastModifiedTime { get; private set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual Role Role { get; private set; }

        public static User Create(
            string name,
            string password,
            string trueName,
            Gender gender,
            string cardId,
            string phone,
            Guid? roleId,
            Guid? createdUserId,
            DateTime createdTime)
        {
            ValidateFields(name, trueName, cardId, phone);
            var hashedPassword = PasswordHasher.HashedPassword(password);
            return new User
            {
                Name = name,
                Password = hashedPassword.Hash,
                Salt = hashedPassword.Salt,
                TrueName = trueName,
                Gender = gender,
                CardId = cardId,
                Phone = phone,
                RoleId = roleId,
                IsEnabled = IsEnabled.Enabled,
                CreatedUserId = createdUserId,
                CreatedTime = createdTime
            };
        }

        public User Edit(
            string name,
            string trueName,
            IsEnabled isEnabled,
            Gender gender,
            string cardId,
            string phoneId,
            Guid? roleId,
            Guid lastModifiedUserId,
            DateTime lastModifiedTime)
        {
            ValidateFields(name, trueName, cardId, phoneId);
            Name = name;
            TrueName = trueName;
            Gender = gender;
            CardId = cardId;
            Phone = phoneId;
            RoleId = roleId;
            IsEnabled = isEnabled;
            LastModifiedUserId = lastModifiedUserId;
            LastModifiedTime = lastModifiedTime;
            return this;
        }

        public User EditPassword(
            string salt,
            string hashedPassword,
            Guid lastModifiedUserId,
            DateTime lastModifiedTime)
        {
            Salt = salt;
            Password = hashedPassword;
            LastModifiedUserId = lastModifiedUserId;
            LastModifiedTime = lastModifiedTime;
            return this;
        }

        public User StopUsing(Guid lastModifiedUserId, DateTime lastModifiedTime)
        {
            IsEnabled = IsEnabled.Disabled;
            LastModifiedUserId = lastModifiedUserId;
            LastModifiedTime = lastModifiedTime;
            return this;
        }

        private static void ValidateFields(string name, string trueName, string cardId, string phone)
        {
            if (name.IsNullOrEmpty())
            {
                throw new LogicException(ErrorMessage.UserNameIsNull);
            }
            if (name.Length > 100)
            {
                throw new LogicException(ErrorMessage.UserNameLengthError);
            }
            if (trueName.IsNullOrEmpty())
            {
                throw new LogicException(ErrorMessage.TrueNameIsNull);
            }
            if (trueName.Length > 100)
            {
                throw new LogicException(ErrorMessage.TrueNameLengthError);
            }
            if (cardId.IsNullOrEmpty())
            {
                throw new LogicException(ErrorMessage.CardIdIsNull);
            }
            if (cardId.Length != 15 && cardId.Length != 18)
            {
                throw new LogicException(ErrorMessage.CardIdLengthError);
            }
            if (phone.IsNullOrEmpty())
            {
                throw new LogicException(ErrorMessage.PhoneIdIsNull);
            }
            if (!phone.IsTelephone() && !phone.IsCellphone())
            {
                throw new LogicException(ErrorMessage.PhoneIdLengthError);
            }
        }
    }
}
