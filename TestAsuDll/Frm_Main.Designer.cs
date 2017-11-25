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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_getDeviceNum
            // 
            this.btn_getDeviceNum.Location = new System.Drawing.Point(526, 12);
            this.btn_getDeviceNum.Name = "btn_getDeviceNum";
            this.btn_getDeviceNum.Size = new System.Drawing.Size(121, 23);
            this.btn_getDeviceNum.TabIndex = 0;
            this.btn_getDeviceNum.Text = "获取当前设备数量";
            this.btn_getDeviceNum.UseVisualStyleBackColor = true;
            this.btn_getDeviceNum.Click += new System.EventHandler(this.GetDeviceNum);
            // 
            // btn_getDeviceInfo
            // 
            this.btn_getDeviceInfo.Location = new System.Drawing.Point(526, 41);
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
            this.btn_openDevice.Location = new System.Drawing.Point(670, 12);
            this.btn_openDevice.Name = "btn_openDevice";
            this.btn_openDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_openDevice.TabIndex = 3;
            this.btn_openDevice.Text = "打开设备";
            this.btn_openDevice.UseVisualStyleBackColor = true;
            this.btn_openDevice.Click += new System.EventHandler(this.OpenDevice);
            // 
            // btn_AsuMotionConfigDeviceDefault
            // 
            this.btn_AsuMotionConfigDeviceDefault.Location = new System.Drawing.Point(765, 12);
            this.btn_AsuMotionConfigDeviceDefault.Name = "btn_AsuMotionConfigDeviceDefault";
            this.btn_AsuMotionConfigDeviceDefault.Size = new System.Drawing.Size(113, 23);
            this.btn_AsuMotionConfigDeviceDefault.TabIndex = 4;
            this.btn_AsuMotionConfigDeviceDefault.Text = "设备初始化为默认";
            this.btn_AsuMotionConfigDeviceDefault.UseVisualStyleBackColor = true;
            this.btn_AsuMotionConfigDeviceDefault.Click += new System.EventHandler(this.AsuMotionConfigDeviceDefault);
            // 
            // btn_closeDevice
            // 
            this.btn_closeDevice.Location = new System.Drawing.Point(670, 41);
            this.btn_closeDevice.Name = "btn_closeDevice";
            this.btn_closeDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_closeDevice.TabIndex = 3;
            this.btn_closeDevice.Text = "关闭设备";
            this.btn_closeDevice.UseVisualStyleBackColor = true;
            this.btn_closeDevice.Click += new System.EventHandler(this.CloseDevice);
            // 
            // btn_AsuMotionStopAll
            // 
            this.btn_AsuMotionStopAll.Location = new System.Drawing.Point(765, 42);
            this.btn_AsuMotionStopAll.Name = "btn_AsuMotionStopAll";
            this.btn_AsuMotionStopAll.Size = new System.Drawing.Size(113, 36);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(765, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 36);
            this.button1.TabIndex = 7;
            this.button1.Text = "停止由板卡规划的特定的轴的运动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AsuMotionStop);
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 497);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
    }
}

