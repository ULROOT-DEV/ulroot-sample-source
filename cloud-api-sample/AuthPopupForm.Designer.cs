namespace CloudApiSample
{
    partial class AuthPopupForm
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
            this.lblCallbackUrl = new System.Windows.Forms.Label();
            this.txtCallbackUrl = new System.Windows.Forms.TextBox();
            this.lblLang = new System.Windows.Forms.Label();
            this.cboLang = new System.Windows.Forms.ComboBox();
            this.btnRegenerateUrl = new System.Windows.Forms.Button();
            this.lblRequestUrl = new System.Windows.Forms.Label();
            this.txtRequestUrl = new System.Windows.Forms.TextBox();
            this.btnCopyUrl = new System.Windows.Forms.Button();
            this.btnOpenBrowser = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpCallback = new System.Windows.Forms.GroupBox();
            this.txtParsedRegionId = new System.Windows.Forms.TextBox();
            this.lblParsedRegionId = new System.Windows.Forms.Label();
            this.txtParsedSvrUrl = new System.Windows.Forms.TextBox();
            this.lblParsedSvrUrl = new System.Windows.Forms.Label();
            this.txtParsedUserToken = new System.Windows.Forms.TextBox();
            this.lblParsedUserToken = new System.Windows.Forms.Label();
            this.txtParsedUserId = new System.Windows.Forms.TextBox();
            this.lblParsedUserId = new System.Windows.Forms.Label();
            this.btnParseCallback = new System.Windows.Forms.Button();
            this.txtCallbackRaw = new System.Windows.Forms.TextBox();
            this.lblCallbackRaw = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpCallback.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCallbackUrl
            // 
            this.lblCallbackUrl.AutoSize = true;
            this.lblCallbackUrl.Location = new System.Drawing.Point(14, 18);
            this.lblCallbackUrl.Name = "lblCallbackUrl";
            this.lblCallbackUrl.Size = new System.Drawing.Size(87, 15);
            this.lblCallbackUrl.TabIndex = 0;
            this.lblCallbackUrl.Text = "auth_callback";
            // 
            // txtCallbackUrl
            // 
            this.txtCallbackUrl.Location = new System.Drawing.Point(108, 14);
            this.txtCallbackUrl.Name = "txtCallbackUrl";
            this.txtCallbackUrl.Size = new System.Drawing.Size(356, 23);
            this.txtCallbackUrl.TabIndex = 1;
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Location = new System.Drawing.Point(476, 18);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(32, 15);
            this.lblLang.TabIndex = 2;
            this.lblLang.Text = "lang";
            // 
            // cboLang
            // 
            this.cboLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLang.FormattingEnabled = true;
            this.cboLang.Location = new System.Drawing.Point(514, 14);
            this.cboLang.Name = "cboLang";
            this.cboLang.Size = new System.Drawing.Size(64, 23);
            this.cboLang.TabIndex = 3;
            // 
            // btnRegenerateUrl
            // 
            this.btnRegenerateUrl.Location = new System.Drawing.Point(588, 13);
            this.btnRegenerateUrl.Name = "btnRegenerateUrl";
            this.btnRegenerateUrl.Size = new System.Drawing.Size(118, 25);
            this.btnRegenerateUrl.TabIndex = 4;
            this.btnRegenerateUrl.Text = "URL 새로 생성";
            this.btnRegenerateUrl.UseVisualStyleBackColor = true;
            this.btnRegenerateUrl.Click += new System.EventHandler(this.btnRegenerateUrl_Click);
            // 
            // lblRequestUrl
            // 
            this.lblRequestUrl.AutoSize = true;
            this.lblRequestUrl.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRequestUrl.Location = new System.Drawing.Point(14, 48);
            this.lblRequestUrl.Name = "lblRequestUrl";
            this.lblRequestUrl.Size = new System.Drawing.Size(202, 15);
            this.lblRequestUrl.TabIndex = 5;
            this.lblRequestUrl.Text = "생성된 3.1 Request URL (서명 포함)";
            // 
            // txtRequestUrl
            // 
            this.txtRequestUrl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtRequestUrl.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestUrl.Location = new System.Drawing.Point(14, 68);
            this.txtRequestUrl.Multiline = true;
            this.txtRequestUrl.Name = "txtRequestUrl";
            this.txtRequestUrl.ReadOnly = true;
            this.txtRequestUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequestUrl.Size = new System.Drawing.Size(692, 68);
            this.txtRequestUrl.TabIndex = 6;
            // 
            // btnCopyUrl
            // 
            this.btnCopyUrl.Location = new System.Drawing.Point(14, 144);
            this.btnCopyUrl.Name = "btnCopyUrl";
            this.btnCopyUrl.Size = new System.Drawing.Size(115, 30);
            this.btnCopyUrl.TabIndex = 7;
            this.btnCopyUrl.Text = "URL 복사";
            this.btnCopyUrl.UseVisualStyleBackColor = true;
            this.btnCopyUrl.Click += new System.EventHandler(this.btnCopyUrl_Click);
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOpenBrowser.Location = new System.Drawing.Point(137, 144);
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Size = new System.Drawing.Size(185, 30);
            this.btnOpenBrowser.TabIndex = 8;
            this.btnOpenBrowser.Text = "브라우저로 인증 페이지 열기";
            this.btnOpenBrowser.UseVisualStyleBackColor = true;
            this.btnOpenBrowser.Click += new System.EventHandler(this.btnOpenBrowser_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblStatus.Location = new System.Drawing.Point(14, 182);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(692, 30);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "상태: [브라우저로 인증 페이지 열기]를 눌러 LifeSmart 계정 로그인 및 권한 승인을 진행하세요.";
            // 
            // grpCallback
            // 
            this.grpCallback.Controls.Add(this.txtParsedRegionId);
            this.grpCallback.Controls.Add(this.lblParsedRegionId);
            this.grpCallback.Controls.Add(this.txtParsedSvrUrl);
            this.grpCallback.Controls.Add(this.lblParsedSvrUrl);
            this.grpCallback.Controls.Add(this.txtParsedUserToken);
            this.grpCallback.Controls.Add(this.lblParsedUserToken);
            this.grpCallback.Controls.Add(this.txtParsedUserId);
            this.grpCallback.Controls.Add(this.lblParsedUserId);
            this.grpCallback.Controls.Add(this.btnParseCallback);
            this.grpCallback.Controls.Add(this.txtCallbackRaw);
            this.grpCallback.Controls.Add(this.lblCallbackRaw);
            this.grpCallback.Location = new System.Drawing.Point(14, 216);
            this.grpCallback.Name = "grpCallback";
            this.grpCallback.Size = new System.Drawing.Size(692, 200);
            this.grpCallback.TabIndex = 10;
            this.grpCallback.TabStop = false;
            this.grpCallback.Text = "인증 콜백 결과 수신 / 파싱 (3.3 Authorization Process)";
            // 
            // lblCallbackRaw
            // 
            this.lblCallbackRaw.AutoSize = true;
            this.lblCallbackRaw.Location = new System.Drawing.Point(14, 24);
            this.lblCallbackRaw.Name = "lblCallbackRaw";
            this.lblCallbackRaw.Size = new System.Drawing.Size(374, 15);
            this.lblCallbackRaw.TabIndex = 0;
            this.lblCallbackRaw.Text = "콜백 URL 또는 JSON 응답 (localhost 콜백 시 자동 입력 / 수동 붙여넣기 가능)";
            // 
            // txtCallbackRaw
            // 
            this.txtCallbackRaw.Font = new System.Drawing.Font("Consolas", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCallbackRaw.Location = new System.Drawing.Point(14, 44);
            this.txtCallbackRaw.Multiline = true;
            this.txtCallbackRaw.Name = "txtCallbackRaw";
            this.txtCallbackRaw.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCallbackRaw.Size = new System.Drawing.Size(564, 52);
            this.txtCallbackRaw.TabIndex = 1;
            // 
            // btnParseCallback
            // 
            this.btnParseCallback.Location = new System.Drawing.Point(586, 44);
            this.btnParseCallback.Name = "btnParseCallback";
            this.btnParseCallback.Size = new System.Drawing.Size(92, 52);
            this.btnParseCallback.TabIndex = 2;
            this.btnParseCallback.Text = "결과 파싱";
            this.btnParseCallback.UseVisualStyleBackColor = true;
            this.btnParseCallback.Click += new System.EventHandler(this.btnParseCallback_Click);
            // 
            // lblParsedUserId
            // 
            this.lblParsedUserId.AutoSize = true;
            this.lblParsedUserId.Location = new System.Drawing.Point(14, 114);
            this.lblParsedUserId.Name = "lblParsedUserId";
            this.lblParsedUserId.Size = new System.Drawing.Size(54, 15);
            this.lblParsedUserId.TabIndex = 3;
            this.lblParsedUserId.Text = "User ID";
            // 
            // txtParsedUserId
            // 
            this.txtParsedUserId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtParsedUserId.Location = new System.Drawing.Point(94, 110);
            this.txtParsedUserId.Name = "txtParsedUserId";
            this.txtParsedUserId.Size = new System.Drawing.Size(160, 23);
            this.txtParsedUserId.TabIndex = 4;
            // 
            // lblParsedUserToken
            // 
            this.lblParsedUserToken.AutoSize = true;
            this.lblParsedUserToken.Location = new System.Drawing.Point(268, 114);
            this.lblParsedUserToken.Name = "lblParsedUserToken";
            this.lblParsedUserToken.Size = new System.Drawing.Size(68, 15);
            this.lblParsedUserToken.TabIndex = 5;
            this.lblParsedUserToken.Text = "UserToken";
            // 
            // txtParsedUserToken
            // 
            this.txtParsedUserToken.Location = new System.Drawing.Point(342, 110);
            this.txtParsedUserToken.Name = "txtParsedUserToken";
            this.txtParsedUserToken.Size = new System.Drawing.Size(336, 23);
            this.txtParsedUserToken.TabIndex = 6;
            // 
            // lblParsedSvrUrl
            // 
            this.lblParsedSvrUrl.AutoSize = true;
            this.lblParsedSvrUrl.Location = new System.Drawing.Point(14, 152);
            this.lblParsedSvrUrl.Name = "lblParsedSvrUrl";
            this.lblParsedSvrUrl.Size = new System.Drawing.Size(44, 15);
            this.lblParsedSvrUrl.TabIndex = 7;
            this.lblParsedSvrUrl.Text = "svrurl";
            // 
            // txtParsedSvrUrl
            // 
            this.txtParsedSvrUrl.Location = new System.Drawing.Point(94, 148);
            this.txtParsedSvrUrl.Name = "txtParsedSvrUrl";
            this.txtParsedSvrUrl.Size = new System.Drawing.Size(366, 23);
            this.txtParsedSvrUrl.TabIndex = 8;
            // 
            // lblParsedRegionId
            // 
            this.lblParsedRegionId.AutoSize = true;
            this.lblParsedRegionId.Location = new System.Drawing.Point(474, 152);
            this.lblParsedRegionId.Name = "lblParsedRegionId";
            this.lblParsedRegionId.Size = new System.Drawing.Size(56, 15);
            this.lblParsedRegionId.TabIndex = 9;
            this.lblParsedRegionId.Text = "svrrgnid";
            // 
            // txtParsedRegionId
            // 
            this.txtParsedRegionId.Location = new System.Drawing.Point(536, 148);
            this.txtParsedRegionId.Name = "txtParsedRegionId";
            this.txtParsedRegionId.Size = new System.Drawing.Size(142, 23);
            this.txtParsedRegionId.TabIndex = 10;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(476, 426);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(134, 30);
            this.btnApply.TabIndex = 11;
            this.btnApply.Text = "메인 화면에 적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(618, 426);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // AuthPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 468);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.grpCallback);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOpenBrowser);
            this.Controls.Add(this.btnCopyUrl);
            this.Controls.Add(this.txtRequestUrl);
            this.Controls.Add(this.lblRequestUrl);
            this.Controls.Add(this.btnRegenerateUrl);
            this.Controls.Add(this.cboLang);
            this.Controls.Add(this.lblLang);
            this.Controls.Add(this.txtCallbackUrl);
            this.Controls.Add(this.lblCallbackUrl);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthPopupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "3.1 사용자 인증 Request URL 팝업 (auth.authorize)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuthPopupForm_FormClosing);
            this.grpCallback.ResumeLayout(false);
            this.grpCallback.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCallbackUrl;
        private System.Windows.Forms.TextBox txtCallbackUrl;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ComboBox cboLang;
        private System.Windows.Forms.Button btnRegenerateUrl;
        private System.Windows.Forms.Label lblRequestUrl;
        private System.Windows.Forms.TextBox txtRequestUrl;
        private System.Windows.Forms.Button btnCopyUrl;
        private System.Windows.Forms.Button btnOpenBrowser;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpCallback;
        private System.Windows.Forms.Label lblCallbackRaw;
        private System.Windows.Forms.TextBox txtCallbackRaw;
        private System.Windows.Forms.Button btnParseCallback;
        private System.Windows.Forms.Label lblParsedUserId;
        private System.Windows.Forms.TextBox txtParsedUserId;
        private System.Windows.Forms.Label lblParsedUserToken;
        private System.Windows.Forms.TextBox txtParsedUserToken;
        private System.Windows.Forms.Label lblParsedSvrUrl;
        private System.Windows.Forms.TextBox txtParsedSvrUrl;
        private System.Windows.Forms.Label lblParsedRegionId;
        private System.Windows.Forms.TextBox txtParsedRegionId;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
    }
}
