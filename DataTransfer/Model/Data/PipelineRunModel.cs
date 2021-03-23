using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataTransfer.Model.Data
{
    [Table("TbPipelineRuns")]
    public class PipelineRunModel: IDbEntity<int>
    {
        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual int PipelineRunId { get; set; }
        public virtual int PipelineId { get; set; }
        public virtual string ProjectId { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public virtual string SelfLink { get; set; }
     
        public virtual string WebLink  { get; set; }

        public virtual DateTime LoadDate { get; set; }

        public virtual bool IsActive { get; set; }
        

        
    }
}
