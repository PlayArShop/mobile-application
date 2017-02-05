using UnityEngine;

public class ForceLandscape : MonoBehaviour {

	private bool isPortrait;
	public GameObject [] onlyInLandscape;
	public GameObject [] onlyInPortrait;

	// Use this for initialization
	void Start () {
		if (Input.deviceOrientation == DeviceOrientation.Portrait ||
			Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
			isPortrait = true;
			activePortraitObjects(true);
			activeLandscapeObject(false);			
		} else {
			isPortrait = false;
			activePortraitObjects(false);
			activeLandscapeObject(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isPortrait == false) {
			Debug.Log("Hello");
			Debug.Log(Input.deviceOrientation);
			if (Input.deviceOrientation == DeviceOrientation.Portrait ||
				Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				isPortrait = true;
				activePortraitObjects(true);
				activeLandscapeObject(false);
			}
		}
		else // isPortrait == true
		{
			if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
				Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
				isPortrait = false;
				activePortraitObjects(false);
				activeLandscapeObject(true);
			}
		}
	}

	void activePortraitObjects(bool activation) {
		foreach (GameObject i in onlyInPortrait) { 
			i.SetActive(activation);
		}
	}

	void activeLandscapeObject(bool activation) { 
		foreach (GameObject i in onlyInLandscape) {
			i.SetActive(activation);
		}
	}
}
