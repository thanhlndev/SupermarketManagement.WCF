using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SupermarketManagement.WCF
{
    [ServiceBehavior]
    public class SieuThiService : ISieuThiService
    {
        private readonly string _connectionString = "Data Source=DESKTOP-3OQCIT0;Database=ql_sieuthi;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public List<SieuthiDTO> GetAll()
        {
            var list = new List<SieuthiDTO>();
            string query = "SELECT MaST, TenST, DiaChi, Email, Sdt FROM t_sieuthi";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapDataReaderToDTO(reader));
                        }
                    }
                }
            }
            return list;
        }

        public List<SieuthiDTO> Search(string keyword)
        {
            var list = new List<SieuthiDTO>();

            // Nếu từ khóa trống, trả về toàn bộ danh sách (Best practice cho UX)
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAll();
            }

            // Sử dụng LIKE và bọc dấu % ở hai đầu để tìm kiếm gần đúng
            string query = @"SELECT MaST, TenST, DiaChi, Email, Sdt 
                    FROM t_sieuthi 
                    WHERE TenST LIKE @Keyword OR Email LIKE @Keyword OR Sdt LIKE @Keyword";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm dấu % vào parameter để tránh lỗi SQL Injection nguy hiểm
                    cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 500).Value = "%" + keyword.Trim() + "%";

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapDataReaderToDTO(reader));
                        }
                    }
                }
            }
            return list;
        }
        public SieuthiDTO GetById(string maST)
        {
            string query = "SELECT MaST, TenST, DiaChi, Email, Sdt FROM t_sieuthi WHERE MaST = @MaST";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaST", SqlDbType.VarChar, 50).Value = maST;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapDataReaderToDTO(reader);
                        }
                    }
                }
            }
            return null;
        }

        public bool Insert(SieuthiDTO sieuthi)
        {
            // Defensive Programming: Validate dữ liệu trước khi tống xuống Database để tránh lỗi hạ tầng
            ValidateDTO(sieuthi);

            string query = @"INSERT INTO t_sieuthi (MaST, TenST, DiaChi, Email, Sdt) 
                            VALUES (@MaST, @TenST, @DiaChi, @Email, @Sdt)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Ép kiểu dữ liệu rõ ràng kèm độ dài (Length) để tránh lỗi dính chữ hoặc Truncation bất ngờ
                    cmd.Parameters.Add("@MaST", SqlDbType.VarChar, 50).Value = sieuthi.MaST;
                    cmd.Parameters.Add("@TenST", SqlDbType.NVarChar, 250).Value = sieuthi.TenST ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 500).Value = sieuthi.DiaChi ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = sieuthi.Email ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Sdt", SqlDbType.VarChar, 20).Value = sieuthi.Sdt ?? (object)DBNull.Value;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(SieuthiDTO sieuthi)
        {
            ValidateDTO(sieuthi);

            string query = @"UPDATE t_sieuthi 
                            SET TenST = @TenST, DiaChi = @DiaChi, Email = @Email, Sdt = @Sdt 
                            WHERE MaST = @MaST";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaST", SqlDbType.VarChar, 50).Value = sieuthi.MaST;
                    cmd.Parameters.Add("@TenST", SqlDbType.NVarChar, 250).Value = sieuthi.TenST ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 500).Value = sieuthi.DiaChi ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = sieuthi.Email ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Sdt", SqlDbType.VarChar, 20).Value = sieuthi.Sdt ?? (object)DBNull.Value;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string maST)
        {
            string query = "DELETE FROM t_sieuthi WHERE MaST = @MaST";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaST", SqlDbType.VarChar, 50).Value = maST;
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        #region Helper Methods (Sạch code và dễ maintain)
        private SieuthiDTO MapDataReaderToDTO(SqlDataReader reader)
        {
            return new SieuthiDTO
            {
                MaST = reader["MaST"].ToString().Trim(),
                TenST = reader["TenST"] != DBNull.Value ? reader["TenST"].ToString() : string.Empty,
                DiaChi = reader["DiaChi"] != DBNull.Value ? reader["DiaChi"].ToString() : string.Empty,
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                Sdt = reader["Sdt"] != DBNull.Value ? reader["Sdt"].ToString() : string.Empty
            };
        }

        private void ValidateDTO(SieuthiDTO dto)
        {
            if (dto == null) throw new FaultException("Dữ liệu siêu thị không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.MaST)) throw new FaultException("Mã siêu thị là bắt buộc.");

            // Ngăn chặn lỗi truncation lỗi từ tầng Application
            if (dto.MaST.Length > 50) throw new FaultException("Mã siêu thị không được vượt quá 50 ký tự.");
        }
        #endregion
    }
}