using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomResult : MonoBehaviour {

	// Use this for initialization


	public GameObject ui_goodjob;
	public GameObject ui_score;
	public GameObject ui_score_post;
	public GameObject ui_reduction;

	public GameObject[] row1;
	public GameObject[] row2;
	public GameObject[] row3;

	void Start () {
		firstRow = new RowRandom (row1 [0], row1 [1], row1 [2]);
		secondRow = new RowRandom (row2 [0], row2 [1], row2 [2]);
		thirdRow = new RowRandom (row3 [0], row3 [1], row3 [2]);
	}
	
	// Update is called once per frame
	private   RowRandom firstRow;
	private   RowRandom secondRow;
	private   RowRandom thirdRow;
	 
	private bool haswin = false;

	private PSApi.Communication _api;

	void Awake()
	{
		_api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
	}

	IEnumerator LoadMidScene()
	{
		Debug.Log("win yield");
		yield return new WaitForSeconds(3);
		Debug.Log("win end yield");
		
		Application.LoadLevel("@main");
	}


	private int handleResponsePostScore(int r, string err)
	{
		// TODO need to handle the result, and show win or loose depending to response
		// GameObject.Find("ResultText").GetComponent<Text>().text = _api.Score.result;
		// ui_score.GetComponent<Text>().text = _api.Score.result;
		StartCoroutine(LoadMidScene());
		return 0;
	}


	IEnumerator Win()
	{
		yield return new WaitForSeconds(1);

		ui_goodjob.SetActive(false);
	
		ui_reduction.SetActive(true);
		_api.postScore(_api.Target.game_id, "You won !", handleResponsePostScore);
	}

	IEnumerator Lost()
	{
		yield return new WaitForSeconds(1);

		ui_goodjob.SetActive(false);
	
		ui_reduction.SetActive(true);
		_api.postScore(_api.Target.game_id, "Lost", handleResponsePostScore);
	}

	void Update () {
		if (haswin)
			return;
		bool res1, res2, res3;

		res1 = firstRow.UpdateSet ();
		res2 = secondRow.UpdateSet ();
		res3 = thirdRow.UpdateSet ();

		if (!res1 && !res2 && !res3)
		{
			haswin = true;
			WinDecoration winEngine = GameObject.Find("Decoration").GetComponent<WinDecoration>();
			winEngine.enabled = true;
			
			Debug.Log("did i won");

			if (firstRow.getMiddle().GetComponent<Renderer>().material.name == secondRow.getMiddle().GetComponent<Renderer>().material.name
			    && secondRow.getMiddle().GetComponent<Renderer>().material.name ==  thirdRow.getMiddle().GetComponent<Renderer>().material.name) {
		
				ui_goodjob.GetComponent<Text>().text = "Lucky !";
				ui_goodjob.SetActive(true);
				StartCoroutine(Win()); 
	
				Debug.Log("GAGNER");
			}
			else {
				Debug.Log("perdu");
				StartCoroutine(Lost()); 
				ui_goodjob.GetComponent<Text>().text = "Unlucky";
				ui_goodjob.SetActive(true);
			}
			Debug.Log("Loading main");
			StartCoroutine(LoadMidScene());
		}
			return;
	}
}
