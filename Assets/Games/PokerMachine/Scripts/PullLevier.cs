/**
 * @Author : Aksels Ledins
 * @Email : aksels.ledins@gmail.com
 * @Tél : (+33) 6 99 43 38 40
 * @Date : 08/02/2015
 * @Update : 15/02/2015
 * 
 * Personnalisation of an Unity's GameObject w/ an
 * picture downloaded from the internet
 * 
 */

using UnityEngine;
using System.Collections;

/**
 * (Class) When we pull the "Levier" (mdr), launch the game
 * Extends MonoBehaviour
 */
public class PullLevier : MonoBehaviour {

	/**
	 * (Bool)
	 * used to lightweight the update fonction, as the RayCast is a heavy operation
	 * ATM, the user can play only once the game
	 */
	private bool static_once;

	/**
	 * (Float)
	 * Max Picking Distance
	 */
	private float maxPickingDistance = 10000f;
	
	// (Void) not used
	void Start () {}

	// (Void) Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			gameObject.GetComponent<Animation>().Play ("Levier Pull");
			GameObject.Find("Cover").GetComponent<Animation>().Play ("CoverDown");
			RandomResult res = GetComponent<RandomResult>();
			res.enabled = true;

		}

		/* Loop over all the Touch events */
		foreach (Touch touch in Input.touches) {
			/* Check if it's at the beginning phase of touching, AK the user is not dragging his finger everywhere */
			if (touch.phase == TouchPhase.Began) {
				/* Gets the ray at position where the screen is touched */
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit, maxPickingDistance)) { 
					Transform pickedObject = null;
					pickedObject = hit.transform;
					Debug.Log ("Name of Object : " + pickedObject.name);
					if (pickedObject.name.Equals("Cover"))
					{
						gameObject.GetComponent<Animation>().Play ("Levier Pull");
						GameObject.Find("Cover").GetComponent<Animation>().Play ("CoverDown");
						RandomResult res = GetComponent<RandomResult>();
						res.enabled = true;
					}
				}
			}
		}
	}
}
