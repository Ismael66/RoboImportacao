using System;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Robot
{
    public class Requests
    {
        private IOrganizationService Service { get; set; }
        private Uri Url { get; set; }
        public Requests(IOrganizationService service)
        {
            Url = service.GetOrganizationUrl();
            Service = service;
        }
        public List<Entity> MassCreate(List<Entity> records, bool validador = false)
        {
            var requestWithResults = CreateBulkExecuteRequest();
            var lstlstEntity = Util.SplitList(records);
            var listResponse = new List<Entity>();
            foreach (var lstEntity in lstlstEntity)
            {
                requestWithResults.Requests.Clear();
                foreach (var entity in lstEntity)
                {
                    var upsertRequest = new UpsertRequest { Target = entity };
                    requestWithResults.Requests.Add(upsertRequest);
                }

                var responseWithResults = Service.Execute(requestWithResults) as ExecuteMultipleResponse;
                listResponse.AddRange(ReadResponse(responseWithResults, validador));
            }
            return listResponse;
        }
        private static ExecuteMultipleRequest CreateBulkExecuteRequest()
        {
            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
            return multipleRequest;
        }
        public void AtualizaCrmOrigem(List<Entity> colecaoEntidades)
        {
            ExecuteMultipleRequest request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                { ContinueOnError = false, ReturnResponses = true }
            };

            foreach (var entidade in colecaoEntidades)
            {
                UpdateRequest updateRequest = new UpdateRequest { Target = entidade };
                request.Requests.Add(updateRequest);
            }
            ExecuteMultipleResponse resposta = (ExecuteMultipleResponse)Service.Execute(request);
            ReadResponse(resposta);
        }
        public string Create(Entity entity)
        {
            return Service.Create(entity).ToString();
        }
        public void MassDelete(List<Entity> lista)
        {
            var requestWithResults = CreateBulkExecuteRequest();
            var lstlstEntity = Util.SplitList(lista);
            foreach (var lstEntity in lstlstEntity)
            {
                requestWithResults.Requests.Clear();
                foreach (var entity in lstEntity)
                {
                    var upsertRequest = new UpsertRequest { Target = entity };
                    requestWithResults.Requests.Add(upsertRequest);
                }
                var responseWithResults = Service.Execute(requestWithResults) as ExecuteMultipleResponse;
                foreach (var responseItem in responseWithResults.Responses)
                {
                    if (responseItem.Fault != null)
                        Console.WriteLine($"Req n°: {responseItem.RequestIndex} => Error: {responseItem.Fault}");
                }
            }
        }
        public void MassDelete(EntityCollection lista)
        {
            var requestWithResults = CreateBulkExecuteRequest();
            requestWithResults.Requests.Clear();
            foreach (var entity in lista.Entities)
            {
                var upsertRequest = new UpsertRequest { Target = entity };
                requestWithResults.Requests.Add(upsertRequest);
            }
            var responseWithResults = Service.Execute(requestWithResults) as ExecuteMultipleResponse;
            foreach (var responseItem in responseWithResults.Responses)
            {
                if (responseItem.Fault != null)
                    Console.WriteLine($"Req n°: {responseItem.RequestIndex} => Error: {responseItem.Fault}");
            }

        }
        public List<Entity> ReadResponse(ExecuteMultipleResponse response, bool validador = false)
        {
            int cont = 0;
            var recordsEntity = new List<Entity>();
            foreach (var responseItem in response.Responses)
            {
                if (responseItem.Fault != null)
                {
                    Console.WriteLine($"Req n°: {responseItem.RequestIndex} => Error: {responseItem.Fault}");
                }
                else if (responseItem.Response is UpsertResponse upsertResponse)
                {
                    var target = upsertResponse.Target;
                    var entidade = new Entity(target.LogicalName, target.Id);
                    if (validador)
                    {
                        entidade["cr6de_validacao"] = true;
                    }
                    recordsEntity.Add(entidade);
                    Console.WriteLine($"Req n°: {responseItem.RequestIndex} => {Url}main.aspx?appid=&pagetype=entityrecord&etn={target.LogicalName}&id={target.Id}");
                }
                else if (responseItem.Response is UpdateResponse)
                {
                    cont++;
                }
            }
            if (cont > 0)
            {
                Console.WriteLine($"{cont} entidades atualizadas no ambiente: {Url}");
            }
            return recordsEntity;
        }
        public EntityCollection PegaEntidadesUltimosSeteDias(string entidade)
        {
            var fetchXml = $@"
                <fetch>
                <entity name='{entidade}'>
                    <attribute name='{entidade}id' />

                </entity>
                </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
    }
}