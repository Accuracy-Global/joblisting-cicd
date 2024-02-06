var buildContacts;
function appendModelPrefix(value, prefix) {
    if (value.indexOf('*.') === 0) {
        value = value.replace('*.', prefix);
    }
    return value;
}



function chkJob() {
    this.initialize = function () {
        //   debugger;

        $(document).on('click', '#chkJb', function () {
            var jobs = '';
            $('input:checkbox.chkJob').each(function () {
                jobs = jobs + ',' + (this.checked ? $(this).val() : "");
            });
            $("#hdnSelectedContacts").val(jobs);
        });
    };
}

function ValidateCheckBoxes() {
    var url = window.location.href;

    debugger;
    var checkedInputs = $('input:checkbox.chkJob');
    var counterChk = 0;
    var jobs = '';

    checkedInputs.each(function () {
        if (this.checked) {
            counterChk = 1 + 0;
            jobs = jobs + ',' + (this.checked ? $(this).val() : '');
        }
    });
    
    if (counterChk == 0) {
        alert("Please select at least one job to apply.");
    } else {                
        var url = "/Job/ApplyJobs?jobids=" + jobs;
        $.post(url, {}, function (result) {
            toastr.info(result);
        });
    }
    return false;
}
$(document).ready(function () {
    $('._SelectAll').click(function () {
        var sval = $(this).val();
        if (sval == "Select All") {
            $(this).val("Unselect All");
            $('.chkJob').prop('checked', true);
            return false;
        }
        else (sval == "Unselect All")
        {
            $(this).val("Select All");
            $('.chkJob').prop('checked', false);
            return false;
        }
    });

    $(".chkJob").click(function () {
        if ($(".chkJob").length == $(".chkJob:checked").length) {
            $("#selectall").attr("checked", "checked");
            $("#selectall2").attr("checked", "checked");
            $("#selectall").val("Unselect All");
            $("#selectall2").val("Unselect All");
        }
        else {
            $("#selectall").val("Select All");
            $("#selectall2").val("Select All");
        }
    });

    if ($('#chkAgree').is(':checked')) {

        $('#btnRegiter').removeAttr('disabled', 'disabled');
        $('#btnRegiter').removeAttr('style');
    }
    else {
        $('#btnRegiter').attr('disabled', 'disabled');
        $('#btnRegiter').attr('style', 'color:Silver');
    }


    $("#chkAgree").click(function () {

        if ($(this).is(':checked')) {
            $('#btnRegiter').removeAttr('disabled', 'disabled');
            $('#btnRegiter').removeAttr('style');
        }
        else {
            $('#btnRegiter').attr('disabled', 'disabled');
            $('#btnRegiter').attr('style', 'color:Silver');
        }

    });
});


function logout() {
    window.location = "/Account/LogOff";
}

function validateAccount() {
    $.post('/JsonHelper/Validate', {},
                       function (result) {
                           if (result==false) {
                               window.location = "/Account/InvalidAccount";
                           }
                       });
}
function Validate() {
    setInterval("validateAccount()", 1000 * 10); // 10 seconds
}

function popup_win(loc, wd, hg) {
    var remote = null;
    var xpos = screen.availWidth / 2 - wd / 2;
    var ypos = screen.availHeight / 2 - hg / 2;
    remote = window.open('', '', 'width=' + wd + ',height=' + hg + ',resizable=1,scrollbars=1,screenX=0,screenY=0,top=' + ypos + ',left=' + xpos);
    if (remote != null) {
        if (remote.opener == null) {
            remote.opener = self;
        }
        remote.location.href = loc;
        remote.focus();
    }
    else {
        self.close();
    }
}

$(function () {
    $(".date").datetimepicker({
        format: "mm/dd/yyyy",
        pickDate: true,
        pickTime: false,
        autoclose: true,
        todayBtn:true,
        todayHighlight:true,
        minView: 2,
        maxView: 4,
        initialDate: new Date(),
        icons: {
            up: 'glyphicon-chevron-up',
            down: 'glyphicon-chevron-down'
        },
        pickerPosition: "bottom-left"
    });
});
function isValidDate(day, month, year) {
    var check = false;
    if (day == "" && month == "" && year == "") {
        check = true;
    } else {
        var yyyy = Number(year);
        var mm = Number(month);
        var dd = Number(day);

        var xdata = new Date(year + "/" + month + "/" + day);

        if ((xdata.getFullYear() == yyyy) && (xdata.getMonth() == mm - 1) && (xdata.getDate() == dd))
            check = true;
        else
            check = false;
    }
    return check;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function loadfile(filename, filetype) {
    if (filetype == "js") { //if filename is a external JavaScript file
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", filename)
        alert('called');
    }
    else if (filetype == "css") { //if filename is an external CSS file
        var fileref = document.createElement("link")
        fileref.setAttribute("rel", "stylesheet")
        fileref.setAttribute("type", "text/css")
        fileref.setAttribute("href", filename)
    }
    if (typeof fileref != "undefined")
        document.getElementsByTagName("head")[0].appendChild(fileref)
}

