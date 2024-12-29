namespace NetStudy.Forms
{
    partial class DocumentUpload
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
            button_upload = new Button();
            label_filename = new Label();
            label_filesubject = new Label();
            label_share = new Label();
            checkBox_friend = new CheckBox();
            checkBox_group = new CheckBox();
            checkBox_all = new CheckBox();
            textBox_filetag = new TextBox();
            textBox_filename = new TextBox();
            button_select = new Button();
            textBox_src = new TextBox();
            label_description = new Label();
            textBox_description = new TextBox();
            SuspendLayout();
            // 
            // button_upload
            // 
            button_upload.BackColor = Color.SlateBlue;
            button_upload.FlatStyle = FlatStyle.Popup;
            button_upload.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button_upload.ForeColor = SystemColors.Control;
            button_upload.Location = new Point(355, 404);
            button_upload.Name = "button_upload";
            button_upload.Size = new Size(112, 34);
            button_upload.TabIndex = 8;
            button_upload.Text = "Tải lên";
            button_upload.UseVisualStyleBackColor = false;
            button_upload.Click += button_upload_Click;
            // 
            // label_filename
            // 
            label_filename.AutoSize = true;
            label_filename.Font = new Font("Arial", 10F);
            label_filename.ForeColor = SystemColors.Control;
            label_filename.Location = new Point(12, 125);
            label_filename.Name = "label_filename";
            label_filename.Size = new Size(74, 23);
            label_filename.TabIndex = 1;
            label_filename.Text = "Tên file";
            // 
            // label_filesubject
            // 
            label_filesubject.AutoSize = true;
            label_filesubject.Font = new Font("Arial", 10F);
            label_filesubject.ForeColor = SystemColors.Control;
            label_filesubject.Location = new Point(12, 200);
            label_filesubject.Name = "label_filesubject";
            label_filesubject.Size = new Size(123, 23);
            label_filesubject.TabIndex = 2;
            label_filesubject.Text = "Tên môn học";
            // 
            // label_share
            // 
            label_share.AutoSize = true;
            label_share.Font = new Font("Arial", 10F);
            label_share.ForeColor = SystemColors.Control;
            label_share.Location = new Point(12, 350);
            label_share.Name = "label_share";
            label_share.Size = new Size(76, 23);
            label_share.TabIndex = 3;
            label_share.Text = "Chia sẻ";
            // 
            // checkBox_friend
            // 
            checkBox_friend.AutoSize = true;
            checkBox_friend.Font = new Font("Arial", 10F);
            checkBox_friend.ForeColor = SystemColors.Control;
            checkBox_friend.Location = new Point(141, 350);
            checkBox_friend.Name = "checkBox_friend";
            checkBox_friend.Size = new Size(130, 27);
            checkBox_friend.TabIndex = 5;
            checkBox_friend.Text = "Chỉ bạn bè";
            checkBox_friend.UseVisualStyleBackColor = true;
            // 
            // checkBox_group
            // 
            checkBox_group.AutoSize = true;
            checkBox_group.Font = new Font("Arial", 10F);
            checkBox_group.ForeColor = SystemColors.Control;
            checkBox_group.Location = new Point(361, 350);
            checkBox_group.Name = "checkBox_group";
            checkBox_group.Size = new Size(198, 27);
            checkBox_group.TabIndex = 6;
            checkBox_group.Text = "Chỉ nhóm tham gia";
            checkBox_group.UseVisualStyleBackColor = true;
            // 
            // checkBox_all
            // 
            checkBox_all.AutoSize = true;
            checkBox_all.Font = new Font("Arial", 10F);
            checkBox_all.ForeColor = SystemColors.Control;
            checkBox_all.Location = new Point(696, 350);
            checkBox_all.Name = "checkBox_all";
            checkBox_all.Size = new Size(92, 27);
            checkBox_all.TabIndex = 7;
            checkBox_all.Text = "Tất cả";
            checkBox_all.UseVisualStyleBackColor = true;
            // 
            // textBox_filetag
            // 
            textBox_filetag.Font = new Font("Arial", 10F);
            textBox_filetag.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_filetag.Location = new Point(141, 200);
            textBox_filetag.Name = "textBox_filetag";
            textBox_filetag.Size = new Size(647, 30);
            textBox_filetag.TabIndex = 3;
            // 
            // textBox_filename
            // 
            textBox_filename.Font = new Font("Arial", 10F);
            textBox_filename.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_filename.Location = new Point(141, 125);
            textBox_filename.Name = "textBox_filename";
            textBox_filename.Size = new Size(647, 30);
            textBox_filename.TabIndex = 2;
            // 
            // button_select
            // 
            button_select.BackColor = Color.SlateBlue;
            button_select.FlatStyle = FlatStyle.Popup;
            button_select.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button_select.ForeColor = SystemColors.Control;
            button_select.Location = new Point(12, 50);
            button_select.Name = "button_select";
            button_select.Size = new Size(107, 30);
            button_select.TabIndex = 0;
            button_select.Text = "Chọn";
            button_select.UseVisualStyleBackColor = false;
            button_select.Click += button_select_Click;
            // 
            // textBox_src
            // 
            textBox_src.Font = new Font("Arial", 10F);
            textBox_src.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_src.Location = new Point(141, 50);
            textBox_src.Name = "textBox_src";
            textBox_src.ReadOnly = true;
            textBox_src.Size = new Size(647, 30);
            textBox_src.TabIndex = 1;
            // 
            // label_description
            // 
            label_description.AutoSize = true;
            label_description.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_description.ForeColor = SystemColors.Control;
            label_description.Location = new Point(12, 275);
            label_description.Name = "label_description";
            label_description.Size = new Size(61, 23);
            label_description.TabIndex = 11;
            label_description.Text = "Mô tả";
            // 
            // textBox_description
            // 
            textBox_description.Font = new Font("Arial", 10F);
            textBox_description.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_description.Location = new Point(141, 275);
            textBox_description.Name = "textBox_description";
            textBox_description.Size = new Size(647, 30);
            textBox_description.TabIndex = 4;
            // 
            // DocumentUpload
            // 
            AcceptButton = button_upload;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 33, 74);
            ClientSize = new Size(800, 450);
            Controls.Add(textBox_description);
            Controls.Add(label_description);
            Controls.Add(textBox_src);
            Controls.Add(button_select);
            Controls.Add(textBox_filename);
            Controls.Add(textBox_filetag);
            Controls.Add(checkBox_all);
            Controls.Add(checkBox_group);
            Controls.Add(checkBox_friend);
            Controls.Add(label_share);
            Controls.Add(label_filesubject);
            Controls.Add(label_filename);
            Controls.Add(button_upload);
            Name = "DocumentUpload";
            Text = "Đăng tài tài liệu - NetStudy";
            Load += DocumentUpload_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_upload;
        private Label label_filename;
        private Label label_filesubject;
        private Label label_share;
        private CheckBox checkBox_friend;
        private CheckBox checkBox_group;
        private CheckBox checkBox_all;
        private TextBox textBox_filetag;
        private TextBox textBox_filename;
        private Button button_select;
        private TextBox textBox_src;
        private Label label_description;
        private TextBox textBox_description;
    }
}