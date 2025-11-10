using AISchool.Data;
using AISchool.Models;
using System.Security.Cryptography;
using System.Text;

namespace AISchool.Views
{
	public partial class LoginControl : UserControl
	{
		private TextBox _loginTextBox = null!;
		private TextBox _passwordTextBox = null!;
		private CheckBox _rememberCheckBox = null!;
		private readonly IDataAccess _dataAccess;

		public event Action<AppUser>? LoginSuccess;

		public LoginControl()
		{
			_dataAccess = new DataAccess();
			SetupUI();
			this.Dock = DockStyle.Fill;
			this.Load += (s, e) => {
				LoadSettings();
				_loginTextBox.Focus();
			};
		}
		
		private void SetupUI()
		{
			var centerPanel = new Panel
			{
				Size = new Size(320, 180),
				Location = new Point((this.Width - 320) / 2, (this.Height - 180) / 2),
				Anchor = AnchorStyles.None
			};

			var loginLabel = new Label { Text = "Логин:", Location = new Point(20, 23), AutoSize = true };
			_loginTextBox = new TextBox { Location = new Point(100, 20), Size = new Size(190, 23) };

			var passwordLabel = new Label { Text = "Пароль:", Location = new Point(20, 53), AutoSize = true };
			_passwordTextBox = new TextBox { Location = new Point(100, 50), Size = new Size(190, 23), UseSystemPasswordChar = true };

			_rememberCheckBox = new CheckBox { Text = "Сохранить пароль", Location = new Point(100, 80), AutoSize = true };

			var loginButton = new Button { Text = "Войти", Location = new Point(215, 120), Size = new Size(75, 25) };
			loginButton.Click += LoginButton_Click;

			centerPanel.Controls.AddRange(new Control[] { loginLabel, _loginTextBox, passwordLabel, _passwordTextBox, _rememberCheckBox, loginButton });
			this.Controls.Add(centerPanel);
		}

		private void LoadSettings()
		{
			_rememberCheckBox.Checked = Properties.Settings.Default.RememberMe;
			_loginTextBox.Text = Properties.Settings.Default.Username;

			if (_rememberCheckBox.Checked && !string.IsNullOrEmpty(Properties.Settings.Default.Password))
			{
				try
				{
					byte[] encryptedData = Convert.FromBase64String(Properties.Settings.Default.Password);
					byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
					_passwordTextBox.Text = Encoding.UTF8.GetString(decryptedData);
				}
				catch
				{
					_passwordTextBox.Text = "";
				}
			}
		}

		private void SaveSettings()
		{
			Properties.Settings.Default.RememberMe = _rememberCheckBox.Checked;
			Properties.Settings.Default.Username = _loginTextBox.Text;

			if (_rememberCheckBox.Checked)
			{
				try
				{
					byte[] dataToEncrypt = Encoding.UTF8.GetBytes(_passwordTextBox.Text);
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
			string login = _loginTextBox.Text;
			string password = _passwordTextBox.Text;

			var userAuthData = _dataAccess.GetUserAuthDataByLogin(login);

			if (userAuthData == null)
			{
				MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			bool passwordIsValid = false;

			if (userAuthData.PasswordHash != null && userAuthData.PasswordSalt != null)
			{
				passwordIsValid = PasswordHasher.VerifyPassword(
					password,
					Convert.ToBase64String(userAuthData.PasswordHash),
					Convert.ToBase64String(userAuthData.PasswordSalt)
				);
			}

			if (passwordIsValid && userAuthData != null)
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