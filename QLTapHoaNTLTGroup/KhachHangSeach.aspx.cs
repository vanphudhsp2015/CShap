﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLTapHoaNTLTGroup
{
    public partial class KhachHangSeach : System.Web.UI.Page
    {
        string conString;
        protected void Page_Load(object sender, EventArgs e)
        {
            conString = ConnnectionString.connectionbyname("sqlString");
            if (!Page.IsPostBack)
            {
                BindDataToGridView(-1);
            }
            if ((string)Session["MaNV"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }
        public void BindDataToGridView(int index)
        {
            SqlConnection conn = new SqlConnection(conString);
            SqlCommand com = new SqlCommand();
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    String sql = "SELECT MaKH,TenKH,GioiTinh,CONVERT(nvarchar,NamSinh,101) as NamSinh ,DiaChi,SDT,Email,Diem FROM tb_KhachHang ORDER BY MaKH desc";
                    com = new SqlCommand(sql, conn);
                    DataTable table = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(table);
                    GridView1.DataSource = table;
                    if (index >= 0)
                        GridView1.PageIndex = index;
                    GridView1.DataBind();
                    ((LinkButton)(GridView1.HeaderRow.Cells[0].Controls[0])).Text = "Mã KH";
                    ((LinkButton)(GridView1.HeaderRow.Cells[1].Controls[0])).Text = "Tên KH";
                    ((LinkButton)(GridView1.HeaderRow.Cells[2].Controls[0])).Text = "Giới Tính";
                    ((LinkButton)(GridView1.HeaderRow.Cells[3].Controls[0])).Text = "Năm Sinh";
                    ((LinkButton)(GridView1.HeaderRow.Cells[4].Controls[0])).Text = "Địa Chỉ";
                    ((LinkButton)(GridView1.HeaderRow.Cells[5].Controls[0])).Text = "SĐT";
                    ((LinkButton)(GridView1.HeaderRow.Cells[6].Controls[0])).Text = "Email";
                    ((LinkButton)(GridView1.HeaderRow.Cells[7].Controls[0])).Text = "Điểm";
                }
                else
                {
                    thongbao.Text = "Kết Nối Không Thành Công";
                }
            }
            catch (Exception e)
            {
                thongbao.Text = "Lỗi" + e.Message;
            }
            finally
            {
                conn.Close();
                com.Dispose();
            }
        }
        private void BindDataToGridView(int pageIndex, string sortExpression, string sortDirection = "ASC")
        {
            SqlConnection sqlSconn = new SqlConnection();
            SqlCommand sqlScomm4 = new SqlCommand();
            try
            {
                sqlSconn.ConnectionString = conString;
                sqlSconn.Open();
                if (sqlSconn.State == System.Data.ConnectionState.Open)
                {
                    //Doc cac ban ghi tu CSDL va the hien tren luoi du lieu
                    GridView1.DataSource = null;
                    DataTable dt = new DataTable();
                    sqlScomm4.Connection = sqlSconn;
                    sqlScomm4.CommandType = CommandType.Text;
                    sqlScomm4.CommandText = "SELECT MaKH,TenKH,GioiTinh,CONVERT(nvarchar,NamSinh,101) as NamSinh ,DiaChi,SDT,Email,Diem FROM tb_KhachHang ORDER BY MaKH desc";
                    SqlDataAdapter da = new SqlDataAdapter(sqlScomm4);
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    //sắp xếp
                    dt.DefaultView.Sort = String.Format("{0} {1}", sortExpression, sortDirection);
                    //dung cai nay de phan trang
                    if (pageIndex >= 0)
                        GridView1.PageIndex = pageIndex;

                    GridView1.DataBind();
                    //Đổi tiêu đề của cột
                    //GridView1.Columns[1].HeaderText = "Tên hàng"; //Cách này sẽ làm mất link dùng để sắp xếp
                    //GridView1.Columns[2].HeaderText = "Thể loại";
                    ((LinkButton)(GridView1.HeaderRow.Cells[0].Controls[0])).Text = "Mã KH";
                    ((LinkButton)(GridView1.HeaderRow.Cells[1].Controls[0])).Text = "Tên KH";
                    ((LinkButton)(GridView1.HeaderRow.Cells[2].Controls[0])).Text = "Giới Tính";
                    ((LinkButton)(GridView1.HeaderRow.Cells[3].Controls[0])).Text = "Năm Sinh";
                    ((LinkButton)(GridView1.HeaderRow.Cells[4].Controls[0])).Text = "Địa Chỉ";
                    ((LinkButton)(GridView1.HeaderRow.Cells[5].Controls[0])).Text = "SĐT";
                    ((LinkButton)(GridView1.HeaderRow.Cells[6].Controls[0])).Text = "Email";
                    ((LinkButton)(GridView1.HeaderRow.Cells[7].Controls[0])).Text = "Điểm";
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show
                Response.Write(String.Format("Loi: {0}\n{1}", exc.Message, exc.StackTrace));
            }
            finally
            {
                sqlSconn.Close();
                sqlScomm4.Dispose();
            }
        }
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortDirection;
                //Mặc định ASC
                if (ViewState["sortDirection"] != null) sortDirection = ViewState["sortDirection"] as string;
                else sortDirection = "ASC";
                //Đưa thông báo dưới dạng javascript alert
                Response.Write(String.Format("<script>alert('Sắp xếp {1} theo cột {0}')</script>", e.SortExpression, sortDirection));

                BindDataToGridView(GridView1.PageIndex, e.SortExpression, sortDirection);
                //Đảo chiều sau mỗi lần click
                if (sortDirection == "ASC") sortDirection = "DESC";
                else sortDirection = "ASC";
                //Lưu cách sắp xếp vào ViewState để có thể sử dụng khi chuyển sang lần gọi sau 
                ViewState["sortDirection"] = sortDirection;
            }
            catch (Exception exc)
            {
                Response.Write(String.Format("Lỗi: {0}<br />{1}", exc.Message, exc.StackTrace));
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindDataToGridView(e.NewPageIndex);
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            SqlConnection conn = new SqlConnection(conString);
            SqlCommand com = new SqlCommand();

            try
            {
                if (TextBox1.Text.Trim() == "" || TextBox2.Text.Trim() == "" || TextBox4.Text.Trim() == "" || TextBox5.Text.Trim() == "" || TextBox6.Text.Trim() == "" || TextBox7.Text.Trim() == "" || TextBox8.Text.Trim() == "" || DropDownList1.SelectedValue.ToString() == "")
                {
                    thongbao.Text = "Điền Đầy ĐỦ Thông Tin";
                }
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    String sql = "SELECT MaKH,TenKH,GioiTinh,CONVERT(nvarchar,NamSinh,101) as NamSinh ,DiaChi,SDT,Email,Diem FROM tb_KhachHang where (MaKH like '%'+@1+'%') OR (TenKH like '%'+@2+'%')  AND (GioiTinh like '%'+@3+'%')";
                    com = new SqlCommand(sql, conn);
                    com.Parameters.AddWithValue("@1", TextBox1.Text.Trim());
                    com.Parameters.AddWithValue("@2", TextBox2.Text.Trim());
                    com.Parameters.AddWithValue("@3", DropDownList1.SelectedValue.ToString());
                    com.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    thongbao.Text = "Tìm Kiếm Thành Công";
                }
                else
                {
                    thongbao.Text = "ket noi khong thanh cong";
                }
            }
            catch (Exception ee)
            {
                thongbao.Text = "loi" + ee.Message;
            }
            finally
            {
                conn.Close();
                com.Dispose();
            }

        }
    }
}