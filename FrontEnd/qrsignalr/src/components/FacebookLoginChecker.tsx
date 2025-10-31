import { useEffect } from "react";

declare global {
  interface Window { FB: any }
}

const FacebookLoginChecker = () => {

  useEffect(() => {
    // Wait until SDK is loaded
    const checkFBLoginStatus = () => {
      if (window.FB) {
        window.FB.getLoginStatus(function(response: any) {
          statusChangeCallback(response);
        });
      }
    };

    // Check periodically if FB SDK loaded
    const interval = setInterval(() => {
      if (window.FB) {
        clearInterval(interval);
        checkFBLoginStatus();
      }
    }, 100);

    return () => clearInterval(interval);
  }, []);

  const statusChangeCallback = (response: any) => {
    console.log("FB login status:", response);

    if (response.status === "connected") {
      // User logged in & authorized
      console.log("Access Token:", response.authResponse.accessToken);
    } else {
      // User not logged in or not authorized
      console.log("User not logged in");
    }
  };

  return null; // This component doesn’t render anything
};

export default FacebookLoginChecker;
