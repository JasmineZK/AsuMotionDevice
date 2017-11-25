using System;
using System.Runtime.InteropServices;

namespace AsuDll
{
    #region Enum and Struct

    public enum AsuMotionStopType
    {
        AsuMotion_Stop_Type_Stop = 0,
        AsuMotion_Stop_Type_Exact = 1,
        AsuMotion_Stop_Type_Parabolic = 2,
        AsuMotion_Stop_Type_Tangent = 3
    }

    public enum AsuMotionError
    {
        AsuMotion_Error_Ok = 0,
        AsuMotion_Error_NullPointer = 1,
        AsuMotion_Error = 2,
        AsuMotion_True = 3,
        AsuMotion_False = 4,
        AsuMotion_Buffer_Full = 5,
        AsuMotion_CurrentState_Isnot_PCPlan = 6,
        AsuMotion_CurrentState_Isnot_Idle = 7,
        AsuMotion_CurrentState_Isnot_CardPlan = 8,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionPosition
    {
        public double x, y, z;
        public double a, b, c;
        public double u, v, w;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct AsuMotionCartesian
    {
        public double x, y, z;
    }

    #endregion

    public class AsuSdkHelper
    {
        #region  通用功能
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
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]byte[] InputIOPin,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]sbyte[] EngineDirections
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
        public static extern AsuMotionError GetDeviceInfo(int Num, [Out, MarshalAs(UnmanagedType.LPArray)] byte[] SerialString);

        /// <summary>
        /// 断开与AsuMotion的通讯
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionClose(IntPtr AsuMotion);

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
        public static extern AsuMotionError AsuMotionGetSteps(IntPtr AsuMotion, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]Int32[] Steps);

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
        /// 配置板卡参数：脉冲每毫米，细分数，脉冲方向延时
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="DelayBetweenPulseAndDir">脉冲方向延时</param>
        /// <param name="StepsPerAxis">脉冲每毫米 0 --> X轴脉冲每毫米 1 --> Y轴脉冲每毫米</param>
        /// <param name="WorkOffset">保留参数，暂不使用</param>
        /// <param name="SmoothCoff">细分数</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionSetStepsPerUnitSmoothCoff(IntPtr AsuMotion, ushort DelayBetweenPulseAndDir,
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]int[] StepsPerAxis,
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)]int[] WorkOffset, int SmoothCoff);

        #endregion

        #region  板卡规划运动
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
        /// <param name="Axis">运动轴使能掩码，0 --> 任何轴的运动都被禁止， 1 --> X轴运动使能 3 --> X,Y轴运动使能</param>
        /// <param name="PositionGiven">运动目的位置，单位毫米 0 --> X 1 --> Y 2 --> Z 3 --> A</param>
        /// <returns></returns>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionJogOn(IntPtr AsuMotion, ushort Axis, ref double PositionGiven);
        #endregion

        #region  PC规划运动
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
        /// 当前的PC规划运动已经结束
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
        public static extern AsuMotionError AsuMotionSetCurrentPostion(IntPtr AsuMotion, ref AsuMotionPosition Pos);//DONE

        /// <summary>
        /// 添加直线运动轨迹
        /// </summary>
        /// <param name="AsuMotion">AsuMotion资源句柄</param>
        /// <param name="end">目的终点</param>
        /// <param name="vel">期望直线运动的速度</param>
        /// <param name="ini_maxvel">最大允许速度</param>
        /// <param name="acc">加速度</param>
        [DllImport("AsuMotionDevice.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern AsuMotionError AsuMotionAddLine(IntPtr AsuMotion, ref AsuMotionPosition end, double vel, double ini_maxvel, double acc);


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
                ref AsuMotionPosition end,
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
        #endregion
    }
}
