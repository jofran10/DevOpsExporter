using DataTransfer.Model.Data;
using System.Collections.Generic;

namespace DataTransfer.Helper
{
    public sealed class ProjectEqualityComparer : IEqualityComparer<ProjectModel>
    {
        public bool Equals(ProjectModel x, ProjectModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = (x.ProjectId == y.ProjectId &&
                    (x.Description != y.Description ||
                     x.LastUpdateTime != y.LastUpdateTime ||
                     x.Name != y.Name ||
                     x.Revision != y.Revision ||
                     x.State != y.State ||
                     x.Url != y.Url ||
                     x.Visibility != y.Visibility ||
                     x.IsActive != y.IsActive));
            }

            return result;
        }
               
        public int GetHashCode(ProjectModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
