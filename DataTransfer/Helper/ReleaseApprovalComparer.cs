using DataTransfer.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransfer.Helper
{
    public sealed class ReleaseApprovalEqualityComparer : IEqualityComparer<ReleaseApprovalModel>
    {
        public bool Equals(ReleaseApprovalModel x, ReleaseApprovalModel y)
        {
            bool result = false;

            if (x == null)
                result = (y == null);
            else if (y == null)
                result = false;
            else
            {
                result = ((x.ApprovalId == y.ApprovalId && x.ProjectId == y.ProjectId && x.ReleaseId == y.ReleaseId) &&
                    (
                        x.ApprovalType != y.ApprovalType ||
                        x.Approver != y.Approver ||
                        x.ApproverDescriptor != y.ApproverDescriptor ||
                        x.ApproverId != y.ApproverId ||
                        x.ApproverImageUrl != y.ApproverImageUrl ||
                        
                        x.ApproverUniqueName != y.ApproverUniqueName ||
                        x.ApproverUrl != y.ApproverUrl ||
                        x.Attempt != y.Attempt ||
                        x.Comments != y.Comments ||
                        x.CreatedOn != y.CreatedOn ||
                        x.DefinitionId != y.DefinitionId ||
                        x.DefinitionName != y.DefinitionName ||
                        x.DefinitionPath != y.DefinitionPath ||
                        x.DefinitionUrl != y.DefinitionUrl ||
                        x.EnvironmentId != y.EnvironmentId ||
                        x.EnvironmentName != y.EnvironmentName ||
                        x.EnvironmentUrl != y.EnvironmentUrl ||
                        //x.IsActive != y.IsActive ||
                        x.IsAutomated != y.IsAutomated ||
                        x.IsNotificationOn != y.IsNotificationOn ||
                        x.ModifiedOn != y.ModifiedOn ||
                        x.Rank != y.Rank ||
                        x.ReleaseId != y.ReleaseId ||
                        x.ReleaseName != y.ReleaseName ||
                        x.ReleaseUrl != y.ReleaseUrl ||
                        x.Revision != y.Revision ||
                        x.Status != y.Status ||
                        x.TrialNumber != y.TrialNumber ||
                        x.Url != y.Url ||
                        x.ApprovedBy != y.ApprovedBy ||
                        x.ApprovedByDescriptor != y.ApprovedByDescriptor ||
                        x.ApprovedById != y.ApprovedById ||
                        x.ApprovedByImageUrl != y.ApprovedByImageUrl ||
                        x.ApprovedByUniqueName != y.ApprovedByUniqueName ||
                        x.ApprovedByUrl != y.ApprovedByUrl





                     ));
            }

            return result;
        }

        public int GetHashCode(ReleaseApprovalModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
