namespace EA.Iws.Web.Infrastructure.Logging
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Elmah;

    public class ApiErrorLog : ErrorLog
    {
        private readonly IDictionary config;
        private readonly IIwsClient apiClient;

        public ApiErrorLog(IDictionary config)
        {
            this.config = config;
            this.apiClient = DependencyResolver.Current.GetService<IIwsClient>();

            ApplicationName = (string)(config["applicationName"] ?? string.Empty);
        }

        public override string Log(Error error)
        {
            var errorXml = ErrorXml.EncodeString(error);
            var id = Guid.NewGuid();

            var errorData = new ErrorData(id, ApplicationName, error.HostName, error.Type, error.Source, error.Message,
                error.User,
                error.StatusCode, error.Time.ToUniversalTime(), errorXml);

            Task.Run(() => apiClient.ErrorLog.Create(errorData));

            return id.ToString();
        }

        public override ErrorLogEntry GetError(string id)
        {
            var errorData = Task.Run(() => apiClient.ErrorLog.Get(id)).Result;

            if (errorData == null)
            {
                return null;
            }

            var error = ErrorXml.DecodeString(errorData.ErrorXml);
            return new ErrorLogEntry(this, id, error);
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            var errorList = Task.Run(() => apiClient.ErrorLog.GetList(pageIndex, pageSize)).Result;

            foreach (var errorData in errorList.Errors)
            {
                var error = ErrorXml.DecodeString(errorData.ErrorXml);
                errorEntryList.Add(new ErrorLogEntry(this, errorData.Id.ToString(), error));
            }

            return errorList.TotalRecords;
        }
    }
}