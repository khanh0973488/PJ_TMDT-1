using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace _123.Controllers
{
    public class GoldPriceController : Controller
    {
        private readonly HttpClient _httpClient;

        public GoldPriceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // API Key của bạn
        private const string ApiKey = "goldapi-hqv9xsm43maetf-io";  // Sử dụng API key của bạn

        [HttpGet]
        public async Task<IActionResult> GetGoldPrice()
        {
            try
            {
                var url = "https://www.goldapi.io/api/XAU/USD"; // Lấy giá vàng (XAU) theo USD
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("x-access-token", ApiKey); // Thêm API key vào header

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var goldPriceData = JsonConvert.DeserializeObject<GoldPriceResponse>(responseData);

                    return Json(goldPriceData);
                }
                else
                {
                    return StatusCode(500, "Không thể lấy dữ liệu từ API.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi lấy dữ liệu giá vàng: " + ex.Message);
            }
        }

        // Gửi dữ liệu giá vàng để vẽ biểu đồ
        [HttpGet]
        public async Task<IActionResult> GetGoldPriceForChart()
        {
            try
            {
                var url = "https://www.goldapi.io/api/XAU/USD"; // Lấy giá vàng (XAU) theo USD
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("x-access-token", ApiKey); // Thêm API key vào header

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var goldPriceData = JsonConvert.DeserializeObject<GoldPriceResponse>(responseData);

                    // Trả về dữ liệu giá vàng cho biểu đồ (thời gian thực)
                    return Json(new { price = goldPriceData.Price, timestamp = DateTime.UtcNow });
                }
                else
                {
                    return StatusCode(500, "Không thể lấy dữ liệu từ API.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi lấy dữ liệu giá vàng: " + ex.Message);
            }
        }
    }

    public class GoldPriceResponse
    {
        public string Symbol { get; set; }  // Ký hiệu của vàng
        public decimal Price { get; set; }  // Giá vàng hiện tại
    }
}
