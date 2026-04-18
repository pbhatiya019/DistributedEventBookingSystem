using ClientApp.Models;
using ClientApp.Services;

Console.WriteLine("=== Distributed Event Booking System Client ===");
Console.WriteLine();

var apiClient = new ApiClientService();

// Use a unique email every time so repeated register calls do not fail
var uniqueEmail = $"pratham{DateTime.UtcNow.Ticks}@example.com";

var registerRequest = new RegisterRequest
{
    FullName = "Pratham Bhatiya",
    Email = uniqueEmail,
    Password = "Password123"
};

Console.WriteLine("Registering user...");
var registerResponse = await apiClient.RegisterAsync(registerRequest);

if (registerResponse == null)
{
    Console.WriteLine("Stopping because registration failed.");
    return;
}

Console.WriteLine($"User registered successfully. UserId = {registerResponse.UserId}");
Console.WriteLine();

// Login
var loginRequest = new LoginRequest
{
    Email = registerRequest.Email,
    Password = registerRequest.Password
};

Console.WriteLine("Logging in...");
var loginResponse = await apiClient.LoginAsync(loginRequest);

if (loginResponse == null)
{
    Console.WriteLine("Stopping because login failed.");
    return;
}

Console.WriteLine("Login successful.");
Console.WriteLine($"JWT Token: {loginResponse.Token}");
Console.WriteLine();

// Create event
var createEventRequest = new CreateEventRequest
{
    Title = "Distributed Systems Final Demo",
    Description = "A complete microservices workflow demonstration",
    Location = "Conestoga College",
    EventDate = new DateTime(2026, 5, 25, 14, 0, 0),
    Capacity = 200
};

Console.WriteLine("Creating event...");
var eventResponse = await apiClient.CreateEventAsync(createEventRequest, loginResponse.Token);

if (eventResponse == null)
{
    Console.WriteLine("Stopping because event creation failed.");
    return;
}

Console.WriteLine($"Event created successfully. EventId = {eventResponse.Id}");
Console.WriteLine($"Event Title = {eventResponse.Title}");
Console.WriteLine();

// Create ticket
var createTicketRequest = new CreateTicketRequest
{
    EventId = eventResponse.Id,
    AttendeeName = "Pratham Bhatiya",
    AttendeeEmail = registerRequest.Email
};

Console.WriteLine("Creating ticket...");
var ticketResponse = await apiClient.CreateTicketAsync(createTicketRequest, loginResponse.Token);

if (ticketResponse == null)
{
    Console.WriteLine("Stopping because ticket creation failed.");
    return;
}

Console.WriteLine($"Ticket created successfully. TicketId = {ticketResponse.Id}");
Console.WriteLine($"Ticket EventId = {ticketResponse.EventId}");
Console.WriteLine($"Booked By UserId = {ticketResponse.BookedByUserId}");
Console.WriteLine();
Console.WriteLine("=== Full distributed workflow completed successfully ===");