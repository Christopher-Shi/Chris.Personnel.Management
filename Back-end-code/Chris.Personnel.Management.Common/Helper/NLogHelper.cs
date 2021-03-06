﻿using System;
using NLog;
using NLog.Web;

namespace Chris.Personnel.Management.Common.Helper
{
    /// <summary>
    /// Nlog日志帮助类
    /// Trace 包含大量的信息，例如 protocol payloads。一般仅在开发环境中启用, 仅输出不存文件。
    /// Debug  比 Trance 级别稍微粗略，一般仅在开发环境中启用, 仅输出不存文件。
    /// Info 一般在生产环境中启用。
    /// Warn 一般用于可恢复或临时性错误的非关键问题。
    /// Error 一般是异常信息。
    /// Fatal - 非常严重的错误！
    /// </summary>
    public class NLogHelper
    {
        private readonly ILogger _logger;

        private NLogHelper(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 自定义 ${logger} (我用于区分文件夹)
        /// </summary>
        /// <param name="name"></param>
        public NLogHelper(string name) : this(NLogBuilder.ConfigureNLog("NLog.config").GetLogger(name))
        {
        }

        /// <summary>
        /// 默认 ${logger} (Default 文件夹下)
        /// </summary>
        public static NLogHelper Default { get; }
        static NLogHelper()
        {
            Default = new NLogHelper(NLogBuilder.ConfigureNLog("NLog.config").GetLogger("Default"));
        }

        public static NLogHelper GetNLogHelper()
        {
            return Default;
        }

        public void Debug(string msg, params object[] args)
        {
            _logger.Debug(msg, args);
        }
        public void Debug(string msg, Exception err)
        {
            _logger.Debug(err, msg);
        }

        public void Info(string msg, params object[] args)
        {
            _logger.Info(msg, args);
        }

        public void Info(string msg, Exception err)
        {
            _logger.Info(err, msg);
        }

        public void Trace(string msg, params object[] args)
        {
            _logger.Trace(msg, args);
        }

        public void Trace(string msg, Exception err)
        {
            _logger.Trace(err, msg);
        }

        public void Error(string msg, params object[] args)
        {
            _logger.Error(msg, args);
        }

        public void Error(string msg, Exception err)
        {
            _logger.Error(err, msg);
        }

        public void Fatal(string msg, params object[] args)
        {
            _logger.Fatal(msg, args);
        }

        public void Fatal(string msg, Exception err)
        {
            _logger.Fatal(err, msg);
        }
    }
}
