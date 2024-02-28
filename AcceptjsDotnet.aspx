<%@ Page Title="" Language="C#" AutoEventWireup="True" Inherits="AcceptjsDotnetOrder" Codebehind="AcceptjsDotnet.aspx.cs" %>

<!doctype html>

<html lang="en">

<head>
    <title>Accept.js ASP.NET Demo</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">
</head>

<body>
    <form id="form1" action="AcceptjsDotnet.aspx?OrderComplete=True" runat="server">
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous"></script>

        <script type="text/javascript"
            src='<%=acceptUIURL %>'
            charset="utf-8">
        </script>

        <asp:HiddenField runat="server" ID="hfDataValue" />
        <asp:HiddenField runat="server" ID="hfDataDescriptor" />
        <asp:HiddenField runat="server" ID="hfTotal" />

        <h2 class="m-4">Accept.js ASP.NET Demo</h2>

        <div class="m-4 card bg-success text-white" id="cardSuccess" runat="server">
            <div class="card-header">
                <h4 class="card-title">Order complete</h4>
            </div>
            <div class="card-body">
                <p>Your transaction was successful. Thank you for your order!</p>
            </div>
        </div>

        <div class="m-4 card bg-danger text-white" id="cardError" runat="server">
            <div class="card-header">
                <h4 class="card-title">An error occurred</h4>
            </div>
            <div class="card-body">
                <p id="pError" runat="server"></p>
            </div>
        </div>

        <div class="m-4 card">
            <div class="card-header">
                <h4 class="card-title">Order Information</h4>
            </div>
            <div class="card-body">
                <table class="table">
                    <thead>
                        <tr>
                            <th>Item</th>
                            <th>Quantity</th>
                            <th>Price</th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td>T-Shirt</td>
                            <td>
                                <input type="number" id="inputTshirts" class="form-control" onchange="recalculateTotal()" runat="server">
                            </td>
                            <td>$15.00</td>
                        </tr>

                        <tr>
                            <td>Jeans</td>
                            <td>
                                <input type="number" id="inputJeans" class="form-control" onchange="recalculateTotal()" runat="server">
                            </td>
                            <td>$55.00</td>
                        </tr>

                        <tr>
                            <td>Hoodie</td>
                            <td>
                                <input type="number" id="inputHoodies" class="form-control" onchange="recalculateTotal()" runat="server">
                            </td>
                            <td>$70.00</td>
                        </tr>
                    </tbody>

                    <tfoot>
                        <tr>
                            <th>Total</th>
                            <th></th>
                            <th>
                                <p id="pTotal" runat="server">$0.00</p>
                            </th>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>

        <div class="m-4 card">
            <div class="card-header">
                <h4 class="card-title">Billing Information</h4>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-6">
                        <label for="inputFirstName" class="form-label">First name</label>
                        <input type="text" class="form-control" id="inputFirstName" placeholder="Jane" runat="server">
                    </div>
                    <div class="col-md-6">
                        <label for="inputLastName" class="form-label">Last name</label>
                        <input type="text" class="form-control" id="inputLastName" placeholder="Doe" runat="server">
                    </div>
                    <div class="col-12">
                        <label for="inputAddress" class="form-label">Address</label>
                        <input type="text" class="form-control" id="inputAddress" placeholder="1234 Main St" runat="server">
                    </div>
                    <div class="col-md-3">
                        <label for="inputCity" class="form-label">City</label>
                        <input type="text" class="form-control" id="inputCity" placeholder="Minneapolis" runat="server">
                    </div>
                    <div class="col-md-3">
                        <label for="inputState" class="form-label">State</label>
                        <input type="text" class="form-control" id="inputState" placeholder="Minnesota" runat="server">
                    </div>
                    <div class="col-md-3">
                        <label for="inputCountry" class="form-label">Country</label>
                        <input type="text" class="form-control" id="inputCountry" placeholder="United States" runat="server">
                    </div>
                    <div class="col-md-3">
                        <label for="inputZip" class="form-label">Zip</label>
                        <input type="text" class="form-control" id="inputZip" placeholder="12345" runat="server">
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function recalculateTotal() {
                var total = (parseFloat(document.getElementById("inputTshirts").value.replace("", "0")) * 15.00 + parseFloat(document.getElementById("inputJeans").value.replace("", "0")) * 55.00 + parseFloat(document.getElementById("inputHoodies").value.replace("", "0")) * 70.00).toFixed(2);
                document.getElementById("pTotal").innerHTML = `$${total}`;
                document.getElementById('<%= hfTotal.ClientID %>').value = total;
            }
        </script>

        <asp:button class="m-4 btn btn-primary btn-lg AcceptUI" id="btnPay" onclientclick="return false;" runat="server"
            data-billingaddressoptions='{"show":true, "required":false}'
            data-apiloginid=""
            data-clientkey=""
            data-acceptuiformbtntxt="Submit"
            data-acceptuiformheadertxt="Card Information"
            data-paymentoptions='{"showCreditCard":true,"showBankAccount":false}'
            data-responsehandler="responseHandler"
            text="Pay" />

        <script type="text/javascript">
            function responseHandler(response) {
                if (response.messages.resultCode === "Error") {
                    var i = 0;
                    while (i < response.messages.message.length) {
                        console.log(
                            response.messages.message[i].code + ": " +
                            response.messages.message[i].text
                        );
                        alert(response.messages.message[i].code + ": " +
                            response.messages.message[i].text);
                        i = i + 1;
                    }
                } else {
                    paymentFormUpdate(response.opaqueData);
                }
            }

            function paymentFormUpdate(opaqueData) {
                document.getElementById('<%= hfDataDescriptor.ClientID %>').value = opaqueData.dataDescriptor;
                document.getElementById('<%= hfDataValue.ClientID %>').value = opaqueData.dataValue;

                // If using your own form to collect the sensitive data from the customer,
                // blank out the fields before submitting them to your server.
                // document.getElementById("cardNumber").value = "";
                // document.getElementById("expMonth").value = "";
                // document.getElementById("expYear").value = "";
                // document.getElementById("cardCode").value = "";

                document.getElementById("form1").submit();
            }
        </script>

        <asp:button class="m-4 btn btn-success btn-lg" id="btnReload" postbackurl="~/AcceptjsDotnet.aspx?OrderComplete=False" runat="server" Text="Place a new order" />
    </form>
</body>

</html>