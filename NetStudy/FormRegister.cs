﻿using NetStudy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetStudy
{
    public partial class FormRegister : Form
    {
        private readonly UserService userService;
        public FormRegister()
        {
            InitializeComponent();
            userService = new UserService();
            LoadForm();
        }
        public void LoadForm()
        {
            int year = DateTime.Now.Year;

            for (int i = 1900; i <= year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }

            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add(i.ToString());
            }


        }
        public bool checkEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w\.-]{4,30}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
        }
        public bool checkUsername(string username)
        {
            return Regex.IsMatch(username, "^[a-zA-Z0-9]{4,25}$");
        }
        public void UpdateDate()
        {
            if (cmbYear.SelectedItem == null || cmbMonth.SelectedItem == null)
            {
                return;
            }
            int month = Convert.ToInt32(cmbMonth.SelectedItem.ToString());
            int year = Convert.ToInt32(cmbYear.SelectedItem.ToString());

            int days = DateTime.DaysInMonth(year, month);

            cmbDate.Items.Clear();
            for (int i = 1; i <= days; i++)
            {
                cmbDate.Items.Add(i);
            }
            cmbDate.SelectedIndex = 0;
        }


        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text;
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmedPass = txtConfirmedPassword.Text;

            int day = int.Parse(cmbDate.SelectedItem.ToString());
            int month = int.Parse(cmbMonth.SelectedItem.ToString());
            int year = int.Parse(cmbYear.SelectedItem.ToString());

            DateTime dateOfBirth;
            if (!DateTime.TryParse($"{year}-{month}-{day}", out dateOfBirth))
            {
                MessageBox.Show("Ngày sinh không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!checkUsername(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập không chứa kí tự đặc biệt và dài từ 5 đến 15 chữ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!checkEmail(email))
            {
                MessageBox.Show("Vui lòng nhập email đúng định dạng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (password != confirmedPass)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var register = new
            {
                name = fullName,
                username = username,
                dateOfBirth = dateOfBirth.ToString("dd/MM/yyyy"),
                email = email,
                password = password,
                confirmPassword = confirmedPass
            };
            var response = await userService.Register(register);
            if (response.Contains("thành công", StringComparison.OrdinalIgnoreCase))
            {
                this.Hide();
                MessageBox.Show($"Mã OTP đã được gửi qua email {email}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VerifyOtp verifyOtp = new VerifyOtp();
                verifyOtp.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show(response, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }
    }
}
