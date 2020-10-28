var d = new Date();
var h = (d.getHours <= 9) ? 9: d.getHours();
var m = d.getMinutes();
//var t = getHoursString(h) + ":" + getMinutesString(m) + getMeridium(h);
var t = get12HourClock(h) + ":" + getMinutesString(m) + getMeridium(h);

var timeInput = document.getElementById("timepicker");
timeInput.value = t;


//Currently using cdn but can install jquery-timepicker from nuget
$(function () {
    //need to add jquery ui
    $("#timepicker").timepicker({ 'minTime': '9:00 AM', 'maxTime': '9:00 PM', step: '30' })
});

//Dont need leading zero for input(wont match the selector)
/* Reason for converting to string is for leading 0 with single digits (less than 10) */
/* Not because the input feilds require strings */
//function getHoursString(h) {
//    var c = get12HourClock(h);
//    var hString = (c < 10) ? "0" + c.toString() : c.toString();
//    return hString;
//}

//Change hour to 12 hour clock
function get12HourClock(h) {
    var clock = (h > 12) ? (h - 12) : h;
    return clock;
}

function getMinutesString(m) {
    var mString = (m < 10) ? "0" + m.toString() : m.toString();
    return mString;
}

function getMeridium(h) {
    //first is true
    var md = (h > 12) ? "pm" : "am";
    return md;
}