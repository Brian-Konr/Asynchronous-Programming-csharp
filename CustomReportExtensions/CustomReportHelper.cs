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

        public async Task<QueryDelegateResponse?> PostCustomReport(CustomReportRequest requestBody)
        {
            string requestBodyInJson = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(
                content: requestBodyInJson, 
                encoding: Encoding.UTF8, 
                mediaType: MediaTypeNames.Application.Json
            );

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(
                    requestUri: "http://192.168.10.146:5000/api/customreport",
                    content: httpContent);
                response.EnsureSuccessStatusCode();
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