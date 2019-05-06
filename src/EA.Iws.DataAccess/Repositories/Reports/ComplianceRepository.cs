namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.Reports.Compliance;
    using Domain.Reports;
    using Domain.Security;

    internal class ComplianceRepository : IComplianceRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public ComplianceRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }
        public async Task<IEnumerable<ComplianceData>> GetComplianceReport(ComplianceReportDates dateType,
           DateTime from,
           DateTime to,
           ComplianceTextFields? textFieldType,
           TextFieldOperator? operatorType,
           string textSearch,
           UKCompetentAuthority competentAuthority)
        {
            var reportlist = await context.Database.SqlQuery<ComplianceDataReport>(@"EXEC [Reports].[Compliance] 
                @DateType,
                @CompetentAuthority,
                @From,
                @To",
                new SqlParameter("@DateType", dateType.ToString()),
                new SqlParameter("@CompetentAuthority", (int)competentAuthority),
                new SqlParameter("@From", from),
                new SqlParameter("@To", to)).ToArrayAsync();

            if (!textFieldType.HasValue ||
                !operatorType.HasValue ||
                string.IsNullOrEmpty(textSearch))
            {
                return reportlist;
            }
            else if (reportlist.Length > 0)
            {
                var predicate = CreatePredicate(textFieldType.Value.ToString(), textSearch, operatorType);
                var query = reportlist.AsQueryable<ComplianceDataReport>();

                return query.Where(predicate);
            }
            else
            {
                return reportlist;
            }            
        }

        public Expression<Func<ComplianceDataReport, bool>> CreatePredicate(string columnName, object searchValue, TextFieldOperator? operatorType)
        {
            var columnType = typeof(ComplianceDataReport);
            var x = Expression.Parameter(columnType, "x");
            var column = columnType.GetProperties().FirstOrDefault(p => p.Name == columnName);
            var property = Expression.Property(x, columnName);
            MethodInfo method = typeof(String).GetMethod("Contains", new[] { typeof(String) });
            var toLower = Expression.Call(property, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
            Expression filter;

            switch (operatorType)
            {
                case TextFieldOperator.Contains:
                    filter = Expression.Call(toLower, method, Expression.Constant(searchValue.ToString().ToLower()));
                    break;
                case TextFieldOperator.DoesNotContain:
                    filter = Expression.Not(Expression.Call(toLower, method, Expression.Constant(searchValue.ToString().ToLower())));
                    break;
                default:
                    throw new ArgumentException(string.Format("Invalid operator type supplied: {0}", operatorType), "operatorType type");
            }
            return Expression.Lambda<Func<ComplianceDataReport, bool>>(filter, x);
        }      
    }  
}
