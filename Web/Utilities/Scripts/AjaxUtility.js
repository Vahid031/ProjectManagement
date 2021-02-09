

function Ajax(Type, Url, Data, Success, DataType, ContentType, ProcessData, Async, Cache) {
    var ajax = {
        type: (typeof Type === "undefined") ? "GET" : Type,
        url: (typeof Url === "undefined") ? "" : Url,
        data: (typeof Data === "undefined") ? "" : Data,
        success: (typeof Success === "undefined") ? function (data) { } : Success,
        dataType: (typeof DataType === "undefined") ? "html" : DataType,
        contentType: (typeof ContentType === "undefined") ? "application/x-www-form-urlencoded; charset=utf-8" : ContentType,//"application/json; charset=utf-8",
        processData: (typeof ProcessData === "undefined") ? true : ProcessData,
        async: (typeof Async === "undefined") ? false : Async,
        cache: (typeof Cache === "undefined") ? false : Cache,
        error: function (xhr, Result, ajaxOptions, thrownError) {
            $('#loading').fadeOut();

            //alert(xhr.responseText);
            if (xhr.status == 403) {
                Messages('danger', 'شما به این صفحه دسترسی ندارید');
            }
            else if (xhr.status == 401) {
                Messages('warning', 'دسترسی ندارید');
            }
            else {
                // alert(xhr.status);
                alert(xhr.responseText);
                
            }

            
        },
        failure: function () { $('#loading').fadeOut(); },
        beforeSend: BeforeSend,
        complete: Complete
    };

    $.ajax(ajax);
}

function LoadPartialView(Type, url, data, container) {
    Ajax(Type, url, data, function (data) {
        $(container).fadeOut(50, function () {
            $(container).html(data);

            if ($('.operationform').length > 0) {
                prepareLoadedForm();
            }

            $(container).fadeIn(400);
        });
    });
}

function LoadPartialView(Type, url, data, container, events) {
    var def = $.Deferred();
    Ajax(Type, url, data, function (data) {
    
        $(container).fadeOut(50, function () {
            $(container).html(data);
            
            if ($('.operationform').length > 0) {
                prepareLoadedForm();
            }
            eval(events);
            $(container).fadeIn(400);
            def.resolve();
        });
    });

    return def.promise();
}




function BeforeSend() {
    $('#loading').show();
}

function Complete() {
    $('#loading').fadeOut();
}


function prepareLoadedForm() {
    $('.operationform').removeData('validator');
    $('.operationform').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('.operationform');
}