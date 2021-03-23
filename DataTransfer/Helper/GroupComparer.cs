using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class GroupEqualityComparer : IEqualityComparer<GroupModel>
    {
        public bool Equals(GroupModel x, GroupModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.ProjectId == y.ProjectId && x.OriginId == y.OriginId) &&
                    (x.Descriptor != y.Descriptor ||
                     x.DisplayName != y.DisplayName ||
                     x.Domain != y.Domain ||
                     x.MailAddress != y.MailAddress ||
                     x.Origin != y.Origin ||
                     x.PrincipalName != y.PrincipalName ||
                     x.Url != y.Url ||
                     x.IsActive != y.IsActive

                     ));
            }

            return result;
        }

        public int GetHashCode(GroupModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
