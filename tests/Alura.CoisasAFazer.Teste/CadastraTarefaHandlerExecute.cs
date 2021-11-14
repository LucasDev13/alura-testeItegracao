using System;
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
            //handler -> tratador desse comando
            var handler = new CadastraTarefaHandler();
            //Act
            handler.Execute(comando); //SUT - CadastraTarefaHandlerExecute

            //Assert
        }
    }
}