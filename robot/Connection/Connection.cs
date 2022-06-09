using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;

namespace Robot
{
    public static class Connection
    {
        static IOrganizationService[] _service = new IOrganizationService[2];
        public static IOrganizationService Obter(string connectionStringName = "Dev")
        {
            var ambiente = connectionStringName == "Dev" ? 0 : 1;
            if (_service[ambiente] != null)
                return _service[ambiente];
            else
            {
                var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
                if (connectionString != null)
                {
                    var crmServiceClient = new CrmServiceClient(connectionString.ConnectionString);
                    if (!crmServiceClient.IsReady)
                    {
                        throw new Exception(crmServiceClient.LastCrmError);
                    }
                    else
                    {
                        _service[ambiente] = crmServiceClient.OrganizationWebProxyClient;
                        return _service[ambiente];
                    }
                }
                throw new Exception("String de conexão não encontrada.");
            }
        }
    }
}