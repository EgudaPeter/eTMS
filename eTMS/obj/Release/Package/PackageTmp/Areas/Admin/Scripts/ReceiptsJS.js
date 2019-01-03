var getID;
var paymentType;
var idForInsertingReceipt;
var selectedIDsReceiptHeader;
var selectedIDsReceiptDetail;
var stopSubmitBecauseReceiptValidationsFailed;
var receiptValidationFailedErrorMessage
var stopSubmitBecauseOfTransactionDateFlagged;
var transactionDateFlaggedErrorMessage;
var stopSubmitBecauseOfBTRNoFlagged;
var BTRNoFlaggedErrorMessage;
var submitType;

var stopSubmitBecauseOfEditedTransactionDateFlagged;
var editedTransactionDateFlaggedErrorMessage;
var stopSubmitBecauseOfEditedBTRNoFlagged;
var EditedBTRNoFlaggedErrorMessage;


function onOtherTypeChange(url) {
    var otherType = OtherTypes.GetValue();
    if (otherType == null) {
        return; //do nothing...'@Url.Action("GetGLActNo","Receipts")',
    } 
    $.ajax({
        url: url,
        type: 'Post',
        data: { _OtherType: otherType },
        success: function (data) {
            if (data.success == true) {
                var glactno = data.GLactNo;
                GLAct.SetEnabled(true);
                GLAct.SetText(glactno);
                Narration.SetText(data.narration);
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function onReceiptTypeChange() {
    var receiptType = ReceiptType.GetValue();
    var date;
    if (receiptType == null) {
        return; //do nothing...
    }
    switch (receiptType) {
        case "PD":
            $.ajax({
                url: '@Url.Action("GetDataIfSelectedReceiptTypeIsDepositOrOtherType", "Receipts")',
                type: 'Post',
                data: {},
                success: function (data) {
                    if (data.success == true) {
                        date = data.nextDueDate == null ? null : new Date(data.nextDueDate);
                        PolicyNo.SetSelectedIndex(-1);
                        PolicyNo.SetText(data.policyNo)
                        GrossPrem.SetText(data.regularPremium);
                        ProposalID.SetText(data.proposalID);
                        NextDueDate.SetDate(date);
                        CurrentStatusID.SetText(data.currentStatusID);
                        ReceivedFrom.SetText(data.receivedFrom);

                        PolicyNo.SetEnabled(false);
                        ProposalNo.SetEnabled(true);
                        ProposalNo.SetText();
                        ProposalNo.GetInputElement().readOnly = false;
                        OtherTypes.SetEnabled(false);
                        GLAct.SetEnabled(false);
                        OtherTypes.SetSelectedIndex(-1);
                        GLAct.SetText();

                        Narration.SetText("Premium Deposit");
                        return;
                    }
                    if (data.success == -1) {
                        swal({
                            title: 'Error!',
                            html: data.errorMessage,
                            type: 'error'
                        });
                        return;
                    }
                }
            });
            break;
        case "OT":
            $.ajax({
                url: '@Url.Action("GetDataIfSelectedReceiptTypeIsDepositOrOtherType", "Receipts")',
                type: 'Post',
                data: {},
                success: function (data) {
                    if (data.success == true) {
                        date = data.nextDueDate == null ? null : new Date(data.nextDueDate);
                        PolicyNo.SetSelectedIndex(-1);
                        PolicyNo.SetText(data.policyNo)
                        GrossPrem.SetText(data.regularPremium);
                        ProposalID.SetText(data.proposalID);
                        NextDueDate.SetDate(date);
                        CurrentStatusID.SetText(data.currentStatusID);
                        ReceivedFrom.SetText(data.receivedFrom);

                        OtherTypes.SetEnabled(true);
                        PolicyNo.SetEnabled(false);
                        ProposalNo.SetEnabled(true);
                        ProposalNo.SetText();
                        ProposalNo.GetInputElement().readOnly = false;

                        Narration.SetText();
                        return;
                    }
                    if (data.success == -1) {
                        swal({
                            title: 'Error!',
                            html: data.errorMessage,
                            type: 'error'
                        });
                        return;
                    }
                }
            });
            break;
        case "AC":
            PolicyNo.SetEnabled(true);
            PolicyNo.SetText();
            PolicyNo.SetSelectedIndex(-1);
            ProposalNo.SetEnabled(true);
            ProposalNo.GetInputElement().readOnly = true;

            ProposalNo.SetText();
            OtherTypes.SetSelectedIndex(-1);
            GLAct.SetText();

            GrossPrem.SetText();
            ProposalID.SetText();
            NextDueDate.SetDate(null);
            CurrentStatusID.SetText();
            ReceivedFrom.SetText();

            OtherTypes.SetEnabled(false);
            GLAct.SetEnabled(false);

            Narration.SetText("Additional Contribution");
            break;
        case "RP":
            PolicyNo.SetEnabled(true);
            PolicyNo.SetText();
            PolicyNo.SetSelectedIndex(-1);
            ProposalNo.SetEnabled(true);
            ProposalNo.GetInputElement().readOnly = true;

            ProposalNo.SetText();
            OtherTypes.SetSelectedIndex(-1);
            GLAct.SetText();

            GrossPrem.SetText();
            ProposalID.SetText();
            NextDueDate.SetDate(null);
            CurrentStatusID.SetText();
            ReceivedFrom.SetText();

            OtherTypes.SetEnabled(false);
            GLAct.SetEnabled(false);

            Narration.SetText("Premium Renuwal");
            break;
        case "LR":
            PolicyNo.SetEnabled(true);
            PolicyNo.SetText();
            PolicyNo.SetSelectedIndex(-1);
            ProposalNo.SetEnabled(true);
            ProposalNo.GetInputElement().readOnly = true;

            ProposalNo.SetText();
            OtherTypes.SetSelectedIndex(-1);
            GLAct.SetText();

            GrossPrem.SetText();
            ProposalID.SetText();
            NextDueDate.SetDate(null);
            CurrentStatusID.SetText();
            ReceivedFrom.SetText();

            OtherTypes.SetEnabled(false);
            GLAct.SetEnabled(false);

            Narration.SetText("Loan Repayment");
            break;
    }
}

function onPolicyNoChange() {
    var date;
    var policyNo = PolicyNo.GetValue();
    if (policyNo == null) {
        return; //do nothing...
    }
    $.ajax({
        url: '@Url.Action("GetDataOnPolicyNoChange", "Receipts")',
        type: 'Post',
        data: { _policyNo: policyNo },
        success: function (data) {
            if (data.success == true) {
                date = data.nextDueDate == null ? null : new Date(data.nextDueDate);
                GrossPrem.SetText(data.regularPremium);
                ProposalID.SetText(data.proposalID);
                NextDueDate.SetDate(date);
                CurrentStatusID.SetText(data.currentStatusID);
                ReceivedFrom.SetText(data.receivedFrom);
                ProposalNo.SetText(data.proposalNo);
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function validateBTRNo() {
    var btr = BTR.GetValue();
    if (btr == null) {
        return;
    }
    $.ajax({
        url: '@Url.Action("checkIfPJNoAlreadyExists","Receipts")',
        type: 'Post',
        data: { PJNo: btr },
        success: function (data) {
            if (data.success == true) {
                stopSubmitBecauseOfBTRNoFlagged = true;
                BTRNoFlaggedErrorMessage = data.infoMessage;
                swal({
                    title: 'Warning!',
                    html: BTRNoFlaggedErrorMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == false) {
                stopSubmitBecauseOfBTRNoFlagged = false;
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function validateEditedBTRNo() {
    var btr = E_BTR.GetValue();
    if (btr == null) {
        return;
    }
    $.ajax({
        url: '@Url.Action("checkIfPJNoAlreadyExists","Receipts")',
        type: 'Post',
        data: { PJNo: btr },
        success: function (data) {
            if (data.success == true) {
                stopSubmitBecauseOfEditedBTRNoFlagged = true;
                EditedBTRNoFlaggedErrorMessage = data.infoMessage;
                swal({
                    title: 'Warning!',
                    html: EditedBTRNoFlaggedErrorMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == false) {
                stopSubmitBecauseOfEditedBTRNoFlagged = false;
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function validateReceiptNoLength() {
    var receiptNo = ReceiptNo.GetValue();
    if (receiptNo == null) {
        return; //do nothing...
    }
    $.ajax({
        url: '@Url.Action("validateReceiptNoLength", "Receipts")',
        type: 'Post',
        data: { _receiptNo: receiptNo },
        success: function (data) {
            if (data.success == true) {
                stopSubmitBecauseReceiptValidationsFailed = false;
                return;
            }
            if (data.success == false) {
                stopSubmitBecauseReceiptValidationsFailed = true;
                receiptValidationFailedErrorMessage = data.infoMessage;
                swal({
                    title: 'Warning!',
                    html: data.infoMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function ValidateBatchTransactionDateAgainstPostPeriod() {
    var transactionDate = BatchTransactionDate.GetValue();
    var postPeriod = PostPeriod.GetValue();
    if (transactionDate == null) {
        return; // do nothing...
    }
    if (postPeriod == null) {
        return; // do nothing...
    }
    $.ajax({
        url: '@Url.Action("ValidateBatchTransactionDateAgainstPostPeriod", "Receipts")',
        type: 'Post',
        data: { TDate: transactionDate, pPeriod: postPeriod },
        success: function (data) {
            if (data.success == true) {
                stopSubmitBecauseOfTransactionDateFlagged = false;
                return
            }
            if (data.success == false) {
                transactionDateFlaggedErrorMessage = data.infoMessage;
                stopSubmitBecauseOfTransactionDateFlagged = true;
                swal({
                    title: 'Warning!',
                    html: transactionDateFlaggedErrorMessage,
                    type: 'warning'
                });
                return
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return
            }
        }
    });
}

function ValidateEditedBatchTransactionDateAgainstPostPeriod() {
    var transactionDate = E_BatchTransactionDate.GetValue();
    var postPeriod = E_PostPeriod.GetValue();
    if (transactionDate == null) {
        return; // do nothing...
    }
    if (postPeriod == null) {
        return; // do nothing...
    }
    $.ajax({
        url: '@Url.Action("ValidateBatchTransactionDateAgainstPostPeriod", "Receipts")',
        type: 'Post',
        data: { TDate: transactionDate, pPeriod: postPeriod },
        success: function (data) {
            if (data.success == true) {
                stopSubmitBecauseOfEditedTransactionDateFlagged = false;
                return
            }
            if (data.success == false) {
                editedTransactionDateFlaggedErrorMessage = data.infoMessage;
                stopSubmitBecauseOfEditedTransactionDateFlagged = true;
                swal({
                    title: 'Warning!',
                    html: editedTransactionDateFlaggedErrorMessage,
                    type: 'warning'
                });
                return
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return
            }
        }
    });
}

function confirmRequiredFieldsForReceiptHaveValues() {
    var policyNo = PolicyNo.GetValue();
    var otherTypes = OtherTypes.GetValue();
    var proposalNo = ProposalNo.GetValue();
    var chequeAmount = ChequeAmount.GetValue();
    var cashAmount = CashAmount.GetValue();
    var reference = Reference.GetValue();
    var receiptNo = ReceiptNo.GetValue();
    var valueDate = ValueDate.GetDate();
    var receiptType = ReceiptType.GetValue();
    var totalReceipt = TotalReceipt.GetValue();

    if (receiptType == null) {
        return false;
    }

    if (receiptType == "PD") {
        if (stopSubmitBecauseReceiptValidationsFailed == true) {
            swal({
                title: 'Warning!',
                html: receiptValidationFailedErrorMessage,
                type: 'warning'
            });
            return;
        }
        else {
            if (proposalNo != null && receiptNo != null && valueDate != null
                && cashAmount != null && chequeAmount == null && reference == null) {
                return true;
            }
            if (proposalNo != null && receiptNo != null && valueDate != null
                && cashAmount == null && chequeAmount != null && reference != null) {
                return true;
            }
            else {
                submitType = "PD"
                return false;
            }
        }
    }

    if (receiptType == "OT") {
        if (stopSubmitBecauseReceiptValidationsFailed == true) {
            swal({
                title: 'Warning!',
                html: receiptValidationFailedErrorMessage,
                type: 'warning'
            });
            return;
        }
        else {
            if (otherTypes != null && proposalNo != null && receiptNo != null && cashAmount != null
                && chequeAmount == null && reference == null && valueDate != null) {
                return true;
            }
            if (otherTypes != null && proposalNo != null && receiptNo != null && cashAmount == null
                && chequeAmount != null && reference != null && valueDate != null) {
                return true;
            }
            else {
                submitType = "OT";
                return false;
            }
        }
    }

    if (receiptType != "OT" && receiptType != "PD") {
        if (stopSubmitBecauseReceiptValidationsFailed == true) {
            swal({
                title: 'Warning!',
                html: receiptValidationFailedErrorMessage,
                type: 'warning'
            });
            return;
        }
        else {
            if (policyNo != null && proposalNo != null && receiptNo != null && cashAmount != null
                && chequeAmount == null && reference == null && valueDate != null) {
                return true;
            }
            if (policyNo != null && proposalNo != null && receiptNo != null && cashAmount == null
                && chequeAmount != null && reference != null && valueDate != null) {
                return true;
            }
            else {
                submitType = "Other";
                return false;
            }
        }
    }

}

function confirmRequiredFieldsForBatchHaveValues() {
    var headerID = HeaderID.GetValue();
    var market = Market.GetValue();
    var agencyCode = AgencyCode.GetValue();
    var postPeriod = PostPeriod.GetValue();
    var btr = BTR.GetValue();
    var receiptDate = ReceiptDate.GetDate();
    var noOfReceipts = NoOfReceipts.GetValue();
    var grossAmount = GrossAmount.GetValue();
    var batchTransactionDate = BatchTransactionDate.GetDate();
    var idBankAct = ID_BankAccount.GetValue();
    var adminFee = AdminFee.GetValue();
    var amountInBank = AmountInBank.GetValue();
    var currency = Currency.GetValue();
    var batchType = BatchType.GetValue();
    if (headerID != null && market != null && agencyCode != null && postPeriod != null && btr != null && receiptDate != null
        && noOfReceipts != null && grossAmount != null && batchTransactionDate != null && idBankAct != null
        && currency != null && batchType != null) {
        if (stopSubmitBecauseOfTransactionDateFlagged == true) {
            return -1;
        }
        if (stopSubmitBecauseOfBTRNoFlagged == true) {
            return -2;
        }
        return true;
    }
    else {
        return false;
    }
}

function confirmRequiredFieldsForEditedBatchHaveValues() {
    var headerID = E_HeaderID.GetValue();
    var market = E_Market.GetValue();
    var agencyCode = E_AgencyCode.GetValue();
    var postPeriod = E_PostPeriod.GetValue();
    var btr = E_BTR.GetValue();
    var receiptDate = E_ReceiptDate.GetDate();
    var noOfReceipts = E_NoOfReceipts.GetValue();
    var grossAmount = E_GrossAmount.GetValue();
    var batchTransactionDate = E_BatchTransactionDate.GetDate();
    var idBankAct = E_ID_BankAccount.GetValue();
    var adminFee = E_AdminFee.GetValue();
    var amountInBank = E_AmountInBank.GetValue();
    var currency = E_Currency.GetValue();
    var batchType = E_BatchType.GetValue();
    if (headerID != null && market != null && agencyCode != null && postPeriod != null && btr != null && receiptDate != null
        && noOfReceipts != null && grossAmount != null && batchTransactionDate != null && idBankAct != null
        && currency != null && batchType != null) {
        if (stopSubmitBecauseOfEditedTransactionDateFlagged == true) {
            return -1;
        }
        if (stopSubmitBecauseOfEditedBTRNoFlagged == true) {
            return -2;
        }
        return true;
    }
    else {
        return false;
    }
}

function calculateTotalReceipts() {
    var chequeAmount = ChequeAmount.GetValue();
    var cashAmout = CashAmount.GetValue();

    if (chequeAmount == null && cashAmout == null) {
        TotalReceipt.SetText();
        paymentType = "";
        return;
    }
    if (chequeAmount == null && cashAmout != null) {
        var totalReceipt = parseFloat(cashAmout) + 0;
        TotalReceipt.SetText(totalReceipt);
        paymentType = "CHS";
        return;
    }
    if (chequeAmount != null && cashAmout == null) {
        var totalReceipt = parseFloat(chequeAmount) + 0;
        TotalReceipt.SetText(totalReceipt);
        paymentType = "CHQ"
        return;
    }
}

function calculateTotalAmountInBank() {
    var grossAmount = GrossAmount.GetValue();
    var adminFee = AdminFee.GetValue();
    if (grossAmount == null) {
        return;
    }
    if (adminFee == null) {
        adminFee = 0;
    }
    var amountInBank = grossAmount - adminFee;
    AmountInBank.SetText(amountInBank);
}

function calculateTotalAmountInBankEditedBatch() {
    var grossAmount = E_GrossAmount.GetValue();
    var adminFee = E_AdminFee.GetValue();
    if (grossAmount == null) {
        return;
    }
    if (adminFee == null) {
        adminFee = 0;
    }
    var amountInBank = grossAmount - adminFee;
    E_AmountInBank.SetText(amountInBank);
}

function OnSelectionChangedReceiptHeader(s, e) {
    s.GetSelectedFieldValues("RcpHeaderID", GetSelectedFieldValuesCallbackReceiptHeader);
    $("#options").attr("disabled", false);
    $("#options").removeClass("btnDisabled");
}

function OnSelectionChangedReceiptDetail(s, e) {
    s.GetSelectedFieldValues("ReceiptID", GetSelectedFieldValuesCallbackReceiptDetail);
    $("#getApproval").attr("disabled", false);
    $("#getApproval").removeClass("btnDisabled");
}

function GetSelectedFieldValuesCallbackReceiptHeader(values) {
    if (values.length == 0) {
        $("#options").attr("disabled", true);
        $("#options").val("Select Action");
        $("#options").addClass("btnDisabled");
    }
    selectedIDsReceiptHeader = values.join(',');
    $("#recordCountHeader").text(ReceiptsGridView.GetSelectedRowCount());
}

function GetSelectedFieldValuesCallbackReceiptDetail(values) {
    if (values.length == 0) {
        $("#getApproval").attr("disabled", true);
        $("#getApproval").addClass("btnDisabled");
    }
    selectedIDsReceiptDetail = values.join(',');
    //$("#recordCountDetail").text(ReceiptsDetailsGridView+count.GetSelectedRowCount());
}

function InsertNewBatch() {
    HeaderID.SetValue();
    Market.SetSelectedIndex(-1);
    AgencyCode.SetSelectedIndex(-1);
    PostPeriod.SetText();
    BTR.SetText();
    ReceiptDate.SetDate();
    NoOfReceipts.SetValue();
    GrossAmount.SetValue();
    BatchTransactionDate.SetDate();
    ID_BankAccount.SetSelectedIndex(-1);
    AdminFee.SetValue(null);
    AmountInBank.SetText();
    Currency.SetSelectedIndex(-1);
    BatchType.SetSelectedItem(null);

    $('#addBatch').modal({ backdrop: 'static', keyboard: false }); //disable backdrop
    $('#addBatch').modal("show");
}

function promptUserBeforeSavingBatch() {
    swal({
        title: 'Confirm?',
        html: 'Save record?',
        type: 'info',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Save',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        var result = confirmRequiredFieldsForBatchHaveValues();
        if (result == true) {
            $('#addBatch').modal("hide");
            saveBatch();
            return;
        }
        if (result == false) {
            var infoMsg = "Please define required fields<b>[ " +
                            "Header ID, " +
                            "Market Code, " +
                            "Agency, " +
                            "Post Period, " +
                            "BTR/PJ No " +
                            "Receipt Date, " +
                            "No of Receipts, " +
                            "Gross Amount, " +
                            "Transaction Date, " +
                            "Bank Account, " +
                            "Currency, " +
                            "Batch Type " +
                            "]</b> before progressing to save batch. Thank you.";
            swal({
                title: 'Warning!',
                html: infoMsg,
                type: 'warning'
            });
            return;
        }
        if (result == -1) {
            swal({
                title: 'Warning!',
                html: transactionDateFlaggedErrorMessage,
                type: 'warning'
            });
            return;
        }
        if (result == -2) {
            swal({
                title: 'Warning!',
                html: BTRNoFlaggedErrorMessage,
                type: 'warning'
            });
            return;
        }
    });
}

function promptUserBeforeUpdatingBatch() {
    swal({
        title: 'Confirm?',
        html: 'Update record?',
        type: 'info',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Update',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        var result = confirmRequiredFieldsForEditedBatchHaveValues();
        if (result == true) {
            $('#editBatch').modal("hide");
            updateBatch();
            return;
        }
        if (result == false) {
            var infoMsg = "Please define required fields<b>[ " +
                            "Header ID, " +
                            "Market Code, " +
                            "Agency, " +
                            "Post Period, " +
                            "BTR/PJ No " +
                            "Receipt Date, " +
                            "No of Receipts, " +
                            "Gross Amount, " +
                            "Transaction Date, " +
                            "Bank Account, " +
                            "Currency, " +
                            "Batch Type " +
                            "]</b> before progressing to save batch. Thank you.";
            swal({
                title: 'Warning!',
                html: infoMsg,
                type: 'warning'
            });
            return;
        }
        if (result == -1) {
            swal({
                title: 'Warning!',
                html: editedTransactionDateFlaggedErrorMessage,
                type: 'warning'
            });
            return;
        }
        if (result == -2) {
            swal({
                title: 'Warning!',
                html: EditedBTRNoFlaggedErrorMessage,
                type: 'warning'
            });
            return;
        }
    });
}

function saveBatch() {
    SiteUtils.loading("Saving...");
    var headerID = HeaderID.GetValue();
    var market = Market.GetValue();
    var agencyCode = AgencyCode.GetValue();
    var postPeriod = PostPeriod.GetValue();
    var btr = BTR.GetValue();
    var receiptDate = ReceiptDate.GetDate();
    var noOfReceipts = NoOfReceipts.GetValue();
    var grossAmount = GrossAmount.GetValue();
    var batchTransactionDate = BatchTransactionDate.GetDate();
    var idBankAct = ID_BankAccount.GetValue();
    var adminFee = AdminFee.GetValue();
    var amountInBank = AmountInBank.GetValue();
    var currency = Currency.GetValue();
    var batchType = BatchType.GetValue();
    var dataArray = {
        RcpHeaderID: headerID,
        MarketCode: market,
        AgencyID: agencyCode,
        PostPeriod: postPeriod,
        BTRNo: btr,
        ReceiptDateString: receiptDate,
        NoOfReceipt: noOfReceipts,
        GrossAmount: grossAmount,
        TransactionDateString: batchTransactionDate,
        ID_BankAccount: idBankAct,
        AdminFee: adminFee,
        TransferAmount: amountInBank,
        ID_Currency: currency,
        BatchType: batchType,
    };
    $.ajax({
        url: '@Url.Action("AddBatch", "Receipts")',
        type: 'Post',
        data: { Data: dataArray },
        success: function (data) {
            if (data.success == true) {
                SiteUtils.loadingOff();
                ReceiptsGridView.PerformCallback();
                swal({
                    title: 'Information!',
                    html: "Batch successfull created!",
                    type: 'info'
                });
                return;
            }
            if (data.success == false) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Warning!',
                    html: data.infoMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == -1) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return
            }
        }
    });
}

function updateBatch() {
    SiteUtils.loading("Updating...");
    var headerID = E_HeaderID.GetValue();
    var market = E_Market.GetValue();
    var agencyCode = E_AgencyCode.GetValue();
    var postPeriod = E_PostPeriod.GetValue();
    var btr = E_BTR.GetValue();
    var receiptDate = E_ReceiptDate.GetDate();
    var noOfReceipts = E_NoOfReceipts.GetValue();
    var grossAmount = E_GrossAmount.GetValue();
    var batchTransactionDate = E_BatchTransactionDate.GetDate();
    var idBankAct = E_ID_BankAccount.GetValue();
    var adminFee = E_AdminFee.GetValue();
    var amountInBank = E_AmountInBank.GetValue();
    var currency = E_Currency.GetValue();
    var batchType = E_BatchType.GetValue();
    var dataArray = {
        RcpHeaderID: headerID,
        MarketCode: market,
        AgencyID: agencyCode,
        PostPeriod: postPeriod,
        BTRNo: btr,
        ReceiptDateString: receiptDate,
        NoOfReceipt: noOfReceipts,
        GrossAmount: grossAmount,
        TransactionDateString: batchTransactionDate,
        ID_BankAccount: idBankAct,
        AdminFee: adminFee,
        TransferAmount: amountInBank,
        ID_Currency: currency,
        BatchType: batchType,
    };
    $.ajax({
        url: '@Url.Action("UpdateBatch", "Receipts")',
        type: 'Post',
        data: { Data: dataArray },
        success: function (data) {
            if (data.success == true) {
                SiteUtils.loadingOff();
                ReceiptsGridView.PerformCallback();
                swal({
                    title: 'Information!',
                    html: "Batch successful updated!",
                    type: 'info'
                });
                return;
            }
            if (data.success == false) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Warning!',
                    html: data.infoMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == -1) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return
            }
        }
    });
}


function promptUserBeforeClosingBatchModal() {
    swal({
        title: 'Are you sure?',
        html: 'Inputs will be lost',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Okay',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        $('#addBatch').modal("hide");
    });
}

function promptUserBeforeClosingEditedBatchModal() {
    swal({
        title: 'Are you sure?',
        html: 'Inputs will be lost',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Okay',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        $('#editBatch').modal("hide");
    });
}

function DisplayReceiptForm() {
    ReceiptType.SetSelectedItem(null);
    OtherTypes.SetSelectedIndex(-1);
    GLAct.SetText();
    PolicyNo.SetSelectedIndex(-1);
    PolicyNo.SetEnabled(false);
    ProposalNo.SetText();
    ProposalNo.GetInputElement().readOnly = true;
    ProposalNo.SetEnabled(false);

    NextDueDate.SetDate();
    ProposalID.SetText();
    GrossPrem.SetValue();
    CurrentStatusID.SetText();

    ReceivedFrom.SetText();
    ValueDate.SetDate();
    ChequeAmount.SetValue();
    CashAmount.SetValue();
    Charges.SetValue();
    ReceiptNo.SetValue();
    Reference.SetText();
    Narration.SetText();
    TotalReceipt.SetText();
    OtherTypes.SetEnabled(false);
    GLAct.SetEnabled(false);

    $('#addReceipt').modal({ backdrop: 'static', keyboard: false }); //disable backdrop
    $('#addReceipt').modal("show");
}

function InsertNewReceipt(id) {
    $.ajax({
        url: '@Url.Action("CheckIfNumberOfReceiptsForBatchCorrelatesWithNumberOfReceiptsAvailable", "Receipts")',
        type: 'Post',
        data: { batchID: id },
        success: function (data) {
            if (data.success == true) {
                idForInsertingReceipt = id;
                return DisplayReceiptForm();
            }
            if (data.success == false) {
                swal({
                    title: 'Warning!',
                    html: "You cannot add receipts to this batch!",
                    type: 'warning'
                });
                return
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return
            }
        }
    });
}

function promptUserBeforeSavingReceipt() {
    swal({
        title: 'Confirm?',
        html: 'Save record?',
        type: 'info',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Save',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        var result = confirmRequiredFieldsForReceiptHaveValues();
        if (result == true) {
            $('#addReceipt').modal("hide");
            saveReceipt(idForInsertingReceipt);
        }
        else {
            switch (submitType) {
                case "PD":
                    var infoMsg = "Please define required fields<b>[ " +
                     "Proposal No, " +
                     "Value Date, " +
                     "Receipt No, " +
                     "Cash Amount or Cheque Amount(with Chq No)" +
                     "]</b> before progressing to save receipt. Thank you.";
                    swal({
                        title: 'Warning!',
                        html: infoMsg,
                        type: 'warning'
                    });
                    break;
                case "OT":
                    var infoMsg = "Please define required fields<b>[ " +
                     "Other Types, " +
                     "Proposal No, " +
                     "Value Date, " +
                     "Receipt No, " +
                     "Cash Amount or Cheque Amount(with Chq No)" +
                     "]</b> before progressing to save receipt. Thank you.";
                    swal({
                        title: 'Warning!',
                        html: infoMsg,
                        type: 'warning'
                    });
                    break;
                case "Other":
                    var infoMsg = "Please define required fields<b>[ " +
                     "Policy No, " +
                     "Proposal No, " +
                     "Value Date, " +
                     "Receipt No, " +
                     "Cash Amount or Cheque Amount(with Chq No)" +
                     "]</b> before progressing to save receipt. Thank you.";
                    swal({
                        title: 'Warning!',
                        html: infoMsg,
                        type: 'warning'
                    });
                    break;
            }
        }
    });
}

function saveReceipt(id) {
    SiteUtils.loading("Please wait...");
    var headerID = id;
    var receiptType = ReceiptType.GetValue();
    var policyNo = receiptType == "PD" || receiptType == "OT" ? "DEPOSIT" : PolicyNo.GetValue();
    var otherTypes = OtherTypes.GetValue();
    var glact = GLAct.GetValue();
    var proposalNo = ProposalNo.GetValue();
    var nextDueDate = NextDueDate.GetDate();
    var proposalId = ProposalID.GetValue();
    var grossPrem = GrossPrem.GetValue();
    var currentStatusID = CurrentStatusID.GetValue();
    var receivedFrom = ReceivedFrom.GetValue();
    var valueDate = ValueDate.GetDate();
    var chequeAmount = ChequeAmount.GetValue();
    var cashAmount = CashAmount.GetValue();
    var charges = Charges.GetValue();
    var receiptNo = ReceiptNo.GetValue();
    var reference = Reference.GetValue();
    var narration = Narration.GetValue();
    var totalReceipt = TotalReceipt.GetValue();
    var dataArray = {
        HeaderID: headerID,
        ReceiptTypeID: receiptType,
        PolicyNo: policyNo,
        OtherTypes: otherTypes,
        PaymentType: paymentType,
        GLActNO: glact,
        ProposalNo: proposalNo,
        NextDueDateString: nextDueDate,
        ProposalId: proposalId,
        GrossPrem: grossPrem,
        CurrentStatusID: currentStatusID,
        ReceivedFrom: receivedFrom,
        ValueDateString: valueDate,
        ChequeAmount: chequeAmount,
        CashAmount: cashAmount,
        BankCharges: charges,
        ReceiptNo: receiptNo,
        Reference: reference,
        Narration: narration,
        TotalReceipt: totalReceipt
    };
    $.ajax({
        url: '@Url.Action("addReceipt", "Receipts")',
        type: 'Post',
        data: { Data: dataArray },
        success: function (data) {
            if (data.success == true) {
                SiteUtils.loadingOff();
                ReceiptsGridView.PerformCallback();
                //ReceiptDetailsGridView.PerformCallback();
                swal({
                    title: 'Information!',
                    html: "Operation successful",
                    type: 'info'
                });
                return;
            }
            if (data.success == false) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Warning!',
                    html: data.infoMessage,
                    type: 'warning'
                });
                return;
            }
            if (data.success == -1) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function promptUserBeforeClosingReceiptModal() {
    swal({
        title: 'Are you sure?',
        html: 'Inputs will be lost',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-info',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Okay',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        $('#addReceipt').modal("hide");
    });
}


function checkIfBatchIsEditable(id) {
    $.ajax({
        url: '@Url.Action("checkStatusOfBatch","Receipts")',
        type: 'Post',
        data: { batchID: id },
        success: function (data) {
            if (data.success == true) {
                return EditBatch(id);
            }
            if (data.success == false) {
                swal({
                    title: 'Warning!',
                    html: "You cannot edit this batch because it has been sent for verification.",
                    type: 'warning'
                });
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function EditBatch(id) {
    var rDate; //ReceiptDate
    var tDate; //TransactionDate
    $.ajax({
        url: '@Url.Action("EditBatch","Receipts")',
        type: 'Post',
        data: { batchID: id },
        success: function (data) {
            if (data.success == true) {
                $('#editBatch').modal({ backdrop: 'static', keyboard: false }); //disable backdrop
                $('#editBatch').modal("show");
                rDate = data.receiptDate == null ? null : new Date(data.receiptDate);
                tDate = data.transactionDate == null ? null : new Date(data.transactionDate);
                E_HeaderID.SetValue(data.headerID),
                E_Market.SetText(data.marketCode);
                E_AgencyCode.SetText(data.agencyID);
                E_PostPeriod.SetText(data.postPeriod);
                E_BTR.SetText(data.btr);
                E_ReceiptDate.SetDate(rDate);
                E_NoOfReceipts.SetText(data.noOfReceipts);
                E_GrossAmount.SetValue(data.grossAmount);
                E_BatchTransactionDate.SetDate(tDate);
                E_ID_BankAccount.SetText(data.bankAct);
                E_AdminFee.SetValue(data.adminFee);
                E_AmountInBank.SetText(data.amountInBank);
                E_Currency.SetText(data.currency);
                E_BatchType.SetValue(data.batchType);
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function EditReceipt(id) {
    var nDate; //NextPremDate
    var vDate; //ValueDate
    $.ajax({
        url: '@Url.Action("EditReceipt","Receipts")',
        type: 'Post',
        data: { receiptID: id },
        success: function (data) {
            if (data.success == true) {
                ReceiptType.SetValue();
                PolicyNo.SetValue();
                OtherTypes.SetText();
                GLAct.SetText();
                ProposalNo.SetText();
                NextDueDate.SetDate();
                ProposalID.SetValue();
                GrossPrem.SetValue();
                CurrentStatusID.SetText();
                ReceivedFrom.SetText();
                ValueDate.SetDate();
                ChequeAmount.SetValue();
                CashAmount.SetValue();
                Charges.SetValue();
                ReceiptNo.SetValue();
                Reference.SetText();
                Narration.SetText();
                TotalReceipt.SetValue();
            }
            if (data.sucess == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function checkIfBatchIsRemoveable(id) {
    $.ajax({
        url: '@Url.Action("checkStatusOfBatch","Receipts")',
        type: 'Post',
        data: { batchID: id },
        success: function (data) {
            if (data.success == true) {
                return DeleteSingleBatch(id);
            }
            if (data.success == false) {
                swal({
                    title: 'Warning!',
                    html: "You cannot delete this batch because it has been sent for verification.",
                    type: 'warning'
                });
                ReceiptsGridView.UnselectRows();
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function checkIfSelectedBatchesAreRemoveable() {
    $.ajax({
        url: '@Url.Action("checkStatusOfBatches", "Receipts")',
        type: 'Post',
        data: { batchIDs: selectedIDsReceiptHeader },
        success: function (data) {
            if (data.success == true) {
                return DeleteMultipleBatches();
            }
            if (data.success == false) {
                swal({
                    title: 'Warning!',
                    html: "You cannot delete selected batches because some have been sent for verification.",
                    type: 'warning'
                });
                ReceiptsGridView.UnselectRows();
                return;
            }
            if (data.success == -1) {
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

function DeleteSingleBatch(id) {
    swal({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-danger',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Delete',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        SiteUtils.loading("Deleting...");
        $.ajax({
            url: '@Url.Action("DeleteSingleBatch","Receipts")',
            type: 'Post',
            data: { selectedKey: id },
            success: function (data) {
                if (data.success == true) {
                    SiteUtils.loadingOff();
                    swal({
                        title: 'Information!',
                        html: data.infoMessage,
                        type: 'info'
                    });
                    ReceiptsGridView.UnselectRows()
                    ReceiptsGridView.Refresh();
                    return;
                }
                if (data.success == -1) {
                    SiteUtils.loadingOff();
                    swal({
                        title: 'Error!',
                        html: data.errorMessage,
                        type: 'error'
                    });
                    return;
                }
            }
        });
    });
}

function DeleteMultipleBatches() {
    swal({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-danger',
        cancelButtonClass: 'btn btn-primary',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> Delete',
        cancelButtonText: '<i class="fa fa-thumbs-down"> Cancel</i>'
    }).then(function () {
        SiteUtils.loading("Deleting...");
        $.ajax({
            url: '@Url.Action("DeleteMultipleBatches","Receipts")',
            type: 'Post',
            data: { selectedKeys: selectedIDsReceiptHeader },
            success: function (data) {
                if (data.success == true) {
                    SiteUtils.loadingOff();
                    swal({
                        title: 'Information!',
                        html: data.infoMessage,
                        type: 'info'
                    });
                    ReceiptsGridView.UnselectRows()
                    ReceiptsGridView.Refresh();
                    return;
                }
                if (data.success == -1) {
                    SiteUtils.loadingOff();
                    swal({
                        title: 'Error!',
                        html: data.errorMessage,
                        type: 'error'
                    });
                    return;
                }
            }
        });
    });
}

function SendBatchesForApproval() {
    SiteUtils.loading("Sending...");
    $.ajax({
        url: '@Url.Action("GetApproval","Receipts")',
        type: 'Post',
        data: { selectedKeys: selectedIDsReceiptHeader },
        success: function (data) {
            if (data.success == true) {
                SiteUtils.loadingOff();
                ReceiptsGridView.PerformCallback();
                ReceiptsGridView.UnselectRows();
                swal({
                    title: 'Information!',
                    html: data.infoMessage,
                    type: 'info'
                });
                return;
            }
            if (data.sucess == -1) {
                SiteUtils.loadingOff();
                swal({
                    title: 'Error!',
                    html: data.errorMessage,
                    type: 'error'
                });
                return;
            }
        }
    });
}

$(document).ready(function () {
    /*on portfolio change perform a callback*/
    $("#status").change(function () {
        ReceiptsGridView.PerformCallback({ status: $("#status").val() });
    });
    $("#options").change(function () {
        var option = $("#options").val();
        if (option == "Delete") {
            return checkIfSelectedBatchesAreRemoveable();
        }
        if (option == "Get Approval") {
            return SendBatchesForApproval();
        }
        return;
    });

    $("#recordCountHeader").text(ReceiptsGridView.GetSelectedRowCount());
    //$("#recordCountDetail").text(ReceiptsDetailsGridView.GetSelectedRowCount());
});

function OnPopupMenuItemClick(s, e) {
    var itemName = e.item.name;
    if (itemName == "Insert_H") {
        return InsertNewReceipt(getID);
    }
    if (itemName == "Edit_H") {
        return checkIfBatchIsEditable(getID);
    }
    if (itemName == "Remove_H") {
        return checkIfBatchIsRemoveable(getID);
    }
    if (itemName == "View_D") {

    }
    if (itemName == "Edit_D") {

    }
    if (itemName == "Print_D") {

    }
}

$(document).on("click", "#ReceiptsGridView tbody tr #dropup", function () {
    getID = $(this).attr('id-slug');
});

$(document).on("click", "#ReceiptsDetailsGridView tbody tr #dropup", function () {
    getID = $(this).attr('id-slug');
});