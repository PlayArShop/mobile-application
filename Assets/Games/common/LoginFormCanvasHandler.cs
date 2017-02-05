using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginFormCanvasHandler : MonoBehaviour {

    public InputField InputEmail;
    public InputField InputPassword;
    public Text Result;
    private PSApi.Communication apiScript;

    // Use this for initialization
    void Start () {
        apiScript = GameObject.Find("PSApi").GetComponent<PSApi.Communication>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoginButtonEvent()
    {
       apiScript.postLogin(InputEmail.text, InputPassword.text, LoginCallBack);
    }

    public int LoginCallBack(int err, string msg)
    {
        if (err == 1)
        {
            Result.text = "invalid credentials";
        }
        else
        {
            Result.text = "login success";
            apiScript.getTarget("2", TargetCallBack);
        }
        return 0;
    }

    public int TargetCallBack(int err, string msg)
    {
        if (err == 1)
        {
            Debug.Log("pas authorisé");
        }
        else
        {
            Debug.Log("Informations de la target récupéré");
            Debug.Log("Chargement du jeu:");
            Debug.Log(apiScript.Target.Games);

            if (apiScript.Target.Games == 2)
            {
                Debug.Log("chargement de mole hunt");
            }
        }   
        return 0;
    }
}
