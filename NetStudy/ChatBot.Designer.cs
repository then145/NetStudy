namespace NetStudy
{
    partial class ChatBot
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
            btn_send = new Button();
            btn_clear2 = new Button();
            btn_upload = new Button();
            tB_filepath = new TextBox();
            tB_message = new TextBox();
            lbl_username = new Label();
            tB_respones = new RichTextBox();
            SuspendLayout();
            // 
            // btn_send
            // 
            btn_send.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_send.BackColor = Color.Indigo;
            btn_send.FlatStyle = FlatStyle.Flat;
            btn_send.Font = new Font("Cambria", 10F, FontStyle.Bold);
            btn_send.ForeColor = Color.Gainsboro;
            btn_send.Location = new Point(714, 44);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(94, 29);
            btn_send.TabIndex = 2;
            btn_send.Text = "Send";
            btn_send.UseVisualStyleBackColor = false;
            btn_send.Click += btn_send_Click;
            // 
            // btn_clear2
            // 
            btn_clear2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_clear2.BackColor = Color.Indigo;
            btn_clear2.FlatStyle = FlatStyle.Flat;
            btn_clear2.Font = new Font("Cambria", 10F, FontStyle.Bold);
            btn_clear2.ForeColor = Color.Gainsboro;
            btn_clear2.Location = new Point(827, 43);
            btn_clear2.Name = "btn_clear2";
            btn_clear2.Size = new Size(94, 29);
            btn_clear2.TabIndex = 6;
            btn_clear2.Text = "Clear";
            btn_clear2.UseVisualStyleBackColor = false;
            btn_clear2.Click += btn_clear2_Click;
            // 
            // btn_upload
            // 
            btn_upload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_upload.Location = new Point(714, 79);
            btn_upload.Name = "btn_upload";
            btn_upload.Size = new Size(94, 29);
            btn_upload.TabIndex = 7;
            btn_upload.Text = "Upload";
            btn_upload.UseVisualStyleBackColor = true;
            btn_upload.Click += btn_upload_Click;
            // 
            // tB_filepath
            // 
            tB_filepath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_filepath.Location = new Point(12, 78);
            tB_filepath.Name = "tB_filepath";
            tB_filepath.Size = new Size(688, 27);
            tB_filepath.TabIndex = 8;
            // 
            // tB_message
            // 
            tB_message.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_message.Location = new Point(12, 45);
            tB_message.Name = "tB_message";
            tB_message.Size = new Size(688, 27);
            tB_message.TabIndex = 9;
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.FlatStyle = FlatStyle.Popup;
            lbl_username.Font = new Font("Cambria", 11F);
            lbl_username.ForeColor = Color.Gainsboro;
            lbl_username.Location = new Point(12, 9);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(94, 22);
            lbl_username.TabIndex = 10;
            lbl_username.Text = "UserName";
            // 
            // tB_respones
            // 
            tB_respones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tB_respones.Location = new Point(12, 120);
            tB_respones.Name = "tB_respones";
            tB_respones.Size = new Size(932, 545);
            tB_respones.TabIndex = 11;
            tB_respones.Text = "";
            // 
            // ChatBot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(26, 25, 62);
            ClientSize = new Size(956, 685);
            Controls.Add(tB_respones);
            Controls.Add(lbl_username);
            Controls.Add(tB_message);
            Controls.Add(tB_filepath);
            Controls.Add(btn_upload);
            Controls.Add(btn_clear2);
            Controls.Add(btn_send);
            Name = "ChatBot";
            Text = "ChatBot";
            FormClosed += ChatBot_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btn_send;
        private Button btn_clear2;
        private Button btn_upload;
        private TextBox tB_filepath;
        private TextBox tB_message;
        private Label lbl_username;
    }
}