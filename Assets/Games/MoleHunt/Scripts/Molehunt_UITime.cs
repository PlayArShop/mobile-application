using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace moleHunt
{
    public class Molehunt_UITime : MonoBehaviour
    {
        moleHunt.Molehunt_logic scriptLogic;
        // Use this for initialization
        void Start()
        {
            scriptLogic = GameObject.Find("GameLogic").GetComponent<moleHunt.Molehunt_logic>();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Slider>().value = scriptLogic.getTimer();
        }
    }
}