using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class ReleaseDefinitionEqualityComparer : IEqualityComparer<ReleaseDefinitionModel>
    {
        public bool Equals(ReleaseDefinitionModel x, ReleaseDefinitionModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.ProjectId == y.ProjectId && x.DefinitionId == y.DefinitionId) &&
                    (x.Revision != y.Revision ||
                     x.Name != y.Name ||
                     x.Path != y.Path ||
                     x.SelfLink != y.SelfLink ||
                     x.Url != y.Url ||
                     x.WebLink != y.WebLink ||
                     x.IsActive != y.IsActive ||
                     x.CreatedBy != y.CreatedBy ||
                     x.CreatedByDescriptor != y.CreatedByDescriptor ||
                     x.CreatedById != y.CreatedById ||
                     x.CreatedByImageUrl != y.CreatedByImageUrl ||
                     x.CreatedByUniqueName != y.CreatedByUniqueName ||
                     x.CreatedByUrl != y.CreatedByUrl ||
                     x.CreatedOn != y.CreatedOn ||
                     x.Description != y.Description ||
                     x.IsDeleted != y.IsDeleted ||
                     x.ModifiedBy != y.ModifiedBy ||
                     x.ModifiedByDescriptor != y.ModifiedByDescriptor ||
                     x.ModifiedById != y.ModifiedById ||
                     x.ModifiedByImageUrl != y.ModifiedByImageUrl ||
                     x.ModifiedByUniqueName != y.ModifiedByUniqueName ||
                     x.ModifiedByUrl != y.ModifiedByUrl ||
                     x.ModifiedOn != y.ModifiedOn ||
                     x.ReleaseNameFormat != y.ReleaseNameFormat||
                     x.Source != y.Source



                     ));
            }

            return result;
        }

        public int GetHashCode(ReleaseDefinitionModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
