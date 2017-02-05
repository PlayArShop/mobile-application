using UnityEngine;
using UnityEngine.UI;

public class ForgotPassword : StartMenuPage
{
    public GameObject SignInPage;
    public InputField email;

    public void GoToSignInButtonEvent()
    {
        this.SignInPage.SetActive(true);
    }

    public void ValidateButtonEvent()
    {
        this._api.resetMyPassword(this.email.text, this.ResetResult);
    }

    public int ResetResult(int k, string l)
    {
        return k;
    }
}