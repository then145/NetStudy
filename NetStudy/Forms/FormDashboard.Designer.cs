namespace NetStudy.Forms
{
    partial class FormDashboard
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            monthCalendar1 = new MonthCalendar();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            btn_save = new Button();
            btn_create = new Button();
            btn_pending = new Button();
            btn_all = new Button();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            Index = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Category = new DataGridViewTextBoxColumn();
            StartDate = new DataGridViewTextBoxColumn();
            EndDate = new DataGridViewTextBoxColumn();
            IsCompleted = new DataGridViewCheckBoxColumn();
            panel2 = new Panel();
            lbl_username = new Label();
            btn_done = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // monthCalendar1
            // 
            monthCalendar1.BackColor = Color.White;
            monthCalendar1.ForeColor = Color.White;
            monthCalendar1.Location = new Point(45, 86);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 0;
            monthCalendar1.DateChanged += monthCalendar1_DateChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(monthCalendar1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(311, 667);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_done);
            groupBox1.Controls.Add(btn_save);
            groupBox1.Controls.Add(btn_create);
            groupBox1.Controls.Add(btn_pending);
            groupBox1.Controls.Add(btn_all);
            groupBox1.FlatStyle = FlatStyle.Flat;
            groupBox1.ForeColor = Color.Violet;
            groupBox1.Location = new Point(18, 320);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(262, 281);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Option";
            // 
            // btn_save
            // 
            btn_save.FlatStyle = FlatStyle.Flat;
            btn_save.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_save.ForeColor = Color.Violet;
            btn_save.Location = new Point(0, 226);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(262, 44);
            btn_save.TabIndex = 5;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_Click;
            // 
            // btn_create
            // 
            btn_create.FlatStyle = FlatStyle.Flat;
            btn_create.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_create.ForeColor = Color.Violet;
            btn_create.Location = new Point(0, 176);
            btn_create.Name = "btn_create";
            btn_create.Size = new Size(262, 44);
            btn_create.TabIndex = 4;
            btn_create.Text = "Edit";
            btn_create.UseVisualStyleBackColor = true;
            btn_create.Click += btn_create_Click;
            // 
            // btn_pending
            // 
            btn_pending.BackColor = Color.FromArgb(26, 25, 65);
            btn_pending.FlatStyle = FlatStyle.Flat;
            btn_pending.Font = new Font("Microsoft Sans Serif", 12F);
            btn_pending.Location = new Point(0, 76);
            btn_pending.Name = "btn_pending";
            btn_pending.Size = new Size(262, 44);
            btn_pending.TabIndex = 1;
            btn_pending.Text = "Pending";
            btn_pending.UseVisualStyleBackColor = false;
            btn_pending.Click += btn_pending_Click;
            // 
            // btn_all
            // 
            btn_all.BackColor = Color.FromArgb(26, 25, 62);
            btn_all.FlatStyle = FlatStyle.Flat;
            btn_all.Font = new Font("Microsoft Sans Serif", 12F);
            btn_all.Location = new Point(0, 26);
            btn_all.Name = "btn_all";
            btn_all.Size = new Size(262, 44);
            btn_all.TabIndex = 0;
            btn_all.Text = "All";
            btn_all.UseVisualStyleBackColor = false;
            btn_all.Click += btn_all_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 20F);
            label1.ForeColor = Color.Violet;
            label1.Location = new Point(64, 31);
            label1.Name = "label1";
            label1.Size = new Size(157, 46);
            label1.TabIndex = 1;
            label1.Text = "Calendar";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(25, 26, 65);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(25, 26, 65);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.Violet;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Index, Description, Category, StartDate, EndDate, IsCompleted });
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.FromArgb(25, 26, 65);
            dataGridView1.Location = new Point(311, 58);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(25, 26, 65);
            dataGridViewCellStyle2.ForeColor = Color.Violet;
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Size = new Size(890, 605);
            dataGridView1.TabIndex = 2;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            // 
            // Index
            // 
            Index.FillWeight = 50F;
            Index.HeaderText = "STT";
            Index.MinimumWidth = 6;
            Index.Name = "Index";
            Index.ReadOnly = true;
            // 
            // Description
            // 
            Description.FillWeight = 200F;
            Description.HeaderText = "Description";
            Description.MinimumWidth = 6;
            Description.Name = "Description";
            Description.ReadOnly = true;
            // 
            // Category
            // 
            Category.HeaderText = "Category";
            Category.MinimumWidth = 6;
            Category.Name = "Category";
            Category.ReadOnly = true;
            Category.Resizable = DataGridViewTriState.True;
            Category.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // StartDate
            // 
            StartDate.HeaderText = "Start Date";
            StartDate.MinimumWidth = 6;
            StartDate.Name = "StartDate";
            StartDate.ReadOnly = true;
            // 
            // EndDate
            // 
            EndDate.HeaderText = "End Date";
            EndDate.MinimumWidth = 6;
            EndDate.Name = "EndDate";
            EndDate.ReadOnly = true;
            // 
            // IsCompleted
            // 
            IsCompleted.HeaderText = "Done";
            IsCompleted.MinimumWidth = 6;
            IsCompleted.Name = "IsCompleted";
            IsCompleted.ReadOnly = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(lbl_username);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(311, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(900, 52);
            panel2.TabIndex = 3;
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Font = new Font("Segoe UI", 13F);
            lbl_username.ForeColor = Color.Gainsboro;
            lbl_username.Location = new Point(6, 9);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(115, 30);
            lbl_username.TabIndex = 6;
            lbl_username.Text = "UserName";
            // 
            // btn_done
            // 
            btn_done.FlatStyle = FlatStyle.Flat;
            btn_done.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_done.ForeColor = Color.Violet;
            btn_done.Location = new Point(0, 126);
            btn_done.Name = "btn_done";
            btn_done.Size = new Size(262, 44);
            btn_done.TabIndex = 6;
            btn_done.Text = "Completed";
            btn_done.UseVisualStyleBackColor = true;
            btn_done.Click += btn_done_Click;
            // 
            // FormDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(26, 25, 62);
            ClientSize = new Size(1211, 667);
            Controls.Add(panel2);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Name = "FormDashboard";
            Text = "FormDashboard";
            Load += FormDashboard_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MonthCalendar monthCalendar1;
        private Panel panel1;
        private Label label1;
        private GroupBox groupBox1;
        private Button btn_pending;
        private Button btn_all;
        private DataGridView dataGridView1;
        private Panel panel2;
        private Button btn_create;
        private Label lbl_username;
        private Button btn_save;
        private DataGridViewTextBoxColumn Index;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Category;
        private DataGridViewTextBoxColumn StartDate;
        private DataGridViewTextBoxColumn EndDate;
        private DataGridViewCheckBoxColumn IsCompleted;
        private Button btn_done;
    }
}