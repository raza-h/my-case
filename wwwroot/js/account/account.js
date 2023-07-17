(function () {
    var remoteServerURl = "";
    $(function () {
        getRemoteServerURl();
    });

    function loginToApp(data) {
        
    }

    function getRemoteServerURl() {
        $.ajax({
            type: 'GET',
            url: '',
            data: { section: "app", paramName = "remoteServerUrl" }
        }).done(function (paramsValue) {
            console.log(paramsValue)
        })
    }

    



})();