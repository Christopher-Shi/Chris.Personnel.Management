namespace Chris.Personnel.Management.Common
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Response { get; set; }
    }
}