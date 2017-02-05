using UnityEngine;
using System.Collections;


namespace moleHunt
{
    public class molehunt_createMoles : MonoBehaviour
    {
        /*
         * Reference to the object (originObject) who will be cloned afterwards
         * with a distance of (offset) in x and z.
         */
        public GameObject originObject;
        public float offset;

        #region UnityMethods
        // Use this for initialization
        void Start()
        {
            instantiateMoles();
        }
        
        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Methods

        private void instantiateMoles()
        {
            for (int x = 0; x < 6 ; x++)
            {
                for (int z = 0; z < 6; z++)
                {
                    GameObject clone;
                    // reference to position & rotation for lisibility
                    Vector3 oP = originObject.transform.localPosition;
                    Quaternion oR = originObject.transform.localRotation;

                    // create a clone and set his parent and fix its localscale
                    clone = Instantiate(originObject, oP, oR) as GameObject;
                    clone.transform.SetParent (originObject.transform.parent);
                    clone.transform.localScale = originObject.transform.localScale;
                    
                    // move it to intended position
                    clone.transform.localPosition = new Vector3(oP.x - x * offset, oP.y, oP.z + z * offset);

                    // activate the object
                    clone.GetComponent<MeshRenderer>().enabled = true;
                    clone.SetActive(true);

                    // change name
                    clone.name = "mole_" + x + "_" + z;
                }
            }
        }

        #endregion
    }
}