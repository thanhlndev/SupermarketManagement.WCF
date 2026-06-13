using SupermarketManagement.WinForms.STServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagement.WinForms
{
    public partial class SieuThi : Form
    {
        public SieuThi()
        {
            InitializeComponent();
            EventBindings();
            LoadData();
        }

        private void EventBindings()
        {
            this.txtSearch.TextChanged += TxtSearch_TextChanged;
            //this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnInsert_Click);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            this.dgvSieuThi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSieuThi_CellClick);
        }
        #region Các Hàm Xử Lý Dữ Liệu (Logic Gọi WCF)

        private void LoadData()
        {
            var client = new SieuThiServiceClient();
            try
            {
                List<SieuthiDTO> list = new List<SieuthiDTO>(client.GetAll());
                dgvSieuThi.DataSource = list;

                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                    client.Close();
            }
        }

        private void FormatDataGridView()
        {
            if (dgvSieuThi.Columns["MaST"] != null) dgvSieuThi.Columns["MaST"].HeaderText = "Mã Siêu Thị";
            if (dgvSieuThi.Columns["TenST"] != null) dgvSieuThi.Columns["TenST"].HeaderText = "Tên Siêu Thị";
            if (dgvSieuThi.Columns["DiaChi"] != null) dgvSieuThi.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            if (dgvSieuThi.Columns["Email"] != null) dgvSieuThi.Columns["Email"].HeaderText = "Email";
            if (dgvSieuThi.Columns["Sdt"] != null) dgvSieuThi.Columns["Sdt"].HeaderText = "Số Điện Thoại";
        }

        private void ClearFields()
        {
            txtMaST.Text = "";
            txtTenST.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtSdt.Text = "";
            txtMaST.Enabled = true;
            txtMaST.Focus();
        }

        #endregion

        #region Form Events

        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    var client = new SieuThiServiceClient();
        //    try
        //    {
        //        string keyword = txtSearch.Text.Trim();
        //        var result = client.Search(keyword);
        //        dgvSieuThi.DataSource = new List<SieuthiDTO>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        client.Close();
        //    }
        //}
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (var client = new SieuThiServiceClient())
            {
                try
                {
                    var result = client.Search(keyword);

                    dgvSieuThi.DataSource = (result != null) ? new List<SieuthiDTO>(result) : null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi tìm kiếm: " + ex.Message);
                }
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaST.Text) || string.IsNullOrWhiteSpace(txtTenST.Text))
            {
                MessageBox.Show("Mã và Tên siêu thị không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var client = new SieuThiServiceClient();
            try
            {
                var newSieuThi = new SieuthiDTO
                {
                    MaST = txtMaST.Text.Trim(),
                    TenST = txtTenST.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Sdt = txtSdt.Text.Trim()
                };

                bool isSuccess = client.Insert(newSieuThi);
                if (isSuccess)
                {
                    MessageBox.Show("Thêm mới siêu thị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi từ Server: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                client.Close();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var client = new SieuThiServiceClient();
            try
            {
                var updateSieuThi = new SieuthiDTO
                {
                    MaST = txtMaST.Text.Trim(),
                    TenST = txtTenST.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Sdt = txtSdt.Text.Trim()
                };

                if (client.Update(updateSieuThi))
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                client.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string maST = txtMaST.Text.Trim();
            if (string.IsNullOrEmpty(maST)) return;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa siêu thị {maST} không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var client = new SieuThiServiceClient();
                try
                {
                    if (client.Delete(maST))
                    {
                        MessageBox.Show("Xóa siêu thị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    client.Close();
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadData();
        }

        private void dgvSieuThi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSieuThi.Rows[e.RowIndex];

                txtMaST.Text = row.Cells["MaST"].Value?.ToString();
                txtTenST.Text = row.Cells["TenST"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtSdt.Text = row.Cells["Sdt"].Value?.ToString();

                // Không cho phép sửa Mã Siêu Thị (Khóa chính) khi đang chọn edit dòng đó
                txtMaST.Enabled = false;
            }
        }

        #endregion
    }
}