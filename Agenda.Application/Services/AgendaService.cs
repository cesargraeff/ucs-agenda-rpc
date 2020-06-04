using System.Net.Http;
using System.Collections.Generic;
using Grpc.Net.Client;

namespace Agenda.Application.Services
{
    public class AgendaService
    {
        private readonly Agenda.AgendaClient _client;

        public AgendaService()
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            _client = new Agenda.AgendaClient(channel);
        }

        public void Create(Contato contato)
        {
            _client.Create(contato);
        }

        public void Update(Contato contato)
        {
            _client.Update(contato);
        }

        public void Delete(string search)
        {
            _client.Delete(new SearchRequest
            {
                Search = search
            });
        }

        public Contato Get(string search)
        {
            return _client.Get(new SearchRequest
            {
                Search = search
            });
        }

        public IEnumerator<Contato> List()
        {
            var list = _client.List(new Empty());
            return list.Contatos_.GetEnumerator();
        }

        public IEnumerator<Contato> Search(string search)
        {
            var list = _client.Search(new SearchRequest
            {
                Search = search
            });
            return list.Contatos_.GetEnumerator();
        }
    }
}