using UnityEngine;
using System.Collections;

public class KeepAlive : MonoBehaviour {

	public GameObject [] keepAliveObjects;

	void Awake() {
		foreach (GameObject item in keepAliveObjects) {
			DontDestroyOnLoad(item);
		}
	}
}
