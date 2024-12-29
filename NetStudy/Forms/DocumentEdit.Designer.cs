namespace NetStudy.Forms
{
    partial class DocumentEdit
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
            label_oldname = new Label();
            label_newname = new Label();
            label_olddescription = new Label();
            button_edit = new Button();
            textBox_oldname = new TextBox();
            textBox_newname = new TextBox();
            textBox_olddescription = new TextBox();
            label_newsubject = new Label();
            label_share = new Label();
            textBox_newdescription = new TextBox();
            checkBox_friend = new CheckBox();
            checkBox_group = new CheckBox();
            checkBox_all = new CheckBox();
            SuspendLayout();
            // 
            // label_oldname
            // 
            label_oldname.AutoSize = true;
            label_oldname.Font = new Font("Arial", 10F);
            label_oldname.ForeColor = SystemColors.Control;
            label_oldname.Location = new Point(12, 20);
            label_oldname.Name = "label_oldname";
            label_oldname.Size = new Size(100, 23);
            label_oldname.TabIndex = 0;
            label_oldname.Text = "Tên file cũ";
            // 
            // label_newname
            // 
            label_newname.AutoSize = true;
            label_newname.Font = new Font("Arial", 10F);
            label_newname.ForeColor = SystemColors.Control;
            label_newname.Location = new Point(12, 100);
            label_newname.Name = "label_newname";
            label_newname.Size = new Size(113, 23);
            label_newname.TabIndex = 1;
            label_newname.Text = "Tên file mới";
            // 
            // label_olddescription
            // 
            label_olddescription.AutoSize = true;
            label_olddescription.Font = new Font("Arial", 10F);
            label_olddescription.ForeColor = SystemColors.Control;
            label_olddescription.Location = new Point(12, 180);
            label_olddescription.Name = "label_olddescription";
            label_olddescription.Size = new Size(87, 23);
            label_olddescription.TabIndex = 2;
            label_olddescription.Text = "Mô tả cũ";
            // 
            // button_edit
            // 
            button_edit.BackColor = Color.SlateBlue;
            button_edit.FlatStyle = FlatStyle.Popup;
            button_edit.Font = new Font("Arial", 10F);
            button_edit.Location = new Point(355, 404);
            button_edit.Name = "button_edit";
            button_edit.Size = new Size(112, 34);
            button_edit.TabIndex = 7;
            button_edit.Text = "Chỉnh sửa";
            button_edit.UseVisualStyleBackColor = false;
            button_edit.Click += button_edit_Click;
            // 
            // textBox_oldname
            // 
            textBox_oldname.Font = new Font("Arial", 10F);
            textBox_oldname.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_oldname.Location = new Point(142, 20);
            textBox_oldname.Name = "textBox_oldname";
            textBox_oldname.ReadOnly = true;
            textBox_oldname.Size = new Size(646, 30);
            textBox_oldname.TabIndex = 0;
            // 
            // textBox_newname
            // 
            textBox_newname.Font = new Font("Arial", 10F);
            textBox_newname.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_newname.Location = new Point(142, 100);
            textBox_newname.Name = "textBox_newname";
            textBox_newname.Size = new Size(646, 30);
            textBox_newname.TabIndex = 1;
            // 
            // textBox_olddescription
            // 
            textBox_olddescription.Font = new Font("Arial", 10F);
            textBox_olddescription.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_olddescription.Location = new Point(142, 180);
            textBox_olddescription.Name = "textBox_olddescription";
            textBox_olddescription.ReadOnly = true;
            textBox_olddescription.Size = new Size(646, 30);
            textBox_olddescription.TabIndex = 2;
            // 
            // label_newsubject
            // 
            label_newsubject.AutoSize = true;
            label_newsubject.Font = new Font("Arial", 10F);
            label_newsubject.ForeColor = SystemColors.Control;
            label_newsubject.Location = new Point(12, 260);
            label_newsubject.Name = "label_newsubject";
            label_newsubject.Size = new Size(100, 23);
            label_newsubject.TabIndex = 7;
            label_newsubject.Text = "Mô tả mới";
            // 
            // label_share
            // 
            label_share.AutoSize = true;
            label_share.Font = new Font("Arial", 10F);
            label_share.ForeColor = SystemColors.Control;
            label_share.Location = new Point(12, 340);
            label_share.Name = "label_share";
            label_share.Size = new Size(76, 23);
            label_share.TabIndex = 8;
            label_share.Text = "Chia sẻ";
            // 
            // textBox_newdescription
            // 
            textBox_newdescription.Font = new Font("Arial", 10F);
            textBox_newdescription.ForeColor = Color.FromArgb(34, 33, 74);
            textBox_newdescription.Location = new Point(142, 260);
            textBox_newdescription.Name = "textBox_newdescription";
            textBox_newdescription.Size = new Size(646, 30);
            textBox_newdescription.TabIndex = 3;
            // 
            // checkBox_friend
            // 
            checkBox_friend.AutoSize = true;
            checkBox_friend.Font = new Font("Arial", 10F);
            checkBox_friend.ForeColor = SystemColors.Control;
            checkBox_friend.Location = new Point(142, 340);
            checkBox_friend.Name = "checkBox_friend";
            checkBox_friend.Size = new Size(130, 27);
            checkBox_friend.TabIndex = 4;
            checkBox_friend.Text = "Chỉ bạn bè";
            checkBox_friend.UseVisualStyleBackColor = true;
            // 
            // checkBox_group
            // 
            checkBox_group.AutoSize = true;
            checkBox_group.Font = new Font("Arial", 10F);
            checkBox_group.ForeColor = SystemColors.Control;
            checkBox_group.Location = new Point(380, 340);
            checkBox_group.Name = "checkBox_group";
            checkBox_group.Size = new Size(198, 27);
            checkBox_group.TabIndex = 5;
            checkBox_group.Text = "Chỉ nhóm tham gia";
            checkBox_group.UseVisualStyleBackColor = true;
            // 
            // checkBox_all
            // 
            checkBox_all.AutoSize = true;
            checkBox_all.Font = new Font("Arial", 10F);
            checkBox_all.ForeColor = SystemColors.Control;
            checkBox_all.Location = new Point(696, 340);
            checkBox_all.Name = "checkBox_all";
            checkBox_all.Size = new Size(92, 27);
            checkBox_all.TabIndex = 6;
            checkBox_all.Text = "Tất cả";
            checkBox_all.UseVisualStyleBackColor = true;
            // 
            // DocumentEdit
            // 
            AcceptButton = button_edit;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 33, 74);
            ClientSize = new Size(800, 450);
            Controls.Add(checkBox_all);
            Controls.Add(checkBox_group);
            Controls.Add(checkBox_friend);
            Controls.Add(textBox_newdescription);
            Controls.Add(label_share);
            Controls.Add(label_newsubject);
            Controls.Add(textBox_olddescription);
            Controls.Add(textBox_newname);
            Controls.Add(textBox_oldname);
            Controls.Add(button_edit);
            Controls.Add(label_olddescription);
            Controls.Add(label_newname);
            Controls.Add(label_oldname);
            ForeColor = SystemColors.Control;
            Name = "DocumentEdit";
            Text = "Chỉnh sửa tài liệu - NetStudy";
            Load += DocumentEdit_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_oldname;
        private Label label_newname;
        private Label label_olddescription;
        private Button button_edit;
        private TextBox textBox_oldname;
        private TextBox textBox_newname;
        private TextBox textBox_olddescription;
        private Label label_newsubject;
        private Label label_share;
        private TextBox textBox_newdescription;
        private CheckBox checkBox_friend;
        private CheckBox checkBox_group;
        private CheckBox checkBox_all;
    }
}