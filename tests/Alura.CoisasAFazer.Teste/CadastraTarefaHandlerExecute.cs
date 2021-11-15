using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Services.Handlers;
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

            var repo = new RepositorioFake();
            //handler -> tratador desse comando
            var handler = new CadastraTarefaHandler(repo);
            //Act
            handler.Execute(comando); //SUT - CadastraTarefaHandlerExecute

            //Assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar xUnit").FirstOrDefault();
            Assert.NotNull(tarefa);

        }
    }
}