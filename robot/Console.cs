using System;

namespace Robot
{
    public static class ConsoleMenu
    {
        public static void Opcoes()
        {
            try
            {
                Console.WriteLine("----------------------------------------------\n" +
                "Opções\n" +
                "----------------------------------------------\n" +
                "[1] Preparar ambiente de origem\n" +
                "[2] Criação em massa no ambiente de origem\n" +
                "[3] Preparar ambiente de destino\n" +
                "[4] Copiar dados\n" +
                "[5] Sair do programa\n" +
                "----------------------------------------------\n" +
                "Digite a opção desejada: ");
                Acao(Console.ReadLine());
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        static void Acao(string escolha)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("----------------------------------------------");
                switch (escolha)
                {
                    case "1":
                        PrepareOrigem.CreatePrepare();
                        break;
                    case "2":
                        CreateOrigem.CreateAll();
                        break;
                    case "3":
                        Prepare.PastePrepare();
                        break;
                    case "4":
                        PasteData.PasteAll();
                        break;
                    case "5":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("----------------------------------------------");
                        Console.WriteLine("Escolha uma opção valida.");
                        Opcoes();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
