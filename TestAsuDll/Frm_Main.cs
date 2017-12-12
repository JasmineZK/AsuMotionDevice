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
using log4net.Config;

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
        short[] Analog = new short[6];
        ushort SpindlePos;
        AsuDll.AsuMotionAxisData AsuMotionPos = new AsuDll.AsuMotionAxisData() { x = 0, y = 0, z = 0, a = 0, b = 0, c = 0, u = 0, v = 0, w = 0 };
        byte[] DiffFunSel = new byte[16] { 0x00, 0x10, 0x01, 0x11, 0x02, 0x12, 0x03, 0x13, 0x04, 0x14, 0x05, 0x15, 0x06, 0x16, 0x07, 0x17 }; // 差分输出信号

        public Frm_Main()
        {
            InitializeComponent();

            InitLog4net();
        }

        #region 打印日志 初始化Log4Net

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

        /// 初始化 Log4net
        private void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        #endregion

        #region TODO 

        private void AsuMotionStop(object sender, EventArgs e)
        {
            //int ret = AsuInvoke.Asu_MotionStop(handle, 0);
            //if (ret == 0)
            //{
            //    Print("停止由板卡规划的特定的轴的运动 成功");
            //    LogHelper.WriteLog("停止由板卡规划的特定的轴的运动 成功");
            //}
            //else
            //{
            //    Print("停止由板卡规划的特定的轴的运动 失败");
            //    LogHelper.WriteLog("停止由板卡规划的特定的轴的运动 失败");
            //}
        }

        private void AsuMotionSetInputIOEngineDir(object sender, EventArgs e)
        {
            //byte[] InputIOPin = new byte[128];
            //InputIOPin[0] = 0;  //X++正限位 I0端口
            ////	InputIOPin[1] = 0;  //X--负限位 
            ////	InputIOPin[2] = 0;	//x零点  
            //InputIOPin[3] = 1;  //Y++正限位 I1端口
            ////	InputIOPin[4] = 1;  //Y++
            ////	InputIOPin[5] = 1;  //Y零点 
            //InputIOPin[25] = 3; //急停  I3

            //int ret = AsuInvoke.Asu_MotionSetInputIOEngineDir(handle, 9, 0, InputIOPin);
            //if (ret == 0)
            //{
            //    Print("配置输入引脚的特殊功能 成功");
            //    LogHelper.WriteLog("配置输入引脚的特殊功能 成功");
            //}
            //else
            //{
            //    Print("配置输入引脚的特殊功能 失败");
            //    LogHelper.WriteLog("配置输入引脚的特殊功能 失败");
            //}
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
                LogHelper.WriteLog("没有设备连接");
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
                LogHelper.WriteLog("有 " + ret + " 台设备连接");
            }
        }

        private void GetDeviceInfo(object sender, EventArgs e)
        {
            byte[] serial = new byte[100];
            NumOfCurDevice = this.cbb_DeviceInfo.SelectedIndex + 1;
            //NumOfCurDevice = 1;
            int ret = AsuInvoke.Asu_GetDeviceInfo(NumOfCurDevice, out serial);
            if (ret == 0)
            {
                Print("获取设备 " + NumOfCurDevice + " 信息成功，序列号为：" + Encoding.UTF8.GetString(serial));
                LogHelper.WriteLog("获取设备 " + NumOfCurDevice + " 信息成功，序列号为：" + Encoding.UTF8.GetString(serial));
            }
            else
            {
                Print("获取设备 " + NumOfCurDevice + " 信息失败");
                LogHelper.WriteLog("获取设备 " + NumOfCurDevice + " 信息失败");
            }
        }

        private void AsuMotionOpen(object sender, EventArgs e)
        {
            NumOfCurDevice = this.cbb_DeviceInfo.SelectedIndex + 1;
            //NumOfCurDevice = 1;
            int ret = AsuInvoke.Asu_MotionOpen(NumOfCurDevice, out handle);
            if (ret == 0)
            {
                Print("打开设备 " + NumOfCurDevice + " 成功");
                LogHelper.WriteLog("打开设备 " + NumOfCurDevice + " 成功");
            }
            else
            {
                Print("打开设备 " + NumOfCurDevice + " 失败");
                LogHelper.WriteLog("打开设备 " + NumOfCurDevice + " 失败");
            }
        }

        private void AsuMotionClose(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionClose(handle);
            if (ret == 0)
            {
                Print("设备 " + NumOfCurDevice + " 关闭 成功");
                LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭 成功");
            }
            else
            {
                Print("设备 " + NumOfCurDevice + " 关闭 失败");
                LogHelper.WriteLog("设备 " + NumOfCurDevice + " 关闭 失败");
            }
        }

        private void AsuMotionConfigDeviceDefault(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionConfigDeviceDefault(handle);
            if (ret == 0)
            {
                Print("设备 " + NumOfCurDevice + " 初始化为默认成功");
                LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认成功");
            }
            else
            {
                Print("设备 " + NumOfCurDevice + " 初始化为默认失败");
                LogHelper.WriteLog("设备 " + NumOfCurDevice + " 初始化为默认失败");
            }
        }

        private void AsuMotionSetSmoothCoff(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetSmoothCoff(handle, 100, 64);
            if (ret == 0)
            {
                Print("配置光滑系数和脉冲延时 成功");
                LogHelper.WriteLog("配置光滑系数和脉冲延时 成功");
            }
            else
            {
                Print("配置光滑系数和脉冲延时 失败");
                LogHelper.WriteLog("配置光滑系数和脉冲延时 失败");
            }
        }

        private void AsuMotionSetStepsPerUnit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetStepsPerUnit(handle, StepsPerUnit);
            if (ret == 0)
            {
                Print("配置单位脉冲数 成功");
                LogHelper.WriteLog("配置单位脉冲数 成功");
            }
            else
            {
                Print("配置单位脉冲数 失败");
                LogHelper.WriteLog("配置单位脉冲数 失败");
            }
        }

        private void AsuMotionSetWorkOffset(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetWorkOffset(handle, WorkOffset);
            if (ret == 0)
            {
                Print("配置工作偏移 成功");
                LogHelper.WriteLog("配置工作偏移 成功");
            }
            else
            {
                Print("配置工作偏移 失败");
                LogHelper.WriteLog("配置工作偏移 失败");
            }
        }

        private void AsuMotionSetDifferentialOutputMapping(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetDifferentialOutputMapping(handle, DiffFunSel, (AsuMotionBitMaskType)0);
            if (ret == 0)
            {
                Print("配置运动卡差分输出的信号映射 成功");
                LogHelper.WriteLog("配置运动卡差分输出的信号映射 成功");
            }
            else
            {
                Print("配置运动卡差分输出的信号映射 失败");
                LogHelper.WriteLog("配置运动卡差分输出的信号映射 失败");
            }
        }

        private void AsuMotionSetAccelaration(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetAccelaration(handle, Accelaration);
            if (ret == 0)
            {
                Print("配置运动卡规划运动的加速度 成功");
                LogHelper.WriteLog("配置运动卡规划运动的加速度 成功");
            }
            else
            {
                Print("配置运动卡规划运动的加速度 失败");
                LogHelper.WriteLog("配置运动卡规划运动的加速度 失败");
            }
        }

        private void AsuMotionSetMaxSpeed(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetMaxSpeed(handle, MaxSpeed);
            if (ret == 0)
            {
                Print("配置运动卡规划运动的最大速度 成功");
                LogHelper.WriteLog("配置运动卡规划运动的最大速度 成功");
            }
            else
            {
                Print("配置运动卡规划运动的最大速度 失败");
                LogHelper.WriteLog("配置运动卡规划运动的最大速度 失败");
            }
        }

        private void AsuMotionStopAll(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionStopAll(handle);
            if (ret == 0)
            {
                Print("停止由运动控制卡规划的所有轴的运动 成功");
                LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 成功");
            }
            else
            {
                Print("停止由运动控制卡规划的所有轴的运动 失败");
                LogHelper.WriteLog("停止由运动控制卡规划的所有轴的运动 失败");
            }
        }

        private void AsuMotionIsDone(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionIsDone(handle);
            if (ret == 0)
            {
                Print("当前运动卡处于 空闲 状态");
                LogHelper.WriteLog("当前运动卡处于 空闲 状态");
            }
            else if (ret == 1)
            {
                Print("当前运动卡处于 运动 状态");
                LogHelper.WriteLog("当前运动卡处于 运动 状态");
            }
            else // -1
            {
                Print("查询当前运动卡状态 发生异常");
                LogHelper.WriteLog("查询当前运动卡状态 发生异常");
            }
        }

        private void AsuMotionGetSteps(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionGetSteps(handle, out fullSteps);
            if (ret == 0)
            {
                Print("当前机器坐标位置脉冲数获取 成功");
                LogHelper.WriteLog("当前机器坐标位置脉冲数获取 成功");
            }
            else
            {
                Print("当前机器坐标位置脉冲数获取 失败");
                LogHelper.WriteLog("当前机器坐标位置脉冲数获取 失败");
            }
        }

        private void AsuMotionSetMachineCoordinate(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetMachineCoordinate(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All, PositionGiven);
            if (ret == 0)
            {
                Print("配置机器坐标 当前运动卡处于 空闲 状态");
                LogHelper.WriteLog("配置机器坐标 当前运动卡处于 空闲 状态");
            }
            else
            {
                Print("配置机器坐标 当前运动卡处于 运动 状态");
                LogHelper.WriteLog("配置机器坐标 当前运动卡处于 运动 状态");
            }
        }

        private void AsuMotionSetSoftPositiveLimit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetSoftPositiveLimit(handle, SoftPositiveLimit);
            if (ret == 0)
            {
                Print("配置正向软限位 成功");
                LogHelper.WriteLog("配置正向软限位 成功");
            }
            else
            {
                Print("配置正向软限位 失败");
                LogHelper.WriteLog("配置正向软限位 失败");
            }
        }

        private void AsuMotionSetSoftNegtiveLimit(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetSoftNegtiveLimit(handle, SoftNegtiveLimit);
            if (ret == 0)
            {
                Print("配置反向软限位 成功");
                LogHelper.WriteLog("配置反向软限位 成功");
            }
            else
            {
                Print("配置反向软限位 失败");
                LogHelper.WriteLog("配置反向软限位 失败");
            }
        }

        private void AsuMotionJogOn_X_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_X, +100);
            switch (ret)
            {
                case 0:
                    Print("X 轴正向点动运行 成功");
                    LogHelper.WriteLog("X 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("X 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("X 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("X 轴正向点动运行 失败");
                    LogHelper.WriteLog("X 轴正向点动运行 失败");
                    break;
                case 8:
                    Print("X 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("X 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionJogOn_X_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_X, -100);
            switch (ret)
            {
                case 0:
                    Print("X 轴反向点动运行 成功");
                    LogHelper.WriteLog("X 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("X 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("X 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("X 轴反向点动运行 失败");
                    LogHelper.WriteLog("X 轴反向点动运行 失败");
                    break;
                case 8:
                    Print("X 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("X 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionJogOn_Y_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Y, +100);
            switch (ret)
            {
                case 0:
                    Print("Y 轴正向点动运行 成功");
                    LogHelper.WriteLog("Y 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("Y 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("Y 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Y 轴正向点动运行 失败");
                    LogHelper.WriteLog("Y 轴正向点动运行 失败");
                    break;
                case 8:
                    Print("Y 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("Y 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionJogOn_Y_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Y, -100);
            switch (ret)
            {
                case 0:
                    Print("Y 轴反向点动运行 成功");
                    LogHelper.WriteLog("Y 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("Y 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("Y 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Y 轴反向点动运行 失败");
                    LogHelper.WriteLog("Y 轴反向点动运行 失败");
                    break;
                case 8:
                    Print("Y 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("Y 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionJogOn_Z_Increase(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Z, +100);
            switch (ret)
            {
                case 0:
                    Print("Z 轴正向点动运行 成功");
                    LogHelper.WriteLog("Z 轴正向点动运行 成功");
                    break;
                case 1:
                    Print("Z 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("Z 轴正向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Z 轴正向点动运行 失败");
                    LogHelper.WriteLog("Z 轴正向点动运行 失败");
                    break;
                case 8:
                    Print("Z 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("Z 轴正向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionJogOn_Z_Decrease(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionJogOn(handle, AsuMotionAxisIndexType.AsuMotion_AxisIndex_Z, -100);
            switch (ret)
            {
                case 0:
                    Print("Z 轴反向点动运行 成功");
                    LogHelper.WriteLog("Z 轴反向点动运行 成功");
                    break;
                case 1:
                    Print("Z 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("Z 轴反向点动运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("Z 轴反向点动运行 失败");
                    LogHelper.WriteLog("Z 轴反向点动运行 失败");
                    break;
                case 8:
                    Print("Z 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("Z 轴反向点动运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }



        private void AsuMotionPause(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionPause(handle);
            switch (ret)
            {
                case 0:
                    Print("暂停当前运动 成功");
                    LogHelper.WriteLog("暂停当前运动 成功");
                    break;
                case 1:
                    Print("暂停当前运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("暂停当前运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("暂停当前运动 失败");
                    LogHelper.WriteLog("暂停当前运动 失败");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionResume(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionResume(handle);
            switch (ret)
            {
                case 0:
                    Print("恢复暂停的运动 成功");
                    LogHelper.WriteLog("恢复暂停的运动 成功");
                    break;
                case 1:
                    Print("恢复暂停的运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("恢复暂停的运动 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("恢复暂停的运动 失败");
                    LogHelper.WriteLog("恢复暂停的运动 失败");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionEStop(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionEStop(handle);
            switch (ret)
            {
                case 0:
                    Print("急停 成功");
                    LogHelper.WriteLog("急停 成功");
                    break;
                case 1:
                    Print("急停 失败");
                    LogHelper.WriteLog("急停 失败");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionMoveAtConstSpeed(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionMoveAtConstSpeed(handle, AsuMotionAxisMaskType.AsuMotion_AxisMask_All);
            switch (ret)
            {
                case 0:
                    Print("常速运行 成功");
                    LogHelper.WriteLog("常速运行 成功");
                    break;
                case 1:
                    Print("常速运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("常速运行 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("常速运行 失败");
                    LogHelper.WriteLog("常速运行 失败");
                    break;
                case 8:
                    Print("常速运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("常速运行 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionSetCurrentPostion(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionSetCurrentPostion(handle, AsuMotionPos);
            switch (ret)
            {
                case 0:
                    Print("设置当前坐标 成功");
                    LogHelper.WriteLog("设置当前坐标 成功");
                    break;
                case 1:
                    Print("设置当前坐标 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("设置当前坐标 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("设置当前坐标 失败");
                    LogHelper.WriteLog("设置当前坐标 失败");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionAddLine(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_MotionAddLine(handle, AsuMotionPos, 200, 5000, 50);
            switch (ret)
            {
                case 0:
                    Print("添加直线插补规划 成功");
                    LogHelper.WriteLog("添加直线插补规划 成功");
                    break;
                case 1:
                    Print("添加直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("添加直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加直线插补规划 失败");
                    LogHelper.WriteLog("添加直线插补规划 失败");
                    break;
                case 5:
                    Print("添加直线插补规划 失败，缓冲区已满");
                    LogHelper.WriteLog("添加直线插补规划 失败，缓冲区已满");
                    break;
                case 6:
                    Print("添加直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("添加直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionAddLineWithSyncIO(object sender, EventArgs e)
        {
            ushort[] DIO = new ushort[3];
            ushort[] AIO = new ushort[3];

            DIO[0] = 0x0202;

            int ret = AsuInvoke.Asu_MotionAddLineWithSyncIO(handle, AsuMotionPos, 200, 5000, 50, DIO, AIO);
            switch (ret)
            {
                case 0:
                    Print("添加同步IO直线插补规划 成功");
                    LogHelper.WriteLog("添加同步IO直线插补规划 成功");
                    break;
                case 1:
                    Print("添加同步IO直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，设备句柄为空指针，一般因为没有打开设备导致");
                    break;
                case 2:
                    Print("添加同步IO直线插补规划 失败");
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败");
                    break;
                case 5:
                    Print("添加同步IO直线插补规划 失败，缓冲区已满");
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，缓冲区已满");
                    break;
                case 6:
                    Print("添加同步IO直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    LogHelper.WriteLog("添加同步IO直线插补规划 失败，当前状态下不能进行运动控制卡的规划，因为前面提交的其他操作还未完成");
                    break;
                default:
                    break;
            }
        }

        private void AsuMotionGetOutputIO(object sender, EventArgs e)
        {
            ushort[] DIO = new ushort[3];
            DIO[0] = 0x503;

            int ret = AsuInvoke.Asu_MotionGetOutputIO(handle, DIO);
            switch (ret)
            {
                case 0:
                    Print("输出口状态获取 成功");
                    LogHelper.WriteLog("输出口状态获取 成功");
                    break;
                case 1:
                    Print("输出口状态获取 失败");
                    LogHelper.WriteLog("输出口状态获取 失败");
                    break;
                default:
                    break;
            }
        }
    }
}
