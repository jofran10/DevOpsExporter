using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class PipelineRunEqualityComparer : IEqualityComparer<PipelineRunModel>
    {
        public bool Equals(PipelineRunModel x, PipelineRunModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.PipelineId == y.PipelineId && x.PipelineRunId == y.PipelineRunId) &&
                    (x.FinishedDate != y.FinishedDate ||
                     x.Name != y.Name ||
                     x.Result != y.Result ||
                     x.SelfLink != y.SelfLink ||
                     x.State != y.State ||
                     x.Url != y.Url ||
                     x.WebLink != y.WebLink ||
                     x.CreatedDate != y.CreatedDate
                     
                     ));
            }

            return result;
        }

        public int GetHashCode(PipelineRunModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
