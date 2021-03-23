using System;
using System.Collections.Generic;

namespace DataTransfer.Model.Api
{
    public class CommitReference
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Author
        {
            public string name { get; set; }
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public class Committer
        {
            public string name { get; set; }
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public class ChangeCounts
        {
            public int Add { get; set; }
            public int Edit { get; set; }
            public int Delete { get; set; }
        }

        public class Value
        {
            public string commitId { get; set; }
            public Author author { get; set; }
            public Committer committer { get; set; }
            public string comment { get; set; }
            public ChangeCounts changeCounts { get; set; }
            public string url { get; set; }
            public string remoteUrl { get; set; }
            public bool? commentTruncated { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }



    }
}
