using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umfgcloud.loja.dominio.service.DTO;

namespace umfgcloud.aplicacao.service.testes.Classes
{
    [TestClass]
    public sealed class ProdutoServicoTestes : AbstractServicoTestes
    {
        private const string C_OWNER = "Juliano Maciel";
        private const string C_OWNER2 = "Jair Júnior";
        private const string C_CATEGORY = "produto";
        private const decimal C_VALOR_NEGATIVO = -89.90m;

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_Sucesso()
        {
            try
            {
                //o objetivo do using é o desenvolvedor ter controlle sobre o 
                //dispose do objeto
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                await servico.AdicionarAsync(dto);

                var produto = (await servico.ObterTodosAsync()).FirstOrDefault();

                Assert.IsNotNull(produto);
                Assert.IsFalse(Guid.Empty.Equals(produto.Id));
                Assert.AreEqual("TESTE", produto.Descricao);
                Assert.AreEqual("123456789", produto.EAN);
                Assert.AreEqual(39.90m, produto.ValorCompra);
                Assert.AreEqual(89.90m, produto.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorCompraNegativo()
        {
            try
            {
                //o objetivo do using é o desenvolvedor ter controlle sobre o 
                //dispose do objeto
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = -39.90m,
                    ValorVenda = 89.90m,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorVendaNegativo()
        {
            try
            {
                //o objetivo do using é o desenvolvedor ter controlle sobre o 
                //dispose do objeto
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = -89.90m,
                };

                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public void ProdutoServico_Instanciar_Falha()
        {
            try
            {
                //o objetivo do using é o desenvolvedor ter controlle sobre o 
                //dispose do objeto
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                Assert.ThrowsException<InvalidDataException>(() => GetProdutoServicoInvalidJWT(context));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER2)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO 1",
                    EAN = "111",
                    ValorCompra = 10,
                    ValorVenda = 20
                });

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO 2",
                    EAN = "222",
                    ValorCompra = 20,
                    ValorVenda = 40
                });

                var produtos = await servico.ObterTodosAsync();

                Assert.AreEqual(2, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER2)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "NOTEBOOK",
                    EAN = "123456",
                    ValorCompra = 2500,
                    ValorVenda = 3500
                });

                var produtoInserido =
                    (await servico.ObterTodosAsync()).First();

                var produto =
                    await servico.ObterPorIdAsync(produtoInserido.Id);

                Assert.IsNotNull(produto);
                Assert.AreEqual(produtoInserido.Id, produto.Id);
                Assert.AreEqual("NOTEBOOK", produto.Descricao);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER2)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TECLADO",
                    EAN = "123",
                    ValorCompra = 50,
                    ValorVenda = 100
                });

                var produto =
                    (await servico.ObterTodosAsync()).First();

                await servico.RemoverAsync(produto.Id);

                var produtos =
                    await servico.ObterTodosAsync();

                Assert.AreEqual(0, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER2)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_Sucesso()
        {
            try
            {
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                var servico = GetProdutoServicoValidJWT(context);

                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "MOUSE",
                    EAN = "111",
                    ValorCompra = 20,
                    ValorVenda = 40
                });

                var produto =
                    (await servico.ObterTodosAsync()).First();

                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = produto.Id,
                    Descricao = "MOUSE GAMER",
                    EAN = "999",
                    ValorCompra = 30,
                    ValorVenda = 80
                };

                await servico.AtualizarAsync(dto);

                var atualizado =
                    await servico.ObterPorIdAsync(produto.Id);

                Assert.AreEqual("MOUSE GAMER", atualizado.Descricao);
                Assert.AreEqual("999", atualizado.EAN);
                Assert.AreEqual(30m, atualizado.ValorCompra);
                Assert.AreEqual(80m, atualizado.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
