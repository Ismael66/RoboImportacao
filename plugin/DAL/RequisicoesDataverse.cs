using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Plugin
{
    public static class RequisicoesDataverse
    {
        public static EntityCollection ValidarCpfContact(IOrganizationService service, string cpf)
        {
            var fetchXml =
            $@"<fetch top='1' no-lock='true'>
            <entity name='contact'>
                <attribute name='contactid' />
                <filter>
                <condition attribute='cr6de_cpf' operator='eq' value='{cpf}'/>
                </filter>
            </entity>
            </fetch>";
            return service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public static EntityCollection ValidarCnpjAccount(IOrganizationService service, string cnpj)
        {
            var fetchXml =
            $@"<fetch top='1' no-lock='true'>
            <entity name='account'>
                <attribute name='accountid' />
                <filter>
                <condition attribute='cr6de_cnpj' operator='eq' value='{cnpj}'/>
                </filter>
            </entity>
            </fetch>";
            return service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
    }
}