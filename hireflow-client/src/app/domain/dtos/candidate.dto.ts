export interface CandidateDto {
    id : string,
    userId : string,
    resumeUrl : string,
    phoneNumber : string,
    linkedinUrl? : string,
    educationlevel? : string

}

export interface CreateProfileCommand{
    resumeUrl : string,
    phoneNumber : string,
    linkedinUrl? : string,
    educationlevel? : string   
}

export enum EducationLevel {
    Bac = 'Bac',
    BacPlus2 = 'BacPlus2',
    BacPlus3 = 'BacPlus3',
    BacPlus4 = 'BacPlus4',
    BacPlus5 = 'BacPlus5',
    PhD = 'PhD'
}   