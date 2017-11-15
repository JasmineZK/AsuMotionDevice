using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AsuDll.AsuSdkHelper;

namespace AsuDll
{
    public class AsuInvoke
    {
        private static IntPtr handle; // 调用AsuMotionOpen()成功后获得的设备句柄


        /// <summary>
        /// 获取当前PC检测到的AsuMotion数量，一般最先调用
        /// 返回设备数
        /// </summary>
        /// <returns></returns>
        public static int Asu_GetDeviceNum()
        {
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
            AsuMotionError result = GetDeviceInfo(number, serial);
            switch (result)
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
    }
}
