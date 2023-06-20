namespace EA.Iws.Web.Infrastructure
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    public class DefraCompaniesHouseApi
    {
        public static string GetOrganisationNameByRegNum(string registrationNumber)
        {
            string companyHouseApiUrl = ConfigurationManager.AppSettings["Iws.DefraCompanyHouseApiUrl"];
            var requestUrl = companyHouseApiUrl + registrationNumber;
            string filePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + ConfigurationManager.AppSettings["Iws.DefraCertLocation"];

            X509Certificate2 certificate = new X509Certificate2(filePath, ConfigurationManager.AppSettings["Iws.DefraCertPassword"]);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.ClientCertificates.Add(certificate);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Create a StreamReader object to read the response stream
            StreamReader reader = new StreamReader(response.GetResponseStream());

            // Read the content of the response into a string variable
            string content = reader.ReadToEnd();

            reader.Close();
            response.Close();

            JObject jsonObject = JObject.Parse(content);
            var orgName = (string)jsonObject["Organisation"]["Name"];
            return orgName;
        }
    }
}