using UnityEngine;
using System.Collections;

public class WinDecoration : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find( "Explosion" ).GetComponent<ParticleSystem>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
