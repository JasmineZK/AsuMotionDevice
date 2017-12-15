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
    /// 坐标轴数据 int
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionAxisDataInt
    {
        public int x, y, z;
        public int a, b, c;
        public int u, v, w;
    }

    /// <summary>
    /// 坐标轴数据 double
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionAxisData
    {
        public double x, y, z;
        public double a, b, c;
        public double u, v, w;
    }

    /// <summary>
    /// 笛卡尔坐标类型 double
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionCartesian
    {
        public double x, y, z;
    }

    /// <summary>
    /// 直接规划坐标增量类型 short
    /// </summary>
    public struct AsuMotionDirectMoveInc
    {
        public short x, y, z;
        public short a, b, c;
        public short u, v, w;
    }

    #endregion

    public class AsuSdkHelper
    {
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string AsuMotionGetErrorMessage(AsuMotionError errorCode);

        #region AsuMotion控制卡设备操作函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern int GetDeviceNum();

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError GetDeviceInfo(int Num, out byte[] SerialString);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern IntPtr AsuMotionOpen(int Num);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionClose(IntPtr AsuMotion);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionConfigDeviceDefault(IntPtr AsuMotion);

        #endregion

        #region 直接控制规划相关函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionDirectPlanAddMoveIncrease(IntPtr AsuMotion, ref AsuMotionDirectMoveInc inc);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionDirectPlanChangeIO(IntPtr AsuMotion, ushort[] DIO, ushort[] AIO);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionDirectPlanFlush(IntPtr AsuMotion);

        #endregion

        #region PC运动规划相关函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetCurrentPostion(IntPtr AsuMotion, ref AsuMotionAxisData Pos);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionPause(IntPtr AsuMotion);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionResume(IntPtr AsuMotion);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAbort(IntPtr AsuMotion);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStopType(IntPtr AsuMotion, AsuMotionStopType type, double tolerance);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddLine(IntPtr AsuMotion, ref AsuMotionAxisData end, double vel, double ini_maxvel, double acc);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddCircle(IntPtr AsuMotion,
            ref AsuMotionAxisData end,
            ref AsuMotionCartesian center,
            ref AsuMotionCartesian normal,
            int turn,
            double vel,
            double ini_maxvel,
            double acc);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddLineWithSyncIO(IntPtr AsuMotion,
            ref AsuMotionAxisData end,
            double vel,
            double ini_maxvel,
            double acc,
            ushort[] DIO,
            ushort[] AIO);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionAddCircleWithSyncIO(IntPtr AsuMotion,
            ref AsuMotionAxisData end,
            ref AsuMotionCartesian center,
            ref AsuMotionCartesian normal,
            int turn,
            double vel,
            double ini_maxvel,
            double acc,
            ushort[] DIO,
            ushort[] AIO);

        #endregion

        #region 运动控制卡规划相关的函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionMoveAtConstSpeed(IntPtr AsuMotion, AsuMotionAxisMaskType AxisMask);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionJogOn(IntPtr AsuMotion, AsuMotionAxisIndexType Axis, double PositionGiven);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionMoveAbsolute(IntPtr AsuMotion, AsuMotionAxisMaskType AxisMask, ref AsuMotionAxisData PositionGiven);

        #endregion

        #region 运动控制卡状态获取相关的函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetInputIO(IntPtr AsuMotion, out ushort[] Input);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetOutputIO(IntPtr AsuMotion, out ushort[] Output);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetSteps(IntPtr AsuMotion, out AsuMotionAxisDataInt Steps);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetMaxSpeed(IntPtr AsuMotion, out AsuMotionAxisData MaxSpeed);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetSmoothCoff(IntPtr AsuMotion, out uint pSmoothCoff);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionGetStepsPerUnit(IntPtr AsuMotion, out AsuMotionAxisDataInt StepsPerUnit);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionIsDone(IntPtr AsuMotion);

        #endregion

        #region 停止运动函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionEStop(IntPtr AsuMotion);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionCardPlanStop(IntPtr AsuMotion, AsuMotionAxisMaskType Axis);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionCardPlanStopAll(IntPtr AsuMotion);

        #endregion

        #region PC规划与运动控制卡规划共用的配置函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetMachineCoordinate(IntPtr AsuMotion, AsuMotionAxisMaskType AxisMask, ref AsuMotionAxisData Position);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStepsPerUnit(IntPtr AsuMotion, ref AsuMotionAxisDataInt StepsPerUnit);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetWorkOffset(IntPtr AsuMotion, ref AsuMotionAxisDataInt WorkOffset);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSmoothCoff(IntPtr AsuMotion, ushort DelayBetweenPulseAndDir, int SmoothCoff);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetDifferentialOutputMapping(IntPtr AsuMotion, byte[] FunSel, AsuMotionBitMaskType NegMask);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetInputIOEngineDir(IntPtr AsuMotion,
                UInt64 InputIOEnable,
                UInt64 InputIONeg,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] InputIOPin);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionSetOutput(IntPtr AsuMotion, ushort Sync, short[] AnalogOut, ushort[] DigitalOut);

        #endregion

        #region 回原点相关的参数配置函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionSetHomingSignal(IntPtr AsuMotion, byte[] InputIOPin);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern AsuMotionError AsuMotionGoHome(IntPtr AsuMotion,
            AsuMotionHomingType Type,
            AsuMotionAxisMaskType AxisMask,
            ushort Count,
            ref AsuMotionAxisData Acceleration,
            ref AsuMotionAxisData MaxSpeed);

        #endregion

        #region 运动控制卡规划相关的参数配置函数

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetAccelaration(IntPtr AsuMotion, ref AsuMotionAxisData Acceleration);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetMaxSpeed(IntPtr AsuMotion, ref AsuMotionAxisData MaxSpeed);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSoftPositiveLimit(IntPtr AsuMotion, ref AsuMotionAxisData SoftPositiveLimit);

        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetSoftNegtiveLimit(IntPtr AsuMotion, ref AsuMotionAxisData SoftNegtiveLimit);

        #endregion  
    }
}
