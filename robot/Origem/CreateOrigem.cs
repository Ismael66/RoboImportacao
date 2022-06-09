using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;

namespace Robot
{
    public static class CreateOrigem
    {
        public static void CreateAll()
        {
            var serviceOrigem = Connection.Obter();
            var dalOrigem = new GetData(serviceOrigem);
            var requests = new Requests(serviceOrigem);
            var priceLevelRecords = dalOrigem.GetPriceLevel();
            var grupoUnidadesRecord = dalOrigem.GetGrupoUnidades();
            var idGrupoUnidades = grupoUnidadesRecord.GetAttributeValue<Guid>("uomscheduleid");
            var unidadesRecords = dalOrigem.GetUnidade(idGrupoUnidades.ToString());
            var productRecords = dalOrigem.GetProduct();
            var contactRecords = requests.MassCreate(CreateContact().Result);
            var accountRecords = requests.MassCreate(CreateAccount(contactRecords).Result);
            var orderRecords = requests.MassCreate(CreateOrder(accountRecords, priceLevelRecords).Result);
            var orderProductRecords = requests.MassCreate(CreateOrderProduct(orderRecords, productRecords, unidadesRecords));
        }
        public static void DeleteAll(IOrganizationService service)
        {
            var requests = new Requests(service);
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("salesorder"));
            requests.MassDelete(requests.PegaEntidadesUltimosSeteDias("salesorderdetail"));
            var listContas = requests.PegaEntidadesUltimosSeteDias("account").ToListEntity();
            requests.MassDelete(listContas);
            var listContatos = requests.PegaEntidadesUltimosSeteDias("contact").ToListEntity();
            requests.MassDelete(listContatos);
        }
        public static async ValueTask<List<Entity>> CreateOrder(List<Entity> accountRecords, EntityCollection priceLevelRecords)
        {
            var results = await FakeData.RequisicaoMockaroo<Product>("order");
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("salesorder");
                entidade["name"] = item.Nome;
                entidade["pricelevelid"] = priceLevelRecords.RandomToReference();
                entidade["customerid"] = accountRecords.RandomToReference();
                collection.Add(entidade);
            }
            return collection;
        }
        public static List<Entity> CreateOrderProduct(List<Entity> orderRecords, EntityCollection productRecords, Entity unidadeRecords)
        {
            var collection = new List<Entity>();
            for (int i = 0; i < 5; i++)
            {
                var entidade = new Entity("salesorderdetail");
                entidade["salesorderid"] = orderRecords.RandomToReference();
                entidade["productid"] = productRecords.RandomToReference();
                entidade["uomid"] = new EntityReference("uom", new Guid("600c5fe1-65c3-ec11-a7b5-002248d30524"));
                entidade["quantity"] = 1.00000m;
                collection.Add(entidade);
            }
            return collection;
        }
        public static async ValueTask<List<Entity>> CreateAccount(List<Entity> contactRecords)
        {
            var results = await FakeData.RequisicaoMockaroo<Person>("person");
            var dadosCep = await FakeData.RequisicaoViaCep();
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("account");
                entidade["name"] = $"{item.Nome} {item.Sobrenome}";
                entidade["primarycontactid"] = contactRecords.RandomToReference();
                entidade["telephone1"] = item.Telefone;
                entidade["cr6de_cnpj"] = FakeData.Cnpj();
                entidade["address1_postalcode"] = dadosCep.Cep;
                entidade["address1_line1"] = dadosCep.Rua;
                entidade["address1_line3"] = dadosCep.Bairro;
                entidade["address1_city"] = dadosCep.Cidade;
                entidade["address1_stateorprovince"] = dadosCep.Estado;
                collection.Add(entidade);
            }
            return collection;
        }
        public static async ValueTask<List<Entity>> CreateContact()
        {
            var results = await FakeData.RequisicaoMockaroo<Person>("person");
            var dadosCep = await FakeData.RequisicaoViaCep();
            var collection = new List<Entity>();
            foreach (var item in results)
            {
                var entidade = new Entity("contact");
                entidade["firstname"] = item.Nome;
                entidade["lastname"] = item.Sobrenome;
                entidade["cr6de_cpf"] = FakeData.Cpf();
                entidade["emailaddress1"] = item.Email;
                entidade["telephone1"] = item.Telefone;
                entidade["address1_postalcode"] = dadosCep.Cep;
                entidade["address1_line1"] = dadosCep.Rua;
                entidade["address1_line3"] = dadosCep.Bairro;
                entidade["address1_city"] = dadosCep.Cidade;
                entidade["address1_stateorprovince"] = dadosCep.Estado;
                collection.Add(entidade);
            }
            return collection;
        }
    }
}
