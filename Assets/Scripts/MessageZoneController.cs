using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class MessageZoneController : MonoBehaviour
    {
        public bool presentOnlyOnce = true;
        private bool presentedOnce = false;

        private void OnTriggerEnter(Collider other)
        {
            if (presentOnlyOnce == false || presentedOnce == false)
            {
                presentedOnce = true;

                string s = SceneManager.Instance.textDictionary["tutorial-altar1"];
                SceneManager.Instance.overlayController.SetTutorialPrompt(s);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            SceneManager.Instance.overlayController.HideTutorialPrompt();
        }
    }
}