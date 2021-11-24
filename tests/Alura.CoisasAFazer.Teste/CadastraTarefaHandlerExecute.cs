using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

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

            var mock = new Mock<ILogger<CadastraTarefaHandler>>();

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;//padrão builder utilizado
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            //handler -> tratador desse comando
            var handler = new CadastraTarefaHandler(repo, mock.Object);

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

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())) 
                .Throws(new Exception("Houve um erro na inclusão de tarefas"));
            
            
            var repo = mock.Object;

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            //Act
            CommandResult resultado = handler.Execute(comando);

            //Assert
            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void QuandoExceptionForLancadaDeveLoggarAMensagemDaExcecao()
        {
            //Arrange
            var mensagemEsperada = "Houve um erro na inclusão de tarefas";
            var excecaoEsperada = new Exception(mensagemEsperada);

            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), new DateTime(2021, 11, 14));

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(excecaoEsperada);


            var repo = mock.Object ;

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            //Act
            CommandResult resultado = handler.Execute(comando);

            //Assert
            mockLogger.Verify(l => 
                l.Log(
                    LogLevel.Error, //nível de log => LogError
                    It.IsAny<EventId>(), //identificador do evento
                    It.IsAny<object>(), //objeto que será logado
                    excecaoEsperada, //exceção que será logada
                    It.IsAny<Func<object, Exception, string>>()
                ), //função que converte objeto+exceção >> string 
                Times.Once()); //O método vai ser chamada uma única vez.
        }
    }
}