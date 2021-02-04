document.getElementById("tabs")
    .addEventListener("click", (ev) => {
        if (ev.target && ev.target.localName == "a") {
            open(ev, ev.target.id);
        }
    });

function open(evt, id) {
    let tabcontent = document.getElementsByClassName("tabcontent");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...document.getElementsByClassName("tablinks")].forEach((t) => {
        t.className = t.className.replace(" active-tab", "");
    });
    document.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
    evt.target.className += " active-tab";
}

function UpdateNextAction(i, disable) {
    let nextTab = document.querySelector("#tabs a[id=action" + (i + 1) + "]");
    let currTab = document.querySelector("#tabs a[id=action" + i + "]");
    //let checkbox = document.getElementsByName("actions[" + (i + 1) + "].IsFinnished")[0];
    //if (checkbox == undefined) {
    //    return;
    //}
    //let reason = document.getElementsByName("actions[" + (i + 1) + "].NotFinishedReasonId")[0];
    //let NNCheck = document.getElementById("actions_" + (i + 1) + "__NoNotes");
    //let notes = document.getElementById("actions_" + (i + 1) + "__Notes");
    if (disable) {
        if (nextTab) {
            nextTab.classList.add("disabled");
        }
        currTab.classList.remove("bg-green");
        //checkbox.removeAttribute("checked");
        //reason.setAttribute("disabled", "disabled");
        ////checkbox.setAttribute("disabled", "disabled");
        //NNCheck.setAttribute("disabled", "disabled");
        //notes.setAttribute("disabled", "disabled");
    }
    else {
        if (nextTab) {
            nextTab.classList.remove("disabled");
        }
        currTab.classList.add("bg-green");
        //reason.removeAttribute("disabled");
        ////checkbox.removeAttribute("disabled");
        //NNCheck.removeAttribute("disabled");
        //notes.removeAttribute("disabled");
    }
}
function UpdateBtns() {
    console.log($("input:radio[id*='yes']:checked").length);
    console.log($("input:radio[id*='yes']").length);

    if ($("input:radio[id*='yes']:checked").length != $("input:radio[id*='yes']").length) {
        $(".btn-primary").attr("disabled", null);
        $(".btn-success").attr("disabled", "disabled");
    }
    else {
        $(".btn-primary").attr("disabled", "disabled");
        $(".btn-success").attr("disabled", null);
    }
}
$(document).ready(UpdateBtns);
//$(":checkbox").change(UpdateBtns);

[...document.getElementsByClassName("isFinishedRadios")].forEach((v) => {
    v.addEventListener("change", function (ev) {
        let id = ev.target.id;
        let idIndex = ev.target.id.slice(-1, id.length);
        let NNCheck = document.getElementById("actions_" + idIndex + "__NoNotes");
        let WNCheck = document.getElementById("withCom" + idIndex);

        if (id.includes("no")) {
            UpdateBtns();
            UpdateNextAction(Number(idIndex), true);
            ToggleReason(idIndex);
        }
        else {
            ToggleReason(idIndex, true);
            if (NNCheck.checked || WNCheck.checked) {
                UpdateNextAction(Number(idIndex), false);
            }
        }
    });
});

[...document.getElementsByClassName("noteChecks")].forEach((v) => {
    v.addEventListener("change", function (ev) {
        let id = ev.currentTarget.id;
        if (ev.target.id.includes("withCom")) {
            ToggleNotes(id);
        }
        else {
            ToggleWNCheck(id);
        }
        UpdateBtns();
        UpdateNextAction(Number(id), false);
    });
});

function ToggleReason(i, disable = false) {
    console.log(i);
    let reason = document.getElementById("actions_" + i + "__NotFinishedReasonId");
    let WNCheck = document.getElementById("withCom" + i);
    let NNCheck = document.getElementById("actions_" + i + "__NoNotes");
    let notes = document.getElementById("actions_" + i + "__Notes");
    //let NNCheck = document.getElementById("actions_" + i + "__NoNotes");
    //let notes = document.getElementById("actions_" + i + "__Notes");
    console.log(reason.disabled);
    if (disable) {
        reason.setAttribute("disabled", "disabled");
        NNCheck.removeAttribute("disabled");
        WNCheck.removeAttribute("disabled");
        //notes.removeAttribute("disabled");
    }
    else {
        reason.removeAttribute("disabled");
        NNCheck.setAttribute("disabled", "disabled");
        NNCheck.checked = false;
        WNCheck.setAttribute("disabled", "disabled");
        WNCheck.checked = false;
        notes.setAttribute("disabled", "disabled");
    }
}

function ToggleNotes(i) {
    let notes = document.getElementById("actions_" + i + "__Notes");
    let NNCheck = document.getElementById("actions_" + i + "__NoNotes");
    //let notes = document.getElementById("actions_" + i + "__Notes");

    if (notes.disabled) {
        notes.removeAttribute("disabled");
    }
    else {
        notes.setAttribute("disabled", "disabled");
    }
    if (NNCheck.disabled) {
        NNCheck.removeAttribute("disabled");
    }
    else {
        NNCheck.setAttribute("disabled", "disabled");
        NNCheck.checked = false;
    }
}

function ToggleWNCheck(i) {
    let WNCheck = document.getElementById("withCom" + i);

    if (WNCheck.disabled) {
        WNCheck.removeAttribute("disabled");
    }
    else {
        WNCheck.setAttribute("disabled", "disabled");
        WNCheck.checked = false;
    }
}