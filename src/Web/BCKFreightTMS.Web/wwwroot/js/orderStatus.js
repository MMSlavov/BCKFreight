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


[...document.querySelectorAll(".course")].forEach(tc => {
    tc.querySelector("#aTabs")
        .addEventListener("click", (ev) => {
            if (ev.target && ev.target.localName == "a") {
                openAddress(ev, ev.target.id);
            }
        });

    function openAddress(evt, id) {
        let tabcontent = [...tc.querySelectorAll(".tabcontent")];
        for (const t of tabcontent) {
            t.style.display = "none";
        }
        [...tc.querySelectorAll(".tablinks")].forEach((t) => {
            t.className = t.className.replace(" active-tab", "");
        });
        tc.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
        evt.target.className += " active-tab";
    }

    function UpdateNextAction(i, disable) {
        let nextTab = tc.querySelector("#aTabs a[id=action" + (i + 1) + "]");
        let currTab = tc.querySelector("#aTabs a[id=action" + i + "]");
        if (disable) {
            if (nextTab) {
                nextTab.classList.add("disabled");
            }
            currTab.classList.remove("bg-green");
        }
        else {
            if (nextTab) {
                nextTab.classList.remove("disabled");
            }
            currTab.classList.add("bg-green");
        }
    }


    [...tc.querySelectorAll(".isFinishedRadios")].forEach((v) => {
        v.addEventListener("change", function (ev) {
            let id = ev.target.id;
            let idIndex = ev.target.id.slice(-1, id.length);
            let NNCheck = tc.querySelector("input[id*=_" + idIndex + "__NoNotes]");
            let WNCheck = tc.querySelector("#withCom" + idIndex);

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

    [...tc.querySelectorAll(".noteChecks")].forEach((v) => {
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
        let reason = tc.querySelector("select[id*=_" + i + "__NotFinishedReasonId]");
        let WNCheck = tc.querySelector("#withCom" + i);
        let NNCheck = tc.querySelector("input[id*=_" + i + "__NoNotes]");
        let notes = tc.querySelector("textarea[id*=_" + i + "__Notes]");
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
        let notes = tc.querySelector("textarea[id*=_" + i + "__Notes]");
        let NNCheck = tc.querySelector("input[id*=_" + i + "__NoNotes]");
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
        let WNCheck = tc.querySelector("#withCom" + i);

        if (WNCheck.disabled) {
            WNCheck.removeAttribute("disabled");
        }
        else {
            WNCheck.setAttribute("disabled", "disabled");
            WNCheck.checked = false;
        }
    }
});

document.getElementById("cTabs")
    .addEventListener("click", (ev) => {
        console.log("click");
        if (ev.target && ev.target.classList.contains("tablinks")) {
            openCourse(ev.target.id);
        }
    });

function openCourse(id) {
    let tabcontent = document.querySelectorAll(".tabcontent[id*=course]");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...document.querySelectorAll(".tablinks[id*=course]")].forEach((t) => {
        t.className = t.className.replace(" active-tab", "");
    });
    document.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
    document.querySelector(".tablinks[id=" + id + "]").className += " active-tab";
}