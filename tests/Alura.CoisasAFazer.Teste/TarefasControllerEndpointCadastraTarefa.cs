using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Alura.CoisasAFazer.WebApp.Controllers;
using Alura.CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Alura.CoisasAFazer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.Extensions.Logging;
using Alura.CoisasAFazer.Core.Models;

namespace Alura.CoisasAFazer.Teste
{
    public class TarefasControllerEndpointCadastraTarefa
    {
        [Fact]
        public void DadaTarefaComInformacoesValidasDeveRetornar200()
        {
            //arrange - cenário de entrada
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;
            var contexto = new DbTarefasContext(options);

            contexto.Categorias.Add(new Categoria(20, "Estudo"));
            contexto.SaveChanges();

            var repo = new RepositorioTarefa(contexto);

            var controlador = new TarefasController(repo, mockLogger.Object); 
            var model = new CadastraTarefaVM();
            model.IdCategoria = 20;
            model.Titulo = "Estudar xUnit";
            model.Prazo = new DateTime(2019, 12, 31);

            //act - método sobre teste
            var retorno = controlador.EndpointCadastraTarefa(model);

            //assert - condição de saída que quero testar
            Assert.IsType<OkResult>(retorno);
        }
    }
}
