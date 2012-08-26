function showjquerypopup(id, widgetidentifier) {

   
    
    $.ajax({
        
        type: 'GET',
        url: 'Orchard.WidgetStyler/WidgetStyler/ShowJqueryPopup',

        data: { widgetid: id,widgetidentifier:widgetidentifier},
        dataType: 'html',
        success: function (htmlData) {

            //clear content from dom if exists 
            $("#jquerydialog").remove();

            $('body').append(htmlData);

           
            $("#jquerydialog").dialog({
                autoOpen: true,
                width: 910,
                height:440
            });



        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });



}


function Jqueryformpost(frmobj, targetmsgcontainer) {

    $(frmobj).submit(function (e) {

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),

            success: function (result) {
                // The AJAX call succeeded and the server returned a JSON 
                // with a property "s" => we can use this property
                // and set the html of the target div
                if (targetmsgcontainer != null) {
                    $(targetmsgcontainer).html(result.s);

                }

            }
        });
        // it is important to return false in order to 
        // cancel the default submission of the form
        // and perform the AJAX call
        return false;
        
 

    });

}