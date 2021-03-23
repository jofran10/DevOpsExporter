using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbReleaseApprovals")]
    public class ReleaseApprovalModel : IDbEntity<int>
    {

#pragma warning disable 1591

        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual int ApprovalId { get; set; }
        public virtual string ProjectId { get; set; }
        public virtual int Revision { get; set; }

        public virtual string ApprovedBy { get; set; }
        public virtual string ApprovedByUrl { get; set; }
        public virtual string ApprovedById { get; set; }
        public virtual string ApprovedByUniqueName { get; set; }
        public virtual string ApprovedByImageUrl { get; set; }
        public virtual string ApprovedByDescriptor { get; set; }

        public virtual string Approver { get; set; }
        public virtual string ApproverUrl { get; set; }
        public virtual string ApproverId { get; set; }
        public virtual string ApproverUniqueName { get; set; }
        public virtual string ApproverImageUrl { get; set; }
        public virtual string ApproverDescriptor { get; set; }
        public virtual string ApprovalType { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual string Status { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool? IsAutomated { get; set; }
        public virtual bool? IsNotificationOn { get; set; }
        public virtual int TrialNumber { get; set; }
        public virtual int Attempt { get; set; }
        public virtual int Rank { get; set; }
        public virtual int ReleaseId { get; set; }
        public virtual string ReleaseName { get; set; }
        public virtual string ReleaseUrl { get; set; }
        public virtual int DefinitionId { get; set; }
        public virtual string DefinitionName { get; set; }
        public virtual string DefinitionPath { get; set; }
        public virtual string DefinitionUrl { get; set; }
        public virtual int EnvironmentId { get; set; }
        public virtual string EnvironmentName { get; set; }
        public virtual string EnvironmentUrl { get; set; }
        public virtual string Url { get; set; }


        public virtual DateTime LoadDate { get; set; }
        public virtual bool IsActive { get; set; }
    }

}

