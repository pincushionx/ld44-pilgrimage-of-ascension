using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private bool interacting = false;
        private bool modalInteraction = false;
        private bool interactionPromptTodo = false;
        private Altar interactingAltar = null;
        private GrandAltar interactingGrandAltar = null;


        // Use this for initialization
        void Start()
        {
            StartCoroutine("RaycastCoroutine");
        }

        private void Update()
        {
            // we are interacting with something
            // look for keystrokes
            if (modalInteraction == false)//interacting)
            {
                // we found something to interact with
                // raise a prompt
                if (interactionPromptTodo)
                {
                    if (interactingAltar != null || interactingGrandAltar != null)
                    {
                        string msg = SceneManager.Instance.textDictionary["prompt-altar-interaction"];
                        SceneManager.Instance.overlayController.SetInteractionPrompt(msg);
                    }

                    interactionPromptTodo = false;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactingGrandAltar != null)
                    {
                        SceneManager.Instance.overlayController.ShowGrandAltarPrompt();
                        modalInteraction = true;
                    }
                    else if (interactingAltar != null)
                    {
                        SceneManager.Instance.overlayController.HideInteractionPrompt();
                        SceneManager.Instance.overlayController.ShowSacrificePrompt(interactingAltar);
                        modalInteraction = true;
                    }
                }
            }
            // if we were in a modal interaction, and the scene manager says we're not in modal mode
            // then we're done the modal interaction
            if (modalInteraction && SceneManager.Instance.modalMode == false)
            {
                modalInteraction = false;
                interacting = false; // not currently used

                // reset the altar vars in case 'E' is pressed before the coroutine executes
                interactingAltar = null;
                interactingGrandAltar = null;
            }
        }


        IEnumerator RaycastCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);

                // only change targets when not interacting
                if (modalInteraction == false)
                {
                    RaycastHit hitInfo = new RaycastHit();
                    Camera camera = gameObject.GetComponentInChildren<Camera>();
                    Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);

                    bool hit = Physics.Raycast(cameraRay, out hitInfo, 10f, LayerMask.GetMask("InteractableObject")); // 5f distance

                    // reset targets
                    interactingAltar = null;
                    interactingGrandAltar = null;

                    if (hit)
                    {
                        GameObject hitObject = hitInfo.collider.gameObject;
                        if ((interactingAltar = hitObject.GetComponent<Altar>()) != null)
                        {
                            // an altar may only be used one for sacrifice.
                            // it will become non interactable after using
                            if (interactingAltar.Interactable)
                            {
                                interacting = true;
                                interactionPromptTodo = true;
                            }
                            else
                            {
                                interactingAltar = null;
                            }
                        }
                        else if ((interactingGrandAltar = hitObject.GetComponent<GrandAltar>()) != null)
                        {
                            // an altar may only be used one for sacrifice.
                            // it will become non interactable after using
                            if (interactingGrandAltar.Interactable)
                            {
                                interacting = true;
                                interactionPromptTodo = true;
                            }
                            else
                            {
                                interactingGrandAltar = null;
                            }
                        }
                    }
                    else
                    {
                        // object is no longer hit
                        SceneManager.Instance.overlayController.HideInteractionPrompt();
                    }
                }
            }
        }
    }
}