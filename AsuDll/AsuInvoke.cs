using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AsuDll.AsuSdkHelper;

namespace AsuDll
{
    public class AsuInvoke
    {
        #region 获取当前PC检测到的AsuMotion数量，一般最先调用

        public static int Asu_GetDeviceNum()
        {
            int result = GetDeviceNum();

            return result;
        }

        #endregion

        #region 获取设备序列号信息，在获取设备数量 大于0 之后调用

        public static int Asu_GetDeviceInfo(int num, byte[] serial)
        {
            AsuMotionError result = GetDeviceInfo(num, serial);
            switch (result)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                case AsuMotionError.AsuMotion_True: return 3;
                default: return 4;  // AsuMotion_False
            }
        }

        #endregion
    }
}
