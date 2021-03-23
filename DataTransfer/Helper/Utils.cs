using DataTransfer.Context;
using DataTransfer.Enums;
using DataTransfer.Extensions.Enums;
using DataTransfer.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Helper
{
    public class Utils
    {
        public bool AlreadyBeenExecuted(Methods method)
        {
            DbDevOpsDashContext ctx = new DbDevOpsDashContext();

            var _lastExecution = ctx.LoadHistory
                    .Where(y => y.IsSuccessful && y.Method == (int)method).OrderByDescending(x => x.Id).FirstOrDefault();

            if (_lastExecution == null)
                return false;

            if (_lastExecution.LoadDate < DateTime.Today)
                return false;

            //carga já executada
            return true;
        }

        public async Task SaveLoadHistory(Methods method, int result, bool isSuccessful, string message, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var _reg = LoadHistoryHelper(method, result, isSuccessful, message, fromDate, toDate);

            using (DbDevOpsDashContext ctx = new DbDevOpsDashContext())
            {
                ctx.LoadHistory.Add(_reg);
                await ctx.SaveChangesAsync();
            };
        }

        public LoadHistoryModel LoadHistoryHelper(Methods method, int result, bool isSuccessful, string message, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string msg;
            if (isSuccessful)
            {
                if (string.IsNullOrEmpty(message))
                {
                    if (result > 0)
                        msg = $"{method.GetDescription()}, {result} lines updated.";
                    else
                        msg = $"{method.GetDescription()} is up to date.";
                }
                else
                {
                    msg = $"{method.GetDescription()} {message}";
                }

            }
            else
            {
                msg = $"Error updating {method.GetDescription()}: [{message}].";
            }

            return new LoadHistoryModel()
            {
                ExecutionDateTime = DateTime.Now,
                IsSuccessful = isSuccessful,
                Method = (int)method,
                Message = msg,
                LoadDate = DateTime.Now,
                FromDate = fromDate,
                ToDate = toDate

            };
        }

        public Dictionary<string, string> GetRestorePoint(Methods method, DateTime loadDate)
        {
            DbDevOpsDashContext ctx = new DbDevOpsDashContext();

            var restorePoint = ctx.LoadRestorePoint
                    .AsNoTracking()
                    .Where(rp => rp.Method == (int)method && rp.LoadDate == loadDate)
                    .FirstOrDefault();

            var dict = new Dictionary<string, string>();

            if (restorePoint == null)
                return dict;

            string[] keys = restorePoint.LastId.Split(',');

            for (var i = 0; i < keys.Length; i++)
            {
                string[] keyx = keys[i].Split('=');
                dict.Add(keyx[0], keyx[1]);
            }

            return dict;
        }

        public DateTime GetLastToDate(Methods method)
        {
            DateTime? _lastExecution = null;
            DbDevOpsDashContext ctx = new DbDevOpsDashContext();

            _lastExecution = ctx.LoadHistory.Where(h => h.Method == (int)method).Max(h => h.ToDate);

            if (_lastExecution == null)
                return DateTime.MinValue;

            // carga já executada
            return (DateTime)_lastExecution;

        }

        public int GetOurGroupCode(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                return (int)Groups.Undefined;

            if (displayName.Contains(Groups.ApplicationOwners.GetDescription()))
                return (int)Groups.ApplicationOwners;

            if (displayName.Contains(Groups.BoardsRestrictions.GetDescription()))
                return (int)Groups.BoardsRestrictions;

            if (displayName.Contains(Groups.BuildAdministrators.GetDescription()))
                return (int)Groups.BuildAdministrators;

            if (displayName.Contains(Groups.BussinessAnalysts.GetDescription()))
                return (int)Groups.BussinessAnalysts;

            if (displayName.Contains(Groups.Contributors.GetDescription()))
                return (int)Groups.Contributors;

            if (displayName.Contains(Groups.DeploymentGroupAdministrators.GetDescription()))
                return (int)Groups.DeploymentGroupAdministrators;

            if (displayName.Contains(Groups.Developers.GetDescription()))
                return (int)Groups.Developers;

            if (displayName.Contains(Groups.EndpointAdministrators.GetDescription()))
                return (int)Groups.EndpointAdministrators;

            if (displayName.Contains(Groups.EndpointCreators.GetDescription()))
                return (int)Groups.EndpointCreators;

            if (displayName.Contains(Groups.GESTAO_DEVOPS.GetDescription()))
                return (int)Groups.GESTAO_DEVOPS;

            if (displayName.Contains(Groups.PipelinesRestrictions.GetDescription()))
                return (int)Groups.PipelinesRestrictions;

            if (displayName.Contains(Groups.ProjectAdministrators.GetDescription()))
                return (int)Groups.ProjectAdministrators;

            if (displayName.Contains(Groups.ProjectManagers.GetDescription()))
                return (int)Groups.ProjectManagers;

            if (displayName.Contains(Groups.ProjectValidUsers.GetDescription()))
                return (int)Groups.ProjectValidUsers;

            if (displayName.Contains(Groups.Readers.GetDescription()))
                return (int)Groups.Readers;

            if (displayName.Contains(Groups.ReleaseAdministrators.GetDescription()))
                return (int)Groups.ReleaseAdministrators;

            if (displayName.Contains(Groups.ReposRestrictions.GetDescription()))
                return (int)Groups.ReposRestrictions;

            if (displayName.Contains(Groups.TechLeaders.GetDescription()))
                return (int)Groups.TechLeaders;

            // project team and others with 'Team'
            if (displayName.Contains(Groups.ProjectTeam.GetDescription()))
                return (int)Groups.ProjectTeam;

            return 0;

        }


    }
}
