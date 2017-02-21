namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Xml;
    using Client.Entities;
    using DataAccess;
    using Elmah;

    [RoutePrefix("api/ErrorLog")]
    [AllowAnonymous]
    public class ErrorLogController : ApiController
    {
        private readonly IwsContext context;

        public ErrorLogController(IwsContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(ErrorData errorData)
        {
            await context.Database.ExecuteSqlCommandAsync("ELMAH_LogError @ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @AllXml, @StatusCode, @TimeUtc",
                new SqlParameter("@ErrorId", errorData.Id),
                new SqlParameter("@Application", errorData.ApplicationName),
                new SqlParameter("@Host", errorData.HostName),
                new SqlParameter("@Type", errorData.Type),
                new SqlParameter("@Source", errorData.Source),
                new SqlParameter("@Message", errorData.Message),
                new SqlParameter("@User", errorData.User),
                new SqlParameter("@AllXml", errorData.ErrorXml),
                new SqlParameter("@StatusCode", errorData.StatusCode),
                new SqlParameter("@TimeUtc", errorData.Date.ToUniversalTime()));

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var xml = await context.Database.SqlQuery<string>("ELMAH_GetErrorXml @Application, @ErrorId",
                new SqlParameter("@ErrorId", id),
                new SqlParameter("@Application", string.Empty)).SingleOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(xml))
            {
                return Ok();
            }

            var errorData = CreateErrorDataFromXml(new Guid(id), xml);

            return Ok(errorData);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetList([FromUri] int pageIndex, [FromUri] int pageSize)
        {
            var totalCountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            int totalCount;
            ICollection<ErrorData> errorData;

            using (var command = context.Database.Connection.CreateCommand())
            {
                await command.Connection.OpenAsync();

                command.CommandText = "ELMAH_GetErrorsXml";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(new[]
                {
                    new SqlParameter("@Application", string.Empty),
                    new SqlParameter("@PageIndex", pageIndex),
                    new SqlParameter("@PageSize", pageSize),
                    totalCountParameter
                });

                var xml = ReadSingleXmlStringResult(await command.ExecuteReaderAsync());
                errorData = ErrorsXmlToList(xml);

                totalCount = (int)command.Parameters["@TotalCount"].Value;

                command.Connection.Close();
            }

            return Ok(new PagedErrorDataList
            {
                Errors = errorData.ToArray(),
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalRecords = totalCount
            });
        }

        private static string ReadSingleXmlStringResult(IDataReader reader)
        {
            using (reader)
            {
                if (!reader.Read())
                {
                    return null;
                }

                var sb = new StringBuilder(/* capacity */ 2033);
                do
                {
                    sb.Append(reader.GetString(0));
                }
                while (reader.Read());
                return sb.ToString();
            }
        }

        private static ICollection<ErrorData> ErrorsXmlToList(string xml)
        {
            var errorEntryList = new List<ErrorData>();

            if (string.IsNullOrWhiteSpace(xml))
            {
                return errorEntryList;
            }

            var settings = new XmlReaderSettings
            {
                CheckCharacters = false,
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using (var reader = XmlReader.Create(new StringReader(xml), settings))
            {
                while (reader.IsStartElement("error"))
                {
                    var id = reader.GetAttribute("errorId");
                    var errorXml = reader.ReadOuterXml();

                    errorEntryList.Add(CreateErrorDataFromXml(new Guid(id), errorXml));
                }

                return errorEntryList;
            }
        }

        private static ErrorData CreateErrorDataFromXml(Guid id, string xml)
        {
            var error = ErrorXml.DecodeString(xml);
            return new ErrorData(id, error.ApplicationName, error.HostName,
                error.Type, error.Source, error.Message, error.User, error.StatusCode, error.Time, xml);
        }
    }
}