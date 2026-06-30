var IsDateFilter = false;
var FromDate = "", ToDate = "";
var IsParty = false;
var PartyList = [];
var FullPartyList;
var ProductWiseGrid = [];
var PreviouslySelectedFromPickerDate = "", type = "";
var PreviouslySelectedToPickerDate = "";
var grid, dialog;
var billno = true;
var billDate = true;
var idno = true;
var idname = true;
var order = true;

var cash = true;
var cheque = true;
var dd = true;
var bank = true;
var creditcard = true;
var debitcard = true;
var netbanking = true;
var credit = true;
var wallet = true;
var PVPurchaseWallet = true;
var BVPurchaseWallet = true;
var BillDtCaption = "Bill Date";

$(document).ready(function () {
    $(".preloader").hide();
    if ($("#GroupId").val() != 0) {
        $("#PartyName").prop("readonly", "readonly");
        $('#IsParty').attr('disabled', 'disabled');
    }
    else {
        $("#PartyName").prop("readonly", "");
        $('#IsParty').prop("readonly", "");
        $("#PartyName").val('All');
        $('#IsParty').removeAttr('disabled');
    }
    GetAllParty();

    $('#IsDateFilter').on('ifChecked', function () {
        IsDateFilter = true;
        FromDate = "";
        ToDate = "";
        $("#StartDate").val("All");
        $("#StartDate").prop("readonly", "readonly");
        $("#EndDate").val("All");
        $("#EndDate").prop("readonly", "readonly");
    });

    $('#IsDateFilter').on('ifUnchecked', function () {
        FromDate = "";
        ToDate = "";
        IsDateFilter = false;
        $("#StartDate").val("");
        $("#StartDate").prop("readonly", "");
        $("#EndDate").val("");
        $("#EndDate").prop("readonly", "");
    });

    $("#StartDate").datetimepicker({
        format: 'DD-MM-YYYY',
        widgetPositioning: {
            horizontal: 'auto',
            vertical: 'bottom'
        }

    }).on('dp.change', function (e) {
        var selectedDate = e.date._d;
        var lengthOfMonth = ((selectedDate.getMonth() + 1).toString()).length;
        var twoDigitMonth = (lengthOfMonth > 1) ? (selectedDate.getMonth() + 1) : '0' + (selectedDate.getMonth() + 1);
        var lengthOfDate = ((selectedDate.getDate()).toString()).length;
        var twoDigitdate = (lengthOfDate > 1) ? (selectedDate.getDate()) : '0' + (selectedDate.getDate());

        var newFromDate = twoDigitdate + "-" + twoDigitMonth + "-" + selectedDate.getFullYear();

        FromDate = newFromDate;
        PreviouslySelectedFromPickerDate = newFromDate;
        ToDate = PreviouslySelectedToPickerDate;
    });

    $("#EndDate").datetimepicker({
        format: 'DD-MM-YYYY',
        widgetPositioning: {
            horizontal: 'auto',
            vertical: 'bottom'
        }

    }).on('dp.change', function (e) {
        var selectedDate = e.date._d;
        var lengthOfMonth = ((selectedDate.getMonth() + 1).toString()).length;
        var twoDigitMonth = (lengthOfMonth > 1) ? (selectedDate.getMonth() + 1) : '0' + (selectedDate.getMonth() + 1);

        var lengthOfDate = ((selectedDate.getDate()).toString()).length;
        var twoDigitdate = (lengthOfDate > 1) ? (selectedDate.getDate()) : '0' + (selectedDate.getDate());

        var newFromDate = twoDigitdate + "-" + twoDigitMonth + "-" + selectedDate.getFullYear();

        ToDate = newFromDate;
        PreviouslySelectedToPickerDate = newFromDate;
        FromDate = PreviouslySelectedFromPickerDate;
    });

    $("#btnExport").on('click', function () {

        console.log("in export gridExport", ProductWiseGrid);
        var tableString = "<thead><tr>";
        tableString += "<th>S.No.</th>";
        if (billno == false) tableString += "<th>Bill No.</th>";
        if (billDate == false) tableString += "<th>" + BillDtCaption + "</th>";
        tableString += "<td>Bill By</td>";
        tableString += "<th>Name</th>";
        if (order == false) tableString += "<th>Order</th>";
        if (idno == false) tableString += "<th>ID No.</th>";
        if (idname == false) tableString += "<th>Name</th>";
        tableString += "<th>Gross sale</th>";
        tableString += "<th>FPV Sale</th>";
        tableString += "<th>Net Sale</th>";
        tableString += "<th>Excess +Tax Collected</th>";
        tableString += "<th>Cash in Hand</th>";
        if (cash == false) tableString += "<th>Cash</th>";
        if (!cheque) tableString += "<th>Cheque</th>";
        if (!dd) tableString += "<th>DD</th>";
        if (!bank) tableString += "<th>Bank Deposit</th>";
        if (!creditcard) tableString += "<th>Credit Card</th>";
        if (!debitcard) tableString += "<th>Dedit Card</th>";
        if (!netbanking) tableString += "<th>Net Banking</th>";
        if (!debitcard) tableString += "<th>Dedit Card</th>";
        if (!credit) tableString += "<th>Credit</th>";
        if (!wallet) tableString += "<th>Wallet</th>";
        tableString += "</tr></thead><tbody>";

        for (var i = 0; i < ProductWiseGrid.length; i++) {
            var sno = parseFloat(i) + 1;
            tableString += "<tr>";
            tableString += "<td>" + sno + "</td>";
            if (!billno) tableString += "<td>" + ProductWiseGrid[i].BillNo + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].BillDate + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].BillBy + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].Name + "</td>";
            if (!order) tableString += "<td>" + ProductWiseGrid[i].Order + "</td>";
            if (!idno) tableString += "<td>" + ProductWiseGrid[i].IDNo + "</td>";
            if (!idname) tableString += "<td>" + ProductWiseGrid[i].IdName + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].Amount + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].FPVAmt + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].NetAmt + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].ExcessAmt + "</td>";
            tableString += "<td>" + ProductWiseGrid[i].NetPayable + "</td>";
            if (!cash) tableString += "<td>" + ProductWiseGrid[i].Cash + "</td>";
            if (!cheque) tableString += "<td>" + ProductWiseGrid[i].Cheque + "</td>";
            if (!dd) tableString += "<td>" + ProductWiseGrid[i].dd + "</td>";
            if (!bank) tableString += "<td>" + ProductWiseGrid[i].BankDeposit + "</td>";
            if (!creditcard) tableString += "<td>" + ProductWiseGrid[i].CreditCard + "</td>";
            if (!debitcard) tableString += "<td>" + ProductWiseGrid[i].DeditCard + "</td>";
            if (!netbanking) tableString += "<td>" + ProductWiseGrid[i].NetBanking + "</td>";
            if (!credit) tableString += "<td>" + ProductWiseGrid[i].Credit + "</td>";
            if (!wallet) tableString += "<td>" + ProductWiseGrid[i].Wallet + "</td>";
            tableString += "</tr>";
        }

        tableString += "</tbody>";
        $("#gridExport").empty();
        $("#gridExport").append(tableString);
        tableToExcel('gridExport', "PaymentSummary");
    });


    $('#IsParty').on('ifChecked', function () {
        IsParty = true;
        $("#PartyName").val('All');
        $("#PartyName").prop("readonly", "readonly");
        $("#PartyCode").val('0');
    });

    $('#IsParty').on('ifUnchecked', function () {
        IsParty = false;
        $("#PartyName").val('');
        $("#PartyName").prop("readonly", "");
        $("#PartyCode").val('');
    });

    $("#PartyName").autocomplete({
        source: function (request, response) {
            var results = $.ui.autocomplete.filter(PartyList, request.term);
            response(results.slice(0, 50));

        },
        minLength: 1,
        scroll: true,
        select: function (event, ui) {
            $("#PartyName").val(ui.item.label);
            SetSpecificCode("Party", ui.item.label);
            return false;
        },
    }).focus(function () {
        $(this).autocomplete("search", "");
    });

    $(".getReport").on('click', function () {
        $(".preloader").show();
        type = $(this).attr("data-type");
        ProductWiseGrid = [];
        fillGrid(type);
        PartyCode = $("#PartyCode").val();

        var DateError = false;
        if (FromDate != "" && FromDate != null && ToDate != null && ToDate != null) {
            var d1 = toDate(FromDate);
            var d2 = toDate(ToDate);

            if (d1 > d2) {

                DateError = true;
                $(".preloader").hide();
                OpenDialog("dialogMessage", "From Date should be less than To Date!", "false");
            }
            else {
                DateError = false;

            }
        }
        else {
            DateError = false;
            if (DateError == false) {
                if (IsDateFilter == false) {
                }
            }
            if (FromDate == "" || FromDate == null) {
                FromDate = "All";
            }
            if (ToDate == "" || ToDate == null) {
                ToDate = "All";
            }

        }

        if (DateError == false) {
            var InvoiceTypeVal = "";
            var Totalcash = 0;
            var Totalwallet = 0;
            var TotalVoucher = 0;
            var TotalPVPurchaseWallet = 0;
            var TotalBVPurchaseWallet = 0;
            var TotalBankDeposit = 0;
            $.ajax({
                url: '/Report/GetPaymentSummaryReport',
                type: 'POST',
                data: { "FromDate": FromDate, "ToDate": ToDate, "PartyCode": PartyCode, "Type": type },
                async: false,
                dataType: "json",
                success: function (objResult) {
                    ProductWiseGrid = [];
                    if (objResult.length > 0) {
                        for (var i = 0; i < objResult.length; i++) {

                            ProductWiseGrid.push({
                                "BillDate": objResult[i].BillDate, "BillNo": objResult[i].BillNo,
                                "BillBy": objResult[i].BillBy, "Name": objResult[i].Name, "Order": objResult[i].Order,
                                "IDNo": objResult[i].IDNo, "IdName": objResult[i].IdName, "Amount": objResult[i].Amount,
                                "Cash": objResult[i].Cash, "Cheque": objResult[i].Cheque, "dd": objResult[i].dd,
                                "BankDeposit": objResult[i].BankDeposit, "CreditCard": objResult[i].CreditCard,
                                "Cheque": objResult[i].Cheque, "DeditCard": objResult[i].DeditCard,
                                "NetBanking": objResult[i].NetBanking, "Credit": objResult[i].Credit, "Wallet": objResult[i].Wallet,
                                "FPVAmt": objResult[i].FPVAmt, "NetAmt": objResult[i].NetAmt, "ExcessAmt": objResult[i].ExcessAmt,
                                "NetPayable": objResult[i].NetPayable, "PVPurchaseWallet": objResult[i].PVPurchaseWallet,
                                "BVPurchaseWallet": objResult[i].BVPurchaseWallet, "VoucherAmt": objResult[i].VoucherAmt
                            });
                            Totalcash = Totalcash + parseFloat(objResult[i].Cash);
                            Totalwallet = Totalwallet + parseFloat(objResult[i].Wallet);
                            TotalVoucher = TotalVoucher + parseFloat(objResult[i].VoucherAmt);
                            TotalPVPurchaseWallet = TotalPVPurchaseWallet + parseFloat(objResult[i].PVPurchaseWallet);
                            TotalBVPurchaseWallet = TotalBVPurchaseWallet + parseFloat(objResult[i].BVPurchaseWallet);
                            TotalBankDeposit = TotalBankDeposit + parseFloat(objResult[i].BankDeposit);
                        }
                        $("#Totalcash").text(Totalcash);
                        $("#Totalwallet").text(Totalwallet);
                        $("#TotalVoucher").text(TotalVoucher);
                        $("#TotalPVPurchaseWallet").text(TotalPVPurchaseWallet);
                        $("#TotalBVPurchaseWallet").text(TotalBVPurchaseWallet);
                        $("#TotalBankDeposit").text(TotalBankDeposit);
                    }
                    fillGrid(type);
                    $(".preloader").hide();
                },
                error: function (xhr, data) {
                    console.log(xhr);
                    console.log("Error:", data);
                }
            });
        }
    });

});

function GetAllParty() {
    $.ajax({
        url: '/Report/GetAllPartyListForReports',
        dataType: 'JSON',
        method: 'GET',
        success: function (data) {
            FullPartyList = data;
            PartyList = [];
            if (data != null) {
                var i = 0;
                for (i = 0; i < data.length; i++) {
                    PartyList.push(data[i].PartyName);
                }
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function toDate(dateStr) {
    var parts = dateStr.split("-");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function SetSpecificCode(type, label) {
    if (type == "Party") {
        for (var i = 0; i < FullPartyList.length; i++) {
            if (FullPartyList[i].PartyName == label) {
                $("#PartyCode").val(FullPartyList[i].PartyCode);
                PartyCode = FullPartyList[i].PartyCode;
                break;
            }
        }
    }
}

function OpenDialog(dialogId, Message, isConfirmation) {
    $("#" + dialogId).empty();
    $("#" + dialogId).append('<p>' + Message + '</p>');
    if (isConfirmation == "true") {
        $("#" + dialogId).dialog({
            modal: true,
            buttons: [{
                text: "Yes",
                id: "btnYes" + dialogId,
                click: function () {
                    IsYes = true;
                    $("#" + dialogId).dialog("close");
                }
            },
            {
                text: "No",
                id: "btnNo" + dialogId,
                click: function () {
                    IsYes = false;
                    $("#" + dialogId).dialog("close");
                }
            }
            ]
        });
        $(".ui-dialog-titlebar-close").empty();
        $(".ui-dialog-titlebar-close").append('<i class="fa fa-close"></i>');
    }
    else {


        $("#" + dialogId).dialog({
            modal: true,
            buttons: [{
                text: "OK",
                id: "btn" + dialogId,
                click: function () {
                    $("#" + dialogId).dialog("close");
                }
            }]
        });
        $(".ui-dialog-titlebar-close").empty();
        $(".ui-dialog-titlebar-close").append('<i class="fa fa-close"></i>');
    }

}

function fillGrid(type) {
    $("#noRecord").hide();
    if (grid != null) {
        grid.destroy();
        $('#grid').empty();

    }
    billno = true;
    billDate = true;
    idno = true;
    idname = true;
    order = true;

    cash = true;
    cheque = true;
    dd = true;
    bank = true;
    creditcard = true;
    debitcard = true;
    netbanking = true;
    credit = true;
    wallet = true;
    PVPurchaseWallet = true;
    BVPurchaseWallet = true;
    var Voucher = true;
    BillDtCaption = "Bill Date";
    $('input[name="paymode"]:checked').each(function (i) {
        var val = $(this).val();

        if (val == "Cash") {
            cash = false;
        }
        else if (val == "Cheque") {
            cheque = false;
        }
        else if (val == "DD") {
            dd = false;
        }
        else if (val == "Bank Deposit") {
            bank = false;
        }
        else if (val == "Credit Card") {
            creditcard = false;
        }
        else if (val == "Debit Card") {
            debitcard = false;
        }
        else if (val == "Net Banking") {
            netbanking = false;
        }
        else if (val == "Credit") {
            credit = false;
        }
        else if (val == "Wallet") {
            wallet = false;
        }
        else if (val == "PV Purchase Wallet") {
            PVPurchaseWallet = false;
        }
        else if (val == "BV Purchase Wallet") {
            BVPurchaseWallet = false;
        }
        else if (val == "Voucher") {
            Voucher = false;
        }
    });

    if (type == "B") {
        billno = false;
        billDate = false;
        idno = false;
        idname = false;
        order = false;
    }
    else if (type == "D") {
        billDate = false;
    }
    else if (type == "M") {
        billno = true;
        billDate = false;
        BillDtCaption = "Month";
        order = true;
        idno = true;
        idname = true;
    }

    grid = $('#grid').grid({
        dataSource: ProductWiseGrid,
        uiLibrary: 'bootstrap',
        headerFilter: true,
        columns: [
            { field: 'BillNo', title: 'BillNo', width: 100, sortable: true, hidden: billno, filterable: false },
            { field: 'BillDate', title: BillDtCaption, width: 150, sortable: true, hidden: billDate, filterable: false },
            { field: 'BillBy', title: 'Bill By', width: 80, sortable: true, hidden: false, filterable: false },
            { field: 'Name', title: 'Name', width: 200, sortable: true, hidden: false, filterable: false },
            { field: 'Order', title: 'Order', width: 120, sortable: true, hidden: order, filterable: false },
            { field: 'IDNo', title: 'ID No.', width: 80, sortable: true, hidden: idno, filterable: false },
            { field: 'IdName', title: 'Name', width: 150, sortable: true, hidden: idname, filterable: false },
            { field: 'Amount', title: 'Gross Sale', width: 80, sortable: true, hidden: true, filterable: false },

            { field: 'FPVAmt', title: 'FPV Sale', width: 80, sortable: true, hidden: true, filterable: false },
            { field: 'NetAmt', title: 'Net Sale', width: 80, sortable: true, hidden: true, filterable: false },
            { field: 'ExcessAmt', title: 'Excess +Tax Collected', width: 80, sortable: true, hidden: true, filterable: false },


            { field: 'Cash', title: 'via Cash', width: 80, sortable: true, hidden: cash, filterable: false },
            { field: 'Cheque', title: 'Cheque', width: 80, sortable: true, hidden: cheque, filterable: false },
            { field: 'dd', title: 'DD', width: 80, sortable: true, hidden: dd, filterable: false },
            { field: 'BankDeposit', title: 'Bank Deposit', width: 80, sortable: true, hidden: bank, filterable: false },
            { field: 'CreditCard', title: 'Credit Card', width: 80, sortable: true, hidden: creditcard, filterable: false },
            { field: 'DeditCard', title: 'Dedit Card', width: 80, sortable: true, hidden: debitcard, filterable: false },
            { field: 'NetBanking', title: 'Net Banking', width: 80, sortable: true, hidden: netbanking, filterable: false },
            { field: 'Credit', title: 'Credit', width: 80, sortable: true, hidden: credit, filterable: false },
            { field: 'Wallet', title: 'Wallet', width: 80, sortable: true, hidden: wallet, filterable: false },
            { field: 'VoucherAmt', title: 'Voucher', width: 80, sortable: true, hidden: Voucher, filterable: false },
            { field: 'PVPurchaseWallet', title: 'PV Purchase Wallet', width: 80, sortable: true, hidden: PVPurchaseWallet, filterable: false },
            { field: 'BVPurchaseWallet', title: 'BV Purchase Wallet', width: 80, sortable: true, hidden: BVPurchaseWallet, filterable: false },
            { field: 'NetPayable', title: 'Net Payable', width: 80, sortable: true, hidden: false, filterable: false }
        ],
        pager: { limit: 100, sizes: [15, 20, 35, 50, 65, 80, 95, 100] }
    });
}