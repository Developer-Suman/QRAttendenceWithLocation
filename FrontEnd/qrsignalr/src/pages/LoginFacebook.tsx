declare global {
  interface Window {
    fbAsyncInit: () => void;
    FB: any;
  }
}

export class FacebookAuthService {
  private appId: string;

  constructor(appId: string) {
    this.appId = appId;
    this.initSDK();
  }

  private initSDK() {
    window.fbAsyncInit = () => {
      window.FB.init({
        appId: this.appId,
        cookie: true,
        xfbml: true,
        version: "v16.0",
      });
    };
  }

  login(): Promise<string | null> {
    return new Promise((resolve) => {
      window.FB.login(
        (response: any) => {
          if (response.authResponse?.accessToken) {
            resolve(response.authResponse.accessToken);
          } else {
            resolve(null);
          }
        },
        { scope: "email,public_profile" }
      );
    });
  }
}
