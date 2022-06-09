using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Organization;

namespace Robot
{
    public static class Util
    {
        static Random Rdn = new Random();
        public static EntityReference RandomToReference(this List<Entity> e)
        {
            if (e.Count <= 0)
            {
                throw new Exception("Maximo invalido.");
            }
            return e[SorteioNumero(e.Count)].ToEntityReference();
        }
        public static EntityReference RandomToReference(this EntityCollection e)
        {
            if (e.Entities.Count <= 0)
            {
                throw new Exception("Maximo invalido.");
            }
            return e.Entities[SorteioNumero(e.Entities.Count)].ToEntityReference();
        }
        public static List<Entity> ToListEntity(this EntityCollection e)
        {
            var listEntity = new List<Entity>();
            if (e.Entities.Count <= 0)
            {
                throw new Exception("Maximo invalido.");
            }
            listEntity.AddRange(e.Entities);
            return listEntity;
        }
        public static List<List<Entity>> SplitList(List<Entity> records, int nSize = 500)
        {
            var list = new List<List<Entity>>();
            for (int i = 0; i < records.Count; i += nSize)
            {
                list.Add(records.GetRange(i, Math.Min(nSize, records.Count - i)));
            }
            return list;
        }
        public static Uri GetOrganizationUrl(this IOrganizationService organizationService)
        {
            var request = new RetrieveCurrentOrganizationRequest();
            var organzationResponse = (RetrieveCurrentOrganizationResponse)organizationService.Execute(request);

            var uriString = organzationResponse.Detail.Endpoints[EndpointType.WebApplication];
            return new Uri(uriString);
        }
        public static int SorteioNumero(int maximo)
        {
            return Rdn.Next(maximo);
        }
    }
}