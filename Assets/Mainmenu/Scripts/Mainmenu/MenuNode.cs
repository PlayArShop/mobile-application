using UnityEngine;

namespace Assets.Scripts.Menus
{
    public abstract class MenuNode : MonoBehaviour
    {
        protected PSApi.Communication _api;
        protected bool _toDestroy;
        protected abstract void MenuUpdate();
        protected abstract void MenuCreation();

        public void Update()
        {
            if (this._toDestroy)
            {
                Object.Destroy(this._api);
                Object.Destroy(this.gameObject);
            }
            else
            {
                this.MenuUpdate();
            }
        }

        public virtual void Awake()
        {
            this._api = null;
            if (this.transform.parent == null || this.transform.parent.gameObject == null)
            {
                DontDestroyOnLoad(this.transform.gameObject);
            }
            try
            {
                this._api = GameObject.Find("/@ComAPI").GetComponent<PSApi.Communication>();
            }
            catch (System.Exception)
            {
                Debug.LogError("/!\\ Missing PSApi global object /!\\");
            }
            this.MenuCreation();
        }

        protected void Kill()
        {
            this._toDestroy = true;
        }
    }
}