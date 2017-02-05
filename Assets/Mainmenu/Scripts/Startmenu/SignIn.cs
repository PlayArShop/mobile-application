using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using SimpleJSON;
using System.Collections.Generic;

public class SignIn : StartMenuPage
{
    public InputField email;
    public InputField password;
    public Text resultText;
    public GameObject SignUpPage;
    public GameObject ForgotPasswordPage;
	
    // Callback when Signin button is pressed
    public void SignInButtonEvent()
    {
        this.resultText.text = "";
        // Debug secret :p
        if (this.email.text.Length == 0 && this.password.text.Length == 0)
        {
            this.email.text = "dev@playarshop.com";
            this.password.text = "password";
        }

        // Call API with form values
        this._api.postLogin(this.email.text, this.password.text, this.SigninCallBack);
    }

    private void GoToSignUpButtonEvent()
    {
        this.SignUpPage.SetActive(true);
    }

    private void GoToForgotPasswordButtonEvent()
    {
        this.ForgotPasswordPage.SetActive(true);
    }

    // API callback with result
    private int SigninCallBack(int result, string error)
    {
        if (result != 0)
        {
            this.resultText.text = "Identifiants incorrects";
            return 1;
        }
        // Start mainmenu scene
		SceneManager.LoadScene("@main");
        return 0;
    }


    // https://developers.facebook.com/tools/explorer/
    // https://developers.facebook.com/docs/facebook-login/multiple-providers
    // Doesn't get the same ID from within the Unity Editor :^)
    private void FacebookAuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            FB.API("/me?fields=id,email", HttpMethod.GET, (IGraphResult res) => 
            {
                // generation of a obfuscated password based on the ID
                string fbId = (JSON.Parse(res.RawResult) as JSONNode)["id"].Value;
                bool add_sub = ((int)char.GetNumericValue(fbId[(int)char.GetNumericValue(fbId[0])]) % 2) != 0;
                string passwd = "";
                foreach (char c_ in fbId)
                {
                    char c = (char)char.GetNumericValue(c_);
                    passwd += (char)((c % 2 == 0 ? 'F' : 'B') + (add_sub ? c : -c));
                    add_sub = !add_sub;
                }

                // Try to sign up then try to sign-in
                SignUp signUp = SignUpPage.GetComponent<SignUp>();
                signUp.email.text = (JSON.Parse(res.RawResult) as JSONNode)["email"].Value;
                signUp.password.text = passwd;
                signUp.SignUpButtonEvent(this._api);

                // if this code is reached sign up failed
                this.email.text = signUp.email.text;
                this.password.text = signUp.password.text;
                
                this.SignInButtonEvent();
                signUp.clearAll();
            });
        }
        else
        {
            this.resultText.text = "Erreur d'authentification Facebook";
        }
    }

    private void FacebookSigninButtonEvent()
    {
        this.resultText.text = "";
        FB.LogInWithReadPermissions(new List<string>() {"email"}, this.FacebookAuthCallback);
    }
}