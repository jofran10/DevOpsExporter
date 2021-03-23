using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class PipelineEqualityComparer : IEqualityComparer<PipelineModel>
    {
        public bool Equals(PipelineModel x, PipelineModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.ProjectId == y.ProjectId && x.PipelineId == y.PipelineId) &&
                    (x.Revision != y.Revision ||
                     x.Name != y.Name ||
                     x.Folder != y.Folder ||
                     x.SelfLink != y.SelfLink ||
                     x.Url != y.Url ||
                     x.WebLink != y.WebLink ||
                     x.IsActive != y.IsActive
                     
                     ));
            }

            return result;
        }

        public int GetHashCode(PipelineModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
