using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Model.Api
{
    public class ReleaseApprovalReference
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Avatar
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Avatar avatar { get; set; }
        }

        public class Approver
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public bool isContainer { get; set; }
            public string descriptor { get; set; }
        }

        public class ApprovedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Release
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class ReleaseDefinition
        {
            public int id { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public object projectReference { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class ReleaseEnvironment
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class Value
        {
            public int id { get; set; }
            public int revision { get; set; }
            public Approver approver { get; set; }
            public ApprovedBy approvedBy { get; set; }
            public string approvalType { get; set; }
            public DateTime? createdOn { get; set; }
            public DateTime? modifiedOn { get; set; }
            public string status { get; set; }
            public string comments { get; set; }
            public bool isAutomated { get; set; }
            public bool isNotificationOn { get; set; }
            public int trialNumber { get; set; }
            public int attempt { get; set; }
            public int rank { get; set; }
            public Release release { get; set; }
            public ReleaseDefinition releaseDefinition { get; set; }
            public ReleaseEnvironment releaseEnvironment { get; set; }
            public string url { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }



    }
}
