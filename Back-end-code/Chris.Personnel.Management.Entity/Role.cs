using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chris.Personnel.Management.Common.EntityModel;
using Chris.Personnel.Management.Common.Exceptions;

namespace Chris.Personnel.Management.Entity
{
    public class Role : Aggregate
    {
        public string Name { get; private set; }
        public string Memo { get; private set; }
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreatedUserId { get; set; }

        [NotMapped]
        public AggregateReference<User> CreatedUser => CreatedUserId;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后编辑人
        /// </summary>
        public Guid? LastModifiedUserId { get; set; }

        [NotMapped]
        public AggregateReference<User> LastModifiedUser => LastModifiedUserId;

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }

        public virtual ICollection<User> Users { get; private set; } = new List<User>();

        public static Role Create(string name, string memo, Guid createUserId, DateTime createTime)
        {
            ValidateFields(name, memo);

            return new Role
            {
                Name = name,
                Memo = memo,
                IsDeleted = false,
                CreatedUserId = createUserId,
                CreatedTime = createTime,
            };
        }

        public Role Edit(string name, string memo, Guid lastModifiedUserId, DateTime lastModifiedTime)
        {
            ValidateFields(name, memo);

            Name = name;
            Memo = memo;
            LastModifiedUserId = lastModifiedUserId;
            LastModifiedTime = lastModifiedTime;
            return this;
        }

        public Role LogicDelete(Guid lastModifiedUserId, DateTime lastModifiedTime)
        {
            IsDeleted = true;
            LastModifiedUserId = lastModifiedUserId;
            LastModifiedTime = lastModifiedTime;
            return this;
        }

        private static void ValidateFields(string name, string memo)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LogicException(ErrorMessage.RoleNameIsNull);
            }
            if (name.Length > 50)
            {
                throw new LogicException(ErrorMessage.RoleNameLengthError);
            }

            if (!string.IsNullOrEmpty(memo) && memo.Length > 200)
            {
                throw new LogicException(ErrorMessage.RoleMemoLengthError);
            }
        }
    }
}