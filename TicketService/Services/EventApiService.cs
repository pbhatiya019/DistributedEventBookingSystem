using System.Net;
using System.Net.Http.Json;
using TicketService.DTOs;

namespace TicketService.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> EventExistsAsync(int eventId)
        {
            var response = await _httpClient.GetAsync($"/api/events/{eventId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<EventValidationDto?> GetEventByIdAsync(int eventId)
        {
            return await _httpClient.GetFromJsonAsync<EventValidationDto>($"/api/events/{eventId}");
        }
    }
}