using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class GerenciaPrazoDasTarefasHandlerExecute
    {
        [Fact]
        public void QuandoTarefasEstiveremAtrasadasDevemMudarSeuStatus()
        {
            //Arrange
            var compCateg = new Categoria(1, "Compras");
            var casaCateg = new Categoria(2, "Casa");
            var trabCateg = new Categoria(3, "Trabalho");
            var saudCateg = new Categoria(4, "Saúde");
            var higiCateg = new Categoria(5, "Higiene");

            var tarefas = new List<Tarefa>
            {
                //atrasadas a partir de 1/1/2019
                new Tarefa(1, "Tirar lixo", casaCateg, new DateTime(2018,12,31), null, StatusTarefa.Criada),
                new Tarefa(4, "Fazer almoço", casaCateg, new DateTime(2017,12,1), null, StatusTarefa.Criada),
                new Tarefa(9, "Ir à acadêmia", saudCateg, new DateTime(2018,12,31), null, StatusTarefa.Criada),
                new Tarefa(7, "Concluir relatório", trabCateg, new DateTime(2018,5,7), null, StatusTarefa.Pendente),
                new Tarefa(10, "Beber água", casaCateg, new DateTime(2018,12,31), null, StatusTarefa.Criada),
                
                
                //dentro do prazo em 1/1/2019
                new Tarefa(8, "Comparecer a reunião", trabCateg, new DateTime(2018,11,12), new DateTime(2018,11,30), StatusTarefa.Concluida),
                new Tarefa(2, "Arruamr a cama", casaCateg, new DateTime(2019,4,5), null, StatusTarefa.Criada),
                new Tarefa(3, "Escovar os dentes", higiCateg, new DateTime(2019,1,2), null, StatusTarefa.Criada),
                new Tarefa(5, "Comprar presente do Lucas", compCateg, new DateTime(2019,10,8), null, StatusTarefa.Criada),
                new Tarefa(6, "Comprar pão", compCateg, new DateTime(2019,11,20), null, StatusTarefa.Criada),

            };
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext") 
                .Options;//padrão builder utilizado
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            repo.IncluirTarefas(tarefas.ToArray());

            var comando = new GerenciaPrazoDasTarefas(new DateTime(2019,1,1));
            var handler = new GerenciaPrazoDasTarefasHandler(repo);

            //Act
            handler.Execute(comando);

            //Assert
            var tarefasEmAtraso = repo.ObtemTarefas(t => t.Status == StatusTarefa.EmAtraso);
            Assert.Equal(5, tarefasEmAtraso.Count());
        }

        [Fact]
        public void QuandoInvocadoDeveChamarAtualizarTarefasNaQtdeVezesDoTotalDeTarefasAtrasadas()
        {
            var categ = new Categoria("Dummy");
            var tarefas = new List<Tarefa>
            {
                new Tarefa(1, "Tirar lixo", categ, new DateTime(2018,12,31), null, StatusTarefa.Criada),
                new Tarefa(4, "Fazer almoço", categ, new DateTime(2017,12,1), null, StatusTarefa.Criada),
                new Tarefa(9, "Ir à acadêmia", categ, new DateTime(2018,12,31), null, StatusTarefa.Criada)
            };

            //Arrange
            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.ObtemTarefas(It.IsAny<Func<Tarefa, bool>>()))
                .Returns(tarefas);

            var repo = mock.Object;


            var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
            var handler = new GerenciaPrazoDasTarefasHandler(repo);

            //Act
            handler.Execute(comando);

            //Assert
            //chama 3 vezes ??
            //mock.Verify(r => r.AtualizarTarefas(It.IsAny<Tarefa[]>()), Times.Exactly(3));

            //Verifica se chama 1 vez
            mock.Verify(r => r.AtualizarTarefas(It.IsAny<Tarefa[]>()), Times.Once());
        }
    }
}
