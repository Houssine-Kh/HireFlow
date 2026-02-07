export type UserStatus = "Active" | "Pending" | "Banned";

export type UserRole = "Admin" | "Recruiter" | "Candidate";

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;       
  status: UserStatus;
}