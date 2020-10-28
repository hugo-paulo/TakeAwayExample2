function validateName(e) {
    if (e.value.toString() === '') {
        //Why wont the after work, everything else works?
        document.getElementById("txtName").classList.add("err-asterisk");
    }
}

function validateNumber(e) {
    if (e.value.toString() === '')
        document.getElementById("txtPhone").classList.add("err-asterisk");
}

function validateForm() {
    var n = document.getElementById("txtName").value;
    var p = document.getElementById("txtPhone").value;
    var d = document.getElementById("datepicker").value;
    var t = document.getElementById("timepicker").value;

    //Use the toString method to make a true bool and not truethy
    if (n.toString() === '') {
        swal("Name field is empty.", "Please provide us your name.", "warning");
        return false;
    }

    if (p.toString() === '') {
        swal("Phone Number field is empty.", "Please provide us your phone number.", "warning");
        return false;
    }

    if (d.toString() === '') {
        swal("Date field is empty.", "Please provide us the date you wish your order tso be done", "warning");
        return false;
    }

    if (t.toString() === '') {
        swal("Time field is empty.", "Please provide us the time you wish your order to be done.", "warning");
        return false;
    }
}