using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbGroups")]
    public class GroupModel : IDbEntity<int>
    {

#pragma warning disable 1591

        public override string ToString()
        {
            return $"Id:{Id}";
        }

        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public virtual int Id { get; set; }

        public virtual string ProjectId { get; set; }
        public virtual int OurGroupCode { get; set; }
        public virtual string Domain { get; set; }
        public virtual string PrincipalName { get; set; }
        public virtual string MailAddress { get; set; }
        public virtual string Origin { get; set; }
        public virtual string OriginId { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Descriptor { get; set; }
        public virtual string Url { get; set; }

        public virtual DateTime LoadDate { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
