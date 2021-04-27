import { html, render } from '../lib/lit-html/lit-html.js';

const rowTemp = (index, input) => html`<tr id="row_${index}">
                                            <td>
                                                <b>${index + 1}</b>
                                            </td>
                                            <td>
                                                Спедиторски услуги <p class="m-0"><b>${input.Voyage} </b></p>
                                                с автомобил <b>${input.VehicleRegNumber}/${input.VehicleTrailerRegNumber} </b>
                                                по заявка <b>${input.OrderOrderFromReferenceNum == null ? input.ReceiveDate : input.OrderOrderFromReferenceNum}</b>
                                                <input id="OrderTos_${index}__Id" name="OrderTos[${index}].Id" type="hidden" value="${input.Id}">
                                            </td>
                                            <td>
                                                курс
                                            </td>
                                            <td>
                                                1.00
                                            </td>
                                            <td class="price">
                                                ${input.PriceNetIn.toFixed(2)}
                                            </td>
                                            <td class="text-center align-middle">
                                                <i href='javascript:' id="${index}" class='delete fas fa-minus-circle text-danger'  style="cursor: pointer;"></i>
                                            </td>
                                        </tr>`;

let rows = document.getElementById("rows");

function SetBtns(setRows) {
    [...document.querySelectorAll("#orderTos .addRow")].forEach((e) => e.addEventListener("click", (ev) => {
        let data = JSON.parse(ev.target.dataset.orderto);
        let index = rows.children.length;

        let row = document.createElement("tr");
        render(rowTemp(index, data), row);
        rows.appendChild(row.children[0]);

        $('#form-modal').modal('hide');
        setRows();
    }));
};

function ShowModal(companyId, title, setRows) {
    showInPopup("/Invoices/GetOrderTo/" + companyId, title, () => SetBtns(setRows));
}

let invoiceEndEl = document.querySelector("#invoiceEnd");
let finish = document.querySelector("#finish");
let vatEl = invoiceEndEl.querySelector("#vat");
let vatReasonEl = document.querySelector('.vatReason');
const val = vatReasonEl.dataset.selected;
vatReasonEl.querySelector("option[value='" + val + "']").selected = true;

vatReasonEl.addEventListener("change", (ev) => {
    calculateTotal();
})

document.getElementById("yes").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode;
    parent.nextSibling.nextSibling.style.display = "block";
    invoiceEndEl.style.display = 'none';
    //parent.style.display = "none";
});

function SetRows() {
    [...document.querySelectorAll("#rows tr")].forEach((r) => r.addEventListener("click", (ev) => {
        if (ev.target.classList.contains("delete")) {
            let id = ev.target.id;
            RemoveOrderTo(id);
            calculateTotal();
        }
    }))
}

document.querySelector("#addRow a").addEventListener("click", (ev) => ShowModal(ev.target.dataset.companyid, ev.target.dataset.title, SetRows))

document.getElementById("confirmBtn").addEventListener("click", () => HideCurrentDocumentation());

document.getElementById("no").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode.parentNode;
    ShowInvoiceEnd();
    parent.style.display = "none";
    ev.target.parentNode.parentNode.parentNode.nextSibling.nextSibling.style.display = "none";
})
function ShowInvoiceEnd() {
    calculateTotal();
    invoiceEndEl.style.display = "";
}

vatReasonEl.addEventListener("change", (ev) => {
    calculateTotal();
})

function calculateTotal() {
    let subTotal = [...document.getElementsByClassName("price")].reduce((sum, e) => { return sum + parseFloat(e.textContent.trim()) }, 0);
    invoiceEndEl.querySelector("#subTotal").textContent = subTotal.toFixed(2);

    let vat = 0;
    if (vatReasonEl.value == vatReasonEl.dataset.vatitemid) {
        vat = subTotal * 0.2;
    }
    vatEl.textContent = vat.toFixed(2);
    let total = subTotal + vat;
    invoiceEndEl.querySelector("#total").textContent = total.toFixed(2);
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

//function CheckDoc(id) {
//    let reqDoc = $('#doc_' + id + ' #reqDoc label').map(function () { return this.textContent });
//    let courseRow = document.getElementById(`row_${id}`);
//    if ($('#doc_' + id + ' :checkbox:checked').length != reqDoc.length) {
//        $('#modal').modal('show');
//        courseRow.classList.add("table-danger");
//        $('#doc_' + id + ' .solve').show();
//        return;
//    }
//    let isValid = true
//    $('#doc_' + id + ' :checkbox:checked').map(function () { return this.nextSibling }).each(function () {
//        if (jQuery.inArray(this.textContent, reqDoc) == -1) {
//            $('#modal').modal('show');
//            courseRow.classList.add("table-danger");
//            isValid = false;
//            return;
//        }
//    })
//    if (isValid) {
//        courseRow.classList.remove("table-danger");
//        courseRow.classList.add("table-success");
//        $('#doc_' + id + ' .solve').hide();
//        HideCurrentDocumentation();
//    }
//    else {
//        $('#doc_' + id + ' .solve').show();
//    }
//    //let form = document.getElementById("documentation_form");
//    //form.action = "/Orders/FinishOrder";
//    //form.submit();
//}
//function HideCurrentDocumentation() {
//    [...document.querySelectorAll("#rows tr")].forEach((t) => {
//        t.classList.remove("active-row");
//    });
//    let addRow = document.querySelector("#invoiceAddRow");
//    //if (document.querySelectorAll(".table-danger").length > 0) {
//    //    addRow.querySelector("#no").setAttribute("disabled", "true");
//    //}
//    //else {
//    //    addRow.querySelector("#no").removeAttribute("disabled");
//    //}
//    document.querySelector("#docs").style.display = "none";
//    addRow.style.display = "block";
//    $('#modal').modal('hide');
//}

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
        $("#InvoiceOut_BankDetailsId").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#InvoiceOut_BankDetailsId").html(row);
        //let item = new Option("Select", '', true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#InvoiceIn_BankDetailsId").append(item);
    })
}

function RemoveOrderTo(id) {
    document.querySelector("#row_" + id).remove();
}

//function openDocumentation(id) {
//    document.getElementById("invoiceAddRow").style.display = "none";
//    let docs = document.querySelector("#docs");
//    docs.style.display = "";
//    let docsEls = docs.querySelectorAll("div[id*=doc_]");
//    for (const t of docsEls) {
//        t.style.display = "none";
//    }
//    [...document.querySelectorAll("#rows tr")].forEach((t) => {
//        t.classList.remove("active-row");
//    });
//    document.querySelector("#doc_" + id).style.display = "";
//    document.querySelector("#row_" + id).className += " active-row";
//}

//[...document.querySelectorAll(".solve")].forEach((e) => {
//    e.addEventListener("click", (ev) => {
//        ev.target.style.display = "none";
//        ev.target.nextSibling.nextSibling.style.display = "";
//    })
//});

/*export { CheckDoc, openDocumentation };*/
