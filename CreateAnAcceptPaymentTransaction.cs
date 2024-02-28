using System;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace AcceptjsDotnet
{
    public class CreateAnAcceptPaymentTransaction
    {
        // The official Authorize.net documentation uses type ANetApiResponse, which is incorrect
        public static createTransactionResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount, String descriptor, String value, customerAddressType billingAddress, lineItemType[] lineItems)
        {
            System.Diagnostics.Debug.WriteLine("Create an Accept Payment Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = descriptor,
                dataValue = value
                
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
            };
            
            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            
            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();
            
            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if(response.transactionResponse.messages != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        System.Diagnostics.Debug.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        System.Diagnostics.Debug.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        System.Diagnostics.Debug.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        System.Diagnostics.Debug.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            System.Diagnostics.Debug.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        System.Diagnostics.Debug.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error Code: " + response.messages.message[0].code);
                        System.Diagnostics.Debug.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Null Response.");
            }

            return response;
        }
    }
}