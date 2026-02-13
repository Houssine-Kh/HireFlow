using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HireFlow.Domain.Common;
using HireFlow.Domain.Exceptions;
using HireFlow.Domain.Jobs.Enums;

namespace HireFlow.Domain.Jobs.Entities
{
    public class Job : AggregateRoot
    {
        public Guid RecruiterId { get; private set; }
        public string Title { get; private set; } = default!;
        public string? Description { get; private set; }
        public WorkMode? WorkMode { get; private set; }
        public JobStatus Status { get; private set; }

        private Job() { }

        private Job(Guid id, Guid recruiterId, string title, string? description, WorkMode? workMode, JobStatus status)
        {
            Id = id;
            RecruiterId = recruiterId;
            Title = title;
            Description = description;
            WorkMode = workMode;
            Status = status;
        }

        public static Job Create(Guid id, Guid recruiterId, string title, string? description, WorkMode? workMode)
        {
            if (string.IsNullOrEmpty(title))
                throw new DomainException("Title is required ");

            return new Job(id, recruiterId, title, description, workMode, JobStatus.Draft);
        }

        public void Update(string title, string? description, WorkMode? workMode)
        {
            if (Status == JobStatus.Closed)
                throw new DomainException("Cannot edit a closed job.");

            if (string.IsNullOrWhiteSpace(title))   // protection from bad edits.
                throw new DomainException("Title cannot be empty.");

            if (Status == JobStatus.Published)
            {
                if (string.IsNullOrWhiteSpace(description) || description.Length < 50)
                    throw new DomainException("Cannot remove description from a Published job. Unpublish (Draft) it first.");

                if (workMode == null)
                    throw new DomainException("Cannot remove Work Mode from a Published job.");
            }

            Title = title;
            Description = description;
            WorkMode = workMode;
        }

        public void Publish()
        {
            if (Status == JobStatus.Published)
                return;

            if (string.IsNullOrEmpty(Description) || Description.Length < 50)
                throw new DomainException("Cannot publish. A detailed description is required.");

            if (WorkMode == null)
                throw new DomainException("Cannot publish. Please select a Work Mode (Remote/Hybrid/OnSite).");

            Status = JobStatus.Published;
        }

        public void Close()
        {
            Status = JobStatus.Closed;
        }
    }

}