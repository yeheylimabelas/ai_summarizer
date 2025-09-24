using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiSummarizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextGenController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TextGenController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate([FromQuery] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return BadRequest(new { error = "input text is required" });

            var client = _httpClientFactory.CreateClient("ollama");

            var requestBody = JsonSerializer.Serialize(new
            {
                model = "llama3",
                prompt = $"Summarize this text: {input}"
            });

            var response = await client.PostAsync(
                "/api/generate",
                new StringContent(requestBody, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode,
                    new { error = "Failed to call Ollama API" });
            }

            var result = await response.Content.ReadAsStringAsync();
            return Ok(new { input, result });
        }
    }
}
