using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        /// 打印日志
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

        /// <summary>
        /// 获取当前设备数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_getDeviceNum_Click(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_GetDeviceNum();
            Print("当前设备数：" + ret);
        }

        /// <summary>
        /// 获取当前设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_getDeviceInfo_Click(object sender, EventArgs e)
        {
            byte[] serial = new byte[100];
            int ret = AsuInvoke.Asu_GetDeviceInfo(0, serial);
            switch (ret)
            {
                case 0:
                    Print("AsuMotion_Error_Ok 正常，设备 " + 0 + " 的序号为：" + serial);
                    break;
                case 1:
                    Print("AsuMotion_Error_NullPointer 空指针"); break;
                case 2:
                    Print("AsuMotion_Error 有错误发生"); break;
                case 3:
                    Print("AsuMotion_Error_True 真"); break;
                case 4:
                    Print("AsuMotion_Error_False 假"); break;
            }
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_openDevice_Click(object sender, EventArgs e)
        {
            int ret = AsuInvoke.Asu_AsuMotionOpen(0);
            if (ret == 0)
            {
                Print("打开设备" + 0 + "成功");
            }
            else
            {
                Print("打开设备" + 0 + "失败");
            }
        }
    }
}
