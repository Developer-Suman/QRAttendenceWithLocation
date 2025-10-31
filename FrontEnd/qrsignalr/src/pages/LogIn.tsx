import { useEffect, useCallback } from "react";

declare global {
  interface Window {
    google: any;
  }
}

const GoogleLoginButton = () => {
  const handleCredentialLogin = useCallback(async (response: any) => {
    if (!response?.credential) return;

    console.log("Google credential:", response.credential);

    try {
      // ✅ Send token to backend for verification
      const res = await fetch(
        `${import.meta.env.VITE_API_URL}/api/v1/Auth/google-login`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ credential: response.credential }),
        }
      );

      const data = await res.json();

      if (data?.success) {
        localStorage.setItem("token", data.token ?? response.credential);
        window.location.href = "/";
      } else {
        console.error("Google login failed:", data.message);
      }
    } catch (err) {
      console.error("Error verifying Google token:", err);
    }
  }, []);

  useEffect(() => {
    const initializeGoogle = () => {
      if (!window.google) {
        console.error("Google Identity script not loaded");
        return;
      }

      window.google.accounts.id.initialize({
        client_id:
          "247235957979-c2e5dmpjadm4sq4f7juort7f3v9u4ok9.apps.googleusercontent.com",
        callback: handleCredentialLogin,
      });

      window.google.accounts.id.renderButton(
        document.getElementById("google-login-btn"),
        { theme: "outline", size: "large", width: "100%" }
      );
    };

    initializeGoogle();
  }, [handleCredentialLogin]);

  return (
    <div id="google-login-btn" className="flex justify-center mt-4">
      dasdas
    </div>
  );
};

export default GoogleLoginButton;
