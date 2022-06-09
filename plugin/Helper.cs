using System;

namespace Plugin
{
    public static class Helper
    {
        public static int CalculaDigitoCpf(int control, int[] cpf)
        {
            int soma = 0;
            int maximo = control - 1;
            for (int i = 0; i < maximo; i++, control--)
            {
                soma += cpf[i] * control;
            }
            return soma;
        }
        public static int CalculaDigitoCnpj(int control, int[] cnpj)
        {
            int soma = 0;
            var multiplicador = new int[13];
            if (control == 12)
            {
                multiplicador = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            }
            else if (control == 13)
            {
                multiplicador = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            }
            for (int i = 0; i < control; i++)
            {
                soma += cnpj[i] * multiplicador[i];
            }
            return soma;
        }
        public static int Resto(int soma)
        {
            int resto = soma % 11;
            if (resto >= 2)
            {
                return 11 - resto;
            }
            else
            {
                return 0;
            }
        }
        public static bool IsCpfValido(string campoCpf)
        {
            if (String.IsNullOrEmpty(campoCpf)) { return false; }
            campoCpf = campoCpf.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            if (String.IsNullOrEmpty(campoCpf) || campoCpf.Length != 11 ||
                campoCpf == "00000000000" ||
                campoCpf == "11111111111" ||
                campoCpf == "22222222222" ||
                campoCpf == "33333333333" ||
                campoCpf == "44444444444" ||
                campoCpf == "55555555555" ||
                campoCpf == "66666666666" ||
                campoCpf == "77777777777" ||
                campoCpf == "88888888888" ||
                campoCpf == "99999999999")
            {
                return false;
            }
            int[] cpf = new int[campoCpf.Length];
            for (int i = 0; i < campoCpf.Length; i++)
            {
                cpf[i] = (int)Char.GetNumericValue(campoCpf[i]);
            }
            if (Resto(CalculaDigitoCpf(10, cpf)) == cpf[9]
                && Resto(CalculaDigitoCpf(11, cpf)) == cpf[10])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsCnpjValido(string campoCnpj)
        {
            if (String.IsNullOrEmpty(campoCnpj)) { return false; }
            campoCnpj = campoCnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            if (String.IsNullOrEmpty(campoCnpj) || campoCnpj.Length != 14 ||
                campoCnpj == "00000000000000" ||
                campoCnpj == "11111111111111" ||
                campoCnpj == "22222222222222" ||
                campoCnpj == "33333333333333" ||
                campoCnpj == "44444444444444" ||
                campoCnpj == "55555555555555" ||
                campoCnpj == "66666666666666" ||
                campoCnpj == "77777777777777" ||
                campoCnpj == "88888888888888" ||
                campoCnpj == "99999999999999")
            {
                return false;
            }
            int[] cnpj = new int[campoCnpj.Length];
            for (int i = 0; i < campoCnpj.Length; i++)
            {
                cnpj[i] = (int)Char.GetNumericValue(campoCnpj[i]);
            }
            if (Resto(CalculaDigitoCnpj(12, cnpj)) == cnpj[12]
                && Resto(CalculaDigitoCnpj(13, cnpj)) == cnpj[13])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string FormatarCpf(string campoCpf)
        {
            string cpf = campoCpf.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
        public static string FormatarCnpj(string campoCnpj)
        {
            string cnpj = campoCnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}