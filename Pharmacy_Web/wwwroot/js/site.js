
var medicineList = [];


function detectScreenRecording() {
    // Check if the `getDisplayMedia` method is available (indicating screen recording capability)
    if (navigator.mediaDevices && navigator.mediaDevices.getDisplayMedia) {
        // Screen recording is supported
        console.log('Screen recording detected');

        // Here, you can perform actions to prevent screen recording, such as displaying a message or taking other protective measures.
        // For example, you could show a watermark overlay on sensitive content, disable right-click context menu, etc.
    }
}

// Call the detection function when the page loads
window.addEventListener('DOMContentLoaded', detectScreenRecording);


function onModalBegin() {
    $('body :submit').attr('disabled', 'disabled');
    $(".indicator-label").hide();
    $(".indicator-progress").show();
}
function onModalSuccess(row) {
    $('#Modal').modal('hide')
    ShowMessage("Operation Successfully !", "success", "btn btn-primary");
    $('tbody').prepend(row);
}
function showMessageHideModel(masege) {
    $('#Modal').modal('hide')
    ShowMessage(masege, "success", "btn btn-primary");
   
}
function showErrorMessage(message = 'Something went wrong !') {
    Swal.fire({
        icon: 'error',
        title: 'Error...',
        text: message.responseText !== undefined ? message.responseText : message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}
function ShowMessage(message, icon, clas) {
    Swal.fire({
        text: message.responseText !== undefined ? message.responseText : message,
        icon: icon,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: clas
        }
    });
}
function onModalComplete() {
    $('body :submit').removeAttr('disabled');
    $(".indicator-label").show();
    $(".indicator-progress").hide();
}
function addReveal() {
    if (medicineList.length > 0) {
        var form = $("#js-revealForm");
        if (form.valid()) {
            $.post({
                url: '/Reveal/Add',
                data: {
                    '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
                    'revealModel': JSON.stringify({
                        "patientId": $('#PatientId').val(),
                        "bodyTempreature": $('#BodyTempreature').val(),
                        "diagnosis": $('#Diagnosis').val(),
                        "bloodPressure": $('#BloodPressure').val(),
                        "diabites": $('#Diabites').val(),
                        "nextVisit": $('#NextVisit').val(),
                        "notes": $('#Notes').val(),
                        "items": medicineList
                    })
                },
                success: function (data) {
                    onModalSuccess(data);
                },
                error: function (error) {
                    ShowMessage(error, "error", "btn btn-danger");
                }
            });
        }
    }
    else {
        ShowMessage("Medication must be selected !", "error", "btn btn-danger");
    }
   
}
function removeMedicine(itemId) {
    var index = medicineList.findIndex(item => item.ItemId === itemId);
    if (index !== -1) {
        medicineList.splice(index, 1);
        $(`#${itemId}`).remove();
        ShowMessage("Removed Success !", "success", "btn btn-primary");
    }
}
function addMedicine() {
    // Get references to the HTML elements
    let ulMedicine = $('#js-medicine');
    let selecteditemId = $('.js-itemUnit');
    let selectedItemName = $('.js-itemUnit option:selected').text();
    let treatmentMethod = $('#js-treatmentMethod');
    let medicineQty = $('#js-medicineQty');
    // Check that all fields have a value
    if (selecteditemId.val() !== '' && treatmentMethod.val() !== '' && medicineQty.val() !== '') {
        // Check if the item already exists in the medicineList array
        if (!medicineList.some(item => item.ItemId === selecteditemId.val())) {
            // Create a new HTML list item and add it to the unordered list
            let htmlString =
                `<li id="${selecteditemId.val()}">
                    <a onclick="removeMedicine('${selecteditemId.val()}')" href="javascript:;">
                        <i class="fas fa-trash" style="color:red;"></i>
                    </a> ${selectedItemName} - ${medicineQty.val()}
                    <ul>
                        <li class="treatmentMethod">${treatmentMethod.val()}</li>
                    </ul>
                </li>`;
            ulMedicine.append(htmlString);
            // Add the new item to the medicineList array
            medicineList.push({
                "ItemId": selecteditemId.val(),
                "Qty": medicineQty.val(),
                "Notes": treatmentMethod.val()
            });
            // Clear the input fields

            treatmentMethod.val('');
            medicineQty.val('');
        }
        else {
            // If the item already exists, show an error message
            showErrorMessage("This drug already exists !");
        }
    }
    else {
        // If any of the fields are empty, show an error message
        showErrorMessage("Make sure to choose valid data !");
    }
}



$(document).ready(function () {

    //Handel render modal
    $('body').delegate('.js-render-modal', 'click', function () {

        var btn = $(this);
        var modal = $('#Modal');
        if (btn.data('url').includes("Reveal")) {
            modal.addClass("modal-xl");
        }
        else {
            modal.removeClass("modal-xl");
            if (btn.data('url').includes("Password")) {
                modal.removeClass("modal-lg");
                modal.addClass("modal");
            }
        }
        modal.find('#ModalLabel').text(btn.data('title'));
        modal.find('.modal-body').html("");
        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
                $(".indicator-progress").hide();
                if (btn.data('url').includes("Reveal")) {
                    $('.form__input').css('width', '100%');
                    var inputDate = new Date($('#NextVisit').val());
                    var formattedDate = inputDate.getDate() + '-' + (inputDate.getMonth() + 1) + '-' + inputDate.getFullYear();
                    $('#NextVisit').val(formattedDate);
                }
                else {
                    $('.form__input').css('width', '350px');
                }
                $('.js-select').selectize({
                    sortField: 'text'
                });
            },
            error: function (errors) {
                console.log(errors);
                ShowMessage("An Error !", "error", "btn btn-danger");
            }
        });
        modal.modal('show');
    });

    //Handel Delete
    $('body').delegate('.js-deleteRosheta', 'click', function () {
        var btn = $(this);
        bootbox.confirm({
            message: "Are You Sure That You Need To Delete Rosheta ?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function () {
                            var row = btn.parents('tr');
                            row.remove();
                            ShowMessage("Removed Sucess !", "success", "btn btn-primary");
                        },
                        error: function () {
                            ShowMessage("An Error !", "error", "btn btn-danger");
                        }
                    });
                }
            }
        });
    });
});
