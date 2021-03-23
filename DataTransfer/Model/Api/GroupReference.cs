using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public class GroupReference
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Self
        {
            public string href { get; set; }
        }

        public class Memberships
        {
            public string href { get; set; }
        }

        public class MembershipState
        {
            public string href { get; set; }
        }

        public class StorageKey
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public Memberships memberships { get; set; }
            public MembershipState membershipState { get; set; }
            public StorageKey storageKey { get; set; }
        }

        public class Value
        {
            public string subjectKind { get; set; }
            public string description { get; set; }
            public string domain { get; set; }
            public string principalName { get; set; }
            public string mailAddress { get; set; }
            public string origin { get; set; }
            public string originId { get; set; }
            public string displayName { get; set; }
            public Links _links { get; set; }
            public string url { get; set; }
            public string descriptor { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }


    }
}
