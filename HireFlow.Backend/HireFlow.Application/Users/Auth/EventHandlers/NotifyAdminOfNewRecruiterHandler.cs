using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Domain.Users.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HireFlow.Application.Users.Auth.EventHandlers
{
    public class NotifyAdminOfNewRecruiterHandler : INotificationHandler<RecruiterSubmittedForApprovalEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<NotifyAdminOfNewRecruiterHandler> _logger;

        public NotifyAdminOfNewRecruiterHandler(IEmailService emailService, ILogger<NotifyAdminOfNewRecruiterHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(RecruiterSubmittedForApprovalEvent notification, CancellationToken ct)
        {
           _logger.LogInformation($"Processing new recruiter approval for {notification.Email}...");

            // Define the Admin Email (For now, hardcode or fetch from config)
          /*   var adminEmail = "admin@hireflow.com";
            
            var subject = "Action Required: New Recruiter Approval";
            var body = $@"
                <h1>New Recruiter Pending</h1>
                <p><strong>Name:</strong> {notification.FirstName} {notification.LastName}</p>
                <p><strong>Email:</strong> {notification.Email}</p>
                <p>Please log in to the admin dashboard to approve or reject this request.</p>
            ";


            await _emailService.SendAsync(adminEmail, subject, body); */
        }
    }
}