using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
namespace Robot
{
    public static class PrepareOrigem
    {
        public static void CreatePrepare()
        {
            var serviceOrigem = Connection.Obter();
            var requestsOrigem = new Requests(serviceOrigem);
            var dalOrigem = new GetData(serviceOrigem);
            var grupoUnidadesRecord = requestsOrigem.Create(CreateGrupoUnidades().Result);
            var priceLevelRecords = requestsOrigem.MassCreate(CreatePriceLevel(dalOrigem).Result);
            var unidadesRecords = requestsOrigem.MassCreate(CreateUnidades(grupoUnidadesRecord).Result);
            var productRecords = requestsOrigem.MassCreate(CreateProduct(unidadesRecords, grupoUnidadesRecord).Result);
        }
        public static async ValueTask<Entity> CreateGrupoUnidades()
        {
            var results = await FakeData.RequisicaoMockaroo<Product>("product");
            var entidade = new Entity("uomschedule");
            entidade["name"] = results[0].Nome;
            entidade["baseuomname"] = results[0].Nome + " Base";
            return entidade;
        }
        public static async ValueTask<List<Entity>> CreatePriceLevel(GetData dalOrigem)
        {
            var results = await FakeData.RequisicaoMockaroo<Product>("product");
            var moeda = dalOrigem.GetMoeda();
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("pricelevel");
                entidade["name"] = item.Nome;
                entidade["transactioncurrencyid"] = moeda.ToEntityReference();
                collection.Add(entidade);
            }
            return collection;
        }
        public static async ValueTask<List<Entity>> CreateUnidades(string grupoUnidadesRecord)
        {
            var results = await FakeData.RequisicaoMockaroo<Product>("product");
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("uom");
                entidade["name"] = item.Nome;
                entidade["uomscheduleid"] = new EntityReference("uomschedule", new Guid(grupoUnidadesRecord));
                entidade["quantity"] = 1.00000m;
                collection.Add(entidade);
            }
            return collection;
        }
        public static async ValueTask<List<Entity>> CreateProduct(List<Entity> unidadesRecords, string grupoUnidadesRecord)
        {
            var results = await FakeData.RequisicaoMockaroo<Product>("product");
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("product");
                entidade["name"] = item.Nome;
                entidade["productnumber"] = item.Id;
                entidade["quantitydecimal"] = 1;
                entidade["defaultuomscheduleid"] = new EntityReference("uomschedule", new Guid(grupoUnidadesRecord));
                entidade["defaultuomid"] = unidadesRecords.RandomToReference();
                collection.Add(entidade);
            }
            return collection;
        }
    }
}