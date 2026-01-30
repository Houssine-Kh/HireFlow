using HireFlow.Application.Common.Interfaces;
using HireFlow.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HireFlow.Infrastructure.Services
{
    public class FileEmailService : IEmailService
    {
        private readonly ILogger<FileEmailService> _logger;
        // Optional: Save to a folder on your Desktop
        private readonly string _pickupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SentEmails");

        public FileEmailService(ILogger<FileEmailService> logger)
        {
            _logger = logger;
            if (!Directory.Exists(_pickupDirectory))
            {
                Directory.CreateDirectory(_pickupDirectory);
            }
        }

        public Task SendAsync(string to, string subject, string body)
        {
            // 1. Log to Console (So you see it running)
            _logger.LogWarning($"[Fake Email Sent] To: {to} | Subject: {subject}");

            // 2. Save to File (So you can inspect the HTML)
            var filename = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{Guid.NewGuid()}.txt";
            var path = Path.Combine(_pickupDirectory, filename);

            var emailContent = $"TO: {to}\nSUBJECT: {subject}\n\nBODY:\n{body}";

            File.WriteAllText(path, emailContent);

            return Task.CompletedTask;
        }
    }
}