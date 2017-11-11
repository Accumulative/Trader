//using System;
//using System.Web.Routing;
//using PayPalMvc;
//using Trader.Models;
//using System.Collections.Generic;

//namespace TraderServices
//{

//    public interface ITransactionService
//    {
//        SetExpressCheckoutResponse SendPayPalSetExpressCheckoutRequest(ApplicationCart cart, string serverURL, Address address, string userEmail = null);
//        GetExpressCheckoutDetailsResponse SendPayPalGetExpressCheckoutDetailsRequest(string token, ApplicationCart cart);
//        DoExpressCheckoutPaymentResponse SendPayPalDoExpressCheckoutPaymentRequest(ApplicationCart cart, string token, string payerId);
//    } 

//    /// <summary>
//    /// The Transaction Service is used to transform a purchase object (eg cart, basket, or single item) into a sale request with PayPal (in this case a cart)
//    /// It also allows your app to store the transactions in your database (create a table to match the PayPalTransaction model)
//    /// 
//    /// You should copy this file into your project and modify it to accept your purchase object, store PayPal transaction responses in your database,
//    /// as well as log events with your favourite logger.
//    /// </summary>
//    public class TransactionService : ITransactionService
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();
//        private PayPalMvc.ITransactionRegistrar _payPalTransactionRegistrar = new PayPalMvc.TransactionRegistrar();

//        public SetExpressCheckoutResponse SendPayPalSetExpressCheckoutRequest(ApplicationCart cart, string serverURL, Address address, string userEmail = null)
//        {
//            try
//            {
//                WebUILogging.LogMessage("SendPayPalSetExpressCheckoutRequest");

//                // Optional handling of cart items: If there is only a single item being sold we don't need a list of expressCheckoutItems
//                // However if you're selling a single item as a sale consider also adding it as an ExpressCheckoutItem as it looks better once you get to PayPal's site
//                // Note: ExpressCheckoutItems are currently NOT stored by PayPal against the sale in the users order history so you need to keep your own records of what items were in a cart
//                List<ExpressCheckoutItem> expressCheckoutItems = null;
//                if (cart.Items != null)
//                {
//                    expressCheckoutItems = new List<ExpressCheckoutItem>();
//                    foreach (ApplicationCartItem item in cart.Items)
//                        expressCheckoutItems.Add(new ExpressCheckoutItem((int)item.Quantity, item.Price, item.Name, item.Name));
//                }
//                SetExpressCheckoutResponse response = _payPalTransactionRegistrar.SendSetExpressCheckout(cart.Currency, cart.TotalPrice, cart.deliveryPrice, cart.PurchaseDescription, cart.Id.ToString(), serverURL, address.contactName, address.firstLine, address.city, address.postcode, "GB", expressCheckoutItems, userEmail, address.secondLine);
//                // Add a PayPal transaction record
//                PayPalTransaction transaction = new PayPalTransaction
//                {
//                    TransactionId = Guid.NewGuid(),
//                    SaleID = cart.SaleId,
//                    RequestId = response.RequestId,
//                    TrackingReference = cart.Id.ToString(),
//                    RequestTime = DateTime.UtcNow,
//                    RequestStatus = response.ResponseStatus.ToString(),
//                    TimeStamp = response.TIMESTAMP,
//                    RequestError = response.ErrorToString,
//                    Token = response.TOKEN,
//                };

//                db.PayPalTransactions.Add(transaction);
//                db.SaveChanges();

//                return response;
//            }
//            catch (Exception ex)
//            {
//                WebUILogging.LogException(ex.Message, ex);
//            }
//            return null;
//        }

//        public GetExpressCheckoutDetailsResponse SendPayPalGetExpressCheckoutDetailsRequest(string token, ApplicationCart cart)
//        {
//            try
//            {
//                WebUILogging.LogMessage("SendPayPalGetExpressCheckoutDetailsRequest");
//                GetExpressCheckoutDetailsResponse response = _payPalTransactionRegistrar.SendGetExpressCheckoutDetails(token);

//                // Add a PayPal transaction record
//                PayPalTransaction transaction = new PayPalTransaction
//                {
//                    TransactionId = Guid.NewGuid(),
//                    RequestId = response.RequestId,
//                    SaleID = cart.SaleId,
//                    TrackingReference = response.TrackingReference,
//                    RequestTime = DateTime.UtcNow,
//                    RequestStatus = response.ResponseStatus.ToString(),
//                    TimeStamp = response.TIMESTAMP,
//                    RequestError = response.ErrorToString,
//                    Token = response.TOKEN,
//                    PayerId = response.PAYERID,
//                    RequestData = response.ToString,
//                };

//                // Store this transaction in your Database
//                db.PayPalTransactions.Add(transaction);
//                db.SaveChanges();

//                return response;
//            }
//            catch (Exception ex)
//            {
//                WebUILogging.LogException(ex.Message, ex);
//            }
//            return null;
//        }

//        public DoExpressCheckoutPaymentResponse SendPayPalDoExpressCheckoutPaymentRequest(ApplicationCart cart, string token, string payerId)
//        {
//            try
//            {
//                WebUILogging.LogMessage("SendPayPalDoExpressCheckoutPaymentRequest");
//                DoExpressCheckoutPaymentResponse response = _payPalTransactionRegistrar.SendDoExpressCheckoutPayment(token, payerId, cart.Currency, cart.TotalPrice);

//                // Add a PayPal transaction record
//                PayPalTransaction transaction = new PayPalTransaction
//                {
//                    TransactionId = Guid.NewGuid(),
//                    RequestId = response.RequestId,
//                    TrackingReference = cart.Id.ToString(),
//                    RequestTime = DateTime.UtcNow,
//                    RequestStatus = response.ResponseStatus.ToString(),
//                    TimeStamp = response.TIMESTAMP,
//                    RequestError = response.ErrorToString,
//                    Token = response.TOKEN,
//                    RequestData = response.ToString,
//                    PaymentTransactionId = response.PaymentTransactionId,
//                    PaymentError = response.PaymentErrorToString,
//                };

//                // Store this transaction in your Database
//                db.PayPalTransactions.Add(transaction);
//                db.SaveChanges();

//                return response;
//            }
//            catch (Exception ex)
//            {
//                WebUILogging.LogException(ex.Message, ex);
//            }
//            return null;
//        }
//    }
//}