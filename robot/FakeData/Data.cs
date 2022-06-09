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
    public static class FakeData
    {
        static Random Rdn = new Random();
        private static string[] ceps = new string[10] {"35700-034", "68904-600", "58085-375", "69317-300", "89809-540", "67033-441",
        "74691-844", "96501-360", "76873-534", "12233-320"};
        public static async ValueTask<List<T>> RequisicaoMockaroo<T>(string schemaName)
        {
            try
            {
                string key = ConfigurationManager.AppSettings.Get("ChaveMock");
                HttpClient client = new HttpClient();
                var resultado = await client.GetStringAsync($"https://my.api.mockaroo.com/{schemaName}.json?key={key}");
                return JsonConvert.DeserializeObject<List<T>>(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async ValueTask<ViaCep> RequisicaoViaCep(string cep = null)
        {
            try
            {
                if (string.IsNullOrEmpty(cep))
                    cep = ceps[Util.SorteioNumero(ceps.Length)];
                HttpClient client = new HttpClient();
                var resultado = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");
                return JsonConvert.DeserializeObject<ViaCep>(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string Cpf()
        {
            var cpf = new int[11];
            for (int i = 0; i < 9; i++)
            {
                cpf[i] = Rdn.Next(10);
            }
            var multpDigito1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += multpDigito1[i] * cpf[i];
            }
            cpf[9] = Resto(soma);
            var multpDigito2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += multpDigito2[i] * cpf[i];
            }
            cpf[10] = Resto(soma);
            return Convert.ToUInt64(string.Join("", cpf)).ToString(@"000\.000\.000\-00");
        }
        public static string Cnpj()
        {
            var cnpj = new int[14];
            for (int i = 0; i < 12; i++)
            {
                cnpj[i] = Rdn.Next(10);
            }
            var multpDigito1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                soma += multpDigito1[i] * cnpj[i];
            }
            cnpj[12] = Resto(soma);
            var multpDigito2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 13; i++)
            {
                soma += multpDigito2[i] * cnpj[i];
            }
            cnpj[13] = Resto(soma);
            return Convert.ToUInt64(string.Join("", cnpj)).ToString(@"00\.000\.000\/0000\-00");
        }

        static int Resto(int soma)
        {
            int resto = soma % 11;
            return (resto < 2) ? 0 : (11 - resto);
        }
    }
}