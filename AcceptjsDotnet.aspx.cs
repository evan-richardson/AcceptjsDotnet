using System;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

public partial class AcceptjsDotnetOrder : System.Web.UI.Page
{
    public string apiLoginId = AcceptjsDotnet.AppConfig.AuthorizeNetLoginID;
    public string transactionKey = AcceptjsDotnet.AppConfig.AuthorizeNetTransactionKey;
    public string acceptUIURL = AcceptjsDotnet.AppConfig.AcceptUIURL;
    public string publicKey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initializeOrder();
        }
        else if (Request.QueryString["OrderComplete"] == "False")
        {
            // Update site to show new order
            inputTshirts.Attributes.Remove("readonly");
            inputTshirts.Value = "";
            inputJeans.Attributes.Remove("readonly");
            inputJeans.Value = "";
            inputHoodies.Attributes.Remove("readonly");
            inputHoodies.Value = "";
            inputFirstName.Attributes.Remove("readonly");
            inputFirstName.Value = "";
            inputLastName.Attributes.Remove("readonly");
            inputLastName.Value = "";
            inputAddress.Attributes.Remove("readonly");
            inputAddress.Value = "";
            inputCity.Attributes.Remove("readonly");
            inputCity.Value = "";
            inputState.Attributes.Remove("readonly");
            inputState.Value = "";
            inputCountry.Attributes.Remove("readonly");
            inputCountry.Value = "";
            inputZip.Attributes.Remove("readonly");
            inputZip.Value = "";
            pTotal.InnerHtml = "$0.00";
            btnPay.Visible = true;

            initializeOrder();
        }
        else
        {
            initializeOrderComplete();
        }
    }

    protected void initializeOrder()
    {
        hfTotal.Value = "0.00";
        cardSuccess.Visible = false;
        cardError.Visible = false;
        btnReload.Visible = false;

        // initialize form for submission to online payment form.
        // Initialize for Authorize.net
        // Retrieve Public Key
        ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

        var request = new getMerchantDetailsRequest
        {
            merchantAuthentication = new merchantAuthenticationType() { name = apiLoginId, Item = transactionKey, ItemElementName = ItemChoiceType.transactionKey }
        };

        // instantiate the controller that will call the service
        var controller = new getMerchantDetailsController(request);
        controller.Execute();

        // get the response from the service (errors contained if any)
        var response = controller.GetApiResponse();

        // validate
        if (response != null)
        {
            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                Trace.Write("Merchant Name: " + response.merchantName);
                Trace.Write("Gateway ID: " + response.gatewayId);
                Trace.Write("Processors: ");
                foreach (processorType processor in response.processors)
                {
                    Trace.Write(processor.name + "; ");
                }
                publicKey = response.publicClientKey;
                btnPay.Attributes["data-apiloginid"] = apiLoginId;
                btnPay.Attributes["data-clientkey"] = publicKey;
            }
            else
            {
                pError.InnerHtml = "Failed to get merchant details.";
                cardError.Visible = true;
            }
        }
        else
        {
            pError.InnerHtml = "Null response.";
            cardError.Visible = true;
        }
    }

    protected void initializeOrderComplete()
    {
        // Update site to show order completion
        inputTshirts.Attributes.Add("readonly", "");
        inputJeans.Attributes.Add("readonly", "");
        inputHoodies.Attributes.Add("readonly", "");
        inputFirstName.Attributes.Add("readonly", "");
        inputLastName.Attributes.Add("readonly", "");
        inputAddress.Attributes.Add("readonly", "");
        inputCity.Attributes.Add("readonly", "");
        inputState.Attributes.Add("readonly", "");
        inputCountry.Attributes.Add("readonly", "");
        inputZip.Attributes.Add("readonly", "");
        pTotal.InnerHtml = "$" + hfTotal.Value;
        btnPay.Visible = false;
        btnReload.Visible = true;

        if (!string.IsNullOrEmpty(hfDataValue.Value))
        {
            // Create billing address
            var billingAddress = new customerAddressType
            {
                firstName = inputFirstName.Value,
                lastName = inputLastName.Value,
                address = inputAddress.Value,
                city = inputCity.Value,
                state = inputState.Value,
                country = inputCountry.Value,
                zip = inputZip.Value
            };

            // Create line items
            var lineItems = new lineItemType[3];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = GetInputValue(inputTshirts.Value), unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "jeans", quantity = GetInputValue(inputJeans.Value), unitPrice = new Decimal(55.00) };
            lineItems[2] = new lineItemType { itemId = "3", name = "hoodie", quantity = GetInputValue(inputHoodies.Value), unitPrice = new Decimal(70.00) };

            var transResponse = AcceptjsDotnet.CreateAnAcceptPaymentTransaction.Run(apiLoginId, transactionKey, Convert.ToDecimal(hfTotal.Value), hfDataDescriptor.Value, hfDataValue.Value, billingAddress, lineItems);

            if (transResponse.transactionResponse.responseCode != "1")
            {
                pError.InnerHtml = "Response code " + transResponse.transactionResponse.responseCode + ": " + transResponse.transactionResponse.messages[0].description;
                cardError.Visible = true;
            }
            else
            {
                cardSuccess.Visible = true;
            }
        }
        else
        {
            // We should never get here. Something went wrong.
            pError.InnerHtml = "An unexpected error occurred.";
            cardError.Visible = true;
        }
    }

    protected decimal GetInputValue(string inputText)
    {
        return Convert.ToDecimal(string.IsNullOrEmpty(inputText) ? "0" : inputText);
    }
}