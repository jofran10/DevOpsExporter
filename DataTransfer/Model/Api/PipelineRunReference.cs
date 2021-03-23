using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Model.Api
{
    public class PipelineRunReference
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Self
        {
            public string href { get; set; }
        }

        public class Web
        {
            public string href { get; set; }
        }

        public class PipelineWeb
        {
            public string href { get; set; }
        }

        public class Pipeline
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public Web web { get; set; }

            [JsonProperty("pipeline.web")]
            public PipelineWeb PipelineWeb { get; set; }
            public Pipeline pipeline { get; set; }
        }

        public class PipelineDetails
        {
            public string url { get; set; }
            public int id { get; set; }
            public int revision { get; set; }
            public string name { get; set; }
            public string folder { get; set; }
        }

        public class Value
        {
            public Links _links { get; set; }
            public PipelineDetails pipeline { get; set; }
            public string state { get; set; }
            public string result { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime? finishedDate { get; set; }
            public string url { get; set; }
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }


    }
}
