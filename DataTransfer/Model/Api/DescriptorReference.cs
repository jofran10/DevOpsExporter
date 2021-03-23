using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Model.Api
{
    public class DescriptorReference
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Self
        {
            public string href { get; set; }
        }

        public class StorageKey
        {
            public string href { get; set; }
        }

        public class Subject
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public StorageKey storageKey { get; set; }
            public Subject subject { get; set; }
        }

        public class Root
        {
            public string value { get; set; }
            public Links _links { get; set; }
        }


    }
}
