using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbPipelines")]
    public class PipelineModel : IDbEntity<int>
    {
        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual int PipelineId {get; set;}

        public virtual string ProjectId { get; set; }

        public virtual string Url { get; set; }
        
        public virtual string SelfLink { get; set; }

        public virtual string WebLink { get; set; }

        public virtual int Revision { get; set; }

        public virtual string Name { get; set; }

        public virtual string Folder { get; set; }

        public virtual DateTime LoadDate { get; set; }
                
        public virtual bool IsActive { get; set; }








    }
}
