using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Projects to database DevOpsApi.
    /// </summary>
    [Table("TbProjects")]
	public class ProjectModel : IDbEntity<int>
	{

#pragma warning disable 1591

		public override string ToString()
		{
			return $"Id:{Id} - IsActive:{IsActive}";
		}

		[Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
		public virtual int Id { get; set; }

		public virtual string ProjectId { get; set; }

		public virtual string Name { get; set; }

		public virtual string Description { get; set; }

		public virtual string Url { get; set; }

		public virtual string State { get; set; }

		public virtual int Revision { get; set; }

		public virtual string Visibility { get; set; }

		public virtual DateTime LastUpdateTime { get; set; }

		public bool IsActive { get; set; }

		public DateTime LoadDate { get; set; }

	}

}


