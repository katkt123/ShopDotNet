using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
    public partial class User : System.Web.UI.MasterPage
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái khung chat từ Session và hiển thị tương ứng
            

            if (!Request.Url.AbsoluteUri.ToString().Contains("Default.aspx"))
            {
                form1.Attributes.Add("class", "sub_page");
            }
            else
            {
                form1.Attributes.Remove("class");
                Control sliderUserControl = (Control)Page.LoadControl("SliderUserControl.ascx");
                pnlSlideUC.Controls.Add(sliderUserControl);
            }

            if (Session["userId"] != null)
            {
                lbLoginOrLogout.Text = "Logout";
                Utils utils = new Utils();
                Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"])).ToString();
            }
            else
            {
                lbLoginOrLogout.Text = "Login";
                Session["cartCount"] = "0";
            }

            // Hiển thị lại lịch sử chat sau postback
            if (ViewState["ChatHistory"] != null)
            {
                List<(string role, string message)> chatHistory = (List<(string role, string message)>)ViewState["ChatHistory"];
                string script = "var chatContent = $('#chatContent'); chatContent.empty();";

                foreach (var chat in chatHistory)
                {
                    string sanitizedMessage = System.Web.HttpUtility.JavaScriptStringEncode(chat.message);
                    string cssClass = chat.role == "user" ? "user-msg" : "bot-msg";
                    script += $"chatContent.append('<div class=\"{cssClass}\">{sanitizedMessage}</div>');";
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "reloadChatHistory", script, true);
            }
            
        }

        protected void lbLoginOrLogout_Click(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
        }

        protected void lbRegisterOrProfile_Click(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                lbRegisterOrProfile.ToolTip = "User Profile";
                Response.Redirect("Profile.aspx");
            }
            else
            {
                lbRegisterOrProfile.ToolTip = "User Registration";
                Response.Redirect("Registration.aspx");
            }
        }

        protected void btnCallOpenAI_Click(object sender, EventArgs e)
        {
            // Giả lập dữ liệu sản phẩm
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Connection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("Product_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
            }

            string productDetails = "Hệ thống tôi có các sản phẩm như sau:\n";
            foreach (DataRow row in dt.Rows)
            {
                string name = row["Name"].ToString();
                string quantity = row["Quantity"].ToString();
                productDetails += $"- Tên Sản Phẩm: {name}, Số Lượng: {quantity}\n";
            }

            string userMessage = txtUserInput.Text;
            string apiKey = "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"; // Đừng để key này trong mã nguồn thực tế!
            string url = "https://api.openai.com/v1/chat/completions";

            // Lấy lịch sử chat từ ViewState
            List<(string role, string message)> chatHistory = ViewState["ChatHistory"] as List<(string role, string message)> ?? new List<(string role, string message)>();

            // Thêm tin nhắn người dùng vào lịch sử
            chatHistory.Add(("user", userMessage));

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = $"You are a helpful assistant. The system have the product details.\n\n{productDetails}\n\nAdditionally, Ngoài ra nếu user hỏi các câu hỏi không liên quan đến sản phẩm thì hãy trả lời không có thông tin hay gì đó chẳng hạn " },
                    new { role = "user", content = userMessage }
                },
                max_tokens = 2000,
                temperature = 0.7
            };

            try
            {
                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                    var response = client.PostAsync(url, jsonContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);
                        string aiResponse = jsonObject["choices"][0]["message"]["content"].ToString();

                        // Thêm phản hồi của AI vào lịch sử
                        chatHistory.Add(("bot", aiResponse));

                        // Cập nhật ViewState với lịch sử mới
                        ViewState["ChatHistory"] = chatHistory;

                        // Hiển thị lại toàn bộ lịch sử chat
                        string script = "var chatContent = $('#chatContent'); chatContent.empty();";

                        foreach (var chat in chatHistory)
                        {
                            string sanitizedMessage = System.Web.HttpUtility.JavaScriptStringEncode(chat.message);
                            string cssClass = chat.role == "user" ? "user-msg" : "bot-msg";
                            script += $"chatContent.append('<div class=\"{cssClass}\">{sanitizedMessage}</div>');";
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "updateChat", script, true);
                        txtUserInput.Text = "";

                    }
                    else
                    {
                        lblResult.Text = "Error: Unable to fetch response.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Có lỗi xảy ra: {ex.Message}";
            }
        }
    }
}
