showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');

            // to make popup draggable
            $(".modal-header").on("mousedown", function (mousedownEvt) {
                var $draggable = $(this);
                var x = mousedownEvt.pageX - $draggable.offset().left,
                    y = mousedownEvt.pageY - $draggable.offset().top;
                $("body").on("mousemove.draggable", function (mousemoveEvt) {
                    $draggable.closest(".modal-dialog").offset({
                        "left": mousemoveEvt.pageX - x,
                        "top": mousemoveEvt.pageY - y
                    });
                });
                $("body").one("mouseup", function () {
                    $("body").off("mousemove.draggable");
                });
                $draggable.closest(".modal").one("bs.modal.hide", function () {
                    $("body").off("mousemove.draggable");
                });
            });
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    window.location = res.redirectToUrl;
                }
                else {
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxBtnGet = (url) => {
    try {
        $.ajax({
            type: 'GET',
            url: url,
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#content-wrapper').html(res.html);
                }
                //else {
                //    $('#form-modal .modal-body').html(res.html);
                //}
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function AddSelectOption ()
{
    var item = new Option("Select", "", true);
    $(item).html("Select");
    $('select').append(item);
};

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('#back-to-top').fadeIn();
        } else {
            $('#back-to-top').fadeOut();
        }
    });
    $('#back-to-top').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 400);
        return false;
    });
});