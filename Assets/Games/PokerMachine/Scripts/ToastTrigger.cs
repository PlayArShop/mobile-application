using UnityEngine;
using System.Collections;

public class ToastTrigger : MonoBehaviour {

	Animator anim;
	private float maxPickingDistance = 2000000;// increase if needed, depending on your scene size
	private Transform pickedObject = null;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		foreach (Touch touch in Input.touches) 
		{
			Debug.Log("Touching at: " + touch.position);
			
			//Gets the ray at position where the screen is touched
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			Debug.Log("nice un peu");
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit, maxPickingDistance)) 
			{ 
				pickedObject = hit.transform;
				pickedObject.GetComponent<Animation>().Play("CoverDown");
				//anim.SetTrigger("TapSlot");
				Debug.Log("NICE");
			} 
			else
			{
				Debug.Log ("je pick rien");
				pickedObject = null;
			}
		}

	}
}
