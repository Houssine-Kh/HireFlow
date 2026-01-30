using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Events;

namespace HireFlow.Application.Users.EventHandlers
{
    public class NotifyAdminOfNewRecruiterHandler
    {
        // Inject NotificationService or SignalR Hub

        public async Task Handle(RecruiterSubmittedForApprovalEvent notification, CancellationToken ct)
        {
            // Logic: Send push notification to all users with Role=Admin
            Console.WriteLine($"[ALERT] New Recruiter Pending: {notification.Email}");
        }
    }
}