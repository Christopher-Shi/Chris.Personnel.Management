namespace Chris.Personnel.Management.Common
{
    public class ErrorMessage
    {
        #region 用户管理
        public const string UserNameIsNull = "用户名不能为空";
        public const string UserNameLengthError = "用户名不能超过50个字符";
        public const string TrueNameIsNull = "用户姓名不能为空";
        public const string TrueNameLengthError = "用户姓名不能超过50个字符";
        public const string CardIdIsNull = "用户身份证号不能为空";
        public const string CardIdLengthError = "用户身份证号长度为15或18";
        public const string PhoneIdIsNull = "联系电话不能为空";
        public const string PhoneIdLengthError = "联系电话格式错误";
        public const string LoginFailed = "登陆用户不存在或者密码错误";
        public const string OriginPasswordInvalidate = "原始密码输入错误";
        public const string ConfirmedNewPasswordError = "两次输入密码不一致";
        #endregion

        #region 角色管理
        public const string RoleNameIsNull = "角色名称不能为空";
        public const string RoleNameIsExisted = "角色名称已经存在";
        public const string RoleNameLengthError = "角色名称不能超过50个字符";
        public const string RoleMemoLengthError = "备注不能超过200个字符";
        public const string RoleFunctionNotExisted = "角色功能不存在";
        public const string RoleIdUsed = "当前角色已被使用";
        public const string RoleIsDeleted = "当前角色已被删除";
        #endregion

    }
}
