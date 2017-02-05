using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PokerMachine
{
	public class PM_Logic : MonoBehaviour {


		private bool gameStarted;
		private PSApi.Communication _api;

		void Awake()
		{
			_api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
		}

		// Use this for initialization
		void Start () {
			DontDestroyOnLoad(_api);

			gameStarted = false;
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		public void startGame()
        {
            gameStarted = true;
        }

        public void endGame()
        {
            int score = 150; //GameObject.Find("moles").GetComponent<moleHunt.molehunt_scoreHandling>().getScore();
            gameStarted = false;

            GameObject.Find("ResultText").GetComponent<Text>().text = score < 100 ? "Nice try.." : "You won!";

            GameObject.Find("ResultScore").GetComponent<Text>().text = score.ToString();

            //GameObject endScreen = GameObject.Find("EndScreen");           
            _api.postScore("Gagne !", score.ToString(), handleResponsePostScore);
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

	}

}