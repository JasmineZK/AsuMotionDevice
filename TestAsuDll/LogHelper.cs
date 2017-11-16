using log4net;

namespace TestAsuDll
{
    /// <summary>
    /// 帮助类 日志记录
    /// </summary>
    public class LogHelper
    {
        static ILog loger = LogManager.GetLogger( "Asu_Sdk" );

        public static void WriteLog(string info)
        {
            if (loger.IsInfoEnabled)
            {
                loger.Info( info );
            }
        }
    }
}
