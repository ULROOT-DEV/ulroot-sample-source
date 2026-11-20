namespace LocalUdpSample
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpStationInfo = new System.Windows.Forms.GroupBox();
            this.lblPortHint = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.lblModel = new System.Windows.Forms.Label();
            this.txtStationIp = new System.Windows.Forms.TextBox();
            this.lblStationIp = new System.Windows.Forms.Label();
            this.grpCommands = new System.Windows.Forms.GroupBox();
            this.btnReboot = new System.Windows.Forms.Button();
            this.btnGetVersion = new System.Windows.Forms.Button();
            this.btnGetScenes = new System.Windows.Forms.Button();
            this.btnGetDevices = new System.Windows.Forms.Button();
            this.grpEventReceiver = new System.Windows.Forms.GroupBox();
            this.lblReceiveStatus = new System.Windows.Forms.Label();
            this.btnToggleReceive = new System.Windows.Forms.Button();
            this.txtReceivePort = new System.Windows.Forms.TextBox();
            this.lblReceivePort = new System.Windows.Forms.Label();
            this.txtReceiveIp = new System.Windows.Forms.TextBox();
            this.lblReceiveIp = new System.Windows.Forms.Label();
            this.grpDeviceControl = new System.Windows.Forms.GroupBox();
            this.btnControlDevice = new System.Windows.Forms.Button();
            this.btnPresetOff = new System.Windows.Forms.Button();
            this.btnPresetOn = new System.Windows.Forms.Button();
            this.txtControlVal = new System.Windows.Forms.TextBox();
            this.lblControlVal = new System.Windows.Forms.Label();
            this.txtControlType = new System.Windows.Forms.TextBox();
            this.lblControlType = new System.Windows.Forms.Label();
            this.txtDeviceIdx = new System.Windows.Forms.TextBox();
            this.lblDeviceIdx = new System.Windows.Forms.Label();
            this.txtDeviceMe = new System.Windows.Forms.TextBox();
            this.lblDeviceMe = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.notifyTimer = new System.Windows.Forms.Timer(this.components);
            this.grpStationInfo.SuspendLayout();
            this.grpCommands.SuspendLayout();
            this.grpEventReceiver.SuspendLayout();
            this.grpDeviceControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpStationInfo
            // 
            this.grpStationInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStationInfo.Controls.Add(this.lblPortHint);
            this.grpStationInfo.Controls.Add(this.txtLocalPort);
            this.grpStationInfo.Controls.Add(this.lblLocalPort);
            this.grpStationInfo.Controls.Add(this.btnDiscover);
            this.grpStationInfo.Controls.Add(this.txtToken);
            this.grpStationInfo.Controls.Add(this.lblToken);
            this.grpStationInfo.Controls.Add(this.txtModel);
            this.grpStationInfo.Controls.Add(this.lblModel);
            this.grpStationInfo.Controls.Add(this.txtStationIp);
            this.grpStationInfo.Controls.Add(this.lblStationIp);
            this.grpStationInfo.Location = new System.Drawing.Point(14, 12);
            this.grpStationInfo.Name = "grpStationInfo";
            this.grpStationInfo.Size = new System.Drawing.Size(832, 96);
            this.grpStationInfo.TabIndex = 0;
            this.grpStationInfo.TabStop = false;
            this.grpStationInfo.Text = "Smart Station 접속 정보";
            // 
            // lblStationIp
            // 
            this.lblStationIp.AutoSize = true;
            this.lblStationIp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStationIp.ForeColor = System.Drawing.Color.Firebrick;
            this.lblStationIp.Location = new System.Drawing.Point(14, 28);
            this.lblStationIp.Name = "lblStationIp";
            this.lblStationIp.Size = new System.Drawing.Size(67, 15);
            this.lblStationIp.TabIndex = 0;
            this.lblStationIp.Text = "Station IP";
            // 
            // txtStationIp
            // 
            this.txtStationIp.Location = new System.Drawing.Point(92, 24);
            this.txtStationIp.Name = "txtStationIp";
            this.txtStationIp.Size = new System.Drawing.Size(145, 23);
            this.txtStationIp.TabIndex = 1;
            // 
            // btnDiscover
            // 
            this.btnDiscover.Location = new System.Drawing.Point(243, 23);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(78, 25);
            this.btnDiscover.TabIndex = 2;
            this.btnDiscover.Text = "자동 검색";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblModel.ForeColor = System.Drawing.Color.Firebrick;
            this.lblModel.Location = new System.Drawing.Point(338, 28);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(44, 15);
            this.lblModel.TabIndex = 3;
            this.lblModel.Text = "Model";
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(388, 24);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(230, 23);
            this.txtModel.TabIndex = 4;
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(634, 28);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(65, 15);
            this.lblLocalPort.TabIndex = 5;
            this.lblLocalPort.Text = "Local Port";
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(706, 24);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(110, 23);
            this.txtLocalPort.TabIndex = 6;
            this.txtLocalPort.Text = "12346";
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblToken.ForeColor = System.Drawing.Color.Firebrick;
            this.lblToken.Location = new System.Drawing.Point(14, 62);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(43, 15);
            this.lblToken.TabIndex = 7;
            this.lblToken.Text = "Token";
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(92, 58);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(526, 23);
            this.txtToken.TabIndex = 8;
            // 
            // lblPortHint
            // 
            this.lblPortHint.AutoSize = true;
            this.lblPortHint.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPortHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblPortHint.Location = new System.Drawing.Point(634, 62);
            this.lblPortHint.Name = "lblPortHint";
            this.lblPortHint.Size = new System.Drawing.Size(151, 13);
            this.lblPortHint.TabIndex = 9;
            this.lblPortHint.Text = "App: 12346 / QR: 임의포트";
            // 
            // grpCommands
            // 
            this.grpCommands.Controls.Add(this.btnReboot);
            this.grpCommands.Controls.Add(this.btnGetVersion);
            this.grpCommands.Controls.Add(this.btnGetScenes);
            this.grpCommands.Controls.Add(this.btnGetDevices);
            this.grpCommands.Location = new System.Drawing.Point(14, 116);
            this.grpCommands.Name = "grpCommands";
            this.grpCommands.Size = new System.Drawing.Size(390, 124);
            this.grpCommands.TabIndex = 1;
            this.grpCommands.TabStop = false;
            this.grpCommands.Text = "조회 / 설정 명령 (GET / SET config)";
            // 
            // btnGetDevices
            // 
            this.btnGetDevices.Location = new System.Drawing.Point(14, 26);
            this.btnGetDevices.Name = "btnGetDevices";
            this.btnGetDevices.Size = new System.Drawing.Size(175, 38);
            this.btnGetDevices.TabIndex = 0;
            this.btnGetDevices.Text = "디바이스 목록 (GET eps)";
            this.btnGetDevices.UseVisualStyleBackColor = true;
            this.btnGetDevices.Click += new System.EventHandler(this.btnGetDevices_Click);
            // 
            // btnGetScenes
            // 
            this.btnGetScenes.Location = new System.Drawing.Point(200, 26);
            this.btnGetScenes.Name = "btnGetScenes";
            this.btnGetScenes.Size = new System.Drawing.Size(175, 38);
            this.btnGetScenes.TabIndex = 1;
            this.btnGetScenes.Text = "씬 목록 (GET scene)";
            this.btnGetScenes.UseVisualStyleBackColor = true;
            this.btnGetScenes.Click += new System.EventHandler(this.btnGetScenes_Click);
            // 
            // btnGetVersion
            // 
            this.btnGetVersion.Location = new System.Drawing.Point(14, 72);
            this.btnGetVersion.Name = "btnGetVersion";
            this.btnGetVersion.Size = new System.Drawing.Size(175, 38);
            this.btnGetVersion.TabIndex = 2;
            this.btnGetVersion.Text = "버전 조회 (cfg:getver)";
            this.btnGetVersion.UseVisualStyleBackColor = true;
            this.btnGetVersion.Click += new System.EventHandler(this.btnGetVersion_Click);
            // 
            // btnReboot
            // 
            this.btnReboot.ForeColor = System.Drawing.Color.Firebrick;
            this.btnReboot.Location = new System.Drawing.Point(200, 72);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(175, 38);
            this.btnReboot.TabIndex = 3;
            this.btnReboot.Text = "스테이션 재부팅 (reboot)";
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // grpEventReceiver
            // 
            this.grpEventReceiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEventReceiver.Controls.Add(this.lblReceiveStatus);
            this.grpEventReceiver.Controls.Add(this.btnToggleReceive);
            this.grpEventReceiver.Controls.Add(this.txtReceivePort);
            this.grpEventReceiver.Controls.Add(this.lblReceivePort);
            this.grpEventReceiver.Controls.Add(this.txtReceiveIp);
            this.grpEventReceiver.Controls.Add(this.lblReceiveIp);
            this.grpEventReceiver.Location = new System.Drawing.Point(416, 116);
            this.grpEventReceiver.Name = "grpEventReceiver";
            this.grpEventReceiver.Size = new System.Drawing.Size(430, 124);
            this.grpEventReceiver.TabIndex = 2;
            this.grpEventReceiver.TabStop = false;
            this.grpEventReceiver.Text = "실시간 이벤트 수신 (Local Event - cfg:notify)";
            // 
            // lblReceiveIp
            // 
            this.lblReceiveIp.AutoSize = true;
            this.lblReceiveIp.Location = new System.Drawing.Point(14, 30);
            this.lblReceiveIp.Name = "lblReceiveIp";
            this.lblReceiveIp.Size = new System.Drawing.Size(47, 15);
            this.lblReceiveIp.TabIndex = 0;
            this.lblReceiveIp.Text = "수신 IP";
            // 
            // txtReceiveIp
            // 
            this.txtReceiveIp.Location = new System.Drawing.Point(76, 26);
            this.txtReceiveIp.Name = "txtReceiveIp";
            this.txtReceiveIp.Size = new System.Drawing.Size(150, 23);
            this.txtReceiveIp.TabIndex = 1;
            // 
            // lblReceivePort
            // 
            this.lblReceivePort.AutoSize = true;
            this.lblReceivePort.Location = new System.Drawing.Point(238, 30);
            this.lblReceivePort.Name = "lblReceivePort";
            this.lblReceivePort.Size = new System.Drawing.Size(60, 15);
            this.lblReceivePort.TabIndex = 2;
            this.lblReceivePort.Text = "수신 Port";
            // 
            // txtReceivePort
            // 
            this.txtReceivePort.Location = new System.Drawing.Point(304, 26);
            this.txtReceivePort.Name = "txtReceivePort";
            this.txtReceivePort.Size = new System.Drawing.Size(110, 23);
            this.txtReceivePort.TabIndex = 3;
            this.txtReceivePort.Text = "12350";
            // 
            // btnToggleReceive
            // 
            this.btnToggleReceive.Location = new System.Drawing.Point(14, 62);
            this.btnToggleReceive.Name = "btnToggleReceive";
            this.btnToggleReceive.Size = new System.Drawing.Size(145, 38);
            this.btnToggleReceive.TabIndex = 4;
            this.btnToggleReceive.Text = "이벤트 수신 시작";
            this.btnToggleReceive.UseVisualStyleBackColor = true;
            this.btnToggleReceive.Click += new System.EventHandler(this.btnToggleReceive_Click);
            // 
            // lblReceiveStatus
            // 
            this.lblReceiveStatus.AutoSize = true;
            this.lblReceiveStatus.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblReceiveStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblReceiveStatus.Location = new System.Drawing.Point(168, 74);
            this.lblReceiveStatus.Name = "lblReceiveStatus";
            this.lblReceiveStatus.Size = new System.Drawing.Size(244, 15);
            this.lblReceiveStatus.TabIndex = 5;
            this.lblReceiveStatus.Text = "상태: 대기 중 (Windows 방화벽 UDP 허용 필요)";
            // 
            // grpDeviceControl
            // 
            this.grpDeviceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDeviceControl.Controls.Add(this.btnControlDevice);
            this.grpDeviceControl.Controls.Add(this.btnPresetOff);
            this.grpDeviceControl.Controls.Add(this.btnPresetOn);
            this.grpDeviceControl.Controls.Add(this.txtControlVal);
            this.grpDeviceControl.Controls.Add(this.lblControlVal);
            this.grpDeviceControl.Controls.Add(this.txtControlType);
            this.grpDeviceControl.Controls.Add(this.lblControlType);
            this.grpDeviceControl.Controls.Add(this.txtDeviceIdx);
            this.grpDeviceControl.Controls.Add(this.lblDeviceIdx);
            this.grpDeviceControl.Controls.Add(this.txtDeviceMe);
            this.grpDeviceControl.Controls.Add(this.lblDeviceMe);
            this.grpDeviceControl.Location = new System.Drawing.Point(14, 248);
            this.grpDeviceControl.Name = "grpDeviceControl";
            this.grpDeviceControl.Size = new System.Drawing.Size(832, 70);
            this.grpDeviceControl.TabIndex = 3;
            this.grpDeviceControl.TabStop = false;
            this.grpDeviceControl.Text = "단일 서브 디바이스 제어 테스트 (SET ep)";
            // 
            // lblDeviceMe
            // 
            this.lblDeviceMe.AutoSize = true;
            this.lblDeviceMe.Location = new System.Drawing.Point(14, 32);
            this.lblDeviceMe.Name = "lblDeviceMe";
            this.lblDeviceMe.Size = new System.Drawing.Size(92, 15);
            this.lblDeviceMe.TabIndex = 0;
            this.lblDeviceMe.Text = "Device ID (me)";
            // 
            // txtDeviceMe
            // 
            this.txtDeviceMe.Location = new System.Drawing.Point(112, 28);
            this.txtDeviceMe.Name = "txtDeviceMe";
            this.txtDeviceMe.Size = new System.Drawing.Size(95, 23);
            this.txtDeviceMe.TabIndex = 1;
            // 
            // lblDeviceIdx
            // 
            this.lblDeviceIdx.AutoSize = true;
            this.lblDeviceIdx.Location = new System.Drawing.Point(218, 32);
            this.lblDeviceIdx.Name = "lblDeviceIdx";
            this.lblDeviceIdx.Size = new System.Drawing.Size(48, 15);
            this.lblDeviceIdx.TabIndex = 2;
            this.lblDeviceIdx.Text = "IO (idx)";
            // 
            // txtDeviceIdx
            // 
            this.txtDeviceIdx.Location = new System.Drawing.Point(272, 28);
            this.txtDeviceIdx.Name = "txtDeviceIdx";
            this.txtDeviceIdx.Size = new System.Drawing.Size(70, 23);
            this.txtDeviceIdx.TabIndex = 3;
            this.txtDeviceIdx.Text = "L1";
            // 
            // lblControlType
            // 
            this.lblControlType.AutoSize = true;
            this.lblControlType.Location = new System.Drawing.Point(354, 32);
            this.lblControlType.Name = "lblControlType";
            this.lblControlType.Size = new System.Drawing.Size(34, 15);
            this.lblControlType.TabIndex = 4;
            this.lblControlType.Text = "Type";
            // 
            // txtControlType
            // 
            this.txtControlType.Location = new System.Drawing.Point(394, 28);
            this.txtControlType.Name = "txtControlType";
            this.txtControlType.Size = new System.Drawing.Size(68, 23);
            this.txtControlType.TabIndex = 5;
            this.txtControlType.Text = "129";
            // 
            // lblControlVal
            // 
            this.lblControlVal.AutoSize = true;
            this.lblControlVal.Location = new System.Drawing.Point(474, 32);
            this.lblControlVal.Name = "lblControlVal";
            this.lblControlVal.Size = new System.Drawing.Size(24, 15);
            this.lblControlVal.TabIndex = 6;
            this.lblControlVal.Text = "Val";
            // 
            // txtControlVal
            // 
            this.txtControlVal.Location = new System.Drawing.Point(504, 28);
            this.txtControlVal.Name = "txtControlVal";
            this.txtControlVal.Size = new System.Drawing.Size(68, 23);
            this.txtControlVal.TabIndex = 7;
            this.txtControlVal.Text = "1";
            // 
            // btnPresetOn
            // 
            this.btnPresetOn.Location = new System.Drawing.Point(584, 26);
            this.btnPresetOn.Name = "btnPresetOn";
            this.btnPresetOn.Size = new System.Drawing.Size(52, 26);
            this.btnPresetOn.TabIndex = 8;
            this.btnPresetOn.Text = "ON";
            this.btnPresetOn.UseVisualStyleBackColor = true;
            this.btnPresetOn.Click += new System.EventHandler(this.btnPresetOn_Click);
            // 
            // btnPresetOff
            // 
            this.btnPresetOff.Location = new System.Drawing.Point(642, 26);
            this.btnPresetOff.Name = "btnPresetOff";
            this.btnPresetOff.Size = new System.Drawing.Size(52, 26);
            this.btnPresetOff.TabIndex = 9;
            this.btnPresetOff.Text = "OFF";
            this.btnPresetOff.UseVisualStyleBackColor = true;
            this.btnPresetOff.Click += new System.EventHandler(this.btnPresetOff_Click);
            // 
            // btnControlDevice
            // 
            this.btnControlDevice.Location = new System.Drawing.Point(706, 26);
            this.btnControlDevice.Name = "btnControlDevice";
            this.btnControlDevice.Size = new System.Drawing.Size(110, 26);
            this.btnControlDevice.TabIndex = 10;
            this.btnControlDevice.Text = "제어 전송";
            this.btnControlDevice.UseVisualStyleBackColor = true;
            this.btnControlDevice.Click += new System.EventHandler(this.btnControlDevice_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLog.Location = new System.Drawing.Point(16, 330);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(61, 15);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "통신 로그";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(752, 325);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(94, 25);
            this.btnClearLog.TabIndex = 5;
            this.btnClearLog.Text = "로그 지우기";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.BackColor = System.Drawing.Color.White;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.Location = new System.Drawing.Point(14, 355);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(832, 281);
            this.rtbLog.TabIndex = 6;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // notifyTimer
            // 
            this.notifyTimer.Tick += new System.EventHandler(this.notifyTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 650);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.grpDeviceControl);
            this.Controls.Add(this.grpEventReceiver);
            this.Controls.Add(this.grpCommands);
            this.Controls.Add(this.grpStationInfo);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MinimumSize = new System.Drawing.Size(876, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LifeSmart Local UDP Sample (local-UDP-sample)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpStationInfo.ResumeLayout(false);
            this.grpStationInfo.PerformLayout();
            this.grpCommands.ResumeLayout(false);
            this.grpEventReceiver.ResumeLayout(false);
            this.grpEventReceiver.PerformLayout();
            this.grpDeviceControl.ResumeLayout(false);
            this.grpDeviceControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpStationInfo;
        private System.Windows.Forms.Label lblStationIp;
        private System.Windows.Forms.TextBox txtStationIp;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label lblPortHint;
        private System.Windows.Forms.GroupBox grpCommands;
        private System.Windows.Forms.Button btnGetDevices;
        private System.Windows.Forms.Button btnGetScenes;
        private System.Windows.Forms.Button btnGetVersion;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.GroupBox grpEventReceiver;
        private System.Windows.Forms.Label lblReceiveIp;
        private System.Windows.Forms.TextBox txtReceiveIp;
        private System.Windows.Forms.Label lblReceivePort;
        private System.Windows.Forms.TextBox txtReceivePort;
        private System.Windows.Forms.Button btnToggleReceive;
        private System.Windows.Forms.Label lblReceiveStatus;
        private System.Windows.Forms.GroupBox grpDeviceControl;
        private System.Windows.Forms.Label lblDeviceMe;
        private System.Windows.Forms.TextBox txtDeviceMe;
        private System.Windows.Forms.Label lblDeviceIdx;
        private System.Windows.Forms.TextBox txtDeviceIdx;
        private System.Windows.Forms.Label lblControlType;
        private System.Windows.Forms.TextBox txtControlType;
        private System.Windows.Forms.Label lblControlVal;
        private System.Windows.Forms.TextBox txtControlVal;
        private System.Windows.Forms.Button btnPresetOn;
        private System.Windows.Forms.Button btnPresetOff;
        private System.Windows.Forms.Button btnControlDevice;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Timer notifyTimer;
    }
}
