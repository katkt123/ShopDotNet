using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Foodie.User
{
    public partial class OpenAI : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected async void btnCallOpenAI_Click(object sender, EventArgs e)
        {
          
            await CallOpenAIAsync();
        }

        private async Task CallOpenAIAsync()
        {
            string apiKey = "-proj-w45O7t7NVwYLfM7j0pDtIVWEHtZwQqdZa9bKMHYAuX7RLLNB654RSF2UOvxuTa6TKXlCvWD2PdT3BlbkFJvSJsZr2y9plQH3rN9EFDBA3jIP3fdDUMa34YkAVzeebytQQbmpfBrMH0021o-Qwo8gSFDb59oA"; // Đừng để key này trong mã nguồn thực tế!
            string url = "https://api.openai.com/v1/chat/completions";

            // Tạo nội dung request
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = txtUserInput.Text } // Lấy input từ textbox
                },
                max_tokens = 2000,
                temperature = 0.7
            };

            // Chuyển request thành JSON
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            // Gửi yêu cầu qua HttpClient
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var response = await client.PostAsync(url, jsonContent);

                // Đọc kết quả trả về
                string result = await response.Content.ReadAsStringAsync();

                // Hiển thị kết quả trên giao diện
                lblResult.Text = result;
            }
        }
    }
}


