using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using System.Linq;

using Agenda.RPC.Models;

namespace Agenda.RPC
{
    public class AgendaService : Agenda.AgendaBase
    {
        private int _Id = 0;

        private List<ContatoModel> agenda;

        public AgendaService()
        {
            Init();
        }

        public void Init()
        {
            if (File.Exists("save.bin"))
            {
                Stream openFileStream = File.OpenRead("save.bin");
                BinaryFormatter deserializer = new BinaryFormatter();
                agenda = (List<ContatoModel>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
                _Id = agenda.Count;
            }
            else
            {
                agenda = new List<ContatoModel>();
            }
        }

        private void Write()
        {
            Stream SaveFileStream = File.Create("save.bin");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, agenda);
            SaveFileStream.Close();
        }

        public override Task<Empty> Create(Contato request, ServerCallContext context)
        {
            var contato = MapToModel(request);
            contato.Id = ++_Id;
            agenda.Add(contato);
            Write();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Update(Contato request, ServerCallContext context)
        {
            agenda.RemoveAll(x => x.Id == request.Id);
            agenda.Add(MapToModel(request));
            Write();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Delete(SearchRequest request, ServerCallContext context)
        {
            agenda.RemoveAll(
                x => x.Nome.Contains(request.Search, System.StringComparison.OrdinalIgnoreCase) ||
                     x.Telefone.Contains(request.Search)
            );
            Write();
            return Task.FromResult(new Empty());
        }

        public override Task<Contato> Get(SearchRequest request, ServerCallContext context)
        {
            return Task.FromResult(MapToRPC(agenda.First(
                x => x.Nome.Contains(request.Search, System.StringComparison.OrdinalIgnoreCase) ||
                     x.Telefone.Contains(request.Search)
            )));
        }

        public override Task<Contatos> List(Empty request, ServerCallContext context)
        {
            var result = new Contatos();
            result.Contatos_.AddRange(agenda.OrderBy(x => x.Nome).Select(x => MapToRPC(x)));
            return Task.FromResult(result);
        }

        public override Task<Contatos> Search(SearchRequest request, ServerCallContext context)
        {
            var result = new Contatos();

            var search = agenda.Where(
                x => x.Nome.Contains(request.Search, System.StringComparison.OrdinalIgnoreCase) ||
                     x.Telefone.Contains(request.Search)
            ).ToList();

            result.Contatos_.AddRange(search.OrderBy(x => x.Nome).Select(x => MapToRPC(x)));

            return Task.FromResult(result);
        }

        private Contato MapToRPC(ContatoModel contato)
        {
            return new Contato{
                Id = contato.Id,
                Nome = contato.Nome,
                Telefone = contato.Telefone,
                Email = contato.Email,
                Endereco = contato.Endereco
            };
        }

        private ContatoModel MapToModel(Contato contato)
        {
            return new ContatoModel{
                Id = contato.Id,
                Nome = contato.Nome,
                Telefone = contato.Telefone,
                Email = contato.Email,
                Endereco = contato.Endereco
            };
        }
    }
}
