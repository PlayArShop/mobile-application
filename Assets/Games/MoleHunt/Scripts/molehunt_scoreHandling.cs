using UnityEngine;
using System.Collections;


namespace moleHunt
{
    public class molehunt_scoreHandling : MonoBehaviour
    {
        private int score;
        private int scorePerMole;

        // Use this for initialization
        void Start()
        {
            score = 0;
            scorePerMole = 10;
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region score handling methods
        public void tapMole()
        {
            score = score + scorePerMole;
            Debug.Log(score);
        }
        public int getScore()
        {
            return score;
        }
        #endregion
    }
}