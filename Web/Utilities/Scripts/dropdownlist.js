var resetVariable = true;


function GetDropDown(ControlId, DropDownPanel, url, params, values, container, func, outVariable) {
    resetVariable = true;

    $('#' + container).on("keyup click", '#' + ControlId, function (e) {
        if ($(this).val() == 'انتخاب کنید...')
            $(this).val('');

        GetDropDownList(ControlId, DropDownPanel, url, params, values, func, outVariable);
    });

    $('#' + container).on("click", "#" + DropDownPanel + " .DropDownPanelItem", function (e) {
        $('#' + DropDownPanel).css('display', 'none');
    });
}

function GetDropDownList(ControlId, PanelId, url, params, values, func, outVariable) {
    
    var paramValue = values == '' ? '' : eval(values)
    
    if (resetVariable == true && ((params == "") ? true : (eval(values) != ''))) {
        
        Ajax('Post', url, params + paramValue, function (data, textStatus, xhr) {
            var json = JSON.parse(data.Values);

            for (var item in outVariable) {
                delete outVariable[item];
            }

            for (var i = 0; i < json.length; i++) {
                outVariable.push({ Id: (json[i].Id), Code: json[i].Code, Title: json[i].Title });
            }

            resetVariable = false;

        }, 'json');
    }

    $('#' + PanelId).html('');

    var DropDownPanel = "";

    DropDownPanel += "<div class='row' style='background-color: #eee'>";
    DropDownPanel += "    <div class='col-lg-6'>";
    DropDownPanel += "        <span>عنوان</span>";
    DropDownPanel += "    </div>";
    DropDownPanel += "    <div class='col-lg-6'>";
    DropDownPanel += "        <span>کد</span>";
    DropDownPanel += "    </div>";
    DropDownPanel += "</div>";

    DropDownPanel += "<div class='DropDownPanelItem row' onclick='" + func + "(" + null + ", \"\", \"\")'>";
    DropDownPanel += "    <div class='col-lg-12'>";
    DropDownPanel += "        <span>انتخاب کنید...</span>";
    DropDownPanel += "    </div>";
    DropDownPanel += "</div>";

    var results = [];
    var filter = "l";

    for (var i = 0; i < outVariable.length; i++) {
        for(key in outVariable[i])
        {
            if (outVariable[i][key].toString().indexOf($('#' + ControlId).val()) != -1) {
                results.push(outVariable[i]);
                break;
            }
        }
    }


    for (var i = 0; i < results.length; i++) {
        DropDownPanel += "<div class='DropDownPanelItem row' onclick='" + func + "(" + results[i].Id + ", \"" + results[i].Code + "\", \"" + results[i].Title + "\")'>";
        DropDownPanel += "    <div>";
        DropDownPanel += "        <div class='col-lg-6'>";
        DropDownPanel += "            <span>" + results[i].Code + "</span>";
        DropDownPanel += "        </div>";
        DropDownPanel += "        <div class='col-lg-6'>";
        DropDownPanel += "            <span>" + results[i].Title + "</span>";
        DropDownPanel += "        </div>";
        DropDownPanel += "    </div>";
        DropDownPanel += "</div>";
    }
    
    $('#' + PanelId).css('display', 'block');
    
    $('#' + PanelId).html(DropDownPanel);

}
