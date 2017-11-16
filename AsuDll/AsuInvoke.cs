using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net.Config;
using static AsuDll.AsuSdkHelper;

namespace AsuDll
{
    public class AsuInvoke
    {
        private static IntPtr handle; // 调用AsuMotionOpen()成功后获得的设备句柄

        /// 初始化 Log4net
        private static void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        /// <summary>
        /// 获取当前PC检测到的AsuMotion数量，一般最先调用
        /// 返回设备数
        /// </summary>
        /// <returns></returns>
        public static int Asu_GetDeviceNum()
        {
            InitLog4net();

            int result = GetDeviceNum();

            return result;
        }

        /// <summary>
        /// 获取设备序列号信息，在获取设备数量 大于0 之后调用
        /// </summary>
        /// <param name="number"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static int Asu_GetDeviceInfo(int number, byte[] serial)
        {
            AsuMotionError ret = GetDeviceInfo(number, serial);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                case AsuMotionError.AsuMotion_True: return 3;
                default: return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 打开设备
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Asu_AsuMotionOpen(int number)
        {
            handle = AsuMotionOpen(number);
            if (handle != IntPtr.Zero)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 关闭已打开的设备
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        public static int Asu_AsuMotionClose()
        {
            AsuMotionError ret = AsuMotionClose(handle);
            if (ret == AsuMotionError.AsuMotion_True)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 初始化设备为默认
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionConfigDeviceDefault()
        {
            AsuMotionError ret = AsuMotionConfigDeviceDefault(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }
    }
}
