function onlyNumbers(a) { var b = event || a, c = b.which || b.keyCode; return 44 == c || (45 == c || (46 == c || c >= 48 && c <= 57)) }

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
}

function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}

function datediff(first, second) {
    // Take the difference between the dates and divide by milliseconds per day.
    // Round to nearest whole number to deal with DST.
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}

function controlloCoerenzaDate(id_calendarDataInizio, id_calendarDataFine) {
    $('#' + id_calendarDataInizio).datetimepicker({
        locale: 'it',
        format: 'DD/MM/YYYY',
        useCurrent: false //Important! See issue #1075
    });
    $('#' + id_calendarDataFine).datetimepicker({
        locale: 'it',
        format: 'DD/MM/YYYY',
        useCurrent: false //Important! See issue #1075
    });

    if ($('#' + id_calendarDataInizio).val() != "") {
        var dataInizio = $('#' + id_calendarDataInizio).data('DateTimePicker');
        if (dataInizio != null) {
            $('#' + id_calendarDataFine).data("DateTimePicker").minDate(new Date(dataInizio.date()));          
        }
    }

    if ($('#' + id_calendarDataFine).val() != "") {
        var dataFine = $('#' + id_calendarDataFine).data('DateTimePicker');
        if (dataFine != null) {
            $('#' + id_calendarDataInizio).data("DateTimePicker").maxDate(new Date(dataFine.date()));
        }
    }

    $('#' + id_calendarDataInizio).on("dp.change", function (e) {
        
        $('#' + id_calendarDataFine).data("DateTimePicker").minDate(e.date);
        $('#' + id_calendarDataFine).val($('#' + id_calendarDataInizio).val()); // setta automaticamente la data fine uguiale alla data inizio
        
    });
    $('#' + id_calendarDataFine).on("dp.change", function (e) {
        $('#' + id_calendarDataInizio).data("DateTimePicker").maxDate(e.date);
    });
}

