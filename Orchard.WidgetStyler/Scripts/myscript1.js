//function showjquerypopup() {

//    $.ajax({
//        type: 'GET',
//        url: 'Orchard.WidgetStyler/WidgetStyler/ShowJqueryPopup',

//        data: null,
//        dataType: 'html',
//        success: function (htmlData) {

//            //clear content from dom if exists 
//            $("#jquerydialog").remove();

//            $('body').append(htmlData);

//            debugger;
//            $("#jquerydialog").dialog({
//                autoOpen: true,
//                width: 600,
//                modal: true,
//                buttons: {
//                    "Ok": function () {
//                        $(this).dialog("close");
//                    },
//                    "Cancel": function () {
//                        $(this).dialog("close");
//                    },
//                    "Me too": function () {
//                        alert("hello");
//                    }
//                }
//            });



//        },
//        error: function (request, status, error) {
//            alert(request.responseText);
//        }
//    });



//}


