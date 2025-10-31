import { useEffect, useState } from "react";
import { FacebookAuthService } from "../pages/LoginFacebook";
import useFacebookSDK from "./useFacebookSDK";

const FacebookLoginButton = () => {
  useFacebookSDK(import.meta.env.VITE_FACEBOOK_APP_ID);
  const [fbService, setFbService] = useState<FacebookAuthService | null>(null);

  useEffect(() => {
    const service = new FacebookAuthService(
      import.meta.env.VITE_FACEBOOK_APP_ID
    );
    setFbService(service);
  }, []);

  const handleLogin = async () => {
    if (!fbService) return;

    const accessToken = await fbService.login();
    if (!accessToken) return console.error("User cancelled Facebook login.");

    // ✅ Send token to backend for verification
    const res = await fetch(
      `${import.meta.env.VITE_API_URL}/api/v1/Auth/facebook-login`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ accessToken }),
      }
    );

    const data = await res.json();

    if (data.success) {
      localStorage.setItem("token", data.token ?? "");
      window.location.href = "/";
    } else {
      console.error("Facebook login failed:", data.message);
    }
  };

  return (
    <button
      onClick={handleLogin}
      className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
    >
      Login with Facebook
    </button>
  );
};

export default FacebookLoginButton;
