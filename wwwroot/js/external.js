var cms = cms || {};
cms.login = {
    providers: {
        RedirUri: "http://localhost:25601/Attorney/OutlookIntegration/Index",

        LoginUrls: {
            "OutlookLoginUri": "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?response_type=code&scope=https://graph.microsoft.com/User.Read&client_id=c4e85028-7cf7-4fd3-a075-5e23ebd8ec8d&redirect_uri={REDIRECT_URI}",
      

        },

        getParameterByName: function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        },
    }

}