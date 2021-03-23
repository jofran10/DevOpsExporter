using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Model.Data
{
    /// <summary>
    /// Provider data model for Table Commits to database 
    /// </summary>
    [Table("TbCommits")]
	public class CommitModel : IDbEntity<int>
	{

#pragma warning disable 1591

		public override string ToString()
		{
			return $"Id:{Id}";
		}

		[Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
		public virtual int Id { get; set; }

		public virtual string CommitId { get; set; }

		public virtual string RepositoryId { get; set; }

		public virtual string Comment { get; set; }

		public virtual string Url { get; set; }

		public virtual string RemoteUrl { get; set; }

		public virtual bool? CommentTruncated { get; set; }

		public virtual string AuthorName { get; set; }

		public virtual string AuthorEmail { get; set; }

		public virtual DateTime AuthorDate { get; set; }

		public virtual string CommitterName { get; set; }

		public virtual string CommitterEmail { get; set; }

		public virtual DateTime CommitterDate { get; set; }

		public virtual int ChangeCountsAdd { get; set; }

		public virtual int ChangeCountsEdit { get; set; }

		public virtual int ChangeCountsDelete { get; set; }

		public DateTime LoadDate { get; set; }

		[NotMapped]
		public bool IsActive { get; set; }

	}

}

