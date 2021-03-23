using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTransfer.Enums
{
    public enum Groups
    {
        [Description("Application Owners")]
        ApplicationOwners = 1,

        [Description("BoardsRestrictions")]
        BoardsRestrictions = 3,

        [Description("Build Administrators")]
        BuildAdministrators = 7,

        [Description("Bussiness Analysts")]
        BussinessAnalysts = 9,

        [Description("Contributors")]
        Contributors = 11,

        [Description("Deployment Group Administrators")]
        DeploymentGroupAdministrators = 13,

        [Description("Developers")]
        Developers = 15,

        [Description("Endpoint Administrators")]
        EndpointAdministrators = 17,

        [Description("Endpoint Creators")]
        EndpointCreators = 19,
        [Description("GESTAO_DEVOPS")]
        GESTAO_DEVOPS = 21,

        [Description("PipelinesRestrictions")]
        PipelinesRestrictions = 23,

        [Description("Project Administrators")]
        ProjectAdministrators = 25,

        [Description("Project Managers")]
        ProjectManagers = 27,

        [Description("Project Valid Users")]
        ProjectValidUsers = 29,

        [Description("Readers")]
        Readers = 31,

        [Description("Release Administrators")]
        ReleaseAdministrators = 33,

        [Description("ReposRestrictions")]
        ReposRestrictions = 35,

        [Description("Tech Leaders")]
        TechLeaders = 37,

        [Description("Team")]
        ProjectTeam = 39,

        [Description("Undefined")]
        Undefined = 0,

    }
}
