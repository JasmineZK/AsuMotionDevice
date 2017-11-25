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

        #region 初始化 Log4net

        private static void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        #endregion

        #region 通用功能

        /// <summary>
        /// 打开设备
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="number">为0:PC连接的第一个AsuMotion，为1:PC连接的第二个AsuMotion，在PC只与一个AsuMotion相连时，此项配为0即可</param>
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
        /// 获取主轴脉冲计数值
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="SpindlePostion">主轴脉冲计数值的存储区域</param>
        /// <returns></returns>
        public static int Asu_AsuMotionGetSpindlePostion(ushort[] SpindlePostion)
        {
            AsuMotionError ret = AsuMotionGetSpindlePostion(handle, SpindlePostion);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        //// <summary>
        /// 板卡轴输出配置
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="StepSel"></param>
        /// <param name="DirSel">轴输出管脚配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值与管脚的对应需对应参考表</param>
        /// <param name="Enable">轴输出使能配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值为true则使能</param>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        public static int Asu_AsuMotionSetAxisOutputConfig(
            byte[] StepSel,
            byte[] DirSel,
            byte[] Enable,
            ushort StepNeg,
            ushort DirNeg)
        {
            AsuMotionError ret = AsuMotionSetAxisOutputConfig(handle, StepSel, DirSel, Enable, StepNeg, DirNeg);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置输入引脚的特殊功能
        /// </summary>
        /// <param name="InputIOEnable">输入引脚使能 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIONeg">输入引脚触发信号反相 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIOPin">输入引脚映射 0--> Input0 2--> Input2 7--> Input7</param>
        /// <param name="EngineDirections">改变运动方向</param>
        /// <returns></returns>
        public static int Asu_AsuMotionSetInputIOEngineDir(
            UInt64 InputIOEnable,
            UInt64 InputIONeg,
            byte[] InputIOPin,
            sbyte[] EngineDirections)
        {
            AsuMotionError ret = AsuMotionSetInputIOEngineDir(handle, InputIOEnable, InputIONeg, InputIOPin, EngineDirections);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 获取当前PC检测到的AsuMotion数量，一般最先调用;
        /// 返回 与PC相连的AsuMotion总数
        /// </summary>
        /// <returns>与PC相连的AsuMotion总数</returns>
        public static int Asu_GetDeviceNum()
        {
            InitLog4net();

            int result = GetDeviceNum();

            return result;
        }

        /// <summary>
        /// 获取AsuMotion板卡序列号，在获取设备数量大于0之后调用
        /// </summary>
        /// <param name="number">为0:PC连接的第一个AsuMotion，为1:PC连接的第二个AsuMotion，在PC只与一个AsuMotion相连时，此项配为0即可</param>
        /// <param name="serial">用于存储序列号的字节数组</param>
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
        /// 断开与AsuMotion的通讯
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
        /// 初始化AsuMotion参数配置
        /// 成功返回 0
        /// 失败返回 1
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

        /// <summary>
        /// 获取输入信号
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="Input">用于存储输入信号值的存储区域</param>
        /// <returns></returns>
        public static int Asu_AsuMotionGetInputIO(ushort[] Input)
        {
            AsuMotionError ret = AsuMotionGetInputIO(handle, Input);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 获取已AsuMotion各轴相对于机械原点的脉冲总量
        /// 目前直接返回 AsuMotion_True，但不确定之后的sdk失败会返回什么，所以按如下返回
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <param name="Steps">各轴相对于机械原点的脉冲总量的存储区域</param>
        /// <returns></returns>
        public static int Asu_AsuMotionGetSteps(int[] Steps)
        {
            AsuMotionError ret = AsuMotionGetSteps(handle, Steps);
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
        public static int Asu_AsuMotionSetSpindle(ushort Out)
        {
            AsuMotionError ret = AsuMotionSetSpindle(handle, Out);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 急停
        /// 成功返回 0
        /// 设备返回 1
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionEStop()
        {
            AsuMotionError ret = AsuMotionEStop(handle);
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
        public static int Asu_AsuMotionGetSmoothCoff(ref UInt32 pSmoothCoff)
        {
            AsuMotionError ret = AsuMotionGetSmoothCoff(handle, ref pSmoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 配置光滑系数和脉冲延时
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="DelayBetweenPulseAndDir">脉冲的有效沿和方向的有效沿之间有充分的时间差</param>
        /// <param name="SmoothCoff">光滑系数</param>
        /// <returns></returns>
        public static int Asu_AsuMotionSetSmoothCoff(ushort DelayBetweenPulseAndDir, int SmoothCoff)
        {
            AsuMotionError ret = AsuMotionSetSmoothCoff(handle, DelayBetweenPulseAndDir, SmoothCoff);
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
        public static int Asu_AsuMotionGetStepsPerUnit(int[] StepsPerUnit)
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
        public static int Asu_AsuMotionSetStepNegAndDirNeg(byte StepNeg, byte DirNeg)
        {
            AsuMotionError ret = AsuMotionSetStepNegAndDirNeg(handle, StepNeg, DirNeg);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        ///  <summary>
        /// 配置板卡参数：脉冲每毫米，细分数，脉冲方向延时
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <param name="DelayBetweenPulseAndDir">脉冲方向延时</param>
        /// <param name="StepsPerAxis">脉冲每毫米 0 --> X轴脉冲每毫米 1 --> Y轴脉冲每毫米</param>
        /// <param name="WorkOffset">保留参数，暂不使用</param>
        /// <param name="SmoothCoff">细分数</param>
        /// <returns></returns>
        public static int Asu_AsuMotionSetStepsPerUnitSmoothCoff(
            ushort DelayBetweenPulseAndDir,
            int[] StepsPerAxis,
            int[] WorkOffset,
            int SmoothCoff)
        {
            AsuMotionError ret = AsuMotionSetStepsPerUnitSmoothCoff(handle, DelayBetweenPulseAndDir, StepsPerAxis, WorkOffset, SmoothCoff);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        #endregion

        #region 板卡规划运动

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
        public static int Asu_AsuMotionMoveAbsolute(ushort AxisMask, double[] PositionGiven)
        {
            AsuMotionError ret = AsuMotionMoveAbsolute(handle, AxisMask, PositionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
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
        public static int Asu_AsuMotionSetAccelarationMaxSpeed(double[] Acceleration, double[] MaxSpeed)
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
        public static int Asu_AsuMotionMoveAtSpeed(ushort AxisMask, double[] Acceleration, double[] MaxSpeed)
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
        public static int Asu_AsuMotionSetSoftLimit(double[] MaxSoftLimit, double[] MinSoftLimit)
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
        public static int Asu_AsuMotionGetMaxSpeed(double[] MaxSpeed)
        {
            AsuMotionError ret = AsuMotionGetMaxSpeed(handle, MaxSpeed);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 停止由运动控制卡规划的所有轴的运动
        /// 成功返回 0
        /// 失败返回 1
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionStopAll()
        {
            AsuMotionError ret = AsuMotionCardPlanStopAll(handle);
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
        public static int Asu_AsuMotionStop(ushort Axis)
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
        public static int Asu_AsuMotionSetCoordinate(double[] Posi)
        {
            AsuMotionError ret = AsuMotionSetCoordinate(handle, Posi);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
            }
        }

        /// <summary>
        /// 点动运行，需要调用Stop停止
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// 返回8 当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="Axis">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        public static int Asu_AsuMotionJogOn(ushort Axis, ref double PositionGiven)
        {
            AsuMotionError ret = AsuMotionJogOn(handle, Axis, ref PositionGiven);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                default: return 8;  // AsuMotion_CurrentState_Isnot_CardPlan
            }
        }

        #endregion

        #region PC规划运动

        /// <summary>
        /// 恢复经AsuMotionPause停止的运动PC规划运动
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionResume()
        {
            AsuMotionError ret = AsuMotionResume(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 停止PC规划运动，可由AsuMotionResume恢复运动状态
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionPause()
        {
            AsuMotionError ret = AsuMotionPause(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 终止PC规划运动，不可由AsuMotionResume恢复运动状态
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionAbort()
        {
            AsuMotionError ret = AsuMotionAbort(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 查询当前运动卡是否处于运动状态中
        /// 空闲返回 0
        /// 运动返回 1
        /// </summary>
        /// <returns></returns>
        public static int Asu_AsuMotionIsDone()
        {
            AsuMotionError ret = AsuMotionIsDone(handle);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_True: return 0;
                default: return 1;  // AsuMotion_False
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
        public static int Asu_AsuMotionSetCurrentPostion(AsuMotionPosition position)
        {
            AsuMotionError ret = AsuMotionSetCurrentPostion(handle, ref position);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        /// <summary>
        /// 添加直线运动轨迹
        /// 返回0 配置成功，
        /// 返回2 配置失败，
        /// 返回1 参数handle为空指针，一般因为没有打开设备导致
        /// 返回5 前状态下不能添加卐千的规划，因为缓冲区已经满了
        /// 返回6 前状态下不能进行卐千的规划，因为前面提交的其他操作还未完成
        /// </summary>
        /// <param name="end">目的终点</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        public static int Asu_AsuMotionAddLine(AsuMotionPosition endPosition, double vel, double ini_maxvel, double acc)
        {
            AsuMotionError ret = AsuMotionAddLine(handle, ref endPosition, vel, ini_maxvel, acc);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                case AsuMotionError.AsuMotion_Error: return 2;
                case AsuMotionError.AsuMotion_Buffer_Full: return 5;
                default: return 6; // AsuMotion_CurrentState_Isnot_PCPlan
            }
        }

        /// <summary>
        /// AsuMotion PC规划添加圆弧  由目的终点，圆心，法线确定圆平面
        /// </summary>
        /// <param name="endPositon">目的终点</param>
        /// <param name="center">圆心</param>
        /// <param name="normal">法线终点，起点为原点</param>
        /// <param name="turn"></param>
        /// <param name="vel">期望圆弧运动的速度</param>
        /// <param name="ini_maxvel">最大速度</param>
        /// <param name="acc">加速度</param>
        public static int Asu_AsuMotionAddCircle(IntPtr AsuMotion,
                ref AsuMotionPosition endPositon,
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
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
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
        public static int Asu_AsuMotionSetStopType(AsuMotionStopType type, double tolerance)
        {
            AsuMotionError ret = AsuMotionSetStopType(handle, type, tolerance);
            switch (ret)
            {
                case AsuMotionError.AsuMotion_Error_Ok: return 0;
                case AsuMotionError.AsuMotion_Error_NullPointer: return 1;
                default: return 2; // AsuMotion_Error
            }
        }

        #endregion
    }
}
