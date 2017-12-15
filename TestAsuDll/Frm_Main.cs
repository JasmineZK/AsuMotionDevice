using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AsuDll;

namespace TestAsuDll
{
    public delegate void PrintLog(string text); // 打印日志 委托

    /// <summary>
    /// 测试 AsuDll
    /// </summary>
    public partial class Frm_Main : Form
    {
        IntPtr handle; // 设备句柄
        int CountOfDevice; // 设备总数
        int NumOfCurDevice; // 当前设备序号
        AsuDll.AsuMotionAxisData PositionGiven = new AsuDll.AsuMotionAxisData() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 }; // 坐标轴数据
        AsuDll.AsuMotionAxisDataInt StepsPerUnit = new AsuDll.AsuMotionAxisDataInt() { x = 2000, y = 2000, z = 2000, a = 2000, b = 2000, c = 2000, u = 2000, v = 2000, w = 2000 }; // 单位脉冲数
        AsuDll.AsuMotionAxisDataInt WorkOffset = new AsuDll.AsuMotionAxisDataInt() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 }; // 工作偏移
        AsuDll.AsuMotionAxisData Accelaration = new AsuDll.AsuMotionAxisData() { x = 20, y = 20, z = 20, a = 20, b = 20, c = 20, u = 20, v = 20, w = 20 }; // 各个轴的加速度
        AsuDll.AsuMotionAxisData MaxSpeed = new AsuDll.AsuMotionAxisData() { x = 60, y = 60, z = 60, a = 60, b = 60, c = 60, u = 60, v = 60, w = 60 }; // 各个轴的最大速度
        AsuDll.AsuMotionAxisData SoftPositiveLimit = new AsuDll.AsuMotionAxisData() { x = 600, y = 600, z = 600, a = 60, b = 60, c = 60, u = 60, v = 60, w = 60 }; // 正向软限位坐标
        AsuDll.AsuMotionAxisData SoftNegtiveLimit = new AsuDll.AsuMotionAxisData() { x = -600, y = -600, z = 2, a = 2, b = 2, c = 2, u = 2, v = 2, w = 2 }; // 负向软限位坐标
        AsuDll.AsuMotionAxisDataInt fullSteps = new AsuDll.AsuMotionAxisDataInt() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 };
        ushort IO;
        byte[] Serial = new byte[100];
        byte[] Path = new byte[200];
        ushort[] Digital = new ushort[6];
        ushort[] Analog = new ushort[6];
        ushort SpindlePos;
        AsuDll.AsuMotionAxisData AsuMotionPos = new AsuDll.AsuMotionAxisData() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 };
        byte[] DiffFunSel = new byte[16] { 0x00, 0x10, 0x01, 0x11, 0x02, 0x12, 0x03, 0x13, 0x04, 0x14, 0x05, 0x15, 0x06, 0x16, 0x07, 0x17 }; // 差分输出信号

        public Frm_Main()
        {
            InitializeComponent();
        }

        #region 打印日志

        private void Print(string text)
        {
            if (this.richTextBox1.InvokeRequired) // 如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.richTextBox1.IsHandleCreated)
                {
                    // 解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.richTextBox1.Disposing || this.richTextBox1.IsDisposed)
                        return;
                }
                PrintLog d = new PrintLog(Print);
                this.richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                string past = this.richTextBox1.Text;
                this.richTextBox1.Text = DateTime.Now + ": " + text + "\r\n" + past;
            }
        }

        #endregion

        private void GetDeviceNum(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_GetDeviceNum();
            if (ret == 0)
            {
                CountOfDevice = 0;
                this.cbb_DeviceInfo.Items.Add("No Device Found");
                this.cbb_DeviceInfo.SelectedIndex = 0;
                Print("没有设备连接");
            }
            else
            {
                CountOfDevice = ret;
                while (ret > 0)
                {
                    this.cbb_DeviceInfo.Items.Add(ret);
                    ret--;
                }
                Print("有 " + ret + " 台设备连接");
            }
        }

        private void GetDeviceInfo(object sender, EventArgs e)
        {
            byte[] serial = new byte[100];
            NumOfCurDevice = this.cbb_DeviceInfo.SelectedIndex + 1;
            //NumOfCurDevice = 1;
            int ret = AsuInvoke.Asu_GetDeviceInfo(NumOfCurDevice, out serial);
            if (ret == 3)
            {
                Print("获取设备 " + NumOfCurDevice + " 信息成功，序列号为：" + Encoding.UTF8.GetString(serial));
            }
            else
            {
                Print("获取设备 " + NumOfCurDevice + " 信息失败");
            }
        }

        private void AsuMotionOpen(object sender, EventArgs e)
        {
            NumOfCurDevice = this.cbb_DeviceInfo.SelectedIndex + 1;
            //NumOfCurDevice = 1;
            handle = AsuInvoke.AsuMotion_Open(NumOfCurDevice);
            if (handle == IntPtr.Zero)
            {
                Print("打开设备 " + NumOfCurDevice + " 成功");
            }
            else
            {
                Print("打开设备 " + NumOfCurDevice + " 失败");
            }
        }

        private void AsuMotionClose(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_Close(handle);
            if (ret == 3)
            {
                Print("设备 " + NumOfCurDevice + " 关闭 成功");
            }
            else
            {
                Print("设备 " + NumOfCurDevice + " 关闭 失败");
            }
        }

        private void AsuMotionConfigDeviceDefault(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_ConfigDeviceDefault(handle);
            if (ret == 3)
            {
                Print("设备 " + NumOfCurDevice + " 初始化为默认成功");
            }
            else
            {
                Print("设备 " + NumOfCurDevice + " 初始化为默认失败");
            }
        }

        private void AsuMotionSetSmoothCoff(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_Motion_SetSmoothCoff(handle, 100, 64);
            if (ret == 3)
            {
                Print("配置光滑系数和脉冲延时 成功");
            }
            else
            {
                Print("配置光滑系数和脉冲延时 失败");
            }
        }

        private void AsuMotionSetStepsPerUnit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetStepsPerUnit(handle, ref StepsPerUnit);
            if (ret == 3)
            {
                Print("配置单位脉冲数 成功");
            }
            else
            {
                Print("配置单位脉冲数 失败");
            }
        }

        private void AsuMotionSetWorkOffset(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetWorkOffset(handle, ref WorkOffset);
            if (ret == 3)
            {
                Print("配置工作偏移 成功");
            }
            else
            {
                Print("配置工作偏移 失败");
            }
        }

        private void AsuMotionSetDifferentialOutputMapping(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetDifferentialOutputMapping(handle, DiffFunSel, (AsuMotionBitMaskType)0);
            if (ret == 3)
            {
                Print("配置运动卡差分输出的信号映射 成功");
            }
            else
            {
                Print("配置运动卡差分输出的信号映射 失败");
            }
        }

        private void AsuMotionSetAccelaration(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetAccelaration(handle, ref Accelaration);
            if (ret == 3)
            {
                Print("配置运动卡规划运动的加速度 成功");
            }
            else
            {
                Print("配置运动卡规划运动的加速度 失败");
            }
        }

        private void AsuMotionSetMaxSpeed(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetMaxSpeed(handle, ref MaxSpeed);
            if (ret == 3)
            {
                Print("配置运动卡规划运动的最大速度 成功");
            }
            else
            {
                Print("配置运动卡规划运动的最大速度 失败");
            }
        }

        private void AsuMotionStopAll(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_CardPlanStopAll(handle);
            if (ret == 3)
            {
                Print("停止由运动控制卡规划的所有轴的运动 成功");
            }
            else
            {
                Print("停止由运动控制卡规划的所有轴的运动 失败");
            }
        }

        private void AsuMotionIsDone(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_IsDone(handle);
            if (ret == 3)
            {
                Print("当前运动卡处于 空闲 状态");
            }
            else if (ret == 4)
            {
                Print("当前运动卡处于 运动 状态");
            }
            else // -1
            {
                Print("查询当前运动卡状态 发生异常");
            }
        }

        private void AsuMotionGetSteps(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_GetSteps(handle, out fullSteps);
            if (ret == 3)
            {
                Print("当前机器坐标位置脉冲数获取 成功");
            }
            else
            {
                Print("当前机器坐标位置脉冲数获取 失败");
            }
        }

        private void AsuMotionSetMachineCoordinate(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetMachineCoordinate(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All, PositionGiven);
            if (ret == 3)
            {
                Print("配置机器坐标 当前运动卡处于 空闲 状态");
            }
            else
            {
                Print("配置机器坐标 当前运动卡处于 运动 状态");
            }
        }

        private void AsuMotionSetSoftPositiveLimit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetSoftPositiveLimit(handle, ref SoftPositiveLimit);
            if (ret == 3)
            {
                Print("配置正向软限位 成功");
            }
            else
            {
                Print("配置正向软限位 失败");
            }
        }

        private void AsuMotionSetSoftNegtiveLimit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetSoftNegtiveLimit(handle, ref SoftNegtiveLimit);
            if (ret == 3)
            {
                Print("配置反向软限位 成功");
            }
            else
            {
                Print("配置反向软限位 失败");
            }
        }

        private void AsuMotionJogOn_X_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_X, +100);
            switch (ret)
            {
                case 0:
                    Print("X 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("X 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("X 轴正向点动运行 失败");
                    break;
                default:
                    Print("X 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionJogOn_X_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_X, -100);
            switch (ret)
            {
                case 0:
                    Print("X 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("X 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("X 轴反向点动运行 失败");
                    break;
                default:
                    Print("X 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionJogOn_Y_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Y, +100);
            switch (ret)
            {
                case 0:
                    Print("Y 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("Y 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Y 轴正向点动运行 失败");
                    break;
                default:
                    Print("Y 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionJogOn_Y_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Y, -100);
            switch (ret)
            {
                case 0:
                    Print("Y 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("Y 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Y 轴反向点动运行 失败");
                    break;
                default:
                    Print("Y 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionJogOn_Z_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Z, +100);
            switch (ret)
            {
                case 0:
                    Print("Z 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("Z 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Z 轴正向点动运行 失败");
                    break;
                default:
                    Print("Z 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionJogOn_Z_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_JogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Z, -100);
            switch (ret)
            {
                case 0:
                    Print("Z 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("Z 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Z 轴反向点动运行 失败");
                    break;
                default:
                    Print("Z 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }



        private void AsuMotionPause(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_Pause(handle);
            switch (ret)
            {
                case 0:
                    Print("暂停当前运动 成功");
                    break;
                case 1:
                    Print("暂停当前运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("暂停当前运动 失败");
                    break;
            }
        }

        private void AsuMotionResume(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_Resume(handle);
            switch (ret)
            {
                case 0:
                    Print("恢复暂停的运动 成功");
                    break;
                case 1:
                    Print("恢复暂停的运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("恢复暂停的运动 失败");
                    break;
            }
        }

        private void AsuMotionEStop(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_EStop(handle);
            switch (ret)
            {
                case 3:
                    Print("急停 成功");
                    break;
                default:
                    Print("急停 失败");
                    break;
            }
        }

        private void AsuMotionMoveAtConstSpeed(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_MoveAtConstSpeed(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All);
            switch (ret)
            {
                case 0:
                    Print("常速运行 成功");
                    break;
                case 1:
                    Print("常速运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("常速运行 失败");
                    break;
                default:
                    Print("常速运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionSetCurrentPostion(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_SetCurrentPostion(handle, ref AsuMotionPos);
            switch (ret)
            {
                case 0:
                    Print("设置当前坐标 成功");
                    break;
                case 1:
                    Print("设置当前坐标 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("设置当前坐标 失败");
                    break;
            }
        }

        private void AsuMotionAddLine(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_AddLine(handle, ref AsuMotionPos, 200, 5000, 50);
            switch (ret)
            {
                case 0:
                    Print("添加直线插补规划 成功");
                    break;
                case 1:
                    Print("添加直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加直线插补规划 失败");
                    break;
                case 5:
                    Print("添加直线插补规划 失败，缓冲区已满");
                    break;
                default:
                    Print("添加直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionAddLineWithSyncIO(object sender, EventArgs e)
        {
            ushort[] DIO = new ushort[3];
            ushort[] AIO = new ushort[3];

            DIO[0] = 0x0202;

            int ret = AsuInvoke.AsuMotion_AddLineWithSyncIO(handle, ref AsuMotionPos, 200, 5000, 50, DIO, AIO);
            switch (ret)
            {
                case 0:
                    Print("添加同步IO直线插补规划 成功");
                    break;
                case 1:
                    Print("添加同步IO直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加同步IO直线插补规划 失败");
                    break;
                case 5:
                    Print("添加同步IO直线插补规划 失败，缓冲区已满");
                    break;
                default:
                    Print("添加同步IO直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionGetOutputIO(object sender, EventArgs e)
        {
            ushort[] DIO = new ushort[3];
            DIO[0] = 0x503;

            int ret = AsuInvoke.AsuMotion_GetOutputIO(handle, out DIO);
            switch (ret)
            {
                case 3:
                    Print("输出口状态获取 成功");
                    break;
                default:
                    Print("输出口状态获取 失败");
                    break;
            }
        }

        private void AsuMotionGoHome(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_GoHome(handle, AsuMotionHomingType.AsuMotion_Homing_AtSwitch, AsuMotionAxisMaskType.AsuMotion_AxisMask_All, 2, ref Accelaration, ref MaxSpeed);
            switch (ret)
            {
                case 0:
                    Print("请求一次回原点 成功");
                    break;
                case 1:
                    Print("请求一次回原点 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("请求一次回原点 失败，当前状态下不能进行回原点，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionDirectPlanAddMoveIncrease(object sender, EventArgs e)
        {
            AsuDll.AsuMotionDirectMoveInc Inc = new AsuDll.AsuMotionDirectMoveInc() { };

            int ret = AsuInvoke.AsuMotion_DirectPlanAddMoveIncrease(handle, ref Inc);
            switch (ret)
            {
                case 0:
                    Print("添加一次坐标增量 成功");
                    break;
                case 1:
                    Print("添加一次坐标增量 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("添加一次坐标增量 失败，当前缓冲区满，等待一会再添加");
                    break;
            }
        }

        private void AsuMotionDirectPlanChangeIO(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_DirectPlanChangeIO(handle, Digital, Analog);
            switch (ret)
            {
                case 0:
                    Print("添加一次IO变化 成功");
                    break;
                case 1:
                    Print("添加一次IO变化 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("添加一次IO变化 失败，当前缓冲区满，等待一会再添加");
                    break;
            }
        }

        private void AsuMotionDirectPlanFlush(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_DirectPlanFlush(handle);
            switch (ret)
            {
                case 0:
                    Print("将缓冲区中添加的直接规划刷新 成功");
                    break;
                case 1:
                    Print("将缓冲区中添加的直接规划刷新 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("将缓冲区中添加的直接规划刷新 失败，当前缓冲区满，等待一会再添加");
                    break;
            }
        }

        private void AsuMotionAbort(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionAbort(handle);
            switch (ret)
            {
                case 0:
                    Print("放弃当前的运动规划 成功");
                    break;
                case 1:
                    Print("放弃当前的运动规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("放弃当前的运动规划 失败");
                    break;
            }
        }

        private void AsuMotionSetStopType(object sender, EventArgs e)
        {
            double tolerance = 1.6;
            int ret = AsuInvoke.Asu_MotionSetStopType(handle, AsuMotionStopType.AsuMotion_Stop_Type_Exact, tolerance);
            switch (ret)
            {
                case 0:
                    Print("设置停止类型 成功");
                    break;
                case 1:
                    Print("设置停止类型 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                default:
                    Print("设置停止类型 失败");
                    break;
            }
        }

        private void AsuMotionAddCircle(object sender, EventArgs e)
        {
            AsuDll.AsuMotionAxisData end = new AsuMotionAxisData() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 }; // 坐标轴数据
            AsuDll.AsuMotionCartesian center = new AsuMotionCartesian() { x = 1, y = 1, z = 1 };
            AsuDll.AsuMotionCartesian normal = new AsuMotionCartesian() { x = 2, y = 2, z = 2 };
            int turn = 1;
            double vel = 1;
            double ini_maxvel = 1;
            double acc = 1;

            int ret = AsuInvoke.Asu_MotionAddCircle(handle, ref end, ref center, ref normal, turn, vel, ini_maxvel, acc);
            switch (ret)
            {
                case 0:
                    Print("添加空间圆弧插补规划 成功");
                    break;
                case 1:
                    Print("添加空间圆弧插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加空间圆弧插补规划 失败");
                    break;
                case 5:
                    Print("添加空间圆弧插补规划 失败，缓冲区已满");
                    break;
                default:
                    Print("添加空间圆弧插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionAddCircleWithSyncIO(object sender, EventArgs e)
        {
            AsuDll.AsuMotionAxisData end = new AsuDll.AsuMotionAxisData() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 }; // 坐标轴数据
            AsuDll.AsuMotionCartesian center = new AsuDll.AsuMotionCartesian() { x = 1, y = 1, z = 1 };
            AsuDll.AsuMotionCartesian normal = new AsuDll.AsuMotionCartesian() { x = 2, y = 2, z = 2 };
            int turn = 1;
            double vel = 1;
            double ini_maxvel = 1;
            double acc = 1;

            int ret = AsuInvoke.AsuMotion_AddCircleWithSyncIO(handle, ref end, ref center, ref normal, turn, vel, ini_maxvel, acc, Digital, Analog);
            switch (ret)
            {
                case 0:
                    Print("添加同步IO空间圆弧插补规划 成功");
                    break;
                case 1:
                    Print("添加同步IO空间圆弧插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加同步IO空间圆弧插补规划 失败");
                    break;
                case 5:
                    Print("添加同步IO空间圆弧插补规划 失败，缓冲区已满");
                    break;
                default:
                    Print("添加同步IO空间圆弧插补规划 失败，当前状态下不能进行PC的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionMoveAbsolute(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_MoveAbsolute(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All, ref PositionGiven);
            switch (ret)
            {
                case 0:
                    Print("绝对位置移动 成功");
                    break;
                case 1:
                    Print("绝对位置移动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("绝对位置移动 失败");
                    break;
                default:
                    Print("绝对位置移动 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
            }
        }

        private void AsuMotionGetInputIO(object sender, EventArgs e)
        {
            ushort[] Input = new ushort[2];

            int ret = AsuInvoke.AsuMotion_GetInputIO(handle, out Input);
            switch (ret)
            {
                case 3:
                    Print("输入口状态获取 成功");
                    break;
                default:
                    Print("输入口状态获取 失败");
                    break;
            }
        }

        private void AsuMotionGetMaxSpeed(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_GetMaxSpeed(handle, out MaxSpeed);
            switch (ret)
            {
                case 3:
                    Print("当前各坐标轴最大速度获取 成功");
                    break;
                default:
                    Print("当前各坐标轴最大速度获取 失败");
                    break;
            }
        }

        private void AsuMotionGetSmoothCoff(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_GetSmoothCoff(handle, out uint pSmoothCoff);
            switch (ret)
            {
                case 3:
                    Print("光滑系数获取 成功");
                    break;
                default:
                    Print("光滑系数获取 失败");
                    break;
            }
        }

        private void AsuMotionGetStepsPerUnit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_GetStepsPerUnit(handle, out AsuMotionAxisDataInt stepsPerUnit);
            switch (ret)
            {
                case 3:
                    Print("单位距离脉冲数获取 成功");
                    break;
                default:
                    Print("单位距离脉冲数获取 失败");
                    break;
            }
        }

        private void AsuMotionCardPlanStop(object sender, EventArgs e)
        {
            int ret = AsuInvoke.AsuMotion_CardPlanStop(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All);
            switch (ret)
            {
                case 3:
                    Print("停止由运动控制卡规划的特定轴的运动 成功");
                    break;
                default:
                    Print("停止由运动控制卡规划的特定轴的运动 失败");
                    break;
            }
        }

        private void AsuMotionSetInputIOEngineDir(object sender, EventArgs e)
        {
            byte[] InputIOPin = new byte[64];
            InputIOPin[0] = 0;  //X++正限位 I0端口
            //	InputIOPin[1] = 0;  //X--负限位 
            //	InputIOPin[2] = 0;	//x零点  
            InputIOPin[3] = 1;  //Y++正限位 I1端口
            //	InputIOPin[4] = 1;  //Y++
            //	InputIOPin[5] = 1;  //Y零点 
            InputIOPin[25] = 3; //急停  I3

            int ret = AsuInvoke.AsuMotion_SetInputIOEngineDir(handle, 9, 0, InputIOPin);
            switch (ret)
            {
                case 3:
                    Print("配置运动卡数字量输入功能 成功");
                    break;
                default:
                    Print("配置运动卡数字量输入功能 失败");
                    break;
            }
        }

        private void AsuMotionSetOutput(object sender, EventArgs e)
        {
            ushort Sync = 1;
            short[] AnalogOut = new short[2] { 123, 234};
            ushort[] DigitalOut = new ushort[2] { 123, 234 };

            int ret = AsuInvoke.AsuMotion_SetOutput(handle, Sync, AnalogOut, DigitalOut);
            switch (ret)
            {
                case 3:
                    Print("配置运动卡模拟量输出和数字量输出 成功");
                    break;
                default:
                    Print("配置运动卡模拟量输出和数字量输出 失败");
                    break;
            }
        }

        private void AsuMotionSetHomingSignal(object sender, EventArgs e)
        {
            byte[] InputIOPin = new byte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9};

            int ret = AsuInvoke.AsuMotion_SetHomingSignal(handle, InputIOPin);
            switch (ret)
            {
                case 0:
                    Print("配置回原点的管脚 成功");
                    break;
                default:
                    Print("配置回原点的管脚 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
            }
        }
    }
}
