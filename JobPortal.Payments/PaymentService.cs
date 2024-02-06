using PayPal.Api;
using System;
using System.Collections.Generic;

namespace JobPortal.Payments
{
    public static class PaymentService
    {
        #region Constants
        private const string DefaultCurrency = "USD";
        #endregion


        #region ExecuteCreditCardPayment
        /// <summary>
        /// Processes the credit card information through paypal service.
        /// </summary>
        /// <param name="creditCard">The value for credit card information.</param>
        /// <param name="intent">The intent of the transaction e.g. sale,authorize.</param>
        /// <returns>Returns the payment resonse <see cref="PaymentResponse"/>.</returns>
        public static PaymentResponse ExecuteCreditCardPayment(CreditCard creditCard, string payerID, string intent = "sale")
        {
            PaymentResponse response = new PaymentResponse();
            var apiContext = PayPalConfiguration.GetContext();

            //  A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "credit_card",
                funding_instruments = new List<FundingInstrument>()
               {
                    new FundingInstrument()
                    {
                        credit_card = new PayPal.Api.CreditCard()
                        {
                            number = creditCard.Number,
                            type = (new Card().GetCardType(creditCard.Number)).ToLower(),
                            cvv2 = creditCard.SecurityCode,
                            expire_month = Convert.ToInt32(creditCard.ExpiryMonth),
                            expire_year = Convert.ToInt32(creditCard.ExpiryYear),
                            first_name = creditCard.FirstName,
                            last_name = creditCard.LastName,
                            billing_address = new Address
                            {
                                line1  = "Add 1",
                                line2 = "Add 2",
                                city = "San Mateo",
                                country_code = "US",
                                postal_code = "94002",
                                state = "CA"
                            }
                        }
                    }
               },
                payer_info = new PayerInfo
                {
                    email = "sb-1naky7929196@personal.example.com",//creditCard.Email,
                    payer_id = payerID
                }
            };

            //A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = intent, // `sale` or `authorize`
                payer = payer,
                transactions = GetTransactionsList(creditCard),
                //payee = new Payee
                //{
                //    //email = "shalinichoudhary@outlook.in",
                //    merchant_id = "W2N9HNTKSWMGL",
                //    //payee_display_metadata = "",
                //    //phone = new Phone() { country_code = "+91", extension = "", national_number = "9999012345" }
                //}
            };
            try
            {
                var createdPayment = payment.Create(apiContext);
                if (createdPayment != null)
                {
                    response.Id = createdPayment.id;
                    response.Status = createdPayment.state;
                    response.Message = "Success";
                }
            }
            catch (PayPal.PayPalException ex)
            {
                response.Status = "Failed";
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region ExecutePayment
        /// <summary>
        /// Executes the payment request based on <paramref name="subscriptionInformation"/> information.
        /// </summary>
        /// <param name="subscriptionInformation">The target transaction information.</param>
        /// <returns>Returns the payment resonse <see cref="PaymentResponse"/>.</returns>
        public static PaymentResponse ExecutePayment(SubscriptionInformation subscriptionInformation)
        {
            PaymentResponse response = new PaymentResponse();
            var apiContext = PayPalConfiguration.GetContext();

            if (string.IsNullOrEmpty(subscriptionInformation.PayerId))
            {
                //Items within a transaction.
                var itemList = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = subscriptionInformation.Name,
                            description = subscriptionInformation.Description,
                            currency = subscriptionInformation.Currency,
                            price = subscriptionInformation.Amount,
                            quantity = subscriptionInformation.Quantity.ToString(),
                            sku = subscriptionInformation.SKU
                        }
                    }
                };

                //Payer A resource representing a Payer that funds a payment Payment Method as paypal
                var payer = new Payer() { payment_method = "paypal" };
                var redirectUrl = subscriptionInformation.Url;
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&cancel=true",
                    return_url = redirectUrl
                };

                //Amount Let's you specify a payment amount.
                var amount = new Amount()
                {
                    currency = subscriptionInformation.Currency,
                    total = subscriptionInformation.Amount, // Total must be equal to sum of shipping, tax and subtotal.
                    //details = details
                };

                //Transaction A transaction defines the contract of a payment - what is the payment for and who is fulfilling it.
                var transactionList = new List<Transaction>();
                //The Payment creation API requires a list of Transaction; add the created Transaction to a List
                transactionList.Add(new Transaction()
                {
                    description = subscriptionInformation.Description,
                    invoice_number = GetRandomInvoiceNumber(),
                    amount = amount,
                    item_list = itemList
                });

                //Payment A Payment Resource; create one using the above types and intent as sale or authorize
                var payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };

                // Ignore workflow code segment Create a payment using a valid APIContext
                var createdPayment = payment.Create(apiContext);

                // Ignore workflow code segment Using the links provided by the createdPayment object, we can give the user the option to redirect to PayPal to approve the payment.
                var links = createdPayment.links.GetEnumerator();
                while (links.MoveNext())
                {
                    var link = links.Current;
                    if (!string.IsNullOrWhiteSpace(link.rel) && link.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))
                        response.Url = link.href;
                }
            }
            else
            {
                var paymentId = subscriptionInformation.PaymentId;
                var paymentExecution = new PaymentExecution() { payer_id = subscriptionInformation.PayerId };
                var payment = new Payment() { id = paymentId };

                try
                {
                    var capture = payment.Execute(apiContext, paymentExecution);
                    if (capture != null)
                    {
                        response.Id = capture.id;
                        response.Status = capture.state;
                        response.Message = "Success";
                    }
                }
                catch (PayPal.PayPalException ex)
                {
                    response.Status = "Failed";
                    response.Message = ex.Message;
                }
            }
            return response;
        }
        #endregion

        #region GetRandomInvoiceNumber
        /// <summary>
        /// Generates a random invoice number.
        /// </summary>
        public static string GetRandomInvoiceNumber()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region GetTransactionsList
        /// <summary>
        /// Gets the list of transaction s performed based on credit card information.
        /// </summary>
        /// <param name="creditCard">The target credit card information.</param>
        private static List<Transaction> GetTransactionsList(CreditCard creditCard)
        {
            // A transaction defines the contract of a payment
            // what is the payment for and who is fulfilling it. 
            var transactionList = new List<Transaction>();

            // The Payment creation API requires a list of Transaction; 
            // add the created Transaction to a List
            transactionList.Add(new Transaction()
            {
                amount = new Amount() { currency = DefaultCurrency, total = creditCard.Amount.ToString() },
                description = "Joblisting payment",
                invoice_number = GetRandomInvoiceNumber()

            });
            return transactionList;
        }
        #endregion
    }
}
