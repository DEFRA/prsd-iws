namespace EA.Iws.DataAccess.Jobs
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class UpdateReportCache
    {
        private readonly string connectionString;

        public UpdateReportCache(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task Execute()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Reports].[uspUpdateReportCache]";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}