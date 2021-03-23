using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Repositories to database DevOpsApi.
    /// </summary>
    [Table("TbLoadHistory")]
	public class LoadHistoryModel : IDbEntity<int>
	{

#pragma warning disable 1591

		public override string ToString()
		{
			return $"Id:{Id}";
		}

		[Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
		public virtual int Id { get; set; }

		public virtual int Method { get; set; }

		public virtual DateTime LoadDate { get; set; } = DateTime.MinValue;

		public virtual DateTime ExecutionDateTime { get; set; } = DateTime.MinValue;

		public virtual DateTime? FromDate { get; set; } 

		public virtual DateTime? ToDate { get; set; } 

		public string Message { get; set; }

		public virtual bool IsSuccessful { get; set; } 

		[NotMapped]
		public virtual bool IsActive { get; set; }

	}

}


