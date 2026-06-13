
--cho csdl ql_sieuthi bao gồm t_sieuthi(MaST, TenST, DiaChi, Email, Sdt)
--1. su dung sql server để tổ chức cơ sở dữ liệu trên, insert 5 truong du liẹu vào bảng
--2. hãy sử dụng visual studio để viết các service thực hiện các chức năng thêm sửa xoá tìm kiếm và hiển thị.
--Xây dựng giao diện là quản lý hệ thống siêu thị tương ứng với bảng t_sieuthi để gọi các service thực hiện 
--chức năng thêm sửa xoá tìm kiếm và hiển thị
USE master;
DROP DATABASE ql_sieuthi;
CREATE DATABASE ql_sieuthi;
GO
USE ql_sieuthi;
GO

CREATE TABLE t_sieuthi(
MaST VARCHAR(50) NOT NULL,
TenST NVARCHAR(100) NOT NULL,
DiaChi    NVARCHAR(200) NULL,
Email     VARCHAR(100)  NULL,
Sdt       VARCHAR(20)   NULL,
);

INSERT INTO t_sieuthi (MaST, TenST, DiaChi, Email, Sdt) VALUES
('ST1', N'Siêu thị 1', N'Số 1 đường abc','st1@sieuthi.com','0987654321'),
('ST2', N'Siêu thị 2', N'Số 1 đường bac','st2@sieuthi.com','0986754321'),
('ST3', N'Siêu thị 3', N'Số 1 đường acb','st3@sieuthi.com','0987564321'),
('ST4', N'Siêu thị 4', N'Số 1 đường cab','st4@sieuthi.com','0987645321'),
('ST5', N'Siêu thị 5', N'Số 1 đường cba','st5@sieuthi.com','0978654321');
