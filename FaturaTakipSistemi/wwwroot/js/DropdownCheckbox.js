var expanded = false;

function showCheckboxes() {
    var checkboxes = document.getElementById("checkboxes");
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}
//-----------------------------------------------------

$(document).ready(function () {
    $('#SelectedRoles input[type="checkbox"]').on('click', function () {
        $(this).attr('checked', !$(this).attr('checked'));
    });
});