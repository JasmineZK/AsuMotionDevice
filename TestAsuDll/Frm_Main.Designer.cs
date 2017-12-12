namespace TestAsuDll
{
    partial class Frm_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            this.btn_getDeviceNum = new System.Windows.Forms.Button();
            this.btn_getDeviceInfo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cbb_DeviceInfo = new System.Windows.Forms.ComboBox();
            this.btn_openDevice = new System.Windows.Forms.Button();
            this.btn_AsuMotionConfigDeviceDefault = new System.Windows.Forms.Button();
            this.btn_closeDevice = new System.Windows.Forms.Button();
            this.btn_AsuMotionStopAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_getDeviceNum
            // 
            this.btn_getDeviceNum.Location = new System.Drawing.Point(399, 55);
            this.btn_getDeviceNum.Name = "btn_getDeviceNum";
            this.btn_getDeviceNum.Size = new System.Drawing.Size(121, 23);
            this.btn_getDeviceNum.TabIndex = 0;
            this.btn_getDeviceNum.Text = "获取当前设备数量";
            this.btn_getDeviceNum.UseVisualStyleBackColor = true;
            this.btn_getDeviceNum.Click += new System.EventHandler(this.GetDeviceNum);
            // 
            // btn_getDeviceInfo
            // 
            this.btn_getDeviceInfo.Location = new System.Drawing.Point(399, 84);
            this.btn_getDeviceInfo.Name = "btn_getDeviceInfo";
            this.btn_getDeviceInfo.Size = new System.Drawing.Size(121, 23);
            this.btn_getDeviceInfo.TabIndex = 0;
            this.btn_getDeviceInfo.Text = "获取当前设备信息";
            this.btn_getDeviceInfo.UseVisualStyleBackColor = true;
            this.btn_getDeviceInfo.Click += new System.EventHandler(this.GetDeviceInfo);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(359, 439);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // cbb_DeviceInfo
            // 
            this.cbb_DeviceInfo.FormattingEnabled = true;
            this.cbb_DeviceInfo.Location = new System.Drawing.Point(399, 12);
            this.cbb_DeviceInfo.Name = "cbb_DeviceInfo";
            this.cbb_DeviceInfo.Size = new System.Drawing.Size(121, 20);
            this.cbb_DeviceInfo.TabIndex = 2;
            // 
            // btn_openDevice
            // 
            this.btn_openDevice.Location = new System.Drawing.Point(399, 113);
            this.btn_openDevice.Name = "btn_openDevice";
            this.btn_openDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_openDevice.TabIndex = 3;
            this.btn_openDevice.Text = "打开设备";
            this.btn_openDevice.UseVisualStyleBackColor = true;
            this.btn_openDevice.Click += new System.EventHandler(this.AsuMotionOpen);
            // 
            // btn_AsuMotionConfigDeviceDefault
            // 
            this.btn_AsuMotionConfigDeviceDefault.Location = new System.Drawing.Point(399, 142);
            this.btn_AsuMotionConfigDeviceDefault.Name = "btn_AsuMotionConfigDeviceDefault";
            this.btn_AsuMotionConfigDeviceDefault.Size = new System.Drawing.Size(113, 23);
            this.btn_AsuMotionConfigDeviceDefault.TabIndex = 4;
            this.btn_AsuMotionConfigDeviceDefault.Text = "设备初始化为默认";
            this.btn_AsuMotionConfigDeviceDefault.UseVisualStyleBackColor = true;
            this.btn_AsuMotionConfigDeviceDefault.Click += new System.EventHandler(this.AsuMotionConfigDeviceDefault);
            // 
            // btn_closeDevice
            // 
            this.btn_closeDevice.Location = new System.Drawing.Point(480, 113);
            this.btn_closeDevice.Name = "btn_closeDevice";
            this.btn_closeDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_closeDevice.TabIndex = 3;
            this.btn_closeDevice.Text = "关闭设备";
            this.btn_closeDevice.UseVisualStyleBackColor = true;
            this.btn_closeDevice.Click += new System.EventHandler(this.AsuMotionClose);
            // 
            // btn_AsuMotionStopAll
            // 
            this.btn_AsuMotionStopAll.Location = new System.Drawing.Point(399, 345);
            this.btn_AsuMotionStopAll.Name = "btn_AsuMotionStopAll";
            this.btn_AsuMotionStopAll.Size = new System.Drawing.Size(220, 23);
            this.btn_AsuMotionStopAll.TabIndex = 5;
            this.btn_AsuMotionStopAll.Text = "停止由运动控制卡规划的所有轴的运动";
            this.btn_AsuMotionStopAll.UseVisualStyleBackColor = true;
            this.btn_AsuMotionStopAll.Click += new System.EventHandler(this.AsuMotionStopAll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 473);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(399, 171);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "配置光滑系数和脉冲延时";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AsuMotionSetSmoothCoff);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(399, 374);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(197, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "查询当前运动卡是否处于运动状态中";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.AsuMotionIsDone);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(399, 200);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "配置单位脉冲数";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.AsuMotionSetStepsPerUnit);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(399, 258);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(182, 23);
            this.button7.TabIndex = 11;
            this.button7.Text = "配置运动卡差分输出的信号映射";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.AsuMotionSetDifferentialOutputMapping);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(399, 229);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(87, 23);
            this.button8.TabIndex = 11;
            this.button8.Text = "配置工作偏移";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.AsuMotionSetWorkOffset);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(399, 287);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(170, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "配置运动卡规划运动的加速度";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.AsuMotionSetAccelaration);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(399, 316);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(182, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "配置运动卡规划运动的最大速度";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.AsuMotionSetMaxSpeed);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(507, 461);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(102, 23);
            this.button11.TabIndex = 12;
            this.button11.Text = "配置反向软限位";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.AsuMotionSetSoftNegtiveLimit);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(399, 461);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(102, 23);
            this.button12.TabIndex = 12;
            this.button12.Text = "配置正向软限位";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.AsuMotionSetSoftPositiveLimit);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(399, 403);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(182, 23);
            this.button13.TabIndex = 12;
            this.button13.Text = "当前机器坐标位置脉冲数获取";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.AsuMotionGetSteps);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(399, 432);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(87, 23);
            this.button14.TabIndex = 12;
            this.button14.Text = "配置机器坐标";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.AsuMotionSetMachineCoordinate);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(676, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Y：+100";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AsuMotionJogOn_Y_Increase);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(603, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "X：-100";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.AsuMotionJogOn_X_Decrease);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(676, 84);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 13;
            this.button10.Text = "Y：-100";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.AsuMotionJogOn_Y_Decrease);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(747, 55);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 13;
            this.button15.Text = "X：+100";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.AsuMotionJogOn_X_Increase);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(735, 126);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 13;
            this.button16.Text = "Z：+100";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.AsuMotionJogOn_Z_Increase);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(616, 126);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 23);
            this.button17.TabIndex = 13;
            this.button17.Text = "Z：-100";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.AsuMotionJogOn_Z_Decrease);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(663, 171);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(104, 23);
            this.button18.TabIndex = 13;
            this.button18.Text = "暂停当前运动";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.AsuMotionPause);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(663, 200);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(104, 23);
            this.button19.TabIndex = 13;
            this.button19.Text = "恢复暂停的运动";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.AsuMotionResume);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(663, 229);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(104, 23);
            this.button20.TabIndex = 13;
            this.button20.Text = "急停";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.AsuMotionEStop);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(587, 316);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 14;
            this.button21.Text = "常速运行";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.AsuMotionMoveAtConstSpeed);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(492, 432);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(89, 23);
            this.button22.TabIndex = 15;
            this.button22.Text = "设置当前坐标";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.AsuMotionSetCurrentPostion);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(587, 432);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(114, 23);
            this.button23.TabIndex = 16;
            this.button23.Text = "添加直线插补规划";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.AsuMotionAddLine);
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(707, 432);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(151, 23);
            this.button24.TabIndex = 16;
            this.button24.Text = "添加同步IO直线插补规划";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.AsuMotionAddLineWithSyncIO);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(707, 374);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(108, 23);
            this.button25.TabIndex = 16;
            this.button25.Text = "输出口状态获取";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.AsuMotionGetOutputIO);
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 497);
            this.Controls.Add(this.button25);
            this.Controls.Add(this.button24);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_AsuMotionStopAll);
            this.Controls.Add(this.btn_AsuMotionConfigDeviceDefault);
            this.Controls.Add(this.btn_closeDevice);
            this.Controls.Add(this.btn_openDevice);
            this.Controls.Add(this.cbb_DeviceInfo);
            this.Controls.Add(this.btn_getDeviceInfo);
            this.Controls.Add(this.btn_getDeviceNum);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestAsuDll";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_getDeviceNum;
        private System.Windows.Forms.Button btn_getDeviceInfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox cbb_DeviceInfo;
        private System.Windows.Forms.Button btn_openDevice;
        private System.Windows.Forms.Button btn_AsuMotionConfigDeviceDefault;
        private System.Windows.Forms.Button btn_closeDevice;
        private System.Windows.Forms.Button btn_AsuMotionStopAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
    }
}

