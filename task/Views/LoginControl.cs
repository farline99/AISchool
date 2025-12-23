using AISchool.Data;
using AISchool.Models;
using System.Security.Cryptography;
using System.Text;

namespace AISchool.Views
{
    public partial class LoginControl : UserControl
    {
        private readonly IDataAccess _dataAccess;

        public event Action<AppUser>? LoginSuccess;

        public LoginControl()
        {
            InitializeComponent();
            _dataAccess = new DataAccess();

            this.Dock = DockStyle.Fill;
            this.Load += (s, e) => {
                LoadSettings();
                loginTextBox.Focus();
            };
        }

        private void LoadSettings()
        {
            rememberCheckBox.Checked = Properties.Settings.Default.RememberMe;
            loginTextBox.Text = Properties.Settings.Default.Username;

            if (rememberCheckBox.Checked && !string.IsNullOrEmpty(Properties.Settings.Default.Password))
            {
                try
                {
                    byte[] encryptedData = Convert.FromBase64String(Properties.Settings.Default.Password);
                    byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                    passwordTextBox.Text = Encoding.UTF8.GetString(decryptedData);
                }
                catch
                {
                    passwordTextBox.Text = "";
                }
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.RememberMe = rememberCheckBox.Checked;
            Properties.Settings.Default.Username = loginTextBox.Text;

            if (rememberCheckBox.Checked)
            {
                try
                {
                    byte[] dataToEncrypt = Encoding.UTF8.GetBytes(passwordTextBox.Text);
                    byte[] encryptedData = ProtectedData.Protect(dataToEncrypt, null, DataProtectionScope.CurrentUser);
                    Properties.Settings.Default.Password = Convert.ToBase64String(encryptedData);
                }
                catch
                {
                    Properties.Settings.Default.Password = "";
                }
            }
            else
            {
                Properties.Settings.Default.Password = "";
            }

            Properties.Settings.Default.Save();
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            var userAuthData = _dataAccess.GetUserAuthDataByLogin(login);

            if (userAuthData == null)
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool passwordIsValid = false;

            if (userAuthData.PasswordHash != null)
            {
                string hashBase64 = Convert.ToBase64String(userAuthData.PasswordHash);
                string saltBase64 = userAuthData.PasswordSalt != null ? Convert.ToBase64String(userAuthData.PasswordSalt) : "";

                passwordIsValid = PasswordHasher.VerifyPassword(password, hashBase64, saltBase64);
            }

            if (passwordIsValid)
            {
                SaveSettings();
                LoginSuccess?.Invoke(userAuthData);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}