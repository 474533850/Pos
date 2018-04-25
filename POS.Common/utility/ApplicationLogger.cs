using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.utility
{
    /// <summary>
    ///     LogManager 的摘要说明。
    /// </summary>
    public class ApplicationLogger
    {
        private static ILog log;

        /// <summary>
        ///     以LogName构造LogManager
        /// </summary>
        /// <param name="logname"></param>
        public ApplicationLogger(string logname)
        {
            log = log4net.LogManager.GetLogger(logname);
        }

        /// <summary>
        ///     以LogName构造LogManager
        /// </summary>
        /// <param name="logname"></param>
        public ApplicationLogger(Type type)
        {
            log = log4net.LogManager.GetLogger(type.Name);
        }

        #region Error

        /// <summary>
        ///     记录一个错误信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="e">异常对象</param>
        public void Error(string message, Exception e)
        {
            try
            {
                log.Error(message, e);
                if (e.InnerException != null)
                {
                    log.Error(e.InnerException.Message, e.InnerException);
                }
            }
            catch (Exception err)
            {
                throw new ApplicationException(String.Format("Logger.Error() failed on logging '{0}'.", message, err));
            }
        }

        /// <summary>
        ///     记录一个错误信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="e">异常对象</param>
        public void Error(Exception e)
        {
            try
            {
                log.Error(e.Message, e);
                if (e.InnerException != null)
                {
                    log.Error(e.InnerException.Message, e.InnerException);
                }
            }
            catch (Exception err)
            {
                throw new ApplicationException(String.Format("Logger.Error() failed on logging '{0}'.", e.Message, err));
            }
        }

        /// <summary>
        ///     记录一个错误信息
        /// </summary>
        /// <param name="message">信息内容</param>
        public void Error(string message)
        {
            try
            {
                log.Error(message);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Logger.Error() failed on logging '{0}'.", message, e));
            }
        }

        #endregion

        #region Warning

        /// <summary>
        ///     记录一个警告信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="exception">异常对象</param>
        public void Warning(string message, Exception exception)
        {
            try
            {
                log.Warn(message, exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    String.Format("Logger.Warn() failed on logging '{0}'.", message + "::" + exception.Message), e);
            }
        }

        /// <summary>
        ///     记录一个警告信息
        /// </summary>
        /// <param name="message">信息内容</param>
        public void Warning(string message)
        {
            try
            {
                log.Warn(message);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Logger.Warn() failed on logging '{0}'.", message), e);
            }
        }

        #endregion

        #region Fatal

        /// <summary>
        ///     记录一个程序致命性错误
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="exception">异常对象</param>
        public void Fatal(string message, Exception exception)
        {
            try
            {
                log.Fatal(message, exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    String.Format("Logger.Fatal() failed on logging '{0}'.", message + "::" + exception.Message), e);
            }
        }

        /// <summary>
        ///     记录一个程序致命性错误
        /// </summary>
        /// <param name="message">信息内容</param>
        public void Fatal(string message)
        {
            try
            {
                log.Fatal(message);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Logger.Fatal() failed on logging '{0}'.", message), e);
            }
        }

        #endregion

        #region Info

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="exception">异常对象</param>
        public void Info(string message, Exception exception)
        {
            try
            {
                log.Info(message, exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    String.Format("Logger.Info() failed on logging '{0}'.", message + "::" + exception.Message), e);
            }
        }

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="message">信息内容</param>
        public void Info(string message)
        {
            try
            {
                log.Info(message);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Logger.Info() failed on logging '{0}'.", message), e);
            }
        }

        #endregion

        #region Debug

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="exception">异常对象</param>
        public void Debug(string message, Exception exception)
        {
            try
            {
                log.Debug(message, exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    String.Format("Logger.Debug() failed on logging '{0}'.", message + "::" + exception.Message), e);
            }
        }

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="message">信息内容</param>
        public void Debug(string message)
        {
            try
            {
                log.Debug(message);
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format("Logger.Debug() failed on logging '{0}'.", message), e);
            }
        }

        #endregion
    }
}
