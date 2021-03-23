using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Repositories to database DevOpsApi.
    /// </summary>
    [Table("TbRepositories")]
	public class RepositoryModel : IDbEntity<int>
	{

#pragma warning disable 1591

		public override string ToString()
		{
			return $"Id:{Id} - IsActive:{IsActive}";
		}

		[Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
		public virtual int Id { get; set; }

		public virtual string RepositoryId { get; set; }

		public virtual string ProjectId { get; set; }

		public virtual string Name { get; set; }

		public virtual DateTime? LastUpdateTime { get; set; }

		public virtual string Url { get; set; }

		public virtual string DefaultBranch { get; set; }

		public virtual int Size { get; set; }

		public virtual string RemoteUrl { get; set; }

		public virtual string SshUrl { get; set; }

		public virtual string WebUrl { get; set; }

		public bool IsActive { get; set; }

		public DateTime LoadDate { get; set; }

	}

}


