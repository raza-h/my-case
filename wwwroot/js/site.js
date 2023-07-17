function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
function validatePassword(password) {
    var re = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*.?&])[A-Za-z\d@$!%*.?&]{6,}$/;
    return re.test(String(password));
}
function validatePhone(phone) {
    var re = /^\d{7,16}$/;
    return re.test(String(phone));
}
function validateCaseName(casename) {
    var re = /^[A-Za-z\s]{3,}$/; 
    return re.test(String(casename));
}
function validateCaseNo(caseno) {
    var re = /^\d{1,}$/;
    return re.test(String(caseno));
}
function validateWebsite(website) {
    var re = /(:?(?:https?:\/\/)?(?:www\.)?)?[-a-z0-9]+\.(?:com|gov|org|net|edu|biz|co)/;
    return re.test(String(website));
}
function validateZip(zipcode) {
    var re = /^[0-9]{5}(?:-[0-9]{4})?$/;
    return re.test(String(zipcode));
}
function showLoader() {
    $("#overlay").show();
}
function hideLoader() {
    $("#overlay").hide();
}
function openpopup(id = "", text = "", method = "") {
    $("#taskid").val(id);
    $("#btn-yes").attr("onclick", method);
    $("#confirmation-text").text(text);
    $("#myModall").modal('show');
}
function closepopup() {
    $("#myModall").modal('hide');
}
function closesubpopup() {
    $("#SubModal").modal('hide');
}
function validateState(state){
    var re = /^[A-Za-z]{2,20}$/;
    return re.test(String(state));
}
function validateCity(city) {
    var re = /^[A-Za-z]{2,30}$/;
    return re.test(String(city));
}
function validateFirstName(firstname) {
    var re = /^[A-Za-z\s]{3,}$/;
    return re.test(String(firstname));
}
function validateLastName(lastname) {
    var re = /^[A-Za-z\s]{1,}$/;
    return re.test(String(lastname));
}
function validateFirmName(firmname) {
    var re = /^[A-Za-z\s]{3,}$/;
    return re.test(String(firmname));
}
function validateOwnerName(ownername) {
    var re = /^[A-Za-z\s]{3,}$/;
    return re.test(String(ownername));
}
