using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {

    private PSApi.Communication _api;

    // Use this for initialization
    void Start () {
        _api = PSApi.Communication.Instance;

        Debug.Log("@main");
                
        DontDestroyOnLoad(_api);
    }
    
    // Update is called once per frame
    void Update () {
	
	}
}
