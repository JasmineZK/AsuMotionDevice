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
        private static bool IsInitLog4net = false; // Log4net是否已初始化
        private static int NumOfCurDevice = 0; // 当前设备序号

        #region 初始化 Log4net

        private static void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        #endregion

        #region 已确定的功能

        /// <summary>
        /// 获取设备数量，一般最先调用
        /// 返回 与PC相连的设备总数
        /// </summary>
        /// <returns>与PC相连的设备总数</returns>
        public static int Asu_GetDeviceNum()
        {
            if (IsInitLog4net == false)
            {
                InitLog4net();
                IsInitLog4net = true;
            }

            int result = GetDeviceNum();
            LogHelper.WriteLog("有 " + result + " 台设备连接");

            return result;
        }

        /// <summary>
        /// 获取设备序列号信息
        /// 成功返回 0
        /// 失败返回 1
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
                    LogHelper.WriteLog("获取设备 " + numberOfDevice + " 信息成功，序列号为：" + Encoding.UTF8.GetString(serialOfDevice));
                    return 0;
                default:
                    LogHelper.WriteLog("获取设备 " + numberOfDevice + " 信息失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 打开设备
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="numberOfDevice">设备序号，从1开始</param>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionOpen(int numberOfDevice, out IntPtr handle)
        {
            handle = AsuMotionOpen(numberOfDevice - 1);
            if (handle != IntPtr.Zero)
            {
                LogHelper.WriteLog("打开设备 " + numberOfDevice + " 成功");
                return 0;
            }
            else
            {
                LogHelper.WriteLog("打开设备 " + numberOfDevice + " 失败");
                return 1;
            }
        }

        /// <summary>
        /// 设备初始化为默认
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionConfigDeviceDefault(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionConfigDeviceDefault(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认成功");
                    return 0;
                default:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认失败");
                    return 1;  // AsuMotion_False
            }
        }


        /// <summary>
        /// 配置光滑系数和脉冲延时
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="DelayBetweenPulseAndDir">脉冲的有效沿和方向的有效沿之间有充分的时间差</param>
        /// <param name="SmoothCoff">光滑系数</param>
        /// <returns></returns>
        public static int Asu_MotionSetSmoothCoff(IntPtr handle, ushort DelayBetweenPulseAndDir, int SmoothCoff)
        {
            AsuMotionError ret = AsuMotionSetSmoothCoff(handle, DelayBetweenPulseAndDir, SmoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置光滑系数和脉冲延时 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置光滑系数和脉冲延时 失败");
                    return 1;  // AsuMotion_False
            }
        }

        ///  <summary>
        /// 配置单位脉冲数
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="SmoothCoff">单位脉冲数</param>
        /// <returns></returns>
        public static int Asu_MotionSetStepsPerUnit(IntPtr handle, AsuMotionAxisDataInt StepsPerUnit)
        {
            AsuMotionError ret = AsuMotionSetStepsPerUnit(handle, StepsPerUnit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置单位脉冲数 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置单位脉冲数 失败");
                    return 1;  // AsuMotion_False
            }
        }


        public static int Asu_MotionSetWorkOffset(IntPtr handle, AsuMotionAxisDataInt WorkOffset)
        {
            AsuMotionError ret = AsuMotionSetWorkOffset(handle, WorkOffset);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置工作偏移 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置工作偏移 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡差分输出的信号映射
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="FunSel">映射号数组，共16个元素，其值得设定将反应16个差分输出的内容</param>
        /// <param name="NegMask">设定16个差分输出口是否反向输出</param>
        /// <returns></returns>
        public static int Asu_MotionSetDifferentialOutputMapping(IntPtr handle, byte[] FunSel, AsuMotionBitMaskType NegMask)
        {
            AsuMotionError ret = AsuMotionSetDifferentialOutputMapping(handle, FunSel, NegMask);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡差分输出的信号映射 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置运动卡差分输出的信号映射 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡规划运动的加速度
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="Acceleration">一个存储各轴加速度的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetAccelaration(IntPtr handle, AsuMotionAxisData Acceleration)
        {
            AsuMotionError ret = AsuMotionSetAccelaration(handle, Acceleration);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡规划运动的加速度 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置运动卡规划运动的加速度 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置运动卡规划运动的最大速度
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="MaxSpeed">一个存储各轴速度的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetMaxSpeed(IntPtr handle, AsuMotionAxisData MaxSpeed)
        {
            AsuMotionError ret = AsuMotionSetMaxSpeed(handle, MaxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置运动卡规划运动的最大速度 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置运动卡规划运动的最大速度 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 停止由运动控制卡规划的所有轴的运动
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionStopAll(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionCardPlanStopAll(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 查询当前运动卡是否处于运动状态中
        /// 空闲返回 0
        /// 运动返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionIsDone(IntPtr handle)
        {
            try
            {
                AsuMotionError ret = AsuMotionIsDone(handle);
                switch (ret)
                {
                    case AsuMotionError.AsuMotion_True:
                        LogHelper.WriteLog("当前运动卡处于 空闲 状态");
                        return 0;
                    default:
                        LogHelper.WriteLog("当前运动卡处于 运动 状态");
                        return 1;  // AsuMotion_False
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("执行 AsuMotionIsDone 异常：" + e.Message);
                return -1;
            }            
        }

        /// <summary>
        /// 当前机器坐标位置脉冲数获取
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="Steps">一个存储各轴脉冲数的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionGetSteps(IntPtr handle, out AsuMotionAxisDataInt Steps)
        {
            AsuMotionError ret = AsuMotionGetSteps(handle, out Steps);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("当前机器坐标位置脉冲数获取 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("当前机器坐标位置脉冲数获取 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置机器坐标
        /// 返回0 当前运动卡处于空闲状态
        /// 返回1 当前运动卡处于运动状态
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="AxisMask">配置当前需要运行的轴</param>
        /// <param name="Position">一个存储各轴机器坐标的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetMachineCoordinate(IntPtr handle, AsuMotionAxisMaskType AxisMask, AsuMotionAxisData Position)
        {
            AsuMotionError ret = AsuMotionSetMachineCoordinate(handle, AxisMask, Position);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置机器坐标 当前运动卡处于 空闲 状态");
                    return 0;
                default:
                    LogHelper.WriteLog("配置机器坐标 当前运动卡处于 运动 状态");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置正向软限位
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="SoftPositiveLimit">正向软限位坐标的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetSoftPositiveLimit(IntPtr handle, AsuMotionAxisData SoftPositiveLimit)
        {
            AsuMotionError ret = AsuMotionSetSoftPositiveLimit(handle, SoftPositiveLimit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置正向软限位 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置正向软限位 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置反向软限位
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="SoftNegtiveLimit">反向软限位坐标的结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetSoftNegtiveLimit(IntPtr handle, AsuMotionAxisData SoftNegtiveLimit)
        {
            AsuMotionError ret = AsuMotionSetSoftPositiveLimit(handle, SoftNegtiveLimit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("配置反向软限位 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("配置反向软限位 失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        ///  关闭设备
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionClose(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionClose(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭成功");
                    return 0;
                default:
                    LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭失败");
                    return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 点动运行，需要调用Stop停止
        /// 返回0 配置成功
        /// 返回1 参数 handle 为空指针，一般因为没有打开设备导致
        /// 返回2 配置不成功
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Axis">配置当前需要运行的轴</param>
        /// <param name="PositionGiven">给定一个点动运行时，机器的目标位置。此位置为相对位置</param>
        /// <returns></returns>
        public static int Asu_MotionJogOn(IntPtr handle, AsuMotionAxisIndexType Axis, double PositionGiven)
        {
            AsuMotionError ret = AsuMotionJogOn(handle, Axis, PositionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("X 轴正向点动运行 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("X 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    return 1;
                case AsuMotionError.AsuMotion_Error:
                    LogHelper.WriteLog("X 轴正向点动运行 失败");
                    return 2;
                default:
                    LogHelper.WriteLog("X 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }


        /// <summary>
        /// 暂停当前运动，可由AsuMotionResume恢复运动状态
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionPause(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionPause(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("暂停当前运动 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("暂停当前运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    return 1;
                default:
                    LogHelper.WriteLog("暂停当前运动 失败");
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 恢复暂停的运动
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionResume(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionResume(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok:
                    LogHelper.WriteLog("恢复暂停的运动 成功");
                    return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null:
                    LogHelper.WriteLog("恢复暂停的运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    return 1;
                default:
                    LogHelper.WriteLog("恢复暂停的运动 失败");
                    return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 急停
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <returns></returns>
        public static int Asu_MotionEStop(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionEStop(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True:
                    LogHelper.WriteLog("急停 成功");
                    return 0;
                default:
                    LogHelper.WriteLog("急停 失败");
                    return 1;  // AsuMotion_False
            }
        }

        #endregion

        #region TODO

        /// <summary>
        /// 配置输入引脚的特殊功能
        /// </summary>
        /// <param name="InputIOEnable">输入引脚使能 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIONeg">输入引脚触发信号反相 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIOPin">输入引脚映射 0--> Input0 2--> Input2 7--> Input7</param>
        /// <returns></returns>
        public static int Asu_MotionSetInputIOEngineDir(IntPtr handle,
            UInt64 InputIOEnable,
            UInt64 InputIONeg,
            byte[] InputIOPin)
        {
            AsuMotionError ret = AsuMotionSetInputIOEngineDir(handle, InputIOEnable, InputIONeg, InputIOPin);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 获取输入信号
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="Input">用于存储输入信号值的存储区域</param>
        /// <returns></returns>
        public static int Asu_MotionGetInputIO(IntPtr handle, ushort[] Input)
        {
            AsuMotionError ret = AsuMotionGetInputIO(handle, Input);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }



        /// <summary>
        /// 配置主轴输出
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="Out">主轴速率配置值，最大为65535</param>
        /// <returns></returns>
        public static int Asu_MotionSetSpindle(IntPtr handle, ushort Out)
        {
            AsuMotionError ret = AsuMotionSetSpindle(handle, Out);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }



        /// <summary>
        /// 获取板卡细分数
        /// </summary>
        /// <param name="pSmoothCoff">板卡细分数的存储区域</param>
        /// <returns></returns>
        public static int Asu_MotionGetSmoothCoff(IntPtr handle, ref UInt32 pSmoothCoff)
        {
            AsuMotionError ret = AsuMotionGetSmoothCoff(handle, ref pSmoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }


        /// <summary>
        /// 获取板卡脉冲每毫米的参数配置值
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="StepsPerUnit">板卡脉冲每毫米的参数配置值的存储区域</param>
        /// <returns></returns>
        public static int Asu_MotionGetStepsPerUnit(IntPtr handle, int[] StepsPerUnit)
        {
            AsuMotionError ret = AsuMotionGetStepsPerUnit(handle, StepsPerUnit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置板卡脉冲与方向输出的反相
        /// </summary>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        public static int Asu_MotionSetStepNegAndDirNeg(IntPtr handle, byte StepNeg, byte DirNeg)
        {
            AsuMotionError ret = AsuMotionSetStepNegAndDirNeg(handle, StepNeg, DirNeg);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 相对于机械坐标的运动
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        public static int Asu_MotionMoveAbsolute(IntPtr handle, ushort AxisMask, double[] PositionGiven)
        {
            AsuMotionError ret = AsuMotionMoveAbsolute(handle, AxisMask, PositionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                default: return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }

        /// <summary>
        /// 配置板卡规划运动的加速度与最大速度
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        public static int Asu_MotionSetAccelarationMaxSpeed(IntPtr handle, double[] Acceleration, double[] MaxSpeed)
        {
            AsuMotionError ret = AsuMotionSetAccelarationMaxSpeed(handle, Acceleration, MaxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 距离无限远的运动
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        public static int Asu_MotionMoveAtSpeed(IntPtr handle, ushort AxisMask, double[] Acceleration, double[] MaxSpeed)
        {
            AsuMotionError ret = AsuMotionMoveAtSpeed(handle, AxisMask, Acceleration, MaxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 软限位配置
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="MaxSoftLimit">允许运动到的最大机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MinSoftLimit">允许运动到的最小机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        public static int Asu_MotionSetSoftLimit(IntPtr handle, double[] MaxSoftLimit, double[] MinSoftLimit)
        {
            AsuMotionError ret = AsuMotionSetSoftLimit(handle, MaxSoftLimit, MinSoftLimit);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 获取板卡各轴最大运动速度
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="MaxSpeed">板卡各轴最大运动速度的存储区域，单位毫米每分钟</param>
        /// <returns></returns>
        public static int Asu_MotionGetMaxSpeed(IntPtr handle, double[] MaxSpeed)
        {
            AsuMotionError ret = AsuMotionGetMaxSpeed(handle, MaxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 停止由板卡规划的特定的轴的运动,NEED TEST
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="Axis">0 --> X轴停止运动， 1 --> Y轴停止运动 2 --> Z轴停止运动</param>
        /// <returns></returns>
        public static int Asu_MotionStop(IntPtr handle, ushort Axis)
        {
            AsuMotionError ret = AsuMotionCardPlanStop(handle, Axis);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置板卡当前的机械坐标
        /// 
        /// </summary>
        /// <param name="Posi">机械坐标，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        public static int Asu_MotionSetCoordinate(IntPtr handle, double[] Posi)
        {
            AsuMotionError ret = AsuMotionSetCoordinate(handle, Posi);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 终止PC规划运动，不可由AsuMotionResume恢复运动状态
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <returns></returns>
        public static int Asu_MotionAbort(IntPtr handle)
        {
            AsuMotionError ret = AsuMotionAbort(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 配置当前AsuMotion PC规划运动的工作坐标
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="position">期望配置坐标值的位置结构体</param>
        /// <returns></returns>
        public static int Asu_MotionSetCurrentPostion(IntPtr handle, AsuMotionAxisData position)
        {
            AsuMotionError ret = AsuMotionSetCurrentPostion(handle, ref position);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 添加直线运动轨迹
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// 返回5 前状态下不能添加PC的规划，因为缓冲区已经满了
        /// 返回6 前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="end">目的终点</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        public static int Asu_MotionAddLine(IntPtr handle, AsuMotionAxisData endPosition, double vel, double ini_maxvel, double acc)
        {
            AsuMotionError ret = AsuMotionAddLine(handle, ref endPosition, vel, ini_maxvel, acc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                case AsuMotionError.AsuMotion_Buffer_Full: return 5;
                default: return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// AsuMotion PC规划添加圆弧  由目的终点，圆心，法线确定圆平面
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// 返回5 前状态下不能添加PC的规划，因为缓冲区已经满了
        /// 返回6 前状态下不能进行PC的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="endPositon">目的终点</param>
        /// <param name="center">圆心</param>
        /// <param name="normal">法线终点，起点为原点</param>
        /// <param name="turn"></param>
        /// <param name="vel">期望圆弧运动的速度</param>
        /// <param name="ini_maxvel">最大速度</param>
        /// <param name="acc">加速度</param>
        public static int Asu_MotionAddCircle(IntPtr handle, 
                IntPtr AsuMotion,
                ref AsuMotionAxisData endPositon,
                ref AsuMotionCartesian center,
                ref AsuMotionCartesian normal,
                int turn,
                double vel,
                double ini_maxvel,
                double acc)
        {
            AsuMotionError ret = AsuMotionAddCircle(handle, ref endPositon, ref center, ref normal, turn, vel, ini_maxvel, acc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                case AsuMotionError.AsuMotion_Buffer_Full: return 5;
                default: return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// 设置PC规划运动的停止类型
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <param name="type">停止类型 </param>
        /// <param name="tolerance">精度要求，目标位值和当前位值的绝对值小于此数时规划结束了</param>
        /// <returns></returns>
        public static int Asu_MotionSetStopType(IntPtr handle, AsuMotionStopType type, double tolerance)
        {
            AsuMotionError ret = AsuMotionSetStopType(handle, type, tolerance);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Device_Is_Null: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        #endregion
    }
}
