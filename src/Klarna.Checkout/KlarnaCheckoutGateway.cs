﻿using System;
using System.Linq;
using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;
using EPiServer.Logging;
using Klarna.Checkout.Steps;
using Klarna.OrderManagement.Steps;

namespace Klarna.Checkout
{
    public class KlarnaCheckoutGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(KlarnaCheckoutGateway));
        private IOrderForm _orderForm;

        public IOrderGroup OrderGroup { get; set; }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            OrderGroup = payment.Parent.Parent;
            _orderForm = payment.Parent;
            return ProcessPayment(payment as IPayment, ref message);
        }

        public bool ProcessPayment(IPayment payment, ref string message)
        {
            try
            {
                Logger.Debug("Klarna checkout gateway. Processing Payment ....");

                if (_orderForm == null)
                {
                    _orderForm = OrderGroup.Forms.FirstOrDefault(form => form.Payments.Contains(payment));
                }
                
                var authorizePaymentStep = new AuthorizePaymentStep(payment, OrderGroup.Market.MarketId);
                var capturePaymentStep = new CapturePaymentStep(payment, OrderGroup.Market.MarketId);
                var creditPaymentStep = new CreditPaymentStep(payment, OrderGroup.Market.MarketId);
                var cancelPaymentStep = new CancelPaymentStep(payment, OrderGroup.Market.MarketId);

                authorizePaymentStep.SetSuccessor(capturePaymentStep);
                capturePaymentStep.SetSuccessor(creditPaymentStep);
                creditPaymentStep.SetSuccessor(cancelPaymentStep);

                return authorizePaymentStep.Process(payment, _orderForm, OrderGroup, OrderGroup.GetFirstShipment(), ref message);
            }
            catch (Exception ex)
            {
                Logger.Error("Process checkout failed with error: " + ex.Message, ex);
                message = ex.Message;
                throw;
            }
        }
    }
}