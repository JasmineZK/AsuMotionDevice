using System;
using System.Runtime.InteropServices;

namespace AsuDll
{
    #region Enum and Struct

    /// <summary>
    /// 运动函数停止类型
    /// </summary>
    public enum AsuMotionStopType
    {
        AsuMotion_Stop_Type_Stop = 0,
        AsuMotion_Stop_Type_Exact = 1,
        AsuMotion_Stop_Type_Parabolic = 2,
        AsuMotion_Stop_Type_Tangent = 3
    }

    /// <summary>
    /// 控制卡回原点模
    /// </summary>
    public enum AsuMotionHomingType
    {
        AsuMotion_Homing_NoMoving = 0x0000,
        AsuMotion_Homing_AtSwitch = 0x0001,
        AsuMotion_Homing_LeaveSwitch = 0x0002,
        AsuMotion_Homing_MultipleHome = 0x0003,
    }

    /// <summary>
    /// 轴索引选取类型
    /// </summary>
    public enum AsuMotionAxisIndexType
    {
        AsuMotion_AxisIndex_X = 0x0000,
        AsuMotion_AxisIndex_Y = 0x0001,
        AsuMotion_AxisIndex_Z = 0x0002,
        AsuMotion_AxisIndex_A = 0x0003,
        AsuMotion_AxisIndex_B = 0x0004,
        AsuMotion_AxisIndex_C = 0x0005,
        AsuMotion_AxisIndex_U = 0x0006,
        AsuMotion_AxisIndex_V = 0x0007,
        AsuMotion_AxisIndex_W = 0x0008
    }

    /// <summary>
    /// 位屏蔽类型
    /// </summary>
    public enum AsuMotionBitMaskType
    {
        AsuMotion_BitMask_NULL = 0x00000000,
        AsuMotion_BitMask_00 = 0x00000001,
        AsuMotion_BitMask_01 = 0x00000002,
        AsuMotion_BitMask_02 = 0x00000004,
        AsuMotion_BitMask_03 = 0x00000008,
        AsuMotion_BitMask_04 = 0x00000010,
        AsuMotion_BitMask_05 = 0x00000020,
        AsuMotion_BitMask_06 = 0x00000040,
        AsuMotion_BitMask_07 = 0x00000080,
        AsuMotion_BitMask_08 = 0x00000100,
        AsuMotion_BitMask_09 = 0x00000200,
        AsuMotion_BitMask_10 = 0x00000400,
        AsuMotion_BitMask_11 = 0x00000800,
        AsuMotion_BitMask_12 = 0x00001000,
        AsuMotion_BitMask_13 = 0x00002000,
        AsuMotion_BitMask_14 = 0x00004000,
        AsuMotion_BitMask_15 = 0x00008000,
        AsuMotion_BitMask_16 = 0x00010000,
        AsuMotion_BitMask_17 = 0x00020000,
        AsuMotion_BitMask_18 = 0x00040000,
        AsuMotion_BitMask_19 = 0x00080000,
        AsuMotion_BitMask_20 = 0x00100000,
        AsuMotion_BitMask_21 = 0x00200000,
        AsuMotion_BitMask_22 = 0x00400000,
        AsuMotion_BitMask_23 = 0x00800000,
        AsuMotion_BitMask_24 = 0x01000000,
        AsuMotion_BitMask_25 = 0x02000000,
        AsuMotion_BitMask_26 = 0x04000000,
        AsuMotion_BitMask_27 = 0x08000000,
        AsuMotion_BitMask_28 = 0x10000000,
        AsuMotion_BitMask_29 = 0x20000000,
        AsuMotion_BitMask_30 = 0x40000000,
        AsuMotion_BitMask_31 = 0x8000000 // 本为 0x80000000 ， 由于C#的enum仅支持Int32，0x80000000超过了最大值，设为0x8000000，后续使用时将其右移一位即可
    }

    /// <summary>
    /// 轴屏蔽选取类型
    /// </summary>
    public enum AsuMotionAxisMaskType
    {
        AsuMotion_AxisMask_X = 0x0001,
        AsuMotion_AxisMask_Y = 0x0002,
        AsuMotion_AxisMask_Z = 0x0004,
        AsuMotion_AxisMask_A = 0x0008,
        AsuMotion_AxisMask_B = 0x0010,
        AsuMotion_AxisMask_C = 0x0020,
        AsuMotion_AxisMask_U = 0x0040,
        AsuMotion_AxisMask_V = 0x0080,
        AsuMotion_AxisMask_W = 0x0100,
        AsuMotion_SpindleMask_X = 0x0200,
        AsuMotion_SpindleMask_Y = 0x0400,
        AsuMotion_SpindleMask_Z = 0x0800,
        AsuMotion_AxisMask_All = 0x01ff,
    }
    /// <summary>
    /// 函数错误类型
    /// </summary>
    public enum AsuMotionError
    {
        AsuMotion_Error_Ok = 0,
        AsuMotion_Device_Is_Null = 1,
        AsuMotion_Error = 2,
        AsuMotion_True = 3,
        AsuMotion_False = 4,
        AsuMotion_Buffer_Full = 5,
        AsuMotion_CurrentState_Isnot_PCPlan = 6,
        AsuMotion_CurrentState_Isnot_Idle = 7,
        AsuMotion_CurrentState_Isnot_CardPlan = 8,
    }

    /// <summary>
    /// 坐标轴数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionAxisDataInt
    {
        public int x, y, z;
        public int a, b, c;
        public int u, v, w;
    }

    /// <summary>
    /// 坐标轴数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionAxisData
    {
        public double x, y, z;
        public double a, b, c;
        public double u, v, w;
    }

    /// <summary>
    /// 笛卡尔坐标类型
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionCartesian
    {
        public double x, y, z;
    }

    #endregion

    public class AsuSdkHelper
    {
        /// <summary>
        /// 输出口状态获取
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="pOutput">IO状态缓冲区，长度至少为2，函数调用成功后，这里面的值将为IO状态</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetOutputIO(IntPtr AsuMotion, ushort[] pOutput);

        /// <summary>
        /// 添加同步IO直线插补规划
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="end">指定当前直线的终点坐标</param>
        /// <param name="vel">指定当前直线运行的速度</param>
        /// <param name="ini_maxvel">由上一段直线或者曲线转入当前直线或者曲线时，所允许的最大速度</param>
        /// <param name="acc">当前直线运行的加速度</param>
        /// <param name="DIO">需要在当前规划阶段设置的数字量输出，低位对齐。每位对应一个数字量输出</param>
        /// <param name="AIO">需要在当前规划阶段设置的模拟量输出。控制卡为12位DA输出输出，即4095对应满量程输出</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddLineWithSyncIO(IntPtr AsuMotion,
        AsuMotionAxisData end,
        double vel,
        double ini_maxvel,
        double acc,
        ushort[] DIO,
        ushort[] AIO);

        /// <summary>
        /// 常速运行
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="AxisMask">配置当前需要运行的轴</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionMoveAtConstSpeed(IntPtr AsuMotion, AsuMotionAxisMaskType AxisMask);

        /// <summary>   
        /// 开启与AsuMotion的通讯
        /// </summary>
        /// <param name="Num">为0:PC连接的第一个AsuMotion，为1:PC连接的第二个AsuMotion，在PC只与一个AsuMotion相连时，此项配为0即可</param>
        /// <returns>AsuMotion资源句柄</returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern IntPtr AsuMotionOpen(int Num);

        /// <summary>
        /// 获取主轴脉冲计数值
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="SpindlePostion">主轴脉冲计数值的存储区域</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetSpindlePostion(IntPtr AsuMotion, [MarshalAs(UnmanagedType.LPArray,SizeParamIndex=1)] ushort[] SpindlePostion);

        /// <summary>
        /// 配置运动卡差分输出的信号映射
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="FunSel">映射号数组，共16个元素，其值得设定将反应16个差分输出的内容</param>
        /// <param name="NegMask">设定16个差分输出口是否反向输出</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetDifferentialOutputMapping(IntPtr AsuMotion, byte[] FunSel, AsuMotionBitMaskType NegMask);

        /// <summary>
        /// 配置运动卡规划运动的加速度
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Acceleration">一个存储各轴加速度的结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetAccelaration(IntPtr AsuMotion, AsuMotionAxisData Acceleration);

        /// <summary>
        /// 配置运动卡规划运动的最大速度
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="MaxSpeed">一个存储各轴速度的结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetMaxSpeed(IntPtr AsuMotion, AsuMotionAxisData MaxSpeed);

        /// <summary>
        /// 配置机器坐标
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="AxisMask">配置当前需要运行的轴</param>
        /// <param name="Position">一个存储各轴机器坐标的结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetMachineCoordinate(IntPtr AsuMotion, AsuMotionAxisMaskType AxisMask, AsuMotionAxisData Position);

        /// <summary>
        /// 配置正向软限位
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="SoftPositiveLimit">正向软限位坐标的结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSoftPositiveLimit(IntPtr AsuMotion, AsuMotionAxisData SoftPositiveLimit);

        /// <summary>
        /// 配置正向软限位
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="SoftNegtiveLimit">反向软限位坐标的结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSoftNegtiveLimit(IntPtr AsuMotion, AsuMotionAxisData SoftNegtiveLimit);

        /// <summary>
        /// 板卡轴输出配置
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="StepSel"></param>
        /// <param name="DirSel">轴输出管脚配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值与管脚的对应需对应参考表</param>
        /// <param name="Enable">轴输出使能配置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴 值为true则使能</param>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetAxisOutputConfig(IntPtr AsuMotion, 
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]byte[] StepSel,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]byte[] DirSel,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]byte[] Enable,
                ushort StepNeg,
                ushort DirNeg);

        /// <summary>
        /// 配置输入引脚的特殊功能
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="InputIOEnable">输入引脚使能 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIONeg">输入引脚触发信号反相 bit0--> Input0 bit2--> Input2 bit7--> Input7</param>
        /// <param name="InputIOPin">输入引脚映射 0--> Input0 2--> Input2 7--> Input7</param>
        /// <param name="EngineDirections">改变运动方向</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetInputIOEngineDir(IntPtr AsuMotion,
                UInt64 InputIOEnable,
                UInt64 InputIONeg,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] InputIOPin
            );

        /// <summary>
        /// 获取当前PC检测到的AsuMotion数量
        /// </summary>
        /// <returns>与PC相连的AsuMotion总数</returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int GetDeviceNum();

        /// <summary>
        /// 获取AsuMotion板卡序列号
        /// </summary>
        /// <param name="Num">为0:PC连接的第一个AsuMotion，为1:PC连接的第二个AsuMotion，在PC只与一个AsuMotion相连时，此项配为0即可</param>
        /// <param name="SerialString">用于存储序列号的字节数组</param>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError GetDeviceInfo(int Num, out byte[] SerialString);

        /// <summary>
        /// 断开与AsuMotion的通讯
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionClose(IntPtr AsuMotion);

        /// <summary>
        /// 配置工作偏移
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="WorkOffset">工作偏移</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetWorkOffset(IntPtr AsuMotion,AsuMotionAxisDataInt WorkOffset);

        /// <summary>
        /// 初始化AsuMotion参数配置
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionConfigDeviceDefault(IntPtr AsuMotion);

        /// <summary>
        /// 获取输入信号
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Input">用于存储输入信号值的存储区域</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetInputIO(IntPtr AsuMotion, [Out, MarshalAs(UnmanagedType.LPArray)] ushort[] Input);

        /// <summary>
        /// 获取已AsuMotion各轴相对于机械原点的脉冲总量
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Steps">各轴相对于机械原点的脉冲总量的存储区域</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetSteps(IntPtr AsuMotion, out AsuMotionAxisDataInt Steps);

        /// <summary>
        /// 配置主轴输出
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Out">主轴速率配置值，最大为65535</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSpindle(IntPtr AsuMotion, ushort Out);


        /// <summary>
        /// 急停
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionEStop(IntPtr AsuMotion);

        /// <summary>
        /// 获取板卡细分数
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="pSmoothCoff">板卡细分数的存储区域</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetSmoothCoff(IntPtr AsuMotion, ref UInt32 pSmoothCoff);

        /// /// <summary>
        /// 配置光滑系数和脉冲延时
        /// </summary>
        /// <param name="DelayBetweenPulseAndDir">脉冲的有效沿和方向的有效沿之间有充分的时间差</param>
        /// <param name="SmoothCoff">光滑系数</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSmoothCoff(IntPtr AsuMotion, UInt16 DelayBetweenPulseAndDir, Int32 SmoothCoff);


        /// <summary>
        /// 获取板卡脉冲每毫米的参数配置值
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="StepsPerUnit">板卡脉冲每毫米的参数配置值的存储区域</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetStepsPerUnit(IntPtr AsuMotion, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] int[] StepsPerUnit);

        /// <summary>
        /// 配置板卡脉冲与方向输出的反相
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="StepNeg">脉冲反相掩码，0 --> 任何轴脉冲输出不反相， 1 --> X轴脉冲输出反相， 5 --> X与Z轴脉冲输出反相</param>
        /// <param name="DirNeg">方向反相掩码，0 --> 任何轴方向输出不反相， 1 --> X轴方向输出反相， 5 --> X与Z轴方向输出反相</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStepNegAndDirNeg(IntPtr AsuMotion, byte StepNeg, byte DirNeg);

        /// <summary>
        /// 配置单位脉冲数
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="StepsPerUnit">单位脉冲数</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStepsPerUnit(IntPtr AsuMotion, AsuMotionAxisDataInt StepsPerUnit);

        /// <summary>
        /// 相对于机械坐标的运动
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionMoveAbsolute(IntPtr AsuMotion, ushort AxisMask, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] double[] PositionGiven);

        /// <summary>
        /// 配置板卡规划运动的加速度与最大速度
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetAccelarationMaxSpeed(IntPtr AsuMotion,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Acceleration,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MaxSpeed);

        /// <summary>
        /// 距离无限远的运动
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="AxisMask">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="Acceleration">加速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MaxSpeed">最大允许速度 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionMoveAtSpeed(IntPtr AsuMotion, ushort AxisMask,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Acceleration,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MaxSpeed);

        /// <summary>
        /// 软限位配置
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="MaxSoftLimit">允许运动到的最大机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <param name="MinSoftLimit">允许运动到的最小机械位置 0 --> X轴 1 --> Y轴 2 --> Z轴 3 --> A轴</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSoftLimit(IntPtr AsuMotion,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] double[] MaxSoftLimit,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] MinSoftLimit);
        
        /// <summary>
        /// 获取板卡各轴最大运动速度
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="MaxSpeed">板卡各轴最大运动速度的存储区域，单位毫米每分钟</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        
        public static extern AsuMotionError AsuMotionGetMaxSpeed(IntPtr AsuMotion, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]Double[] MaxSpeed);
        /// <summary>
        /// 停止由板卡规划的所有轴运动
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionCardPlanStopAll(IntPtr AsuMotion);


        /// <summary>
        /// 停止由板卡规划的特定的轴的运动,NEED TEST
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Axis">0 --> X轴停止运动， 1 --> Y轴停止运动 2 --> Z轴停止运动</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionCardPlanStop(IntPtr AsuMotion, ushort Axis);

        /// <summary>
        /// 配置板卡当前的机械坐标
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Posi">机械坐标，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetCoordinate(IntPtr AsuMotion, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]double[] Posi);

        /// <summary>
        /// 点动运行，需要调用Stop停止
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Axis">配置当前需要运行的轴</param>
        /// <param name="PositionGiven">给定一个点动运行时，机器的目标位置。此位置为相对位置</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionJogOn(IntPtr AsuMotion, AsuMotionAxisIndexType Axis, double PositionGiven);

        /// <summary>
        /// 恢复经AsuMotionPause停止的运动PC规划运动
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionResume(IntPtr AsuMotion);

        /// <summary>
        /// 停止PC规划运动，可由AsuMotionResume恢复运动状态
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionPause(IntPtr AsuMotion);

        /// <summary>
        /// 终止PC规划运动，不可由AsuMotionResume恢复运动状态
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAbort(IntPtr AsuMotion);

        /// <summary>
        /// 查询当前运动卡是否处于运动状态中
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionIsDone(IntPtr AsuMotion);

        /// <summary>
        /// 配置当前AsuMotion PC规划运动的工作坐标
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="Pos">期望配置坐标值的位置结构体</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetCurrentPostion(IntPtr AsuMotion, ref AsuMotionAxisData Pos);//DONE

        /// <summary>
        /// 添加直线运动轨迹
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="end">目的终点</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddLine(IntPtr AsuMotion, ref AsuMotionAxisData end, double vel, double ini_maxvel, double acc);


        /// <summary>
        /// AsuMotion PC规划添加圆弧  由目的终点，圆心，法线确定圆平面
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="end">目的终点</param>
        /// <param name="center">圆心</param>
        /// <param name="normal">法线终点，起点为原点</param>
        /// <param name="turn"></param>
        /// <param name="vel">期望圆弧运动的速度</param>
        /// <param name="ini_maxvel">最大速度</param>
        /// <param name="acc">加速度</param>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddCircle(IntPtr AsuMotion,
                ref AsuMotionAxisData end,
                ref AsuMotionCartesian center,
                ref AsuMotionCartesian normal,
                int turn,
                double vel,
                double ini_maxvel,
                double acc
                );

        /// <summary>
        /// 设置PC规划运动的停止类型
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="type">停止类型 </param>
        /// <param name="tolerance">精度要求，目标位值和当前位值的绝对值小于此数时规划结束了</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStopType(IntPtr AsuMotion, AsuMotionStopType type, double tolerance);
    }
}
