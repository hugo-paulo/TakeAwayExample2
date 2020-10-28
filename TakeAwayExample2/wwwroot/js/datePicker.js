$(function () {
    //currently uses the datepicker cdn but can download the jquery-timepicker-jt package from nuget
    //the second datepicker chain populates the current date on the input (the setDate seem js native mathods)
    $("#datepicker").datepicker({ minDate: 1, maxDate: "+1W"}).datepicker("setDate", new Date())
});