using UnityEngine;
using System.Collections;

public class SlotMachineBoot : MonoBehaviour {
	
	public string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
	// Use this for initialization
	IEnumerator Start () {
	
		WWW www = new WWW(url);

		Debug.Log ("creation www");

		yield return www;
		Debug.Log (" retour de yield");

		GetComponent<Renderer>().material.mainTexture = www.texture;
		Debug.Log ("application de la texture");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
