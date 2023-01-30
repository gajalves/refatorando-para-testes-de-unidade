using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Domain.Entities;
using Store.Domain.Entitites;
using Store.Domain.Enums;

namespace Store.Tests.Entities
{
    [TestClass]
    public class OrderTests
    {
        private readonly Customer _customer;
        private readonly Discount _discount;
        private readonly Product _product;

        public OrderTests()
        {
            _customer = new Customer("gabriel", "ga.mail@mail.com");
            _discount = new Discount(0, DateTime.Now.AddDays(5));
            _product = new Product("Prod 1", 5, true);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_novo_pedido_valido_ele_deve_gerar_um_numero_com_8_caracteres()
        {
            //
            var order = new Order(_customer, 0, _discount);

            //
            Assert.AreEqual(order.Number.Length, 8);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_novo_pedido_seu_status_deve_ser_aguardando_pagamento()
        {
            //
            var order = new Order(_customer, 0, _discount);

            //
            Assert.AreEqual(order.Status, EOrderStatus.WaitingPayment);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_pagamento_do_pedido_seu_status_deve_ser_aguardando_entrega()
        {
            //
            var order = new Order(_customer, 0, _discount);
            order.AddItem(_product, 1);
            //
            order.Pay(5);

            //
            Assert.AreEqual(order.Status, EOrderStatus.WaitingDelivery);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_pedido_cancelado_seu_status_deve_ser_cancelado()
        {
            //
            var order = new Order(_customer, 0, _discount);

            //
            order.Cancel();

            //
            Assert.AreEqual(order.Status, EOrderStatus.Canceled);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_novo_item_sem_produto_o_mesmo_nao_deve_ser_adicionado()
        {
            //
            var order = new Order(_customer, 0, _discount);

            //
            order.AddItem(null, 1);

            //
            Assert.AreEqual(order.Items.Count, 0);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_novo_item_com_quantidade_zero_ou_menor_o_mesmo_nao_deve_ser_adicionado()
        {
            //
            var order = new Order(_customer, 0, _discount);

            //
            order.AddItem(_product, 0);

            //
            Assert.AreEqual(order.Items.Count, 0);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_novo_pedido_valido_seu_total_deve_ser_50()
        {
            var order = new Order(_customer, 0, _discount);

            //
            order.AddItem(_product, 10);

            //
            Assert.AreEqual(order.Total(), 50);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_desconto_expirado_o_valor_do_pedido_deve_ser_60()
        {
            var discount = new Discount(10, DateTime.Now.AddYears(-1));
            var order = new Order(_customer, 0, discount);

            //
            order.AddItem(_product, 12);

            //
            Assert.AreEqual(order.Total(), 60);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_desconto_invalido_o_valor_do_pedido_deve_ser_60()
        {
            var order = new Order(_customer, 0, null);
            
            order.AddItem(_product, 12);
            
            Assert.AreEqual(order.Total(), 60);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_desconto_de_10_o_valor_do_pedido_deve_ser_50()
        {
            var discount = new Discount(10, DateTime.Now.AddYears(2));
            var order = new Order(_customer, 0, discount);

            //
            order.AddItem(_product, 12);

            //
            Assert.AreEqual(order.Total(), 50);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_uma_taxa_de_entrega_de_10_o_valor_do_pedido_deve_ser_60()
        {            
            var order = new Order(_customer, 10, null);

            //
            order.AddItem(_product, 10);

            //
            Assert.AreEqual(order.Total(), 60);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_um_pedido_sem_cliente_o_mesmo_deve_ser_invalido()
        {
            var order = new Order(null, 10, null);

            //
            order.AddItem(_product, 10);

            //
            Assert.AreEqual(order.Valid, false);
        }
    }
}
