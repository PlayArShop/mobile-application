using UnityEngine;
using System.Collections;

namespace moleHunt
{
    public class molehunt_moleTap : MonoBehaviour
    {
        private Transform pickedObject = null;
        private Vector3 lastPlanePoint;
        
        #region UNITY Methods

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            eventInput();
        }

        #endregion

        /*
         * Handles the tap/click event and check if it hits a mole
         */
        private void eventInput()
        {
            Plane targetPlane = new Plane(transform.up, transform.position);

            /*
             * This event covers the left button of a mouse but also the touch input
             * on android & iOS devices (supposed)
             */
            if (Input.GetButtonDown("Fire1"))
            {
                //Gets the position of ray along plane
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float dist = 0.0f;

                //Intersects ray with the plane. Sets dist to distance along the ray where intersects
                targetPlane.Raycast(ray, out dist);

                //Returns point dist along the ray.
                Vector3 planePoint = ray.GetPoint(dist);

                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, 10000))
                { //True when Ray intersects colider. 
                    //If true, hit contains additional info about where collider was hit
                    pickedObject = hit.transform;

                    /* check if the picked object is a mole */
                    if ( pickedObject.name.Substring(0, 5) == "mole_" )
                    {
                        // check if the mole is visible and not hidden
                        if (pickedObject.GetComponent<molehunt_moleLogic>().getState() == true)
                        {
                            // TODO Add score here
                            if (pickedObject.FindChild("default").GetComponent<Renderer>().material.color != Color.red)
                            {
                                GetComponent<moleHunt.molehunt_scoreHandling>().tapMole();
                                pickedObject.FindChild("default").GetComponent<Renderer>().material.color = Color.red;
                                pickedObject.GetComponent<molehunt_moleLogic>().enabled = false;
                            }
                        }
                    }
                    lastPlanePoint = planePoint;
                }
            }
           
        }

    }
}