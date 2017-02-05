using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class SignUp : StartMenuPage
{
    public InputField email;
    public InputField password;
    public Text resultText;
    public GameObject SignInPage;

    private void GoToSignInButtonEvent()
    {
        this.SignInPage.SetActive(true);
    }

    public void SignUpButtonEvent(PSApi.Communication api = null)
    {
        if (api == null) api = this._api;
        this.resultText.text = "";
        api.postRegister(this.email.text, this.password.text, this.SignupCallback);
    }

    private IEnumerator WaitAfterSuccess(int sec)
    {
        yield return new WaitForSeconds(sec);
    }

    public void clearAll()
    {
        this.email.text = "";
        this.password.text = "";
        this.resultText.text = "";
    }

    private int SignupCallback(int result, string error)
    {
        SignIn signIn;

        if (result != 0)
        {
            try
            {
                JSONNode E = JSON.Parse(error);
                this.resultText.text = E["errors"];
            }
            catch (System.Exception)
            {
                this.resultText.text = "Identifiants incorrects";
            }
            return 1;
        }
        this.resultText.text = "Compte crée avec succès";
        this.WaitAfterSuccess(5);

        // Uses the SignIn page to do it
        signIn = SignInPage.GetComponent<SignIn>();
        signIn.email.text = this.email.text;
        signIn.password.text = this.password.text;
        signIn.SignInButtonEvent();
        return 0;
    }
}