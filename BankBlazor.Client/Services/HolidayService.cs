using BankBlazor.Client.Models;
using System.Net.Http.Json;

namespace BankBlazor.Client.Services
{
    public class HolidayService
    {
        private readonly HttpClient _http;

        public HolidayService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HolidayEvent?> GetNextHolidayAsync()
        {
            var response = await _http.GetFromJsonAsync<BankHolidayResponse>(
                "https://www.gov.uk/bank-holidays/scotland.json");

            return response?.Events?.OrderBy(e => DateTime.Parse(e.Date)).FirstOrDefault();
        }
    }

    public class BankHolidayResponse
    {
        public string Division { get; set; } = "";
        public List<HolidayEvent> Events { get; set; } = new();
    }
}
