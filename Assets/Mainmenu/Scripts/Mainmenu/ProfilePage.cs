using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Assets.Scripts.Menus
{
    public class ProfilePage : MenuNode
    {
        public InputField username;
        public InputField location;
        public InputField email;
        public InputField password;
        public Text resultText;
        public Menu mainmenu;

        public void OnEnable()
        {
            this._api.fetchMyProfile(this.UpdateForm);
            if (FB.IsLoggedIn)
            {
            }
        }

        public void OnDisable()
        {
            this.resultText.text = "";
        }

        public int UpdateForm(int k, string l)
        {
            if (k == 0)
            {
                this.email.text = this._api.User.profile.email;
                this.username.text = this._api.User.profile.username;
                this.location.text = this._api.User.profile.location;
            }
            return k;
        }

        public void SignOutButtonEvent()
        {
            this.mainmenu.SwitchToSignInUp();
        }

        public void ValidateButtonEvent()
        {
            this.resultText.text = "";
            this._api.updateMyProfile(this.email.text, this.password.text, this.username.text, this.location.text, this.UpdateResult);
        }

        public int UpdateResult(int k, string l)
        {
            this.resultText.text = (k == 0 ? "Succès" : "Echec");
            return k;
        }

        protected override void MenuUpdate() {}
        protected override void MenuCreation() {}
    }
}