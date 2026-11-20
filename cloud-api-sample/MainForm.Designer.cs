namespace CloudApiSample
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpAppConfig = new System.Windows.Forms.GroupBox();
            this.txtAppToken = new System.Windows.Forms.TextBox();
            this.lblAppToken = new System.Windows.Forms.Label();
            this.txtAppKey = new System.Windows.Forms.TextBox();
            this.lblAppKey = new System.Windows.Forms.Label();
            this.grpStep1 = new System.Windows.Forms.GroupBox();
            this.lblStep1Hint = new System.Windows.Forms.Label();
            this.btnOpenAuthPopup = new System.Windows.Forms.Button();
            this.grpStep2 = new System.Windows.Forms.GroupBox();
            this.txtWsUrl = new System.Windows.Forms.TextBox();
            this.lblWsUrl = new System.Windows.Forms.Label();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.cboRegion = new System.Windows.Forms.ComboBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.btnRefreshToken = new System.Windows.Forms.Button();
            this.txtUserToken = new System.Windows.Forms.TextBox();
            this.lblUserToken = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.grpEpGetAll = new System.Windows.Forms.GroupBox();
            this.btnEpGetAll = new System.Windows.Forms.Button();
            this.cboDegree = new System.Windows.Forms.ComboBox();
            this.lblDegree = new System.Windows.Forms.Label();
            this.grpEpSingle = new System.Windows.Forms.GroupBox();
            this.btnEpSet = new System.Windows.Forms.Button();
            this.btnEpPresetOff = new System.Windows.Forms.Button();
            this.btnEpPresetOn = new System.Windows.Forms.Button();
            this.txtEpTag = new System.Windows.Forms.TextBox();
            this.lblEpTag = new System.Windows.Forms.Label();
            this.txtEpVal = new System.Windows.Forms.TextBox();
            this.lblEpVal = new System.Windows.Forms.Label();
            this.txtEpType = new System.Windows.Forms.TextBox();
            this.lblEpType = new System.Windows.Forms.Label();
            this.txtIdx = new System.Windows.Forms.TextBox();
            this.lblIdx = new System.Windows.Forms.Label();
            this.btnEpGet = new System.Windows.Forms.Button();
            this.txtMe = new System.Windows.Forms.TextBox();
            this.lblMe = new System.Windows.Forms.Label();
            this.txtAgt = new System.Windows.Forms.TextBox();
            this.lblAgt = new System.Windows.Forms.Label();
            this.grpScene = new System.Windows.Forms.GroupBox();
            this.lblSceneHint = new System.Windows.Forms.Label();
            this.txtSceneRgbw = new System.Windows.Forms.TextBox();
            this.lblSceneRgbw = new System.Windows.Forms.Label();
            this.txtSceneType = new System.Windows.Forms.TextBox();
            this.lblSceneType = new System.Windows.Forms.Label();
            this.btnSceneSet = new System.Windows.Forms.Button();
            this.txtSceneId = new System.Windows.Forms.TextBox();
            this.lblSceneId = new System.Windows.Forms.Label();
            this.btnSceneGet = new System.Windows.Forms.Button();
            this.grpWebSocket = new System.Windows.Forms.GroupBox();
            this.lblWsStatus = new System.Windows.Forms.Label();
            this.btnWsToggle = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.grpAppConfig.SuspendLayout();
            this.grpStep1.SuspendLayout();
            this.grpStep2.SuspendLayout();
            this.grpEpGetAll.SuspendLayout();
            this.grpEpSingle.SuspendLayout();
            this.grpScene.SuspendLayout();
            this.grpWebSocket.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAppConfig
            // 
            this.grpAppConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAppConfig.Controls.Add(this.txtAppToken);
            this.grpAppConfig.Controls.Add(this.lblAppToken);
            this.grpAppConfig.Controls.Add(this.txtAppKey);
            this.grpAppConfig.Controls.Add(this.lblAppKey);
            this.grpAppConfig.Location = new System.Drawing.Point(14, 12);
            this.grpAppConfig.Name = "grpAppConfig";
            this.grpAppConfig.Size = new System.Drawing.Size(932, 54);
            this.grpAppConfig.TabIndex = 0;
            this.grpAppConfig.TabStop = false;
            this.grpAppConfig.Text = "기본 앱 설정 (공통 필수 — 모든 인증·OpenAPI·WebSocket 서명에 사용)";
            // 
            // lblAppKey
            // 
            this.lblAppKey.AutoSize = true;
            this.lblAppKey.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAppKey.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAppKey.Location = new System.Drawing.Point(14, 24);
            this.lblAppKey.Name = "lblAppKey";
            this.lblAppKey.Size = new System.Drawing.Size(54, 15);
            this.lblAppKey.TabIndex = 0;
            this.lblAppKey.Text = "AppKey";
            // 
            // txtAppKey
            // 
            this.txtAppKey.Location = new System.Drawing.Point(78, 20);
            this.txtAppKey.Name = "txtAppKey";
            this.txtAppKey.Size = new System.Drawing.Size(330, 23);
            this.txtAppKey.TabIndex = 1;
            // 
            // lblAppToken
            // 
            this.lblAppToken.AutoSize = true;
            this.lblAppToken.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAppToken.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAppToken.Location = new System.Drawing.Point(426, 24);
            this.lblAppToken.Name = "lblAppToken";
            this.lblAppToken.Size = new System.Drawing.Size(69, 15);
            this.lblAppToken.TabIndex = 2;
            this.lblAppToken.Text = "AppToken";
            // 
            // txtAppToken
            // 
            this.txtAppToken.Location = new System.Drawing.Point(504, 20);
            this.txtAppToken.Name = "txtAppToken";
            this.txtAppToken.Size = new System.Drawing.Size(412, 23);
            this.txtAppToken.TabIndex = 3;
            // 
            // grpStep1
            // 
            this.grpStep1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStep1.Controls.Add(this.lblStep1Hint);
            this.grpStep1.Controls.Add(this.btnOpenAuthPopup);
            this.grpStep1.Location = new System.Drawing.Point(14, 72);
            this.grpStep1.Name = "grpStep1";
            this.grpStep1.Size = new System.Drawing.Size(932, 54);
            this.grpStep1.TabIndex = 1;
            this.grpStep1.TabStop = false;
            this.grpStep1.Text = "1단계 (선택): UserToken 발급 (3.1 auth.authorize) — 기존 UserToken이 있으면 생략 가능";
            // 
            // btnOpenAuthPopup
            // 
            this.btnOpenAuthPopup.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOpenAuthPopup.Location = new System.Drawing.Point(14, 19);
            this.btnOpenAuthPopup.Name = "btnOpenAuthPopup";
            this.btnOpenAuthPopup.Size = new System.Drawing.Size(236, 27);
            this.btnOpenAuthPopup.TabIndex = 0;
            this.btnOpenAuthPopup.Text = "3.1 인증 URL 팝업 (UserToken 발급)";
            this.btnOpenAuthPopup.UseVisualStyleBackColor = true;
            this.btnOpenAuthPopup.Click += new System.EventHandler(this.btnOpenAuthPopup_Click);
            // 
            // lblStep1Hint
            // 
            this.lblStep1Hint.AutoSize = true;
            this.lblStep1Hint.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStep1Hint.ForeColor = System.Drawing.Color.DimGray;
            this.lblStep1Hint.Location = new System.Drawing.Point(262, 25);
            this.lblStep1Hint.Name = "lblStep1Hint";
            this.lblStep1Hint.Size = new System.Drawing.Size(622, 15);
            this.lblStep1Hint.TabIndex = 1;
            this.lblStep1Hint.Text = "※ UserToken이 없을 때만 실행하세요. 이미 UserToken이 있다면 이 단계 없이 아래 2단계에 바로 입력해 사용할 수 있습니다.";
            // 
            // grpStep2
            // 
            this.grpStep2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStep2.Controls.Add(this.txtWsUrl);
            this.grpStep2.Controls.Add(this.lblWsUrl);
            this.grpStep2.Controls.Add(this.txtServerUrl);
            this.grpStep2.Controls.Add(this.lblServerUrl);
            this.grpStep2.Controls.Add(this.cboRegion);
            this.grpStep2.Controls.Add(this.lblRegion);
            this.grpStep2.Controls.Add(this.btnRefreshToken);
            this.grpStep2.Controls.Add(this.txtUserToken);
            this.grpStep2.Controls.Add(this.lblUserToken);
            this.grpStep2.Controls.Add(this.txtUserId);
            this.grpStep2.Controls.Add(this.lblUserId);
            this.grpStep2.Location = new System.Drawing.Point(14, 132);
            this.grpStep2.Name = "grpStep2";
            this.grpStep2.Size = new System.Drawing.Size(932, 92);
            this.grpStep2.TabIndex = 2;
            this.grpStep2.TabStop = false;
            this.grpStep2.Text = "2단계: UserToken 및 서버 정보 (기존 UserToken 직접 입력 또는 1단계 발급 결과 자동 반영)";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblUserId.Location = new System.Drawing.Point(14, 28);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(80, 15);
            this.lblUserId.TabIndex = 0;
            this.lblUserId.Text = "UserID (숫자)";
            // 
            // txtUserId
            // 
            this.txtUserId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtUserId.Location = new System.Drawing.Point(104, 24);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(150, 23);
            this.txtUserId.TabIndex = 1;
            this.txtUserId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserId_KeyPress);
            // 
            // lblUserToken
            // 
            this.lblUserToken.AutoSize = true;
            this.lblUserToken.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblUserToken.Location = new System.Drawing.Point(268, 28);
            this.lblUserToken.Name = "lblUserToken";
            this.lblUserToken.Size = new System.Drawing.Size(68, 15);
            this.lblUserToken.TabIndex = 2;
            this.lblUserToken.Text = "UserToken";
            // 
            // txtUserToken
            // 
            this.txtUserToken.Location = new System.Drawing.Point(344, 24);
            this.txtUserToken.Name = "txtUserToken";
            this.txtUserToken.Size = new System.Drawing.Size(442, 23);
            this.txtUserToken.TabIndex = 3;
            // 
            // btnRefreshToken
            // 
            this.btnRefreshToken.Location = new System.Drawing.Point(796, 22);
            this.btnRefreshToken.Name = "btnRefreshToken";
            this.btnRefreshToken.Size = new System.Drawing.Size(120, 26);
            this.btnRefreshToken.TabIndex = 4;
            this.btnRefreshToken.Text = "3.4 토큰 갱신";
            this.btnRefreshToken.UseVisualStyleBackColor = true;
            this.btnRefreshToken.Click += new System.EventHandler(this.btnRefreshToken_Click);
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(14, 62);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(49, 15);
            this.lblRegion.TabIndex = 5;
            this.lblRegion.Text = "Region";
            // 
            // cboRegion
            // 
            this.cboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRegion.FormattingEnabled = true;
            this.cboRegion.Location = new System.Drawing.Point(104, 58);
            this.cboRegion.Name = "cboRegion";
            this.cboRegion.Size = new System.Drawing.Size(184, 23);
            this.cboRegion.TabIndex = 6;
            this.cboRegion.SelectedIndexChanged += new System.EventHandler(this.cboRegion_SelectedIndexChanged);
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(300, 62);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(54, 15);
            this.lblServerUrl.TabIndex = 7;
            this.lblServerUrl.Text = "API URL";
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(360, 58);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(238, 23);
            this.txtServerUrl.TabIndex = 8;
            // 
            // lblWsUrl
            // 
            this.lblWsUrl.AutoSize = true;
            this.lblWsUrl.Location = new System.Drawing.Point(608, 62);
            this.lblWsUrl.Name = "lblWsUrl";
            this.lblWsUrl.Size = new System.Drawing.Size(52, 15);
            this.lblWsUrl.TabIndex = 9;
            this.lblWsUrl.Text = "WS URL";
            // 
            // txtWsUrl
            // 
            this.txtWsUrl.Location = new System.Drawing.Point(666, 58);
            this.txtWsUrl.Name = "txtWsUrl";
            this.txtWsUrl.Size = new System.Drawing.Size(250, 23);
            this.txtWsUrl.TabIndex = 10;
            // 
            // grpEpGetAll
            // 
            this.grpEpGetAll.Controls.Add(this.btnEpGetAll);
            this.grpEpGetAll.Controls.Add(this.cboDegree);
            this.grpEpGetAll.Controls.Add(this.lblDegree);
            this.grpEpGetAll.Location = new System.Drawing.Point(14, 232);
            this.grpEpGetAll.Name = "grpEpGetAll";
            this.grpEpGetAll.Size = new System.Drawing.Size(232, 108);
            this.grpEpGetAll.TabIndex = 3;
            this.grpEpGetAll.TabStop = false;
            this.grpEpGetAll.Text = "4.5.6 전체 조회 (EpGetAll)";
            // 
            // lblDegree
            // 
            this.lblDegree.AutoSize = true;
            this.lblDegree.Location = new System.Drawing.Point(12, 30);
            this.lblDegree.Name = "lblDegree";
            this.lblDegree.Size = new System.Drawing.Size(47, 15);
            this.lblDegree.TabIndex = 0;
            this.lblDegree.Text = "degree";
            // 
            // cboDegree
            // 
            this.cboDegree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDegree.FormattingEnabled = true;
            this.cboDegree.Location = new System.Drawing.Point(64, 26);
            this.cboDegree.Name = "cboDegree";
            this.cboDegree.Size = new System.Drawing.Size(154, 23);
            this.cboDegree.TabIndex = 1;
            // 
            // btnEpGetAll
            // 
            this.btnEpGetAll.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnEpGetAll.Location = new System.Drawing.Point(12, 64);
            this.btnEpGetAll.Name = "btnEpGetAll";
            this.btnEpGetAll.Size = new System.Drawing.Size(206, 28);
            this.btnEpGetAll.TabIndex = 2;
            this.btnEpGetAll.Text = "전체 디바이스 조회 (EpGetAll)";
            this.btnEpGetAll.UseVisualStyleBackColor = true;
            this.btnEpGetAll.Click += new System.EventHandler(this.btnEpGetAll_Click);
            // 
            // grpEpSingle
            // 
            this.grpEpSingle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEpSingle.Controls.Add(this.btnEpSet);
            this.grpEpSingle.Controls.Add(this.btnEpPresetOff);
            this.grpEpSingle.Controls.Add(this.btnEpPresetOn);
            this.grpEpSingle.Controls.Add(this.txtEpTag);
            this.grpEpSingle.Controls.Add(this.lblEpTag);
            this.grpEpSingle.Controls.Add(this.txtEpVal);
            this.grpEpSingle.Controls.Add(this.lblEpVal);
            this.grpEpSingle.Controls.Add(this.txtEpType);
            this.grpEpSingle.Controls.Add(this.lblEpType);
            this.grpEpSingle.Controls.Add(this.txtIdx);
            this.grpEpSingle.Controls.Add(this.lblIdx);
            this.grpEpSingle.Controls.Add(this.btnEpGet);
            this.grpEpSingle.Controls.Add(this.txtMe);
            this.grpEpSingle.Controls.Add(this.lblMe);
            this.grpEpSingle.Controls.Add(this.txtAgt);
            this.grpEpSingle.Controls.Add(this.lblAgt);
            this.grpEpSingle.Location = new System.Drawing.Point(258, 232);
            this.grpEpSingle.Name = "grpEpSingle";
            this.grpEpSingle.Size = new System.Drawing.Size(688, 108);
            this.grpEpSingle.TabIndex = 4;
            this.grpEpSingle.TabStop = false;
            this.grpEpSingle.Text = "4.5.7 단일 조회 (EpGet) / 4.5.8 제어 (EpSet) — AGT · ME 필수";
            // 
            // lblAgt
            // 
            this.lblAgt.AutoSize = true;
            this.lblAgt.Location = new System.Drawing.Point(12, 30);
            this.lblAgt.Name = "lblAgt";
            this.lblAgt.Size = new System.Drawing.Size(102, 15);
            this.lblAgt.TabIndex = 0;
            this.lblAgt.Text = "AGT (Station ID)";
            // 
            // txtAgt
            // 
            this.txtAgt.Location = new System.Drawing.Point(118, 26);
            this.txtAgt.Name = "txtAgt";
            this.txtAgt.Size = new System.Drawing.Size(196, 23);
            this.txtAgt.TabIndex = 1;
            // 
            // lblMe
            // 
            this.lblMe.AutoSize = true;
            this.lblMe.Location = new System.Drawing.Point(324, 30);
            this.lblMe.Name = "lblMe";
            this.lblMe.Size = new System.Drawing.Size(76, 15);
            this.lblMe.TabIndex = 2;
            this.lblMe.Text = "ME (Dev ID)";
            // 
            // txtMe
            // 
            this.txtMe.Location = new System.Drawing.Point(404, 26);
            this.txtMe.Name = "txtMe";
            this.txtMe.Size = new System.Drawing.Size(88, 23);
            this.txtMe.TabIndex = 3;
            // 
            // btnEpGet
            // 
            this.btnEpGet.Location = new System.Drawing.Point(504, 24);
            this.btnEpGet.Name = "btnEpGet";
            this.btnEpGet.Size = new System.Drawing.Size(170, 28);
            this.btnEpGet.TabIndex = 4;
            this.btnEpGet.Text = "4.5.7 단일 조회 (EpGet)";
            this.btnEpGet.UseVisualStyleBackColor = true;
            this.btnEpGet.Click += new System.EventHandler(this.btnEpGet_Click);
            // 
            // lblIdx
            // 
            this.lblIdx.AutoSize = true;
            this.lblIdx.Location = new System.Drawing.Point(12, 70);
            this.lblIdx.Name = "lblIdx";
            this.lblIdx.Size = new System.Drawing.Size(48, 15);
            this.lblIdx.TabIndex = 5;
            this.lblIdx.Text = "IO (idx)";
            // 
            // txtIdx
            // 
            this.txtIdx.Location = new System.Drawing.Point(64, 66);
            this.txtIdx.Name = "txtIdx";
            this.txtIdx.Size = new System.Drawing.Size(54, 23);
            this.txtIdx.TabIndex = 6;
            this.txtIdx.Text = "P1";
            // 
            // lblEpType
            // 
            this.lblEpType.AutoSize = true;
            this.lblEpType.Location = new System.Drawing.Point(126, 70);
            this.lblEpType.Name = "lblEpType";
            this.lblEpType.Size = new System.Drawing.Size(34, 15);
            this.lblEpType.TabIndex = 7;
            this.lblEpType.Text = "Type";
            // 
            // txtEpType
            // 
            this.txtEpType.Location = new System.Drawing.Point(164, 66);
            this.txtEpType.Name = "txtEpType";
            this.txtEpType.Size = new System.Drawing.Size(54, 23);
            this.txtEpType.TabIndex = 8;
            this.txtEpType.Text = "129";
            // 
            // lblEpVal
            // 
            this.lblEpVal.AutoSize = true;
            this.lblEpVal.Location = new System.Drawing.Point(226, 70);
            this.lblEpVal.Name = "lblEpVal";
            this.lblEpVal.Size = new System.Drawing.Size(24, 15);
            this.lblEpVal.TabIndex = 9;
            this.lblEpVal.Text = "Val";
            // 
            // txtEpVal
            // 
            this.txtEpVal.Location = new System.Drawing.Point(254, 66);
            this.txtEpVal.Name = "txtEpVal";
            this.txtEpVal.Size = new System.Drawing.Size(52, 23);
            this.txtEpVal.TabIndex = 10;
            this.txtEpVal.Text = "1";
            // 
            // lblEpTag
            // 
            this.lblEpTag.AutoSize = true;
            this.lblEpTag.Location = new System.Drawing.Point(314, 70);
            this.lblEpTag.Name = "lblEpTag";
            this.lblEpTag.Size = new System.Drawing.Size(28, 15);
            this.lblEpTag.TabIndex = 11;
            this.lblEpTag.Text = "Tag";
            // 
            // txtEpTag
            // 
            this.txtEpTag.Location = new System.Drawing.Point(346, 66);
            this.txtEpTag.Name = "txtEpTag";
            this.txtEpTag.Size = new System.Drawing.Size(38, 23);
            this.txtEpTag.TabIndex = 12;
            this.txtEpTag.Text = "m";
            // 
            // btnEpPresetOn
            // 
            this.btnEpPresetOn.Location = new System.Drawing.Point(394, 64);
            this.btnEpPresetOn.Name = "btnEpPresetOn";
            this.btnEpPresetOn.Size = new System.Drawing.Size(48, 28);
            this.btnEpPresetOn.TabIndex = 13;
            this.btnEpPresetOn.Text = "ON";
            this.btnEpPresetOn.UseVisualStyleBackColor = true;
            this.btnEpPresetOn.Click += new System.EventHandler(this.btnEpPresetOn_Click);
            // 
            // btnEpPresetOff
            // 
            this.btnEpPresetOff.Location = new System.Drawing.Point(448, 64);
            this.btnEpPresetOff.Name = "btnEpPresetOff";
            this.btnEpPresetOff.Size = new System.Drawing.Size(48, 28);
            this.btnEpPresetOff.TabIndex = 14;
            this.btnEpPresetOff.Text = "OFF";
            this.btnEpPresetOff.UseVisualStyleBackColor = true;
            this.btnEpPresetOff.Click += new System.EventHandler(this.btnEpPresetOff_Click);
            // 
            // btnEpSet
            // 
            this.btnEpSet.Location = new System.Drawing.Point(504, 64);
            this.btnEpSet.Name = "btnEpSet";
            this.btnEpSet.Size = new System.Drawing.Size(170, 28);
            this.btnEpSet.TabIndex = 15;
            this.btnEpSet.Text = "4.5.8 제어 (EpSet)";
            this.btnEpSet.UseVisualStyleBackColor = true;
            this.btnEpSet.Click += new System.EventHandler(this.btnEpSet_Click);
            // 
            // grpScene
            // 
            this.grpScene.Controls.Add(this.lblSceneHint);
            this.grpScene.Controls.Add(this.txtSceneRgbw);
            this.grpScene.Controls.Add(this.lblSceneRgbw);
            this.grpScene.Controls.Add(this.txtSceneType);
            this.grpScene.Controls.Add(this.lblSceneType);
            this.grpScene.Controls.Add(this.btnSceneSet);
            this.grpScene.Controls.Add(this.txtSceneId);
            this.grpScene.Controls.Add(this.lblSceneId);
            this.grpScene.Controls.Add(this.btnSceneGet);
            this.grpScene.Location = new System.Drawing.Point(14, 348);
            this.grpScene.Name = "grpScene";
            this.grpScene.Size = new System.Drawing.Size(576, 102);
            this.grpScene.TabIndex = 4;
            this.grpScene.TabStop = false;
            this.grpScene.Text = "씬 조회 / 실행 (4.5.10 SceneGet / 4.5.11 SceneSet)";
            // 
            // btnSceneGet
            // 
            this.btnSceneGet.Location = new System.Drawing.Point(14, 24);
            this.btnSceneGet.Name = "btnSceneGet";
            this.btnSceneGet.Size = new System.Drawing.Size(192, 28);
            this.btnSceneGet.TabIndex = 0;
            this.btnSceneGet.Text = "4.5.10 씬 조회 (SceneGet)";
            this.btnSceneGet.UseVisualStyleBackColor = true;
            this.btnSceneGet.Click += new System.EventHandler(this.btnSceneGet_Click);
            // 
            // lblSceneId
            // 
            this.lblSceneId.AutoSize = true;
            this.lblSceneId.Location = new System.Drawing.Point(216, 30);
            this.lblSceneId.Name = "lblSceneId";
            this.lblSceneId.Size = new System.Drawing.Size(58, 15);
            this.lblSceneId.TabIndex = 1;
            this.lblSceneId.Text = "Scene ID";
            // 
            // txtSceneId
            // 
            this.txtSceneId.Location = new System.Drawing.Point(278, 26);
            this.txtSceneId.Name = "txtSceneId";
            this.txtSceneId.Size = new System.Drawing.Size(134, 23);
            this.txtSceneId.TabIndex = 2;
            // 
            // btnSceneSet
            // 
            this.btnSceneSet.Location = new System.Drawing.Point(420, 24);
            this.btnSceneSet.Name = "btnSceneSet";
            this.btnSceneSet.Size = new System.Drawing.Size(142, 28);
            this.btnSceneSet.TabIndex = 3;
            this.btnSceneSet.Text = "4.5.11 씬 실행";
            this.btnSceneSet.UseVisualStyleBackColor = true;
            this.btnSceneSet.Click += new System.EventHandler(this.btnSceneSet_Click);
            // 
            // lblSceneType
            // 
            this.lblSceneType.AutoSize = true;
            this.lblSceneType.Location = new System.Drawing.Point(14, 66);
            this.lblSceneType.Name = "lblSceneType";
            this.lblSceneType.Size = new System.Drawing.Size(66, 15);
            this.lblSceneType.TabIndex = 4;
            this.lblSceneType.Text = "Type(선택)";
            // 
            // txtSceneType
            // 
            this.txtSceneType.Location = new System.Drawing.Point(86, 62);
            this.txtSceneType.Name = "txtSceneType";
            this.txtSceneType.Size = new System.Drawing.Size(68, 23);
            this.txtSceneType.TabIndex = 5;
            // 
            // lblSceneRgbw
            // 
            this.lblSceneRgbw.AutoSize = true;
            this.lblSceneRgbw.Location = new System.Drawing.Point(166, 66);
            this.lblSceneRgbw.Name = "lblSceneRgbw";
            this.lblSceneRgbw.Size = new System.Drawing.Size(76, 15);
            this.lblSceneRgbw.TabIndex = 6;
            this.lblSceneRgbw.Text = "RGBW(선택)";
            // 
            // txtSceneRgbw
            // 
            this.txtSceneRgbw.Location = new System.Drawing.Point(248, 62);
            this.txtSceneRgbw.Name = "txtSceneRgbw";
            this.txtSceneRgbw.Size = new System.Drawing.Size(110, 23);
            this.txtSceneRgbw.TabIndex = 7;
            // 
            // lblSceneHint
            // 
            this.lblSceneHint.AutoSize = true;
            this.lblSceneHint.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSceneHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblSceneHint.Location = new System.Drawing.Point(370, 66);
            this.lblSceneHint.Name = "lblSceneHint";
            this.lblSceneHint.Size = new System.Drawing.Size(188, 13);
            this.lblSceneHint.TabIndex = 8;
            this.lblSceneHint.Text = "※ 상단 AGT(Station ID) 값을 사용";
            // 
            // grpWebSocket
            // 
            this.grpWebSocket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpWebSocket.Controls.Add(this.lblWsStatus);
            this.grpWebSocket.Controls.Add(this.btnWsToggle);
            this.grpWebSocket.Location = new System.Drawing.Point(602, 348);
            this.grpWebSocket.Name = "grpWebSocket";
            this.grpWebSocket.Size = new System.Drawing.Size(344, 102);
            this.grpWebSocket.TabIndex = 5;
            this.grpWebSocket.TabStop = false;
            this.grpWebSocket.Text = "7. WebSocket 실시간 이벤트 수신";
            // 
            // btnWsToggle
            // 
            this.btnWsToggle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWsToggle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWsToggle.Location = new System.Drawing.Point(14, 24);
            this.btnWsToggle.Name = "btnWsToggle";
            this.btnWsToggle.Size = new System.Drawing.Size(316, 34);
            this.btnWsToggle.TabIndex = 0;
            this.btnWsToggle.Text = "WebSocket 연결 + WbAuth";
            this.btnWsToggle.UseVisualStyleBackColor = true;
            this.btnWsToggle.Click += new System.EventHandler(this.btnWsToggle_Click);
            // 
            // lblWsStatus
            // 
            this.lblWsStatus.AutoSize = false;
            this.lblWsStatus.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWsStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblWsStatus.Location = new System.Drawing.Point(14, 66);
            this.lblWsStatus.Name = "lblWsStatus";
            this.lblWsStatus.Size = new System.Drawing.Size(316, 20);
            this.lblWsStatus.TabIndex = 1;
            this.lblWsStatus.Text = "상태: 연결 안 됨 (연결 시 자동 WbAuth 인증)";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLog.Location = new System.Drawing.Point(16, 462);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(185, 15);
            this.lblLog.TabIndex = 6;
            this.lblLog.Text = "통신 / WebSocket 이벤트 로그";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(852, 457);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(94, 25);
            this.btnClearLog.TabIndex = 7;
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
            this.rtbLog.Location = new System.Drawing.Point(14, 487);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(932, 265);
            this.rtbLog.TabIndex = 8;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 766);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.grpWebSocket);
            this.Controls.Add(this.grpScene);
            this.Controls.Add(this.grpEpSingle);
            this.Controls.Add(this.grpEpGetAll);
            this.Controls.Add(this.grpStep2);
            this.Controls.Add(this.grpStep1);
            this.Controls.Add(this.grpAppConfig);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MinimumSize = new System.Drawing.Size(976, 700);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LifeSmart Cloud API Sample (cloud-api-sample)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpAppConfig.ResumeLayout(false);
            this.grpAppConfig.PerformLayout();
            this.grpStep1.ResumeLayout(false);
            this.grpStep1.PerformLayout();
            this.grpStep2.ResumeLayout(false);
            this.grpStep2.PerformLayout();
            this.grpEpGetAll.ResumeLayout(false);
            this.grpEpGetAll.PerformLayout();
            this.grpEpSingle.ResumeLayout(false);
            this.grpEpSingle.PerformLayout();
            this.grpScene.ResumeLayout(false);
            this.grpScene.PerformLayout();
            this.grpWebSocket.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpAppConfig;
        private System.Windows.Forms.Label lblAppKey;
        private System.Windows.Forms.TextBox txtAppKey;
        private System.Windows.Forms.Label lblAppToken;
        private System.Windows.Forms.TextBox txtAppToken;
        private System.Windows.Forms.GroupBox grpStep1;
        private System.Windows.Forms.Button btnOpenAuthPopup;
        private System.Windows.Forms.Label lblStep1Hint;
        private System.Windows.Forms.GroupBox grpStep2;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label lblUserToken;
        private System.Windows.Forms.TextBox txtUserToken;
        private System.Windows.Forms.Button btnRefreshToken;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.ComboBox cboRegion;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label lblWsUrl;
        private System.Windows.Forms.TextBox txtWsUrl;
        private System.Windows.Forms.GroupBox grpEpGetAll;
        private System.Windows.Forms.Label lblDegree;
        private System.Windows.Forms.ComboBox cboDegree;
        private System.Windows.Forms.Button btnEpGetAll;
        private System.Windows.Forms.GroupBox grpEpSingle;
        private System.Windows.Forms.Label lblAgt;
        private System.Windows.Forms.TextBox txtAgt;
        private System.Windows.Forms.Label lblMe;
        private System.Windows.Forms.TextBox txtMe;
        private System.Windows.Forms.Button btnEpGet;
        private System.Windows.Forms.Label lblIdx;
        private System.Windows.Forms.TextBox txtIdx;
        private System.Windows.Forms.Label lblEpType;
        private System.Windows.Forms.TextBox txtEpType;
        private System.Windows.Forms.Label lblEpVal;
        private System.Windows.Forms.TextBox txtEpVal;
        private System.Windows.Forms.Label lblEpTag;
        private System.Windows.Forms.TextBox txtEpTag;
        private System.Windows.Forms.Button btnEpPresetOn;
        private System.Windows.Forms.Button btnEpPresetOff;
        private System.Windows.Forms.Button btnEpSet;
        private System.Windows.Forms.GroupBox grpScene;
        private System.Windows.Forms.Button btnSceneGet;
        private System.Windows.Forms.Label lblSceneId;
        private System.Windows.Forms.TextBox txtSceneId;
        private System.Windows.Forms.Button btnSceneSet;
        private System.Windows.Forms.Label lblSceneType;
        private System.Windows.Forms.TextBox txtSceneType;
        private System.Windows.Forms.Label lblSceneRgbw;
        private System.Windows.Forms.TextBox txtSceneRgbw;
        private System.Windows.Forms.Label lblSceneHint;
        private System.Windows.Forms.GroupBox grpWebSocket;
        private System.Windows.Forms.Button btnWsToggle;
        private System.Windows.Forms.Label lblWsStatus;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
