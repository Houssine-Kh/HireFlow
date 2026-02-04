namespace HireFlow.Domain.Users.Enums
{
    public enum UserStatus
    {
        Pending = 0, // Default fo Recruiters
        Active = 1,  // Default for Candidates / Approved Recruiter
        Banned = 2   // Locked by Admin
    }
}