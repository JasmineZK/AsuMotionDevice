namespace TestAsuDll
{
    partial class Form_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.btn_getDeviceNum = new System.Windows.Forms.Button();
            this.btn_getDeviceInfo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cbb_DeviceInfo = new System.Windows.Forms.ComboBox();
            this.btn_openDevice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_getDeviceNum
            // 
            this.btn_getDeviceNum.Location = new System.Drawing.Point(139, 12);
            this.btn_getDeviceNum.Name = "btn_getDeviceNum";
            this.btn_getDeviceNum.Size = new System.Drawing.Size(117, 23);
            this.btn_getDeviceNum.TabIndex = 0;
            this.btn_getDeviceNum.Text = "获取当前设备数量及设备信息";
            this.btn_getDeviceNum.UseVisualStyleBackColor = true;
            this.btn_getDeviceNum.Click += new System.EventHandler(this.btn_getDeviceNum_Click);
            // 
            // btn_getDeviceInfo
            // 
            this.btn_getDeviceInfo.Location = new System.Drawing.Point(139, 41);
            this.btn_getDeviceInfo.Name = "btn_getDeviceInfo";
            this.btn_getDeviceInfo.Size = new System.Drawing.Size(117, 23);
            this.btn_getDeviceInfo.TabIndex = 0;
            this.btn_getDeviceInfo.Text = "获取当前设备信息";
            this.btn_getDeviceInfo.UseVisualStyleBackColor = true;
            this.btn_getDeviceInfo.Click += new System.EventHandler(this.btn_getDeviceInfo_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 123);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(310, 171);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // cbb_DeviceInfo
            // 
            this.cbb_DeviceInfo.FormattingEnabled = true;
            this.cbb_DeviceInfo.Location = new System.Drawing.Point(12, 12);
            this.cbb_DeviceInfo.Name = "cbb_DeviceInfo";
            this.cbb_DeviceInfo.Size = new System.Drawing.Size(121, 20);
            this.cbb_DeviceInfo.TabIndex = 2;
            // 
            // btn_openDevice
            // 
            this.btn_openDevice.Location = new System.Drawing.Point(262, 12);
            this.btn_openDevice.Name = "btn_openDevice";
            this.btn_openDevice.Size = new System.Drawing.Size(75, 23);
            this.btn_openDevice.TabIndex = 3;
            this.btn_openDevice.Text = "打开设备";
            this.btn_openDevice.UseVisualStyleBackColor = true;
            this.btn_openDevice.Click += new System.EventHandler(this.btn_openDevice_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 471);
            this.Controls.Add(this.btn_openDevice);
            this.Controls.Add(this.cbb_DeviceInfo);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btn_getDeviceInfo);
            this.Controls.Add(this.btn_getDeviceNum);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestAsuDll";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_getDeviceNum;
        private System.Windows.Forms.Button btn_getDeviceInfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox cbb_DeviceInfo;
        private System.Windows.Forms.Button btn_openDevice;
    }
}

