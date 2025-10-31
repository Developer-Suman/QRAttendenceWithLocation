import { useEffect } from "react";

declare global {
  interface Window {
    FB: any;
  }
}

const useFacebookSDK = (appId: string, version: string = "v16.0") => {
  useEffect(() => {
    if (document.getElementById("facebook-jssdk")) return;

    window.fbAsyncInit = function () {
      window.FB.init({
        appId,
        cookie: true,
        xfbml: true,
        version,
      });
    };

    const js = document.createElement("script");
    js.id = "facebook-jssdk";
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    document.body.appendChild(js);
  }, [appId, version]);
};

export default useFacebookSDK;
