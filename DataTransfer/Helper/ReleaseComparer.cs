using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class ReleaseEqualityComparer : IEqualityComparer<ReleaseModel>
    {
        public bool Equals(ReleaseModel x, ReleaseModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.ProjectId == y.ProjectId && x.DefinitionId == y.DefinitionId) &&
                    (
                     //x.CreatedBy != y.CreatedBy ||
                     //x.CreatedByDescriptor != y.CreatedByDescriptor ||
                     //x.CreatedById != y.CreatedById ||
                     //x.CreatedByImageUrl != y.CreatedByImageUrl ||
                     //x.CreatedByUniqueName != y.CreatedByUniqueName ||
                     //x.CreatedByUrl != y.CreatedByUrl ||
                     //x.CreatedFor != y.CreatedFor ||
                     //x.CreatedForDescriptor != y.CreatedForDescriptor ||
                     //x.CreatedForId != y.CreatedForId ||
                     //x.CreatedForImageUrl != y.CreatedForImageUrl ||
                     //x.CreatedForUniqueName != y.CreatedForUniqueName ||
                     //x.CreatedForUrl != y.CreatedForUrl ||
                     //x.CreatedOn != y.CreatedOn ||
                     //x.DefinitionName != y.DefinitionName ||
                     //x.DefinitionPath != y.DefinitionPath ||
                     //x.DefinitionSelfLink != y.DefinitionSelfLink ||
                     //x.DefinitionSnapshotRevision != y.DefinitionSnapshotRevision ||
                     //x.DefinitionUrl != y.DefinitionUrl ||
                     //x.DefinitionWebLink != y.DefinitionWebLink ||
                     //x.Description != y.Description ||
                     //x.IsActive != y.IsActive ||
                     //x.KeepForever != y.KeepForever ||
                     //x.LogsContainerUrl != y.LogsContainerUrl ||
                     x.ModifiedBy != y.ModifiedBy ||
                     x.ModifiedByDescriptor != y.ModifiedByDescriptor ||
                     x.ModifiedById != y.ModifiedById ||
                     x.ModifiedByImageUrl != y.ModifiedByImageUrl ||
                     x.ModifiedByUniqueName != y.ModifiedByUniqueName ||
                     x.ModifiedByUrl != y.ModifiedByUrl ||
                     x.ModifiedOn != y.ModifiedOn //||
                     //x.Name != y.Name ||
                     //x.ProjectName != y.ProjectName ||
                     //x.Reason != y.Reason ||
                     //x.ReleaseDefinitionRevision != y.ReleaseDefinitionRevision ||
                     //x.ReleaseNameFormat != y.ReleaseNameFormat ||
                     //x.SelfLink != y.SelfLink ||
                     //x.Status != y.Status ||
                     //x.Tags != y.Tags ||
                     //x.TriggeringArtifactAlias != y.TriggeringArtifactAlias ||
                     //x.Url != y.Url ||
                     //x.VariableGroups != y.VariableGroups ||
                     //x.Variables != y.Variables ||
                     //x.WebLink != y.WebLink
                     ));
            }

            return result;
        }

        public int GetHashCode(ReleaseModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
