using AISchool.Data;
using AISchool.Models;

namespace AISchool.Views
{
    public partial class MainForm : Form
    {
        private readonly Stack<UserControl> _navigationHistory = new Stack<UserControl>();
        private AppUser? _currentUser;
        private readonly IDataAccess _dataAccess;

        public MainForm()
        {
            InitializeComponent();
            _dataAccess = new DataAccess();

            NavigateTo(new LoginControl(), false);
        }

        private void backMenuItem_Click(object sender, EventArgs e) => NavigateBack();

        private void ChangePasswordMenuItem_Click(object? sender, EventArgs e)
        {
            if (_currentUser == null) return;
            if (_currentUser.PasswordHash == null || _currentUser.PasswordSalt == null)
            {
                MessageBox.Show("Не удалось проверить текущего пользователя. Попробуйте войти заново.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var form = new Form
            {
                Text = "Смена пароля",
                Size = new Size(400, 240),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var oldPassLabel = new Label { Text = "Старый пароль:", Left = 20, Top = 20 };
            var oldPassText = new TextBox { Left = 150, Top = 20, Width = 200, UseSystemPasswordChar = true };

            var newPassLabel = new Label { Text = "Новый пароль:", Left = 20, Top = 60 };
            var newPassText = new TextBox { Left = 150, Top = 60, Width = 200, UseSystemPasswordChar = true };

            var confirmPassLabel = new Label { Text = "Подтвердите пароль:", Left = 20, Top = 100 };
            var confirmPassText = new TextBox { Left = 150, Top = 100, Width = 200, UseSystemPasswordChar = true };

            var okButton = new Button { Text = "Сохранить", Left = 190, Top = 150, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 280, Top = 150, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { oldPassLabel, oldPassText, newPassLabel, newPassText, confirmPassLabel, confirmPassText, okButton, cancelButton });
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                string oldPassword = oldPassText.Text;
                string newPassword = newPassText.Text;
                string confirmPassword = confirmPassText.Text;

                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
                {
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Новый пароль и подтверждение не совпадают.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isOldPasswordValid = PasswordHasher.VerifyPassword(
                    oldPassword,
                    Convert.ToBase64String(_currentUser.PasswordHash),
                    Convert.ToBase64String(_currentUser.PasswordSalt)
                );

                if (!isOldPasswordValid)
                {
                    MessageBox.Show("Старый пароль введен неверно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var (newHash, newSalt) = PasswordHasher.HashPassword(newPassword);
                    _dataAccess.ChangeUserPassword(_currentUser.Id, _currentUser.Role, newHash, newSalt);

                    _currentUser.PasswordHash = Convert.FromBase64String(newHash);
                    _currentUser.PasswordSalt = Convert.FromBase64String(newSalt);

                    MessageBox.Show("Пароль успешно изменен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при смене пароля: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogoutMenuItem_Click(object? sender, EventArgs e)
        {
            _currentUser = null;
            _navigationHistory.Clear();
            this.Text = "АИС 'Школа'";

            mainMenuStrip.Visible = false;

            contentPanel.Controls.Clear();

            var loginControl = new LoginControl();
            SubscribeEvents(loginControl);
            contentPanel.Controls.Add(loginControl);
        }

        private void NavigateTo(UserControl newControl, bool keepInHistory = true)
        {
            UserControl? oldControl = contentPanel.Controls.Count > 0 ? contentPanel.Controls[0] as UserControl : null;
            if (oldControl != null)
            {
                if (keepInHistory && oldControl is not LoginControl)
                {
                    _navigationHistory.Push(oldControl);
                }
                UnsubscribeEvents(oldControl);
            }
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(newControl);
            SubscribeEvents(newControl);

            backMenuItem.Visible = _navigationHistory.Count > 0;
        }

        private void NavigateBack()
        {
            if (_navigationHistory.Count > 0)
            {
                var previousControl = _navigationHistory.Pop();
                NavigateTo(previousControl, false);
            }
        }

        private void SubscribeEvents(UserControl control)
        {
            if (control is LoginControl login) login.LoginSuccess += LoginControl_LoginSuccess;
            if (control is TeacherDashboardControl dashboard) dashboard.StudentSelected += TeacherDashboard_StudentSelected;
            if (control is StudentProfileControl profile) profile.GoBackRequested += NavigateBack;

            if (control is AdminDashboardControl adminDash)
            {
                adminDash.OpenStudentProfileRequested += AdminDashboard_OpenProfile;
            }
        }

        private void UnsubscribeEvents(UserControl control)
        {
            if (control is LoginControl login) login.LoginSuccess -= LoginControl_LoginSuccess;
            if (control is TeacherDashboardControl dashboard) dashboard.StudentSelected -= TeacherDashboard_StudentSelected;
            if (control is StudentProfileControl profile) profile.GoBackRequested -= NavigateBack;

            if (control is AdminDashboardControl adminDash)
            {
                adminDash.OpenStudentProfileRequested -= AdminDashboard_OpenProfile;
            }
        }

        private void AdminDashboard_OpenProfile(int studentId)
        {
            if (_currentUser != null)
            {
                NavigateTo(new StudentProfileControl(studentId, "admin", _currentUser.Id));
            }
        }

        private void LoginControl_LoginSuccess(AppUser user)
        {
            _currentUser = user;

            mainMenuStrip.Visible = true;
            _navigationHistory.Clear();
            backMenuItem.Visible = false;

            switch (_currentUser.Role)
            {
                case "admin":
                    NavigateTo(new AdminDashboardControl(_currentUser), false);
                    break;
                case "teacher":
                    NavigateTo(new TeacherDashboardControl(_currentUser), false);
                    break;
                case "student":
                    NavigateTo(new StudentProfileControl(_currentUser.Id, _currentUser.Role, _currentUser.Id), false);
                    break;
                case "parent":
                    if (_currentUser.RelatedStudentId.HasValue)
                    {
                        NavigateTo(new StudentProfileControl(_currentUser.RelatedStudentId.Value, _currentUser.Role, _currentUser.Id), false);
                    }
                    else
                    {
                        MessageBox.Show("К вашему профилю не привязан ни один ученик.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogoutMenuItem_Click(null, EventArgs.Empty);
                    }
                    break;
            }
        }

        private void TeacherDashboard_StudentSelected(StudentInfo student)
        {
            if (_currentUser != null)
            {
                NavigateTo(new StudentProfileControl(student.Id, _currentUser.Role, _currentUser.Id));
            }
        }
    }
}