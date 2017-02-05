using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace moleHunt
{
    public class Molehunt_logic : MonoBehaviour
    {
        static float TIME_LEFT = 20f;

        private bool gameStarted;
        private float timeLeft;
  //      private PSApi.Communication apiScript;
        private PSApi.Communication _api;

        void Awake()
        {
            _api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
        }

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(_api);

            gameStarted = false;
            resetTimer(TIME_LEFT);
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameStarted)
                return;
            if (updateTimer())
            {
                
            }
            else
            {
                Debug.Log("End the game");
                endGame();
            }
        }

        #region logic methods
        public void startGame()
        {
            resetTimer(TIME_LEFT);
            gameStarted = true;
        }

        public void endGame()
        {
            int score = GameObject.Find("moles").GetComponent<moleHunt.molehunt_scoreHandling>().getScore();
            gameStarted = false;

            //GameObject.Find("ResultText").GetComponent<Text>().text = score < 100 ? "You loose.." : "You win!";

            GameObject.Find("ResultScore").GetComponent<Text>().text = score.ToString();

            //GameObject endScreen = GameObject.Find("EndScreen");           
            _api.postScore("1", score.ToString(), handleResponsePostScore);
        }

        IEnumerator LoadMidScene()
        {
            yield return new WaitForSeconds(3);
            Application.LoadLevel("@main");
        }

        private int handleResponsePostScore(int r, string err)
        {
            // TODO need to handle the result, and show win or loose depending to response
            GameObject.Find("ResultText").GetComponent<Text>().text = _api.Score.result;
            StartCoroutine(LoadMidScene());
            return 0;
        }

        #region timer
        private bool updateTimer() { timeLeft -= Time.deltaTime; if (timeLeft <= 0) return false; return true; }
        private void resetTimer(float x) { timeLeft = x; }
        public float getTimer() { return timeLeft; }
        #endregion

        #endregion
    }
}