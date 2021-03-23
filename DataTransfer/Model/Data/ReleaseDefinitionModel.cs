using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbReleaseDefinitions")]
    public class ReleaseDefinitionModel : IDbEntity<int>
    {

#pragma warning disable 1591

        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual int DefinitionId { get; set; }
        public virtual string ProjectId { get; set; }
        public virtual string Source { get; set; }
        public virtual int Revision { get; set; }
        public virtual string Description { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string CreatedByUrl { get; set; }
        public virtual string CreatedById { get; set; }
        public virtual string CreatedByUniqueName { get; set; }
        public virtual string CreatedByImageUrl { get; set; }
        public virtual string CreatedByDescriptor { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string ModifiedByUrl { get; set; }
        public virtual string ModifiedById { get; set; }
        public virtual string ModifiedByUniqueName { get; set; }
        public virtual string ModifiedByImageUrl { get; set; }
        public virtual string ModifiedByDescriptor { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual bool? IsDeleted { get; set; }
        public virtual string ReleaseNameFormat { get; set; }
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }
        public virtual string Url { get; set; }
        public virtual string SelfLink { get; set; }
        public virtual string WebLink { get; set; }

        public virtual DateTime LoadDate { get; set; }
        public virtual bool IsActive { get; set; }
    }

}

