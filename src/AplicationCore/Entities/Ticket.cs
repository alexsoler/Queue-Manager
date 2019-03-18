using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Ticket : BaseEntity
    {
        public long Id { get; set; }
        public int OfficeId { get; set; }
        public int TaskEntityId { get; set; }
        public string ApplicationUserId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public DateTime StartAttentionDate { get; set; }
        public DateTime CompletionAttentionDate { get; set; }
        public int NumberTicket { get; set; }
        public string DisplayTokenName { get; set; }

        public TaskEntity TaskEntity { get; set; }
        public Office Office { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
    }
}
