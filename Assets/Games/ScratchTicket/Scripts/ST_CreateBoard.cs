using UnityEngine;
using System.Collections.Generic;

public class ST_CreateBoard : MonoBehaviour {

	public GameObject Reference;

	public List<Object> floor;
	private bool gameStarted;
	// Use this for initialization
	void Start () {
		gameStarted = true;
		floor = new List<Object>();
		float startX = 0.5f;
		float startZ = 0.5f;
		int x, y;
		x = 0;
		for (startX = -0.5f ; startX <= 0.5f ; startX += 0.25f) {
			startZ = 0.5f;
			y = 0;
			for (startZ = -0.5f ; startZ <= 0.5f ; startZ += 0.25f) {
			 	Object newObject = Instantiate(Reference, new Vector3(startX, 0, startZ), new Quaternion());
				newObject.name = "BLOCK_"+x.ToString()+"_"+y.ToString();
				GameObject.Find(newObject.name).transform.parent = GameObject.Find("GameMain").transform;
				floor.Add(newObject);
				Debug.Log("created: "+"BLOCK_"+x.ToString()+"_"+y.ToString());
				y++;
			}
			x++;
			GameObject.Find("GameLogic").GetComponent<ST_Logic>().enabled = true;
		}

		Object.Destroy(Reference);

		foreach (Object item in floor) {
			Debug.Log("enable" + item.name);
			GameObject.Find(item.name).GetComponent<BoxCollider>().enabled = true;
			GameObject.Find(item.name).GetComponent<MeshRenderer>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("BLOCK_0_0")) {
			GameObject.Find("BLOCK_0_0").GetComponent<BoxCollider>().enabled = true;
			GameObject.Find("BLOCK_0_0").GetComponent<MeshRenderer>().enabled = true;	
		}
		if (floor.Count <= 2 && gameStarted == true) {
			GameObject.Find("GameLogic").GetComponent<ST_Logic>().End();
			gameStarted = false;
		}
	}

	public void TouchFloor(string name) {
		foreach (Object item in floor) {
			if (item.name == name) {
				Debug.Log("touche" + name);
				floor.Remove(item);
				Object.Destroy(item);
				return;
			}
		}
	}
}
