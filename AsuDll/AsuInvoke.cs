using System;
using System.IO;
using System.Text;
using log4net.Config;
using static AsuDll.AsuSdkHelper;

namespace AsuDll
{
    public class AsuInvoke
    {
        /// <summary>
        /// 静态构造函数，只在调用此类的成员或属性时 执行一次
        /// 用于初始化 Log4net
        /// </summary>
        static AsuInvoke()
        {
            InitLog4net();
        }

        private static int NumOfCurDevice = 0; // 当前设备序号

        #region 初始化Log4net 及 获取错误信息

        /// <summary>
        /// 初始化 Log4net
        /// </summary>
        private static void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net_sdk.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string AsuMotion_GetErrorMessage(int errorCode)
        {
            return AsuMotionGetErrorMessage((AsuMotionError)errorCode);
        }

        #endregion

        #region 接口列表

        #region AsuMotion控制卡设备操作函数

        /// <summary>
        /// 获取设备数量，一般最先调用
        /// 返回 与PC相连的设备总数
        /// </summary>
        /// <returns></returns>
        public static int Asu_GetDeviceNum()
        {
            int count = GetDeviceNum();
            LogHelper.WriteLog("有 " + count + " 台设备连接");

            return count;
        }

        /// <summary>
        /// 获取设备序列号信息
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="numberOfDevice">设备序号，从 1 开始</param>
        /// <param name="serialOfDevice">用于存储序列号的字节数组</param>
        /// <returns></returns>
        public static int Asu_GetDeviceInfo(int numberOfDevice, out byte[] serialOfDevice)
        {
            NumOfCurDevice = numberOfDevice;
            AsuMotionError ret = GetDeviceInfo(numberOfDevice - 1, out serialOfDevice);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("获取设备 " + numberOfDevice + " 信息 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("获取设备 " + numberOfDevice + " 信息 失败" + "---" +AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 打开设备
        /// 返回有效的设备句柄 成功；
        /// 返回空指针 失败
        /// </summary>
        /// <param name="numberOfDevice">设备序号，从 1 开始</param>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static IntPtr AsuMotion_Open(int numberOfDevice)
        {
            IntPtr handle = AsuMotionOpen(numberOfDevice - 1);
            if (handle != IntPtr.Zero)
            {
                LogHelper.WriteLog("打开设备 " + numberOfDevice + " 成功");
                return handle;
            }
            else
            {
                LogHelper.WriteLog("打开设备 " + numberOfDevice + " 失败");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// 关闭设备
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_Close(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionClose(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭成功");
                    return 3;
                default:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }


        /// <summary>
        /// 设备初始化为默认
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_ConfigDeviceDefault(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionConfigDeviceDefault(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认成功");
                    return 3;
                default:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        #endregion

        #region 直接控制规划相关函数

        /// <summary>
        /// 添加一次坐标增量
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前缓冲区满，等待一会再添加
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="inc">一个指向AsuMotionDirectMoveInc的指针，此指针指向的对象包含需要设置的坐标增量。此增量为要设置的脉冲数和分频系数的乘积</param>
        /// <returns></returns>
        public static int AsuMotion_DirectPlanAddMoveIncrease(IntPtr handle, ref AsuMotionDirectMoveInc inc)
        {
            AsuMotionError ret = AsuMotionDirectPlanAddMoveIncrease(handle, ref inc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加一次坐标增量 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加一次坐标增量 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("添加一次坐标增量 失败，当前缓冲区满，等待一会再添加" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;  // AsuMotion_Buffer_Full
            }
        }

        /// <summary>
        /// 添加一次IO变化
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前缓冲区满，等待一会再添加
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="DIO">需要在当前直接规划阶段设置的数字量输出，低位对齐。每位对应一个数字量输出</param>
        /// <param name="AIO">需要在当前直接规划阶段设置的模拟量输出。控制卡为12位DA输出，即4095对应满量程输出</param>
        /// <returns></returns>
        public static int AsuMotion_DirectPlanChangeIO(IntPtr handle, ushort[] DIO, ushort[] AIO)
        {
            AsuMotionError ret = AsuMotionDirectPlanChangeIO(handle, DIO, AIO);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加一次IO变化 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加一次IO变化 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("添加一次IO变化 失败，当前缓冲区满，等待一会再添加" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;  // AsuMotion_Buffer_Full
            }
        }

        /// <summary>
        /// 将缓冲区中添加的直接规划刷新
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前缓冲区满，等待一会再添加
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_DirectPlanFlush(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionDirectPlanFlush(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("将缓冲区中添加的直接规划刷新 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("将缓冲区中添加的直接规划刷新 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("将缓冲区中添加的直接规划刷新 失败，当前缓冲区满，等待一会再添加" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;  // AsuMotion_Buffer_Full
            }
        }

        #endregion

        #region PC运动规划相关函数

        /// <summary>
        /// 设置当前坐标
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="position">一个指向AsuMotionAxisData的指针，此指针指向的对象包含需要设置的当前坐标</param>
        /// <returns></returns>
        public static int AsuMotion_SetCurrentPostion(IntPtr handle, ref AsuMotionAxisData position)
        {
            AsuMotionError ret = AsuMotionSetCurrentPostion(handle, ref position);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("设置当前坐标 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("设置当前坐标 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("设置当前坐标 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 暂停当前运动
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_Pause(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionPause(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("暂停当前运动 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("暂停当前运动 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("暂停当前运动 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 恢复暂停的运动
        /// 返回0 配置成功；
        /// 返回2 配置失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_Resume(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionResume(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("恢复暂停的运动 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("恢复暂停的运动 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("恢复暂停的运动 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 放弃当前的运动规划
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionAbort(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionAbort(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("放弃当前的运动规划 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("放弃当前的运动规划 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("放弃当前的运动规划 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 设置停止类型
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="type">停止类型</param>
        /// <param name="tolerance">指定停止时所能容忍的误差</param>
        /// <returns></returns>
        public static int Asu_MotionSetStopType(IntPtr handle, AsuMotionStopType type, double tolerance)
        {
            AsuMotionError ret = AsuMotionSetStopType(handle, type, tolerance);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("设置停止类型 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("设置停止类型 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("设置停止类型 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 添加直线插补规划
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前状态下不能添加PC的规划，因为缓冲区已经满了；
        /// 返回6 当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="end">指定当前直线的终点坐标</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        public static int AsuMotion_AddLine(IntPtr handle, ref AsuMotionAxisData end, double vel, double ini_maxvel, double acc)
        {
            AsuMotionError ret = AsuMotionAddLine(handle, ref end, vel, ini_maxvel, acc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加直线插补规划 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("添加直线插补规划 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                case AsuMotionError.AsuMotion_Buffer_Full:
                    LogHelper.WriteLog("添加直线插补规划 失败，缓冲区已满" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;
                default:
                    LogHelper.WriteLog("添加直线插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(6));
                    return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// 添加空间圆弧插补规划
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前状态下不能添加PC的规划，因为缓冲区已经满了；
        /// 返回6 当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="end">指定当前圆弧插补的终点坐标</param>
        /// <param name="center">指定当前圆弧插补定义的球坐标系原点</param>
        /// <param name="normal">法线终点，起点为原点(0，0，0)</param>
        /// <param name="turn">指定当前圆弧插补的圈数</param>
        /// <param name="vel">指定当前圆弧运行的速度</param>
        /// <param name="ini_maxvel">由上一段直线或者曲线转入当前圆弧曲线时，所允许的最大速度</param>
        /// <param name="acc">当前圆弧运行的加速度</param>
        public static int Asu_MotionAddCircle(IntPtr handle,
            ref AsuMotionAxisData end,
            ref AsuMotionCartesian center,
            ref AsuMotionCartesian normal,
            int turn,
            double vel,
            double ini_maxvel,
            double acc)
        {
            AsuMotionError ret = AsuMotionAddCircle(handle, ref end, ref center, ref normal, turn, vel, ini_maxvel, acc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加空间圆弧插补规划 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加空间圆弧插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("添加空间圆弧插补规划 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                case AsuMotionError.AsuMotion_Buffer_Full:
                    LogHelper.WriteLog("添加空间圆弧插补规划 失败，缓冲区已满" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;
                default:
                    LogHelper.WriteLog("添加空间圆弧插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(6));
                    return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// 添加同步IO直线插补规划。
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前状态下不能添加PC的规划，因为缓冲区已经满了；
        /// 返回6 当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="end">指定当前直线的终点坐标</param>
        /// <param name="vel">指定当前直线运行的速度</param>
        /// <param name="ini_maxvel">由上一段直线或者曲线转入当前直线或者曲线时，所允许的最大速度</param>
        /// <param name="acc">当前直线运行的加速度</param>
        /// <param name="DIO">需要在当前规划阶段设置的数字量输出，低位对齐。每位对应一个数字量输出</param>
        /// <param name="AIO">需要在当前规划阶段设置的模拟量输出。控制卡为12位DA输出输出，即4095对应满量程输出</param>
        /// <returns></returns>
        public static int AsuMotion_AddLineWithSyncIO(IntPtr handle,
        ref AsuMotionAxisData end,
        double vel,
        double ini_maxvel,
        double acc,
        ushort[] DIO,
        ushort[] AIO)
        {
            AsuMotionError ret = AsuMotionAddLineWithSyncIO(handle, ref end, vel, ini_maxvel, acc, DIO, AIO);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加同步IO直线插补规划 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                case AsuMotionError.AsuMotion_Buffer_Full:
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，缓冲区已满" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;
                default:
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(6));
                    return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// 添加同步IO空间圆弧插补规划
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回5 当前状态下不能添加PC的规划，因为缓冲区已经满了；
        /// 返回6 当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="end">指定当前圆弧插补的终点坐标</param>
        /// <param name="center">指定当前圆弧插补定义的球坐标系原点</param>
        /// <param name="normal">法线终点，起点为原点(0，0，0)</param>
        /// <param name="turn">指定当前圆弧插补的圈数</param>
        /// <param name="vel">指定当前圆弧运行的速度</param>
        /// <param name="ini_maxvel">由上一段直线或者曲线转入当前圆弧曲线时，所允许的最大速度</param>
        /// <param name="acc">当前圆弧运行的加速度</param>
        /// <param name="DIO">需要在当前规划阶段设置的数字量输出，低位对齐。每位对应一个数字量输出</param>
        /// <param name="AIO">需要在当前规划阶段设置的模拟量输出。控制卡为12位DA输出，即4095对应满量程输出</param>
        /// <returns></returns>
        public static int AsuMotion_AddCircleWithSyncIO(IntPtr handle,
            ref AsuMotionAxisData end,
            ref AsuMotionCartesian center,
            ref AsuMotionCartesian normal,
            int turn,
            double vel,
            double ini_maxvel,
            double acc,
            ushort[] DIO,
            ushort[] AIO
        )
        {
            AsuMotionError ret = AsuMotionAddCircleWithSyncIO(handle, ref end, ref center, ref normal, turn, vel, ini_maxvel, acc, DIO, AIO);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("添加同步IO空间圆弧插补规划 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("添加同步IO空间圆弧插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("添加同步IO空间圆弧插补规划 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                case AsuMotionError.AsuMotion_Buffer_Full:
                    LogHelper.WriteLog("添加同步IO空间圆弧插补规划 失败，缓冲区已满" + "---" + AsuMotion_GetErrorMessage(5));
                    return 5;
                default:
                    LogHelper.WriteLog("添加同步IO空间圆弧插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(6));
                    return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        #endregion

        #region 运动控制卡规划相关的函数

        /// <summary>
        /// 常速运行
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回2 失败；
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="axisMask">配置当前需要运行的轴</param>
        /// <returns></returns>
        public static int AsuMotion_MoveAtConstSpeed(IntPtr handle, AsuMotionAxisMaskType axisMask)
        {
            AsuMotionError ret = AsuMotionMoveAtConstSpeed(handle, axisMask);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("常速运行 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("常速运行 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("常速运行 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                default:
                    LogHelper.WriteLog("常速运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(8));
                    return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }

        /// <summary>
        /// 点动运行
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回2 失败；
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="AsuMotion">设备句柄</param>
        /// <param name="axis">配置当前需要运行的轴</param>
        /// <param name="positionGiven">给定一个点动运行时，机器的目标位置。此位置为相对位置</param>
        /// <returns></returns>
        public static int AsuMotion_JogOn(IntPtr handle, AsuMotionAxisIndexType axis, double positionGiven)
        {
            AsuMotionError ret = AsuMotionJogOn(handle, axis, positionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("点动运行 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("点动运行 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                default:
                    LogHelper.WriteLog("点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(8));
                    return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }

        /// <summary>
        /// 绝对位置移动
        /// 返回0 成功；
        /// 返回2 失败；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="axisMask">配置当前需要运行的轴</param>
        /// <param name="positionGiven">给定一个点动运行时，机器的目标位置</param>
        /// <returns></returns>
        public static int AsuMotion_MoveAbsolute(IntPtr handle, AsuMotionAxisMaskType axisMask, ref AsuMotionAxisData positionGiven)
        {
            AsuMotionError ret = AsuMotionMoveAbsolute(handle, axisMask, ref positionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("绝对位置移动 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("绝对位置移动 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("绝对位置移动 失败" + "---" + AsuMotion_GetErrorMessage(2));
                    return 2;
                default:
                    LogHelper.WriteLog("绝对位置移动 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(8));
                    return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }

        #endregion

        #region 运动控制卡状态获取相关的函数

        /// <summary>
        /// 输入口状态获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="input">用于存储输入信号值的存储区域</param>
        /// <returns></returns>
        public static int AsuMotion_GetInputIO(IntPtr handle, out ushort[] input)
        {
            AsuMotionError ret = AsuMotionGetInputIO(handle, out input);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("输入口状态获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("输入口状态获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 输出口状态获取
        /// 返回3 成功
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">AsuMotion资源句柄</param>
        /// <param name="output">IO状态缓冲区，长度至少为2，函数调用成功后，这里面的值将为IO状态</param>
        /// <returns></returns>
        public static int AsuMotion_GetOutputIO(IntPtr handle, out ushort[] output)
        {
            AsuMotionError ret = AsuMotionGetOutputIO(handle, out output);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("输出口状态获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("输出口状态获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 当前机器坐标位置脉冲数获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="steps">一个存储各轴脉冲数的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_GetSteps(IntPtr handle, out AsuMotionAxisDataInt steps)
        {
            AsuMotionError ret = AsuMotionGetSteps(handle, out steps);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("当前机器坐标位置脉冲数获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("当前机器坐标位置脉冲数获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 当前各坐标轴最大速度获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="maxSpeed">板卡各轴最大运动速度的存储区域，单位毫米每分钟</param>
        /// <returns></returns>
        public static int AsuMotion_GetMaxSpeed(IntPtr handle, out AsuMotionAxisData maxSpeed)
        {
            AsuMotionError ret = AsuMotionGetMaxSpeed(handle, out maxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("当前各坐标轴最大速度获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("当前各坐标轴最大速度获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 光滑系数获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="smoothCoff">板卡光滑系数的存储区域</param>
        /// <returns></returns>
        public static int AsuMotion_GetSmoothCoff(IntPtr handle, out uint smoothCoff)
        {
            AsuMotionError ret = AsuMotionGetSmoothCoff(handle, out smoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("光滑系数获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("光滑系数获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 单位距离脉冲数获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回：
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="stepsPerUnit">板卡脉冲每毫米的参数配置值的存储区域</param>
        /// <returns></returns>
        public static int AsuMotion_GetStepsPerUnit(IntPtr handle, out AsuMotionAxisDataInt stepsPerUnit)
        {
            AsuMotionError ret = AsuMotionGetStepsPerUnit(handle, out stepsPerUnit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("单位距离脉冲数获取 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("单位距离脉冲数获取 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 查询当前运动卡是否处于运动状态中
        /// 返回3 空闲状态
        /// 返回4 运动状态
        /// 返回-1 异常
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_IsDone(IntPtr handle)
        {
            try
            {
                AsuMotionError ret = AsuMotionIsDone(handle);
                switch (ret)
                {
                    case AsuMotionError.AsuMotion_True:
                        LogHelper.WriteLog("当前运动卡处于 空闲 状态");
                        return 3;
                    default:
                        LogHelper.WriteLog("当前运动卡处于 运动 状态");
                        return 4;  // AsuMotion_False
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("执行 AsuMotionIsDone 异常：" + e.Message);
                return -1;
            }
        }

        #endregion

        #region 停止运动函数

        /// <summary>
        /// 急停
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_EStop(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionEStop(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("急停 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("急停 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 停止由运动控制卡规划的某个轴的运动
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="axis">配置当前需要运行的轴</param>
        /// <returns></returns>
        public static int AsuMotion_CardPlanStop(IntPtr handle, AsuMotionAxisMaskType axis)
        {
            AsuMotionError ret = AsuMotionCardPlanStop(handle, axis);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("停止由运动控制卡规划的特定轴的运动 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("停止由运动控制卡规划的特定轴的运动 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 停止由运动控制卡规划的所有轴的运动
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int AsuMotion_CardPlanStopAll(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionCardPlanStopAll(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        #endregion

        #region PC规划与运动控制卡规划共用的配置函数

        /// <summary>
        /// 配置机器坐标
        /// 返回3 空闲状态；
        /// 返回4 运动状态
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="axisMask">配置当前需要运行的轴</param>
        /// <param name="position">一个存储各轴机器坐标的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_SetMachineCoordinate(IntPtr handle, AsuMotionAxisMaskType axisMask, AsuMotionAxisData position)
        {
            AsuMotionError ret = AsuMotionSetMachineCoordinate(handle, axisMask, ref position);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置机器坐标 当前运动卡处于 空闲 状态");
                    return 3;
                default:
                    LogHelper.WriteLog("配置机器坐标 当前运动卡处于 运动 状态");
                    return 4;  // AsuMotion_False
            }
        }

        ///  <summary>
        /// 配置单位脉冲数
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="StepsPerUnit">单位脉冲数</param>
        /// <returns></returns>
        public static int AsuMotion_SetStepsPerUnit(IntPtr handle, ref AsuMotionAxisDataInt StepsPerUnit)
        {
            AsuMotionError ret = AsuMotionSetStepsPerUnit(handle, ref StepsPerUnit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置单位脉冲数 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置单位脉冲数 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置工作偏移
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="WorkOffset">工作偏移</param>
        /// <returns></returns>
        public static int AsuMotion_SetWorkOffset(IntPtr handle, ref AsuMotionAxisDataInt WorkOffset)
        {
            AsuMotionError ret = AsuMotionSetWorkOffset(handle, ref WorkOffset);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置工作偏移 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置工作偏移 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置光滑系数和脉冲延时
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="DelayBetweenPulseAndDir">脉冲的有效沿和方向的有效沿之间有充分的时间差</param>
        /// <param name="SmoothCoff">光滑系数</param>
        /// <returns></returns>
        public static int Asu_Motion_SetSmoothCoff(IntPtr handle, ushort DelayBetweenPulseAndDir, int SmoothCoff)
        {
            AsuMotionError ret = AsuMotionSetSmoothCoff(handle, DelayBetweenPulseAndDir, SmoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置光滑系数和脉冲延时 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置光滑系数和脉冲延时 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡差分输出的信号映射
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="FunSel">映射号数组，共16个元素，其值得设定将反应16个差分输出的内容</param>
        /// <param name="NegMask">设定16个差分输出口是否反向输出</param>
        /// <returns></returns>
        public static int AsuMotion_SetDifferentialOutputMapping(IntPtr handle, byte[] FunSel, AsuMotionBitMaskType NegMask)
        {
            AsuMotionError ret = AsuMotionSetDifferentialOutputMapping(handle, FunSel, NegMask);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡差分输出的信号映射 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置运动卡差分输出的信号映射 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡数字量输入功能
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="InputIOEnable">输入引脚使能 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIONeg">输入引脚触发信号反相 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIOPin">输入引脚映射 0--> Input0 2--> Input2 7--> Input7</param>
        /// <returns></returns>
        public static int AsuMotion_SetInputIOEngineDir(IntPtr handle,
            UInt64 InputIOEnable,
            UInt64 InputIONeg,
            byte[] InputIOPin)
        {
            AsuMotionError ret = AsuMotionSetInputIOEngineDir(handle, InputIOEnable, InputIONeg, InputIOPin);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡数字量输入功能 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置运动卡数字量输入功能 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡模拟量输出和数字量输出
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="AsuMotion">设备句柄</param>
        /// <param name="Sync">是否同步输出，在使用卐千规划时，可以采用同步输出，使得外部时序能够严格保证</param>
        /// <param name="AnalogOut">两个元素的数组，设置模拟量输出</param>
        /// <param name="DigitalOut">两个元素的数组，设置数字量输出</param>
        /// <returns></returns>
        public static int AsuMotion_SetOutput(IntPtr handle, ushort Sync, short[] AnalogOut, ushort[] DigitalOut)
        {
            AsuMotionError ret = AsuMotionSetOutput(handle, Sync, AnalogOut, DigitalOut);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡模拟量输出和数字量输出 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置运动卡模拟量输出和数字量输出 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        #endregion

        #region 回原点相关的参数配置函数

        /// <summary>
        /// 配置回原点的管脚
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="AsuMotion">设备句柄</param>
        /// <param name="inputIOPin">一个包含9个元素的数组，分别代表九个轴的回原点的信号选择，如果选择的值大于等于32，那么选择的信号为其逻辑反，也就是说原来如果高电平有效，那么现在将变成低电平有效</param>
        /// <returns></returns>
        public static int AsuMotion_SetHomingSignal(IntPtr handle, byte[] inputIOPin)
        {
            AsuMotionError ret = AsuMotionSetHomingSignal(handle, inputIOPin);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置回原点的管脚 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置回原点的管脚 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;  // AsuMotion_Device_Is_Null
            }
        }

        /// <summary>
        /// 请求一次回原点
        /// 返回0 成功；
        /// 返回1 设备句柄为空指针，一般因为没有打开设备导致；
        /// 返回7 当前状态下不能进行回原点，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="type">设置回原点的方式，目前支持三种方式</param>
        /// <param name="axisMask">配置当前需要运行的轴</param>
        /// <param name="count">设置回原点采用多次回原点时，回原点的次数</param>
        /// <param name="acceleration">一个存储各轴加速度的结构体</param>
        /// <param name="maxSpeed">一个存储各轴速度的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_GoHome(IntPtr handle,
            AsuMotionHomingType type,
            AsuMotionAxisMaskType axisMask,
            ushort count,
            ref AsuMotionAxisData acceleration,
            ref AsuMotionAxisData maxSpeed)
        {
            AsuMotionError ret = AsuMotionGoHome(handle, type, axisMask, count, ref acceleration, ref maxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("请求一次回原点 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("请求一次回原点 失败，设备句柄为空指针，一般因为没有打开设备导致" + "---" + AsuMotion_GetErrorMessage(1));
                    return 1;
                default:
                    LogHelper.WriteLog("请求一次回原点 失败，当前状态下不能进行回原点，因为前面提交的其他操作还未完成" + "---" + AsuMotion_GetErrorMessage(7));
                    return 7;  // AsuMotion_CurrentState_Isnot_Idle
            }
        }

        #endregion

        #region 运动控制卡规划相关的参数配置函数

        /// <summary>
        /// 配置运动卡规划运动的加速度
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="acceleration">一个存储各轴加速度的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_SetAccelaration(IntPtr handle, ref AsuMotionAxisData acceleration)
        {
            AsuMotionError ret = AsuMotionSetAccelaration(handle, ref acceleration);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡规划运动的加速度 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置运动卡规划运动的加速度 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡规划运动的最大速度
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="maxSpeed">一个存储各轴速度的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_SetMaxSpeed(IntPtr handle, ref AsuMotionAxisData maxSpeed)
        {
            AsuMotionError ret = AsuMotionSetMaxSpeed(handle, ref maxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡规划运动的最大速度 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置运动卡规划运动的最大速度 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置正向软限位
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="softPositiveLimit">正向软限位坐标的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_SetSoftPositiveLimit(IntPtr handle, ref AsuMotionAxisData softPositiveLimit)
        {
            AsuMotionError ret = AsuMotionSetSoftPositiveLimit(handle, ref softPositiveLimit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置正向软限位 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置正向软限位 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置反向软限位
        /// 返回3 成功；
        /// 返回4 失败
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="softNegtiveLimit">反向软限位坐标的结构体</param>
        /// <returns></returns>
        public static int AsuMotion_SetSoftNegtiveLimit(IntPtr handle, ref AsuMotionAxisData softNegtiveLimit)
        {
            AsuMotionError ret = AsuMotionSetSoftNegtiveLimit(handle, ref softNegtiveLimit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置反向软限位 成功");
                    return 3;
                default:
                    LogHelper.WriteLog("配置反向软限位 失败" + "---" + AsuMotion_GetErrorMessage(4));
                    return 4;  // AsuMotion_False
            }
        }

        #endregion

        #endregion
    }
}
