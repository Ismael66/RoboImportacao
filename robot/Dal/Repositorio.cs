using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Robot
{
    public class GetData
    {
        private IOrganizationService Service { get; set; }
        public GetData(IOrganizationService service)
        {
            Service = service;
        }
        public EntityCollection GetContact()
        {
            var fetchXml = $@"
                <fetch no-lock='true'>
                <entity name='contact'>
                    <attribute name='firstname' />
                    <attribute name='lastname' />
                    <attribute name='emailaddress1' />
                    <attribute name='telephone1' />
                    <attribute name='address1_postalcode' />
                    <attribute name='address1_line1' />
                    <attribute name='address1_line3' />
                    <attribute name='address1_city' />
                    <attribute name='address1_stateorprovince' />
                    <filter>
                    <condition attribute='cr6de_validacao' operator='ne' value='1'/>
                    </filter>
                </entity>
                </fetch>";

            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public EntityCollection GetAccount()
        {
            var fetchXml = $@"
                <fetch no-lock='true'>
                <entity name='account'>
                    <attribute name='name' />
                    <attribute name='telephone1' />
                    <attribute name='address1_postalcode' />
                    <attribute name='address1_line1' />
                    <attribute name='address1_line3' />
                    <attribute name='address1_city' />
                    <attribute name='address1_stateorprovince' />
                    <attribute name='primarycontactid' />
                    <filter>
                    <condition attribute='cr6de_validacao' operator='ne' value='1'/>
                    </filter>
                </entity>
                </fetch>";

            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public Entity GetGrupoUnidades()
        {
            var fetchXml = $@"
                <fetch no-lock='true'>
                <entity name='uomschedule'>
                    <attribute name='name' />
                    <attribute name='baseuomname' />
                    <attribute name='uomscheduleid' />
                    <filter>
                    <condition attribute='name' operator='not-like' value='%Unidade%'/>
                    </filter>
                </entity>
                </fetch>";

            return Service.RetrieveMultiple(new FetchExpression(fetchXml)).Entities.FirstOrDefault();
        }
        public EntityCollection GetPriceLevel()
        {
            var fetchXml = $@"
            <fetch no-lock='true'>
            <entity name='pricelevel'>
                <attribute name='name' />
                <attribute name='transactioncurrencyid' />
                    <filter>
                    <condition attribute='name' operator='not-like' value='%Lista%'/>
                    </filter>
            </entity>
            </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public EntityCollection GetOrder()
        {
            var fetchXml = $@"
            <fetch no-lock='true'>
            <entity name='salesorder'>
                <attribute name='name' />
                <attribute name='pricelevelid' />
                <attribute name='accountid' />
                <filter>
                <condition attribute='cr6de_validacao' operator='ne' value='1'/>
                <condition attribute='createdon' operator='last-seven-days' />
                </filter>
            </entity>
            </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public Entity GetUnidade(string id)
        {
            var fetchXml = $@"
                <fetch>
                <entity name='uom'>
                    <attribute name='name' />
                    <attribute name='quantity' />
                    <attribute name='uomid' />
                    <attribute name='uomscheduleid'/>
                    <filter>
                    <condition attribute='uomscheduleid' operator='eq' value='{id}'/>
                    </filter>
                </entity>
                </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml)).Entities.FirstOrDefault();
        }
        public EntityCollection GetProduct()
        {
            var fetchXml = $@"
            <fetch no-lock='true'>
            <entity name='product'>
                <attribute name='name' />
                <attribute name='productnumber' />
                <attribute name='quantitydecimal' />
                <attribute name='defaultuomid' />
                <attribute name='defaultuomscheduleid' />
                <filter>
                <condition attribute='createdon' operator='last-seven-days' />
                </filter>
            </entity>
            </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }
        public EntityCollection GetOrderProduct()
        {
            var fetchXml = $@"
            <fetch no-lock='true'>
            <entity name='salesorderdetail'>
                <attribute name='quantity' />
                <attribute name='salesorderid' />
                <attribute name='productid' />
                <attribute name='uomid' />
                <filter>
                <condition attribute='cr6de_validacao' operator='ne' value='1'/>
                <condition attribute='createdon' operator='last-seven-days' />
                </filter>
            </entity>
            </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml));
        }

        public Entity GetMoeda()
        {
            var fetchXml = $@"
                <fetch top='1' no-lock='true'>
                <entity name='transactioncurrency'>
                    <attribute name='transactioncurrencyid' />
                    <filter>
                    <condition attribute='currencyname' operator='eq' value='Real'/>
                    </filter>
                </entity>
                </fetch>";
            return Service.RetrieveMultiple(new FetchExpression(fetchXml)).Entities.FirstOrDefault();
        }
    }
}