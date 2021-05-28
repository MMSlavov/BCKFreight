$(function () {
    $('[data-toggle="popover"]').popover({
        container: "body",
        placement: "bottom",
        trigger: "hover",
    })
})
const resForm = document.querySelector("#res");
const mvForm = resForm.querySelector("#movementForm");
const invSel = resForm.querySelector("#invoiceSelect");
const invTable = resForm.querySelector("#invoices");

let data;
let currRow;
let currRowData;

resForm.querySelector("#accTypeBtns").addEventListener("click", function (ev) {
    if (ev.target.classList.contains("btn")) {
        mvForm.querySelector("#AccTypeId").value = Number(ev.target.id); 
        if (ev.target.classList.contains("pn")) {
            $.getJSON("/Transactions/SearchCompany", { companyName: currRowData.oppositeSideName, companyIban: currRowData.oppositeSideAccount }, function (res) {
                console.log(res);
                invSel.querySelector("#company").value = res.id;
                getInvoices();
            })
            invSel.style.display = '';
        }
        else{
            safeBankMovement();
        }
    }
})

function safeBankMovement() {
    jQueryAjaxPost(mvForm, (f, r) => {
        loadMovement(currRow + 1);
    })
}

$("#company").change(function () {
    getInvoices();
})

$("#invoices").hide();

function getInvoices() {
    $.getJSON("/Transactions/GetCompanyInvoices", { companyId: $("#company").val(), mvType: currRowData.movementType }, function (d) {
        let row = "";
        console.log(d);
        $("#invoices tbody").empty();
        if (d.length == 0) {
            $("#invoices").hide();
            return;
        }
        $.each(d, function (i, v) {
            row += "<tr class=invRow>" + "<td>" + new Date(Date.parse(v.createDate)).toLocaleDateString() + "</td>" + "<td>" + v.number + "</td>" + "<td class=price>" + v.price.toFixed(2) + "</td>" + "</tr>";
        });
        $("#invoices tbody").html(row);
        $("#invoices").show();
    })
}

invTable.addEventListener("click", function (ev) {
    const parent = ev.target.parentNode;
    if (parent.classList.contains("invRow")) {
        if (parent.classList.contains("active-row")) {
            parent.classList.remove("active-row");
        }
        else {
            parent.classList.add("active-row");
        }
        calculateInvTot();
    }
})

function calculateInvTot() {
    invTable.querySelector("#tot").textContent = [...invTable.querySelectorAll("tr.active-row .price")].reduce((a, v) => { return a += Number(v.textContent) }, 0).toFixed(2);
}

document.getElementById("bsForm").addEventListener("submit", (ev) => {
    ev.preventDefault();
    jQueryAjaxPost(ev.target, (f, res) => {
        data = res;
        loadMovement(0);
        $(".loading").hide();
    });
})

resForm.querySelector("#next").addEventListener("click", () => { loadMovement(currRow + 1) });

function loadMovement(i) {
    if (i >= data.length) {
        return;
    }
    currRowData = data[i];
    currRow = i;
    invSel.style.display = 'none';

    //load movement form
    mvForm.querySelector("#DateIn").value = new Date(Date.parse(currRowData.date)).toISOString();
    mvForm.querySelector("#OSNameIn").value = currRowData.oppositeSideName;
    mvForm.querySelector("#ReasonIn").value = currRowData.reason;
    mvForm.querySelector("#AmountIn").value = currRowData.amount;
    mvForm.querySelector("#OSAccIn").value = currRowData.oppositeSideAccount;

    //load view table
    resForm.querySelector("#index").textContent = currRow + 1;
    resForm.querySelector("#totRows").textContent = data.length;
    resForm.querySelector("#OSName").textContent = currRowData.oppositeSideName ? currRowData.oppositeSideName : "-";
    resForm.querySelector("#reason").textContent = currRowData.reason;
    resForm.querySelector("#date").textContent = new Date(Date.parse(currRowData.date)).toLocaleDateString();
    resForm.querySelector("#amount").textContent = currRowData.amount.toFixed(2);

    if (currRowData.movementType == "Credit") {
        resForm.querySelector("#creditBtns").style.display = "";
        resForm.querySelector("#debitBtns").style.display = "none";
    }
    else {
        resForm.querySelector("#creditBtns").style.display = "none";
        resForm.querySelector("#debitBtns").style.display = "";
    }
    resForm.style.display = "";
}