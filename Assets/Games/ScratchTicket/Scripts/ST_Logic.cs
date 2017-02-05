using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ST_Logic : MonoBehaviour {

	int winLoose;
	private PSApi.Communication _api;

	void Awake()
	{
		_api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
	}
	void Start () {
		winLoose = Random.Range(0, 100) % 2;
		Debug.Log("winloose" + winLoose);
		if (winLoose == 0) {
			GameObject.Find("LOST_DISPLAY").SetActive(false);
		}
		else {
			GameObject.Find("WIN_DISPLAY").SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void End() {

		if (winLoose == 0) {
			GameObject.Find("ResultText").GetComponent<Text>().text =  "You won!";
			_api.postScore(_api.Target.game_id, "You won !", handleResponsePostScore);
			Debug.Log("GG");
		}
		else {
			GameObject.Find("ResultText").GetComponent<Text>().text = "Nice try..";
			_api.postScore(_api.Target.game_id, "Lost", handleResponsePostScore);
			Debug.Log("kappa");
		}
		StartCoroutine(LoadMidScene());
	}


	IEnumerator LoadMidScene()
	{
		yield return new WaitForSeconds(3);
		Application.LoadLevel("@main");
	}

	private int handleResponsePostScore(int r, string err)
	{
		// TODO need to handle the result, and show win or loose depending to response
		// GameObject.Find("ResultText").GetComponent<Text>().text = _api.Score.result;
		StartCoroutine(LoadMidScene());
		return 0;
	}
}
