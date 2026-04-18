using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClientApp.Models;

namespace ClientApp.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _authClient;
        private readonly HttpClient _eventClient;
        private readonly HttpClient _ticketClient;

        public ApiClientService()
        {
            _authClient = new HttpClient { BaseAddress = new Uri("http://localhost:5080") };
            _eventClient = new HttpClient { BaseAddress = new Uri("http://localhost:5037") };
            _ticketClient = new HttpClient { BaseAddress = new Uri("http://localhost:5129") };
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _authClient.PostAsJsonAsync("/api/auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Register failed: {response.StatusCode} - {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _authClient.PostAsJsonAsync("/api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login failed: {response.StatusCode} - {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<EventResponse?> CreateEventAsync(CreateEventRequest request, string token)
        {
            _eventClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _eventClient.PostAsJsonAsync("/api/events", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Create event failed: {response.StatusCode} - {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<EventResponse>();
        }

        public async Task<TicketResponse?> CreateTicketAsync(CreateTicketRequest request, string token)
        {
            _ticketClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _ticketClient.PostAsJsonAsync("/api/tickets", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Create ticket failed: {response.StatusCode} - {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TicketResponse>();
        }
    }
}