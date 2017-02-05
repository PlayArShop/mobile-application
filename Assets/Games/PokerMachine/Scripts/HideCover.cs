using UnityEngine;
using System.Collections;

public class HideCover : MonoBehaviour {

	public bool Anim_Hide = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0))
		{
			gameObject.GetComponent<Animation>().Play ("CoverDown");
			Debug.Log ("Pressed left click.");
		}
	}

	void OnMouseDown()
	{
		Debug.Log ("on mouse down");
	}

	void OnMouseClick()
	{
		Debug.Log ("j'ai cliquer dessus");
	}
}
