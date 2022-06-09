using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace Robot
{
    public static class PasteData
    {
        public static void PasteAll()
        {
            var serviceOrigem = Connection.Obter();
            var dalOrigem = new GetData(serviceOrigem);
            var servicePaste = Connection.Obter("DevPaste");
            var dalPaste = new GetData(servicePaste);
            var requestsOrigem = new Requests(serviceOrigem);
            var requestsPaste = new Requests(servicePaste);
            var listContatos = dalOrigem.GetContact().ToListEntity();
            var contatos = requestsPaste.MassCreate(listContatos, true);
            requestsOrigem.AtualizaCrmOrigem(contatos);
            var listContas = dalOrigem.GetAccount().ToListEntity();
            var contas = requestsPaste.MassCreate(listContas, true);
            requestsOrigem.AtualizaCrmOrigem(contas);
            var ordens = requestsPaste.MassCreate(CreateOrder(dalOrigem, dalPaste));
            requestsOrigem.AtualizaCrmOrigem(ordens);
            var produtoOrdem = requestsPaste.MassCreate(CreateOrderProduct(dalOrigem, dalPaste));
            requestsOrigem.AtualizaCrmOrigem(produtoOrdem);
        }
        public static void DeleteAll(IOrganizationService service)
        {
            var requests = new Requests(service);
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("salesorder"));
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("salesorderdetail"));
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("account"));
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("contact"));
        }
        public static List<Entity> CreateOrder(GetData dalOrigem, GetData dalPaste)
        {
            var data = dalOrigem.GetOrder();
            var priceLevelRecords = dalPaste.GetPriceLevel();
            var collection = new List<Entity>();
            foreach (var item in data.Entities)
            {
                item["pricelevelid"] = priceLevelRecords.RandomToReference();
                collection.Add(item);
            }
            return collection;
        }
        public static List<Entity> CreateOrderProduct(GetData dalOrigem, GetData dalPaste)
        {
            var data = dalOrigem.GetOrderProduct();
            var productRecords = dalPaste.GetProduct();
            var grupoUnidadesRecord = dalOrigem.GetGrupoUnidades();
            var idGrupoUnidades = grupoUnidadesRecord.GetAttributeValue<Guid>("uomscheduleid");
            var unidadesRecords = dalOrigem.GetUnidade(idGrupoUnidades.ToString());
            var collection = new List<Entity>();
            foreach (var item in data.Entities)
            {
                item["productid"] = productRecords.RandomToReference();
                item["uomid"] = unidadesRecords.ToEntityReference();
                item["quantity"] = 1.00000m;
                collection.Add(item);
            }
            return collection;
        }

    }
}
