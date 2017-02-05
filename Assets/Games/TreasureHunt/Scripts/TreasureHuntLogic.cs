using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TreasureHuntLogic : MonoBehaviour {

	public GameObject [] selectionSquares;

	private GameObject vLayout;
	private string [] urls;

	private string [] targets_name;
	private bool [] targets_status;
	private int targetTofindIdx;

	// UI
	public GameObject ui_goodjob;
	public GameObject ui_score;
	public GameObject ui_score_post;
	public GameObject ui_restartBtn;
	public GameObject ui_restartBtnText;

	public GameObject ui_reduction;

	private bool isAskingApi = false;
	private PSApi.Communication _api;

	// Use this for initialization
	void Start () {
		_api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
		targetTofindIdx = 0;
		vLayout = GameObject.Find("VLayout");

		urls = new string[_api.Target.Targets.Count];
		targets_name = new string[_api.Target.Targets.Count];
		targets_status = new bool[_api.Target.Targets.Count];
		int ii = 0;
		foreach (PSApi.Communication.TargetData targetData in _api.Target.Targets)
		{
			urls[ii] = targetData.Url;
			targets_name[ii] = targetData.Name;
			targets_status[ii] = false;
			ii++;
		}
		selectionSquares[targetTofindIdx].GetComponent<DefaultAndRainbowMode>().enabled = true;

		Debug.Log(urls);
		ui_score_post.GetComponent<Text>().text = "/"+targets_name.Length.ToString();
	
		gameObject.GetComponent<LoadTargetsPreview>().LoadUrls(urls);
	}
	
	IEnumerator LateCall()
	{
		yield return new WaitForSeconds(1);
		
		ui_goodjob.SetActive(false);
		ui_score.GetComponent<Text>().text = targetTofindIdx.ToString();
		
		ui_restartBtnText.GetComponent<Text>().text = "Touch me to continue";
		ui_restartBtn.SetActive(true);

		ui_score.GetComponent<Animator>().Play("TriggerManual");
    }

	IEnumerator LateCallFail()
	{
		yield return new WaitForSeconds(1);
		
		ui_goodjob.SetActive(false);
		ui_restartBtnText.GetComponent<Text>().text = "Touch me to try again";
		ui_restartBtn.SetActive(true);
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
		ui_score.GetComponent<Text>().text = _api.Score.result;
		StartCoroutine(LoadMidScene());
		return 0;
	}


	IEnumerator Win()
	{
		yield return new WaitForSeconds(1);
		
		ui_goodjob.SetActive(false);
		
		targetTofindIdx = targets_name.Length;
		ui_score.GetComponent<Text>().text = targetTofindIdx.ToString();
		ui_score.GetComponent<Animator>().Play("TriggerManual");
    	
		ui_reduction.SetActive(true);
		_api.postScore("4", "1337", handleResponsePostScore);
	}


	public void detectTarget(string tname) {
		if (targetTofindIdx >= targets_name.Length)
			return;

		if (targets_name[targetTofindIdx] == tname) {

			// If win
			if (targetTofindIdx + 1 == targets_name.Length) {
				ui_goodjob.GetComponent<Text>().text = "Yay you won !";
				ui_goodjob.SetActive(true);
				StartCoroutine(Win()); 
				return;
			}
			else
			{
				StartCoroutine(LateCall());


				ui_goodjob.GetComponent<Text>().text = "Good job !";
				ui_goodjob.SetActive(true);

				selectionSquares[targetTofindIdx].GetComponent<DefaultAndRainbowMode>().enabled = false;
				targetTofindIdx += 1;			
				selectionSquares[targetTofindIdx].GetComponent<DefaultAndRainbowMode>().enabled = true;
				ui_score.GetComponent<Animator>().Play("TriggerManual");
			}
		}
		else  {
			StartCoroutine(LateCallFail());
			ui_goodjob.GetComponent<Text>().text = "Wrong target !";
	        ui_goodjob.SetActive(true);
		}
	}

	public void disableBtn() {
		ui_restartBtn.SetActive(false);
		return;
	}
}
