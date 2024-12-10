using System;
using System.Web.UI;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;

namespace Foodie.User
{
    public partial class forgetPass : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        
        protected void btn_click(object sender, EventArgs e)
        {
            try
            {
                // Lấy email người dùng nhập
                string userEmail = txtEmailForgot.Text.Trim();

                // Kiểm tra email không được bỏ trống
                if (string.IsNullOrEmpty(userEmail))
                {
                    lblMsg.Text = "Vui lòng nhập email.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                    return;
                }

                // Tạo email và mật khẩu của hệ thống gửi
                string accountMail = "linhdao03121@gmail.com";
                string pass = "daolinh1234";

                // Tạo thông điệp email
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(accountMail);
                mailMessage.To.Add(userEmail); // Gửi đến email của người dùng
                mailMessage.Subject = "Khôi phục mật khẩu - Foodie";

                // Tạo mật khẩu ngẫu nhiên
                Random random = new Random();
                int newPass = random.Next(11111, 99999);
                mailMessage.Body = $"Xin chào,\n\nMật khẩu mới của bạn là: {newPass}\n\nVui lòng đăng nhập lại và thay đổi mật khẩu này.\n\nFoodie Team.";
                mailMessage.IsBodyHtml = true;

                // Cấu hình SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential(accountMail, "wsxoajdxbwsjvhcv");/*PASS lay o day.https://myaccount.google.com/u/0/apppasswords?pli=1&rapt=AEjHL4O5MtwQCj7ef9R_BwkLGqMpul_4ZCjaiW7kE-NcRqq2-D-W3khCWQSQHFcItdYPGCazgnjDGLWbyM0hOD_zBFFq4SLYJ5Wefa1tLhHD7eqqlLcXnwI*/

                // Gửi email
                smtpClient.Send(mailMessage);
                updatePass(newPass);
                // Hiển thị thông báo thành công
                lblMsg.Text = "Mật khẩu mới đã được gửi tới email của bạn.";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                lblMsg.Text = "Có lỗi xảy ra: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Visible = true;
            }
        }


        protected void updatePass(int _newPass)
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "UPDATE1");
            cmd.Parameters.AddWithValue("@Email", txtEmailForgot.Text.Trim());
            cmd.Parameters.AddWithValue("@Password", _newPass);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
        }
    }

    

}

