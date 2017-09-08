namespace WebChatApiWin
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lstProcess = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTip = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtBoxMessage = new System.Windows.Forms.TextBox();
            this.btnGetUserList = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.lblFriends = new System.Windows.Forms.Label();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lstFriendData = new System.Windows.Forms.ListView();
            this.btnRefreshVC = new System.Windows.Forms.Button();
            this.lstSelectFriend = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.ckClearText = new System.Windows.Forms.CheckBox();
            this.ckAppendDateTime = new System.Windows.Forms.CheckBox();
            this.btnClearTextprocess = new System.Windows.Forms.Button();
            this.ckSelectAllFriend = new System.Windows.Forms.CheckBox();
            this.ckUnSelectAllFriend = new System.Windows.Forms.CheckBox();
            this.lblFriendFilter = new System.Windows.Forms.Label();
            this.txtFriendFilter = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ckOnlyFriend = new System.Windows.Forms.CheckBox();
            this.btnFilter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(486, 209);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(277, 220);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "Login";
            // 
            // lstProcess
            // 
            this.lstProcess.FormattingEnabled = true;
            this.lstProcess.HorizontalScrollbar = true;
            this.lstProcess.ItemHeight = 12;
            this.lstProcess.Location = new System.Drawing.Point(486, 209);
            this.lstProcess.Name = "lstProcess";
            this.lstProcess.ScrollAlwaysVisible = true;
            this.lstProcess.Size = new System.Drawing.Size(288, 292);
            this.lstProcess.TabIndex = 1;
            this.lstProcess.Tag = "SendMsg";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(484, 504);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Tag = "Common";
            this.label1.Text = "提示";
            // 
            // txtTip
            // 
            this.txtTip.Location = new System.Drawing.Point(486, 523);
            this.txtTip.Multiline = true;
            this.txtTip.Name = "txtTip";
            this.txtTip.Size = new System.Drawing.Size(288, 70);
            this.txtTip.TabIndex = 3;
            this.txtTip.Tag = "Common";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(486, 101);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Tag = "SendMsg";
            this.btnSend.Text = "发送信息";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtBoxMessage
            // 
            this.txtBoxMessage.Location = new System.Drawing.Point(486, 24);
            this.txtBoxMessage.Multiline = true;
            this.txtBoxMessage.Name = "txtBoxMessage";
            this.txtBoxMessage.Size = new System.Drawing.Size(277, 71);
            this.txtBoxMessage.TabIndex = 6;
            this.txtBoxMessage.Tag = "SendMsg";
            // 
            // btnGetUserList
            // 
            this.btnGetUserList.Location = new System.Drawing.Point(375, 297);
            this.btnGetUserList.Name = "btnGetUserList";
            this.btnGetUserList.Size = new System.Drawing.Size(90, 23);
            this.btnGetUserList.TabIndex = 5;
            this.btnGetUserList.Tag = "SendMsg";
            this.btnGetUserList.Text = "刷新用户";
            this.btnGetUserList.UseVisualStyleBackColor = true;
            this.btnGetUserList.Visible = false;
            this.btnGetUserList.Click += new System.EventHandler(this.btnGetUserList_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(486, 142);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Tag = "SendMsg";
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Visible = false;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // lblFriends
            // 
            this.lblFriends.AutoSize = true;
            this.lblFriends.Location = new System.Drawing.Point(12, 9);
            this.lblFriends.Name = "lblFriends";
            this.lblFriends.Size = new System.Drawing.Size(77, 12);
            this.lblFriends.TabIndex = 8;
            this.lblFriends.Tag = "SendMsg";
            this.lblFriends.Text = "微信好友列表";
            // 
            // lblSelectUser
            // 
            this.lblSelectUser.AutoSize = true;
            this.lblSelectUser.Location = new System.Drawing.Point(12, 297);
            this.lblSelectUser.Name = "lblSelectUser";
            this.lblSelectUser.Size = new System.Drawing.Size(77, 12);
            this.lblSelectUser.TabIndex = 9;
            this.lblSelectUser.Tag = "SendMsg";
            this.lblSelectUser.Text = "选中好友列表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(484, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 10;
            this.label2.Tag = "SendMsg";
            this.label2.Text = "微信登录二维码";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(484, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(53, 12);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "消息内容";
            // 
            // lstFriendData
            // 
            this.lstFriendData.FullRowSelect = true;
            this.lstFriendData.Location = new System.Drawing.Point(2, 24);
            this.lstFriendData.Name = "lstFriendData";
            this.lstFriendData.Size = new System.Drawing.Size(463, 270);
            this.lstFriendData.TabIndex = 12;
            this.lstFriendData.UseCompatibleStateImageBehavior = false;
            this.lstFriendData.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // btnRefreshVC
            // 
            this.btnRefreshVC.Location = new System.Drawing.Point(568, 142);
            this.btnRefreshVC.Name = "btnRefreshVC";
            this.btnRefreshVC.Size = new System.Drawing.Size(128, 23);
            this.btnRefreshVC.TabIndex = 13;
            this.btnRefreshVC.Tag = "Common";
            this.btnRefreshVC.Text = "重新生成二维码";
            this.btnRefreshVC.UseVisualStyleBackColor = true;
            this.btnRefreshVC.Click += new System.EventHandler(this.btnRefreshVC_Click);
            // 
            // lstSelectFriend
            // 
            this.lstSelectFriend.FullRowSelect = true;
            this.lstSelectFriend.Location = new System.Drawing.Point(2, 321);
            this.lstSelectFriend.Name = "lstSelectFriend";
            this.lstSelectFriend.Size = new System.Drawing.Size(463, 270);
            this.lstSelectFriend.TabIndex = 14;
            this.lstSelectFriend.Tag = "SendMsg";
            this.lstSelectFriend.UseCompatibleStateImageBehavior = false;
            this.lstSelectFriend.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "add.png");
            // 
            // ckClearText
            // 
            this.ckClearText.AutoSize = true;
            this.ckClearText.Checked = true;
            this.ckClearText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckClearText.Location = new System.Drawing.Point(568, 107);
            this.ckClearText.Name = "ckClearText";
            this.ckClearText.Size = new System.Drawing.Size(72, 16);
            this.ckClearText.TabIndex = 15;
            this.ckClearText.Tag = "SendMsg";
            this.ckClearText.Text = "清除消息";
            this.ckClearText.UseVisualStyleBackColor = true;
            // 
            // ckAppendDateTime
            // 
            this.ckAppendDateTime.AutoSize = true;
            this.ckAppendDateTime.Checked = true;
            this.ckAppendDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAppendDateTime.Location = new System.Drawing.Point(646, 105);
            this.ckAppendDateTime.Name = "ckAppendDateTime";
            this.ckAppendDateTime.Size = new System.Drawing.Size(84, 16);
            this.ckAppendDateTime.TabIndex = 16;
            this.ckAppendDateTime.Tag = "SendMsg";
            this.ckAppendDateTime.Text = "追加时间戳";
            this.ckAppendDateTime.UseVisualStyleBackColor = true;
            // 
            // btnClearTextprocess
            // 
            this.btnClearTextprocess.Location = new System.Drawing.Point(699, 185);
            this.btnClearTextprocess.Name = "btnClearTextprocess";
            this.btnClearTextprocess.Size = new System.Drawing.Size(75, 23);
            this.btnClearTextprocess.TabIndex = 17;
            this.btnClearTextprocess.Tag = "SendMsg";
            this.btnClearTextprocess.Text = "清除进度";
            this.btnClearTextprocess.UseVisualStyleBackColor = true;
            this.btnClearTextprocess.Click += new System.EventHandler(this.btnClearTextprocess_Click);
            // 
            // ckSelectAllFriend
            // 
            this.ckSelectAllFriend.AutoSize = true;
            this.ckSelectAllFriend.Location = new System.Drawing.Point(161, 4);
            this.ckSelectAllFriend.Name = "ckSelectAllFriend";
            this.ckSelectAllFriend.Size = new System.Drawing.Size(48, 16);
            this.ckSelectAllFriend.TabIndex = 18;
            this.ckSelectAllFriend.Tag = "lstFriendData&SendMsg";
            this.ckSelectAllFriend.Text = "全选";
            this.ckSelectAllFriend.UseVisualStyleBackColor = true;
            this.ckSelectAllFriend.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ckUnSelectAllFriend
            // 
            this.ckUnSelectAllFriend.AutoSize = true;
            this.ckUnSelectAllFriend.Location = new System.Drawing.Point(105, 296);
            this.ckUnSelectAllFriend.Name = "ckUnSelectAllFriend";
            this.ckUnSelectAllFriend.Size = new System.Drawing.Size(60, 16);
            this.ckUnSelectAllFriend.TabIndex = 19;
            this.ckUnSelectAllFriend.Tag = "lstSelectFriend&SendMsg";
            this.ckUnSelectAllFriend.Text = "全不选";
            this.ckUnSelectAllFriend.UseVisualStyleBackColor = true;
            this.ckUnSelectAllFriend.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // lblFriendFilter
            // 
            this.lblFriendFilter.AutoSize = true;
            this.lblFriendFilter.Location = new System.Drawing.Point(253, 5);
            this.lblFriendFilter.Name = "lblFriendFilter";
            this.lblFriendFilter.Size = new System.Drawing.Size(53, 12);
            this.lblFriendFilter.TabIndex = 20;
            this.lblFriendFilter.Tag = "SendMsg";
            this.lblFriendFilter.Text = "好友筛选";
            // 
            // txtFriendFilter
            // 
            this.txtFriendFilter.Location = new System.Drawing.Point(312, 2);
            this.txtFriendFilter.Name = "txtFriendFilter";
            this.txtFriendFilter.Size = new System.Drawing.Size(153, 21);
            this.txtFriendFilter.TabIndex = 21;
            this.txtFriendFilter.Tag = "SendMsg";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(699, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Tag = "ShowList";
            this.button1.Text = "显示列表";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // ckOnlyFriend
            // 
            this.ckOnlyFriend.AutoSize = true;
            this.ckOnlyFriend.Checked = true;
            this.ckOnlyFriend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckOnlyFriend.Location = new System.Drawing.Point(95, 4);
            this.ckOnlyFriend.Name = "ckOnlyFriend";
            this.ckOnlyFriend.Size = new System.Drawing.Size(60, 16);
            this.ckOnlyFriend.TabIndex = 23;
            this.ckOnlyFriend.Text = "仅好友";
            this.ckOnlyFriend.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(171, 296);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 24;
            this.btnFilter.Text = "筛选";
            this.btnFilter.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 642);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.ckOnlyFriend);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFriendFilter);
            this.Controls.Add(this.lblFriendFilter);
            this.Controls.Add(this.ckUnSelectAllFriend);
            this.Controls.Add(this.ckSelectAllFriend);
            this.Controls.Add(this.btnClearTextprocess);
            this.Controls.Add(this.ckAppendDateTime);
            this.Controls.Add(this.ckClearText);
            this.Controls.Add(this.lstSelectFriend);
            this.Controls.Add(this.btnRefreshVC);
            this.Controls.Add(this.lstFriendData);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSelectUser);
            this.Controls.Add(this.lblFriends);
            this.Controls.Add(this.txtBoxMessage);
            this.Controls.Add(this.btnGetUserList);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lstProcess);
            this.Controls.Add(this.txtTip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "转发微信机器人 ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox lstProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTip;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtBoxMessage;
        private System.Windows.Forms.Button btnGetUserList;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Label lblFriends;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ListView lstFriendData;
        private System.Windows.Forms.Button btnRefreshVC;
        private System.Windows.Forms.ListView lstSelectFriend;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.CheckBox ckClearText;
        private System.Windows.Forms.CheckBox ckAppendDateTime;
        private System.Windows.Forms.Button btnClearTextprocess;
        private System.Windows.Forms.CheckBox ckSelectAllFriend;
        private System.Windows.Forms.CheckBox ckUnSelectAllFriend;
        private System.Windows.Forms.Label lblFriendFilter;
        private System.Windows.Forms.TextBox txtFriendFilter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox ckOnlyFriend;
        private System.Windows.Forms.Button btnFilter;

    }
}

