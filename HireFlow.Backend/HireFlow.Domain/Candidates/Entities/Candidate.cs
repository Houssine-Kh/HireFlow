using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Candidates.Enums;
using HireFlow.Domain.Candidates.ValueObjects;
using HireFlow.Domain.Common;

namespace HireFlow.Domain.Candidates.Entities
{
    public class Candidate : AggregateRoot
    {
        public Guid UserId{get; private set;}
        public Resume Resume {get; private set;} = default!;
        public PhoneNumber Phone {get; private set;} = default!;
        
        // Optional Fields
        public LinkedInUrl? LinkedIn{get; private set;}
        public EducationLevel? EducationLevel{get; private set;}

        private Candidate(){} //Ef

        private Candidate(Guid id, Guid userId, Resume resume, PhoneNumber phone, LinkedInUrl? linkedIn, EducationLevel? educationLevel)
        {
            Id = id;
            UserId = userId;
            Resume = resume;
            Phone = phone;
            LinkedIn = linkedIn;
            EducationLevel = educationLevel;
        }

        public static Candidate Create(Guid id, Guid userId ,Resume resume, PhoneNumber phone, LinkedInUrl? linkedIn, EducationLevel? educationLevel)
        {
            return new Candidate(id, userId, resume, phone, linkedIn, educationLevel);
        }

        // for Wizard or Edit Profile 
        public void UpdateProfile(Resume resume, PhoneNumber phone, LinkedInUrl? linkedIn, EducationLevel? educationLevel)
        {
            Resume = resume;
            Phone = phone;
            LinkedIn = linkedIn;
            EducationLevel = educationLevel;
        }
    }
}