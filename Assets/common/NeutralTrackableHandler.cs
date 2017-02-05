/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class NeutralTrackableHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        private bool isAskingApi = false;
        private PSApi.Communication _api;

        private TrackableBehaviour mTrackableBehaviour;

        public GameObject[] objectsInRotation;

        public GameObject[] rainbowItems;

        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Awake()
        {
            _api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
        }

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

        private int AskApiTarget(string tName)
        {
            if (this.isAskingApi == true)
            {
                return -2;
            }
            this.isAskingApi = true;
            Debug.Log("We ask for :" + tName);
            _api.getTarget(tName, GetTargetCallBack);
            return 0;
        }

        private int GetTargetCallBack(int r, string err)
        {
            Debug.Log("Informations de la target sont lancées");

            Debug.Log(_api.Target.toJSON());

            switch (_api.Target.Games)
            {
                case 1:
                    Application.LoadLevel("TreasureHunt");
                    break;
                case 2:
                    Application.LoadLevel("Ballons");
                    break;
                case 3:
                    Application.LoadLevel("BeerPong");
                    break;
                case 4:
                    Application.LoadLevel("molehunt");
                    break;
                case 5:
                    Application.LoadLevel("PokerMachine");
                    break;
                case 6:
                    Application.LoadLevel("ScratchTicket");
                    break;
                case 7:
                    Application.LoadLevel("basketball");
                    break;
  
                default:
                    Debug.Log("Didn't know witch game to launch");
                    break;

            }
            return 0;
        }

        private void OnTrackingFound()
        {
            AskApiTarget(mTrackableBehaviour.TrackableName);
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            foreach (GameObject item in this.objectsInRotation)
            {
                item.GetComponent<RandomRotationOverTime>().max = 500f;
                item.GetComponent<RandomRotationOverTime>().min = 500f;
            }

            foreach (GameObject item in this.rainbowItems)
            {
                item.GetComponent<DefaultAndRainbowMode>().rainbowMode = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            foreach (GameObject item in this.objectsInRotation)
            {
                if (!item) continue;
                item.GetComponent<RandomRotationOverTime>().min = 10f;
                item.GetComponent<RandomRotationOverTime>().max = 100f;
            }

            foreach (GameObject item in this.rainbowItems)
            {
                if (!item) continue;
                item.GetComponent<DefaultAndRainbowMode>().rainbowMode = false;
            }
          
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
