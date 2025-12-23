using AISchool.Models;
using AISchool.Utils;
using System.Data;

namespace AISchool.Views
{
    public partial class LinkParentDialog : Form
    {
        private readonly List<ParentInfo> _allParents;

        public ParentInfo? SelectedParent => dgvParents.CurrentRow?.DataBoundItem as ParentInfo;

        public LinkParentDialog()
        {
            InitializeComponent();
            _allParents = new List<ParentInfo>();
        }

        public LinkParentDialog(List<ParentInfo> allParents) : this()
        {
            _allParents = allParents;
            this.Load += (s, e) => SetupGrid();
            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvParents.SelectionChanged += DgvParents_SelectionChanged;
            dgvParents.CellDoubleClick += DgvParents_CellDoubleClick;
        }

        private void SetupGrid()
        {
            dgvParents.DataSource = new SortableBindingList<ParentInfo>(_allParents);

            if (dgvParents.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dgvParents.Columns)
                {
                    col.Visible = false;
                }

                void ConfigCol(string name, string header, int index, DataGridViewAutoSizeColumnMode mode = DataGridViewAutoSizeColumnMode.DisplayedCells)
                {
                    if (dgvParents.Columns.Contains(name))
                    {
                        var col = dgvParents.Columns[name];
                        col.Visible = true;
                        col.HeaderText = header;
                        col.DisplayIndex = index;
                        col.AutoSizeMode = mode;
                    }
                }

                ConfigCol("FullName", "ФИО Родителя", 0, DataGridViewAutoSizeColumnMode.Fill);
                ConfigCol("Phone", "Телефон", 1);
                ConfigCol("Email", "Email", 2);
                ConfigCol("Login", "Логин", 3);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower().Trim();
            var filteredList = string.IsNullOrWhiteSpace(searchText)
                ? _allParents
                : _allParents.Where(p => p.FullName.ToLower().Contains(searchText)).ToList();

            dgvParents.DataSource = new SortableBindingList<ParentInfo>(filteredList);
        }

        private void DgvParents_SelectionChanged(object? sender, EventArgs e)
        {
            btnOk.Enabled = dgvParents.SelectedRows.Count > 0;
        }

        private void DgvParents_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
