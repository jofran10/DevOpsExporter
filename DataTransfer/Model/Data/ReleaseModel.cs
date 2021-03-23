using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    [Table("TbReleases")]
    public class ReleaseModel: IDbEntity<int>
    {
        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual int ReleaseId { get; set; }
        public virtual int DefinitionId { get; set; }
        public virtual string ProjectId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string ModifiedByUrl { get; set; }
        public virtual string ModifiedById { get; set; }
        public virtual string ModifiedByUniqueName { get; set; }
        public virtual string ModifiedByImageUrl { get; set; }
        public virtual string ModifiedByDescriptor { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string CreatedByUrl { get; set; }
        public virtual string CreatedById { get; set; }
        public virtual string CreatedByUniqueName { get; set; }
        public virtual string CreatedByImageUrl { get; set; }
        public virtual string CreatedByDescriptor { get; set; }
        public virtual string CreatedFor { get; set; }
        public virtual string CreatedForUrl { get; set; }
        public virtual string CreatedForId { get; set; }
        public virtual string CreatedForUniqueName { get; set; }
        public virtual string CreatedForImageUrl { get; set; }
        public virtual string CreatedForDescriptor { get; set; }
        public virtual string Variables { get; set; }
        public virtual string VariableGroups { get; set; }
        public virtual string DefinitionName { get; set; }
        public virtual string DefinitionPath { get; set; }
        public virtual string DefinitionUrl { get; set; }
        public virtual string DefinitionSelfLink { get; set; }
        public virtual string DefinitionWebLink { get; set; }
        public virtual int ReleaseDefinitionRevision { get; set; }
        public virtual string Description { get; set; }
        public virtual string Reason { get; set; }
        public virtual string ReleaseNameFormat { get; set; }
        public virtual bool KeepForever { get; set; }
        public virtual int DefinitionSnapshotRevision { get; set; }
        public virtual string LogsContainerUrl { get; set; }
        public virtual string Url { get; set; }
        public virtual string SelfLink { get; set; }
        public virtual string WebLink { get; set; }
        public virtual string Tags { get; set; }
        public virtual string TriggeringArtifactAlias { get; set; }
        public virtual string ProjectName { get; set; }


        public virtual DateTime LoadDate { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
