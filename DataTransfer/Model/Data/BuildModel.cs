using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbBuilds")]
    public class BuildModel : IDbEntity<int>
    {

#pragma warning disable 1591

        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }
        public virtual int BuildId { get; set; }
        public virtual string BuildNumber { get; set; }
        public virtual int? BuildNumberRevision { get; set; }
        public virtual string BuildUri { get; set; }
        public virtual string BuildUrl { get; set; }
        public virtual string Definition_Drafts { get; set; }
        public virtual int Definition_Id { get; set; }
        public virtual string Definition_Name { get; set; }
        public virtual string Definition_Url { get; set; }
        public virtual DateTime? FinishTime { get; set; }
        public virtual bool? KeepForever { get; set; }
        public virtual DateTime? LastChangeDate { get; set; }
        public virtual string Links_Badge { get; set; }
        public virtual string Links_Self { get; set; }
        public virtual string Links_SourceVersionDisplayUri { get; set; }
        public virtual string Links_Timeline { get; set; }
        public virtual string Links_Web { get; set; }
        public virtual string LogUrl { get; set; }
        public virtual string OrchestrationPlan { get; set; }
        public virtual string Plans { get; set; }
        public virtual string PoolId { get; set; }
        public virtual string Priority { get; set; }
        public virtual string ProjectId { get; set; }
        public virtual string ProjectUrl { get; set; }
        public virtual string Properties { get; set; }
        public virtual string QueueId { get; set; }
        public virtual string QueueName { get; set; }
        public virtual DateTime? QueueTime { get; set; }
        public virtual string Reason { get; set; }
        public virtual string RepositoryId { get; set; }
        public virtual string RepositoryUrl { get; set; }
        public virtual string RequestedForAadDescriptor { get; set; }
        public virtual string RequestedForId { get; set; }
        public virtual string RequestedForIdentitieLink { get; set; }
        public virtual string RequestedForName { get; set; }
        public virtual string RequestedForUniqueName { get; set; }
        public virtual string Result { get; set; }
        public virtual bool? RetainedByRelease { get; set; }
        public virtual string SourceBranch { get; set; }
        public virtual string SourceVersion { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual string Status { get; set; }
        public virtual string Tags { get; set; }
        public virtual string TriggeredByBuild { get; set; }
        public virtual string TriggerInfo { get; set; }
        public virtual string ValidationResults { get; set; }
        public virtual DateTime LoadDate { get; set; }
        public virtual bool IsActive { get; set; }
    }

}

