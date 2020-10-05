//This is called by button html tag Upsert.cshtml line 96 of MenuItem

function validateInput() {
    if (document.getElementById("uploadBox").value == "") {
        swal("Error", "Please Select an Image.", "error");
        return false;
    }

    return true;
}