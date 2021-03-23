import { ShowModal } from './addInvoiceRow.js';

let invoiceEndEl = document.querySelector("#invoiceEnd");
let finish = document.querySelector("#finish");

document.getElementById("yes").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode;
    parent.nextSibling.nextSibling.style.display = "block";
    //parent.style.display = "none";
})

document.querySelector("#docCheck a").addEventListener("click", (ev) => CheckDoc(ev.target.id))

document.querySelector("#addRow a").addEventListener("click", (ev) => ShowModal(ev.target.dataset.companyid, ev.target.dataset.title))

document.getElementById("confirmBtn").addEventListener("click", () => HideCurrentDocumentation());

document.getElementById("no").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode.parentNode;
    ShowInvoiceEnd();
    parent.style.display = "none";
})
function ShowInvoiceEnd() {
    let subTotal = [...document.getElementsByClassName("price")].reduce((sum, e) => { return sum + parseFloat(e.textContent.trim()) }, 0);
    invoiceEndEl.querySelector("#subTotal").textContent = subTotal.toFixed(2);
    let vat = subTotal * 0.2;
    invoiceEndEl.querySelector("#vat").textContent = vat.toFixed(2);
    let total = subTotal + vat;
    invoiceEndEl.querySelector("#total").textContent = total.toFixed(2);
    invoiceEndEl.style.display = "";
}

document.getElementById("showFinish").addEventListener("click", (ev) => {
    console.log(ev.target.parentNode);
    ShowFinish();
    ev.target.parentNode.style.display = "none";
})
function ShowFinish() {
    if (document.querySelectorAll(".table-danger").length > 0) {
        let finishBtn = finish.querySelector("#finishBtn");
        finishBtn.classList.remove("btn-success");
        finishBtn.classList.add("btn-warning");
        finishBtn.value = "Непълна документация";
    }
    finish.style.display = "";
}

function CheckDoc(id) {
    let reqDoc = $('#doc_' + id + ' #reqDoc label').map(function () { return this.textContent });
    let courseRow = document.getElementById(`row_${id}`);
    if ($('#doc_' + id + ' :checkbox:checked').length != reqDoc.length) {
        $('#modal').modal('show');
        courseRow.classList.add("table-danger");
        return;
    }
    let isValid = true
    $('#doc_' + id + ' :checkbox:checked').map(function () { return this.nextSibling }).each(function () {
        if (jQuery.inArray(this.textContent, reqDoc) == -1) {
            $('#modal').modal('show');
            courseRow.classList.add("table-danger");
            isValid = false;
            return;
        }
    })
    if (isValid) {
        courseRow.classList.remove("table-danger");
        courseRow.classList.add("table-success");
        HideCurrentDocumentation();
    }
    //let form = document.getElementById("documentation_form");
    //form.action = "/Orders/FinishOrder";
    //form.submit();
}
function HideCurrentDocumentation() {
    [...document.querySelector("#docs").children].forEach((e) => e.style.display = "none");
    document.getElementById("invoiceAddRow").style.display = "block";
    $('#modal').modal('hide');
}

document.getElementById("addBankBtn").addEventListener("click", (ev) => showInPopup("/Contacts/AddBankDetailsModal/" + ev.target.dataset.companyid, ev.target.dataset.title, SetBankDetailsModal));
function SetBankDetailsModal() {
    document.getElementById("BDForm").addEventListener("submit", (ev) => {
        ev.preventDefault();
        return jQueryAjaxPost(ev.target, RefreshBankDetails)
    })
}

function RefreshBankDetails(form) {
    $.getJSON("/Contacts/GetBankDetails", { companyId: form.dataset.companyid }, function (d) {
        let row = "";
        $("#InvoiceIn_BankDetailsId").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#InvoiceIn_BankDetailsId").html(row);
        //let item = new Option("Select", '', true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#InvoiceIn_BankDetailsId").append(item);
    })
}

export { CheckDoc };