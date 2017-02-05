using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerGoal : MonoBehaviour {

	// In case the ball enters the basket
    private static bool gameStarted = true;
    private bool postedScores;
    private PSApi.Communication _api;

    void Awake()
    {
        _api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Game started :" + gameStarted);
        if (gameStarted == false)
            return;
        if (other.gameObject.name != "Ball") {
            return;
        }
        print("Yay");
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        gameStarted = false;
        endGame();
    }

    void Start()
    {
        DontDestroyOnLoad(_api);

        gameStarted = true;
        postedScores = false;
    }

     public void endGame()
     {
        int score = 1000;
        gameStarted = false;

        GameObject.Find("ResultText").GetComponent<Text>().text = "You win!";

        if (postedScores == false) {
            _api.postScore("10", score.ToString(), handleResponsePostScore);
            postedScores = true;
        }
    }

        IEnumerator LoadMidScene()
        {
            yield return new WaitForSeconds(3);
            Application.LoadLevel("@main");
        }

        private int handleResponsePostScore(int r, string err)
        {
            // TODO need to handle the result, and show win or loose depending to response
            Debug.Log("api result " + _api.Score.result);
            GameObject.Find("ResultText").GetComponent<Text>().text = _api.Score.result;
            StartCoroutine(LoadMidScene());
            return 0;
        }
}
