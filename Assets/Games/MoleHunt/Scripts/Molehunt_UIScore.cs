using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace moleHunt
{
    public class Molehunt_UIScore : MonoBehaviour
    {

        private moleHunt.molehunt_scoreHandling scriptScore;
        // Use this for initialization
        void Start()
        {
            scriptScore = GameObject.Find("moles").GetComponent<moleHunt.molehunt_scoreHandling>();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Text>().text = scriptScore.getScore().ToString();
        }
    }
}