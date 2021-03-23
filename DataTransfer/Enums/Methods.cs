using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTransfer.Enums
{
    public enum Methods
    {
        [Description("TbProjects")]
        Projects = 1,

        [Description("TbRepositories")]
        Repositories = 2,

        [Description("TbCommits")]
        Commits = 3,

        [Description("TbPipelines")]
        Pipelines = 4,

        [Description("TbPipelinesRuns")]
        PipelineRuns = 5,

        [Description("TbBuilds")]
        Builds = 6,

        [Description("TbReleaseDefinitions")]
        ReleaseDefinitions = 7,

        [Description("TbReleases")]
        Releases = 8,

        [Description("TbReleaseApprovals")]
        ReleaseApprovals = 9,

        [Description("GetDescriptor")]
        Descriptor = 10,

        [Description("GetStorageKey")]
        StorageKey = 11,

        [Description("TbGroups")]
        Groups = 12,

        [Description("ReleasePendingApprovals")]
        ReleasePendingApprovals = 13,







    }
}
