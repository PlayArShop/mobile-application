using UnityEngine;
using Facebook.Unity;

public class StartMenuPage : MonoBehaviour
{
    protected PSApi.Communication _api;
    
    public void Awake()
    {
        this._api = GameObject.Find("/@ComAPI").GetComponent<PSApi.Communication>();
        if (!FB.IsInitialized)
        {
            FB.Init(() => 
            {
                if (FB.IsInitialized && FB.IsLoggedIn) // If connected from previous sessions, logout
                {
                    FB.LogOut();
                }
            });
        }
    }

    public void OnEnable()
    {
        Transform parent = this.transform.parent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            GameObject child = parent.GetChild(i).gameObject;
            if (child.activeSelf && child.gameObject != this.gameObject)
            {
                child.SetActive(false);
            }
        }
    }
}