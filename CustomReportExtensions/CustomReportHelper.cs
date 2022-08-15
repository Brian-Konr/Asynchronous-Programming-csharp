using CustomReportExtensions.Schemas;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace CustomReportExtensions
{
    public class CustomReportHelper
    {
        /// <summary>
        /// 建立連線的 client 物件
        /// </summary>
        private readonly HttpClient _httpClient;

        public CustomReportHelper()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// TODO: 要問一下是否需要 dispose
        /// Destructor
        /// </summary>
        ~CustomReportHelper()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        /// 處理 custom report 的 POST request
        /// </summary>
        /// <param name="requestBody">本次 request 的 request body</param>
        /// <returns>response 的 Task 物件</returns>
        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            // 先將 requestBody 轉成 JSON 再 encode 成 StringContent 放在 HttpContent 中
            string requestBodyInJson = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(
                content: requestBodyInJson, 
                encoding: Encoding.UTF8, 
                mediaType: MediaTypeNames.Application.Json
            );

            // 試著 PostAsync，報錯就 throw exception 回去
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(
                    requestUri: "http://192.168.10.146:5000/api/customreport",
                    content: httpContent);
                response.EnsureSuccessStatusCode();

                // content 一樣要先 ReadAsString 成 JSON 才能 deserialize
                string responseContentInJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QueryDelegateResponse>(responseContentInJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}