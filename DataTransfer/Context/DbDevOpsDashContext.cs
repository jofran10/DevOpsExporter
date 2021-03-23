using DataTransfer.Enums;
using DataTransfer.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataTransfer.Context
{
    public class DbDevOpsDashContext : DbContext
    {
        private AmbientTypes _ambient;

        public DbDevOpsDashContext()
        {
        }

        public DbDevOpsDashContext(DbContextOptions<DbDevOpsDashContext> options) : base(options)
        {
            
           
        }

        public DbDevOpsDashContext(AmbientTypes ambient) 
        {
            this._ambient = ambient;
           
        }

        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<RepositoryModel> Repositories { get; set; }
        public DbSet<CommitModel> Commits { get; set; }
        public DbSet<LoadHistoryModel> LoadHistory { get; set; }
        public DbSet<PipelineModel> Pipelines { get; set; }
        public DbSet<PipelineRunModel> PipelineRuns { get; set; }
        public DbSet<BuildModel> Builds { get; set; }
        public DbSet<LoadRestoreModel> LoadRestorePoint { get; set; }
        public DbSet<ReleaseDefinitionModel> ReleaseDefinitions { get; set; }
        public DbSet<ReleaseModel> Releases { get; set; }
        public DbSet<ReleaseApprovalModel> ReleaseApprovals { get; set; }
        public DbSet<GroupModel> Groups { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(GetStringConectionConfig());
            base.OnConfiguring(optionsBuilder);
        }

        private string GetStringConectionConfig()
        {
            string strCon = Environment.GetEnvironmentVariable("DbDevOpsDash");
            return strCon;
        }

        

        
    }
}
