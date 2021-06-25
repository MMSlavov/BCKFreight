import { html, render } from '../lib/lit-html/lit-html.js';

const rowTemp = (index, input) => html`<tr id="row_${index}">
                                            <td>
                                                <b>${index + 1}</b>
                                            </td>
                                            <td>
                                                Транспортна услуга <p class="m-0"><b>${input.Voyage} </b></p>
                                                с автомобил <b>${input.VehicleRegNumber}/${input.VehicleTrailerRegNumber} </b>
                                                по заявка <b>${input.CarrierOrderReferenceNum} </b>
                                                <input id="OrderTos_${index}__Id" name="OrderTos[${index}].Id" type="hidden" value="${input.Id}">
                                            </td>
                                            <td>
                                                курс
                                            </td>
                                            <td>
                                                1.00
                                            </td>
                                            <td class="price">
                                                ${input.PriceNetOut.toFixed(2)}
                                            </td>
                                            <td class="text-center align-middle">
                                                <i href='javascript:' id="${index}" class='delete fas fa-minus-circle text-danger' style="cursor: pointer;"></i>
                                            </td>
                                        </tr>`;

let rows = document.getElementById("rows");

const docParser = {
    "CMR": "CMR",
    "BillOfLading": "Товарителница",
    "AOA": "ППП",
    "DeliveryNote": "Delivery note",
    "PackingList": "Packing list",
    "ListItems": "List items",
    "Invoice": "Фактура",
    "BillOfGoods": "Стокова",
    "WeighingNote": "Кантарна бележка"
};

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

[...document.querySelectorAll("input.price")].forEach(e => e.addEventListener("change", calculateTotal));
vatReasonEl.addEventListener("change", (ev) => {
    calculateTotal();
})

//document.querySelector("#addRow a").addEventListener("click", (ev) => ShowModal(ev.target.dataset.companyid, ev.target.dataset.title, SetRows))

function calculateTotal() {
    let subTotal = [...document.getElementsByClassName("price")].reduce((sum, e) => {
        return sum + parseFloat(e.value ? e.value : e.textContent.trim());
    }, 0);
    invoiceEndEl.querySelector("#subTotal").textContent = subTotal.toFixed(2);

    let vat = 0;
    if (vatReasonEl.value == vatReasonEl.dataset.vatitemid) {
        vat = subTotal * 0.2;
    }
    vatEl.textContent = vat.toFixed(2);
    let total = subTotal + vat;
    invoiceEndEl.querySelector("#total").textContent = total.toFixed(2);
}
calculateTotal();

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

function SetRows() {
    [...document.querySelectorAll("#rows tr")].forEach((r) => r.addEventListener("click", (ev) => {
        if (ev.target.classList.contains("delete")) {
            let id = ev.target.id;
            RemoveOrderTo(id);
        }
    }))
}
SetRows()

function RemoveOrderTo(id) {
    //document.querySelector("#doc_" + id).remove();
    document.querySelector("#row_" + id).remove();
    [...document.querySelectorAll("input[name*=\"" + id + "\"]")].forEach((e) => {
        e.remove()
    });
}
