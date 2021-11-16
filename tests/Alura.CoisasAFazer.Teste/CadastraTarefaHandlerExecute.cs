using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefasComInformacoesValidasDeveIncluirNoDB()
        {
            //criar o comando
            //depois executar o comando
            
            //Arrange
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2021, 11, 14));

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;//padrão builder utilizado
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            //handler -> tratador desse comando
            var handler = new CadastraTarefaHandler(repo);

            //Act
            handler.Execute(comando); //SUT - CadastraTarefaHandlerExecute

            //Assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar xUnit").FirstOrDefault();
            Assert.NotNull(tarefa);

        }

        [Fact]
        public void QuandoExceptionForLancadaResultadoIsSuccessDeveSerFalse()
        {
            //Arrange
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2021, 11, 14));

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;//padrão builder utilizado
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            //handler -> tratador desse comando
            var handler = new CadastraTarefaHandler(repo);

            //Act
            CommandResult resultado = handler.Execute(comando);

            //Assert
            Assert.False(resultado.IsSuccess);
        }
    }
}