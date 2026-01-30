export interface AuthResponseDto{
    id : string;
    FirstName : string;
    LastName : string;
    email : string;
    token : string
    role : string;
    IsProfileComplete : boolean;
}

export interface LoginCommand{
    email : string;
    password : string;
}

export interface RegisterCommand{
    firstName : string;
    lastName : string;
    email : string;
    password : string;
    role : string;
}