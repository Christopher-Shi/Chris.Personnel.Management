using System;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Common.Enums;

namespace Chris.Personnel.Management.Entity
{
    public class User : EntityBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public IsEnabled IsEnabled { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreatedUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后编辑人
        /// </summary>
        public Guid? LastModifiedUserId { get; set; }

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }
    }
}
