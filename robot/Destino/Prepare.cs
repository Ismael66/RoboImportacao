using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.Xrm.Sdk;
namespace Robot
{
    public static class Prepare
    {
        public static void PastePrepare()
        {
            var serviceOrigem = Connection.Obter();
            var dalOrigem = new GetData(serviceOrigem);
            var servicePaste = Connection.Obter("DevPaste");
            var dalPaste = new GetData(servicePaste);
            var requestsOrigem = new Requests(serviceOrigem);
            var requestsPaste = new Requests(servicePaste);
            var grupoUnidades = requestsPaste.Create(dalOrigem.GetGrupoUnidades());
            var levelPreco = requestsPaste.MassCreate(CreatePriceLevel(dalOrigem, dalPaste), true);
            requestsOrigem.AtualizaCrmOrigem(levelPreco);
            var unidades = requestsPaste.Create(dalOrigem.GetUnidade(grupoUnidades));
            var listProdutos = dalOrigem.GetProduct().ToListEntity();
            var produtos = requestsPaste.MassCreate(listProdutos, true);
            requestsOrigem.AtualizaCrmOrigem(produtos);
        }

        static List<Entity> CreatePriceLevel(GetData dalOrigem, GetData dalPaste)
        {
            var data = dalOrigem.GetPriceLevel();
            var moeda = dalPaste.GetMoeda();
            var collection = new List<Entity>();
            foreach (var item in data.Entities)
            {
                item["transactioncurrencyid"] = moeda.ToEntityReference();
                collection.Add(item);
            }
            return collection;
        }
    }
}