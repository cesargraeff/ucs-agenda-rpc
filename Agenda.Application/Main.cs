using System;

using Agenda.Application.Services;

namespace Agenda.Application
{
    public class Main
    {
        private readonly AgendaService _rpc;

        public Main()
        {
            _rpc = new AgendaService();

            while (true)
            {
                WriteMenu();
                ReadOption();
            }
        }

        private void WriteMenu()
        {
            Console.Clear();
            Console.WriteLine("---------------------- AGENDA ---------------------- ");

            var agenda = _rpc.List();
            Console.WriteLine("");
            while (agenda.MoveNext())
            {
                var val = agenda.Current;
                Console.WriteLine(val.Id.ToString() + " - " + val.Nome + " \t" + val.Telefone + " \t" + val.Endereco + " \t" + val.Email);
            }
            Console.WriteLine("");

            Console.WriteLine("---------------------------------------------------- ");
            Console.WriteLine("1 - NOVO \n2 - ALTERAR \n3 - REMOVER \n4 - PESQUISAR \n9 - SAIR \n");
        }

        private void ReadOption()
        {
            Console.Write("Digite a opção: ");
            switch (Convert.ToInt64(Console.ReadLine()))
            {
                case 1:
                    Create();
                    break;
                case 2:
                    Update();
                    break;
                case 3:
                    Delete();
                    break;
                case 4:
                    Search();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
            }
        }

        private void Create()
        {
            var c = new Contato();
            Console.Write("Digite o nome: ");
            c.Nome = Console.ReadLine();
            Console.Write("Digite o telefone: ");
            c.Telefone = Console.ReadLine();
            Console.Write("Digite o endereço: ");
            c.Endereco = Console.ReadLine();
            Console.Write("Digite o email: ");
            c.Email = Console.ReadLine();
            _rpc.Create(c);
        }

        private void Update()
        {
            Console.Write("Informe o contato: ");
            var search = Console.ReadLine();
            var contato = _rpc.Get(search);
            Console.WriteLine("Selecione o campo: ");
            Console.WriteLine("1 - NOME");
            Console.WriteLine("2 - TELEFONE");
            Console.WriteLine("3 - ENDEREÇO");
            Console.WriteLine("4 - EMAIL");
            var op = Convert.ToInt64(Console.ReadLine());
            switch (op)
            {
                case 1:
                    Console.Write("NOME: ");
                    contato.Nome = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("TELEFONE: ");
                    contato.Telefone = Console.ReadLine();
                    break;
                case 3:
                    Console.Write("ENDEREÇO: ");
                    contato.Endereco = Console.ReadLine();
                    break;
                case 4:
                    Console.Write("EMAIL: ");
                    contato.Email = Console.ReadLine();
                    break;
            }
            _rpc.Update(contato);
        }

        private void Delete()
        {
            Console.Write("Informe o contato: ");
            var search = Console.ReadLine();
            _rpc.Delete(search);
        }

        private void Search()
        {
            Console.Write("Digite a pesquisa: ");
            var query = Console.ReadLine();
            var agenda = _rpc.Search(query);
            Console.Clear();
            Console.WriteLine("---- AGENDA ---- ");
            Console.WriteLine("");

            while (agenda.MoveNext())
            {
                var val = agenda.Current;
                Console.WriteLine(val.Id.ToString() + " - " + val.Nome + " \t" + val.Telefone + " \t" + val.Endereco + " \t" + val.Email);
            }

            Console.WriteLine("");
            Console.WriteLine("PARA VOLTAR PRESSIONE ENTER");
            Console.ReadLine();
        }
    }
}