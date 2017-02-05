using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;

namespace Assets.Scripts.Menus
{
    public class Menu : MenuNode
    {
        public void Start()
        {
            if (this._api == null) this.SwitchToSignInUp();
        }

        public void SwitchToSignInUp()
        {
            FB.LogOut();
            this.Kill();
            if (this._api != null) Object.Destroy(this._api.gameObject);
            SceneManager.LoadScene("@start");
        }

        protected override void MenuUpdate() {}
        protected override void MenuCreation() {}
    }
}