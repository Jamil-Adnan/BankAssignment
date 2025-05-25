namespace BankBlazor.Client.Models
{
    public class HolidayEvent
    {
        public string Title { get; set; } = "";
        public string Date { get; set; } = "";
        public string Notes { get; set; } = "";
        public string Bunting { get; set; } = "";
    }
    public class BankHolidayResponse
    {
        public string Division { get; set; } = "";
        public List<HolidayEvent> Events { get; set; } = new();
    }
 }
