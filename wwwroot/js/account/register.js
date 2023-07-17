(function () {

    var remoteServerURl = "";
    $(function () {
        getRemoteServerURl();
        getUserTypes();
    });

    function loginToApp(data) {

    }

    function getRemoteServerURl() {
        //$.ajax({
        //    type: 'GET',
        //    url: '/common/getConfigurationValue',
        //    data: { section: "app", paramName = "remoteServerUrl" }
        //}).done(function (paramsValue) {
        //    console.log(paramsValue)
        //})

        var ReceivedURL = GetURL();
        remoteServerURl = ReceivedURL;
    }

    function getUserTypes() {
        $.ajax({
            type: 'GET',
            url: 'UserManagement/GetRoles'
        }).done(function (data) {
            console.log(data)
        })
    }





})();