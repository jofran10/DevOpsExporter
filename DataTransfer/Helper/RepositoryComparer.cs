using DataTransfer.Model.Data;
using System.Collections.Generic;

namespace DataTransfer.Helper
{
    public sealed class RepositoryEqualityComparer : IEqualityComparer<RepositoryModel>
    {
        public bool Equals(RepositoryModel x, RepositoryModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = (x.RepositoryId == y.RepositoryId &&
                    (x.DefaultBranch != y.DefaultBranch ||
                     x.Name != y.Name ||
                     x.RemoteUrl != y.RemoteUrl ||
                     x.Size != y.Size ||
                     x.SshUrl != y.SshUrl ||
                     x.Url != y.Url) ||
                     x.WebUrl != y.WebUrl ||
                     x.IsActive != y.IsActive ||
                     x.LastUpdateTime != y.LastUpdateTime
                     );
            }

            return result;
        }

        public int GetHashCode(RepositoryModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
