﻿using System;
using System.Net;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace Robot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var serviceOrigem = Connection.Obter();
            ConsoleMenu.Opcoes();
            //CreateOrigem.DeleteAll(serviceOrigem);
            Console.WriteLine("Ação concluida.");
            Console.ReadKey();
        }
    }
}