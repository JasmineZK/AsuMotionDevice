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
        public Frm_Main()
        {
            InitializeComponent();

            InitLog4net();
        }

        #region 打印日志 初始化Log4Net

        private void Print(string text)
        {
            if (this.richTextBox1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.richTextBox1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.richTextBox1.Disposing || this.richTextBox1.IsDisposed)
                        return;
                }
                PrintLog d = new PrintLog(Print);
                this.richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text = text + "\r\n";
            }
        }

        /// 初始化 Log4net
        private void InitLog4net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        #endregion

        private void GetDeviceNum(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_GetDeviceNum();
            Print("当前设备数：" + ret);
            LogHelper.WriteLog("当前设备数：" + ret);
        }

        private void GetDeviceInfo(object sender, EventArgs e)
        {
            byte[] serial = new byte[100];
            int ret = AsuInvoke.Asu_GetDeviceInfo(0, serial);
            switch (ret)
            {
                case 0:
                    string s = Encoding.UTF8.GetString(serial);
                    Print("AsuMotion_Error_Ok 正常，设备 " + 0 + " 的序号为：" + s);
                    LogHelper.WriteLog("AsuMotion_Error_Ok 正常，设备 " + 0 + " 的序号为：" + s);
                    break;
                case 1:
                    Print("AsuMotion_Error_NullPointer 空指针");
                    LogHelper.WriteLog("AsuMotion_Error_NullPointer 空指针");
                    break;
                case 2:
                    Print("AsuMotion_Error 有错误发生");
                    LogHelper.WriteLog("AsuMotion_Error 有错误发生");
                    break;
                case 3:
                    Print("AsuMotion_Error_True 真");
                    LogHelper.WriteLog("AsuMotion_Error_True 真");
                    break;
                case 4:
                    Print("AsuMotion_Error_False 假");
                    LogHelper.WriteLog("AsuMotion_Error_False 假");
                    break;
            }
        }

        private void OpenDevice(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_AsuMotionOpen(0);
            if (ret == 0)
            {
                Print("打开设备" + 0 + "成功");
                LogHelper.WriteLog("打开设备" + 0 + "成功");
            }
            else
            {
                Print("打开设备" + 0 + "失败");
                LogHelper.WriteLog("打开设备" + 0 + "失败");
            }
        }

        private void CloseDevice(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_AsuMotionClose();
            if (ret == 0)
            {
                Print("设备" + 0 + "关闭成功");
                LogHelper.WriteLog("设备" + 0 + "关闭成功");
            }
            else
            {
                Print("设备" + 0 + "关闭失败");
                LogHelper.WriteLog("设备" + 0 + "关闭失败");
            }
        }

        private void AsuMotionConfigDeviceDefault(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_AsuMotionConfigDeviceDefault();
            if (ret == 0)
            {
                Print("设备" + 0 + "初始化为默认成功");
                LogHelper.WriteLog("设备" + 0 + "初始化为默认成功");

            }
            else
            {
                Print("设备" + 0 + "初始化为默认失败");
                LogHelper.WriteLog("设备" + 0 + "初始化为默认失败");
            }
        }

        private void AsuMotionStopAll(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_AsuMotionStopAll();
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


    }
}
