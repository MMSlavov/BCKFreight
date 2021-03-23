import { html, render } from '../lib/lit-html/lit-html.js';
import { CheckDoc } from './finishOrder.js';

const rowTemp = (index, input) => html`<tr id="row_${index}">
                                            <td>
                                                <b>${index + 1}</b>
                                            </td>
                                            <td>
                                                Транспортна услуга <p class="m-0"><b>${input.Voyage} </b></p>
                                                с автомобил <b>${input.VehicleRegNumber} </b>
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
                                        </tr>`;

let rows = document.getElementById("rows");

function SetBtns() {
    [...document.querySelectorAll("#orderTos .addRow")].forEach((e) => e.addEventListener("click", (ev) => {
        let data = JSON.parse(ev.target.dataset.orderto);
        let index = rows.children.length;

        let row = document.createElement("tr");
        render(rowTemp(index, data), row);
        rows.appendChild(row.children[0]);

        let doc = document.querySelector("#doc_0").cloneNode(true);
        let reqDoc = doc.querySelector("#reqDoc");
        let docRow = reqDoc.querySelector("div");
        reqDoc.innerHTML = "";
        for (const [name, val] of Object.entries(data.Documentation)) {
            if (val) {
                docRow.children[1].textContent = name;
                reqDoc.appendChild(docRow.cloneNode(true));
            }
        }
        
        let docHtml = doc.innerHTML.replace(/OrderTos_\d_/g, "OrderTos_" + index + "_").replace(/OrderTos\[\d\]/g, "OrderTos[" + index + "]").replace(/"\d"/g, "\"" + index + "\"");
        $("#docs").append("<div class='form-group m-0 rounded bg-white' id='doc_" + index + "'>" + docHtml + '</div>');
        $("#invoiceAddRow").hide();
        $('#form-modal').modal('hide');
        document.querySelector("#doc_" + index + " #docCheck a").addEventListener("click", (ev) => CheckDoc(ev.target.id))
    }));
};
function ShowModal(companyId, title) {
    showInPopup("/Orders/GetOrderTo/" + companyId, title, SetBtns);
}

export { ShowModal };
//console.log(document.querySelector("#addRow a"));
//document.querySelector("#addRow a").addEventListener("click", SetBtns());