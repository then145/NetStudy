namespace NetStudy.Forms
{
    partial class FormChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox_doanchat = new GroupBox();
            textBox_myusrname = new TextBox();
            textBox_otherusrname = new TextBox();
            textBox_showmsg = new TextBox();
            textBox_msg = new TextBox();
            button_send = new Button();
            SuspendLayout();
            // 
            // groupBox_doanchat
            // 
            groupBox_doanchat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox_doanchat.BackColor = Color.FromArgb(34, 33, 74);
            groupBox_doanchat.FlatStyle = FlatStyle.Flat;
            groupBox_doanchat.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox_doanchat.ForeColor = SystemColors.Control;
            groupBox_doanchat.Location = new Point(12, 51);
            groupBox_doanchat.Name = "groupBox_doanchat";
            groupBox_doanchat.Size = new Size(371, 722);
            groupBox_doanchat.TabIndex = 2;
            groupBox_doanchat.TabStop = false;
            groupBox_doanchat.Text = "Đoạn chat";
            groupBox_doanchat.Paint += groupBox_doanchat_Paint;
            // 
            // textBox_myusrname
            // 
            textBox_myusrname.BackColor = Color.Indigo;
            textBox_myusrname.BorderStyle = BorderStyle.None;
            textBox_myusrname.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_myusrname.ForeColor = SystemColors.Control;
            textBox_myusrname.Location = new Point(12, 12);
            textBox_myusrname.Name = "textBox_myusrname";
            textBox_myusrname.ReadOnly = true;
            textBox_myusrname.Size = new Size(150, 23);
            textBox_myusrname.TabIndex = 0;
            // 
            // textBox_otherusrname
            // 
            textBox_otherusrname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox_otherusrname.BackColor = Color.Indigo;
            textBox_otherusrname.BorderStyle = BorderStyle.None;
            textBox_otherusrname.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_otherusrname.ForeColor = SystemColors.Control;
            textBox_otherusrname.Location = new Point(1266, 12);
            textBox_otherusrname.Name = "textBox_otherusrname";
            textBox_otherusrname.ReadOnly = true;
            textBox_otherusrname.Size = new Size(150, 23);
            textBox_otherusrname.TabIndex = 1;
            // 
            // textBox_showmsg
            // 
            textBox_showmsg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            textBox_showmsg.BackColor = Color.FromArgb(34, 33, 74);
            textBox_showmsg.BorderStyle = BorderStyle.FixedSingle;
            textBox_showmsg.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_showmsg.ForeColor = SystemColors.Control;
            textBox_showmsg.Location = new Point(389, 51);
            textBox_showmsg.Multiline = true;
            textBox_showmsg.Name = "textBox_showmsg";
            textBox_showmsg.ReadOnly = true;
            textBox_showmsg.Size = new Size(1027, 685);
            textBox_showmsg.TabIndex = 3;
            // 
            // textBox_msg
            // 
            textBox_msg.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBox_msg.BackColor = Color.FromArgb(34, 33, 74);
            textBox_msg.BorderStyle = BorderStyle.FixedSingle;
            textBox_msg.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_msg.ForeColor = SystemColors.Control;
            textBox_msg.Location = new Point(389, 742);
            textBox_msg.Name = "textBox_msg";
            textBox_msg.Size = new Size(871, 30);
            textBox_msg.TabIndex = 4;
            textBox_msg.TextChanged += textBox_msg_TextChanged;
            // 
            // button_send
            // 
            button_send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_send.BackColor = Color.Indigo;
            button_send.FlatStyle = FlatStyle.Popup;
            button_send.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button_send.ForeColor = SystemColors.Control;
            button_send.Location = new Point(1266, 742);
            button_send.Name = "button_send";
            button_send.Size = new Size(150, 31);
            button_send.TabIndex = 5;
            button_send.Text = "Gửi";
            button_send.UseVisualStyleBackColor = false;
            button_send.Click += button_send_Click;
            // 
            // FormChat
            // 
            AcceptButton = button_send;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(26, 25, 62);
            ClientSize = new Size(1428, 785);
            Controls.Add(button_send);
            Controls.Add(textBox_myusrname);
            Controls.Add(textBox_msg);
            Controls.Add(textBox_showmsg);
            Controls.Add(textBox_otherusrname);
            Controls.Add(groupBox_doanchat);
            Margin = new Padding(4);
            Name = "FormChat";
            Text = "FormChat";
            Load += FormChat_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox_doanchat;
        private TextBox textBox_myusrname;
        private TextBox textBox_otherusrname;
        private TextBox textBox_showmsg;
        private TextBox textBox_msg;
        private Button button_send;
    }
}