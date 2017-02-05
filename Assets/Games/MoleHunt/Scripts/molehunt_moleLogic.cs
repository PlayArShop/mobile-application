using UnityEngine;
using System.Collections;

namespace moleHunt
{
    public class molehunt_moleLogic : MonoBehaviour
    {
        private static float S_TOP_POS_X = 0.09f;

        private Vector3 topMarker;
        private Vector3 downMarker;

        private float speed = 0.5F;

        private float startTime;
        private float journeyLength;

        private bool isMoving = false;
        private bool up = false;

        private bool call = true;

        #region Unity Methods
            // Use this for initialization
        void Start()
        {
            setMarkers();
        }

        // Update is called once per frame

        void Update()
        {
            if (!isMoving)
            {
                if (call)
                {
                    StartCoroutine(CoroWait());
                    call = false;
                }
            }
            else
            {
                if (up) moveMole(0);
                else moveMole(-1);
            }
        }

        /* This function is a Coroutine witch pause the script */
        IEnumerator CoroWait()
        {
            float seconds = Random.Range(1F, 5F);
            yield return new WaitForSeconds( seconds );

            up = (up == false) ? true : false;
            isMoving = true;
            resetLerp( (up) ? topMarker : downMarker , (!up) ? topMarker : downMarker);
            call = true;
        }
       
        #endregion

        #region Logic Methods

        public bool getState()
        {
            return up;
        }

        /* This function set the markers for the lerp transformation */
        private void setMarkers()
        {
            // set the topMarker
            topMarker = new Vector3(
                gameObject.transform.localPosition.x,
                S_TOP_POS_X,
                gameObject.transform.localPosition.z);

            // set the downMarker
            downMarker = gameObject.transform.localPosition;
            // NOTE : The downMarker is the initial position
        }

        private void resetLerp(Vector3 startMarker, Vector3 endMarker)
        {
            startTime = Time.deltaTime;
            journeyLength = Vector3.Distance(startMarker, endMarker);
            isMoving = true;
        }
        
        private void moveMole(int K)
        {
            Vector3 startMarker;
            Vector3 endMarker;

            startMarker = (K == 0) ? topMarker : downMarker;
            endMarker = (K == 0) ? downMarker : topMarker;

            transform.localPosition = Vector3.Lerp(startMarker, endMarker, Time.deltaTime / 100 );

            if (up && (transform.localPosition.y - S_TOP_POS_X > -0.05 && transform.localPosition.y - S_TOP_POS_X < 0.05))
            {
                //toggleTap(1);
                isMoving = false;
            }
            if (!up && (transform.localPosition.y - downMarker.y > -0.05 && transform.localPosition.y - downMarker.y < 0.05))
            {
                //toggleTap(0);
                isMoving = false;
            }
        }

        private void toggleTap(int K)
        {
            if (K != 0)
                GetComponent<Renderer>().material.color = Color.magenta;
            else
                GetComponent<Renderer>().material.color = Color.white;
        }

        #endregion
    }
}