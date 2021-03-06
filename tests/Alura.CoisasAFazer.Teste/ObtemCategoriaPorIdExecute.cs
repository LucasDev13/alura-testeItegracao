using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;

namespace Alura.CoisasAFazer.Teste
{
    public class ObtemCategoriaPorIdExecute
    {
        [Fact]
        public void QuandoIdForExistenteDeveChamarObtemCategoriaPorIdUmaUnicaVez()
        {
            //Arrange - condições de entrada.
            var idCategoria = 20;
            var comando = new ObtemCategoriaPorId(idCategoria);
            var mock = new Mock<IRepositorioTarefas>();
            var repo = mock.Object;
            var handler = new ObtemCategoriaPorIdHandler(repo);

            //Act
            handler.Execute(comando);
            //Assert - verificação
            mock.Verify(r => r.ObtemCategoriaPorId(idCategoria), Times.Once());
        }

    }
}
