using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pincushion.LD44
{
    public class OverlayController : MonoBehaviour
    {
        public GameObject interactionPromptContainer;
        public Button interactionPromptOkButton;
        public Text interactionPromptText;

        public GameObject sacrificePromptContainer;
        public Button sacrificePromptHeadButton;
        public Button sacrificePromptTorsoButton;
        public Button sacrificePromptLeftArmButton;
        public Button sacrificePromptRightArmButton;
        public Button sacrificePromptLeftFootButton;
        public Button sacrificePromptRightFootButton;
        public Button sacrificePromptSacrificeButton;
        public Text sacrificePromptText;

        public GameObject paperDollContainer;
        public Button paperDollHeadButton;
        public Button paperDollTorsoButton;
        public Button paperDollLeftArmButton;
        public Button paperDollRightArmButton;
        public Button paperDollLeftFootButton;
        public Button paperDollRightFootButton;

        public GameObject paperDollBuffsContainer;
        public GameObject paperDollBuffsVerticalLayoutContainer;
        private GameObject paperDollBuffsItemPrefab;

        public Image bloodMeterForeground;

        public GameObject loseConditionPrompt;

        public GameObject tutorialPromptContainer;
        public Button tutorialPromptOkButton;
        public Text tutorialPromptText;


        private void Awake()
        {
            HideInteractionPrompt();
            HideSacrificePrompt();
            HideLosingConidtionPrompt();

            paperDollBuffsItemPrefab = Resources.Load("BuffItem") as GameObject;
        }

        public void SetInteractionPrompt(string s)
        {
            interactionPromptContainer.SetActive(true);
            interactionPromptOkButton.gameObject.SetActive(false);
            interactionPromptText.text = s;
        }
        public void HideInteractionPrompt()
        {
            interactionPromptContainer.SetActive(false);
        }


        public void ShowGrandAltarPrompt()
        {
            interactionPromptContainer.SetActive(true);
            interactionPromptOkButton.gameObject.SetActive(true);
            string msg = SceneManager.Instance.textDictionary["prompt-grand-altar-next-level"];
            interactionPromptText.text = msg;

            interactionPromptOkButton.onClick.RemoveAllListeners();
            interactionPromptOkButton.onClick.AddListener(() => GameManager.Instance.GoToNextLevel());

            SceneManager.Instance.modalMode = true;
        }


        public void SetTutorialPrompt(string s)
        {
            tutorialPromptContainer.SetActive(true);
            tutorialPromptText.text = s;

            tutorialPromptOkButton.onClick.AddListener(() => HideTutorialPrompt());

            SceneManager.Instance.modalMode = true;

            tutorialPromptContainer.SetActive(false);
            tutorialPromptContainer.SetActive(true);
        }
        public void HideTutorialPrompt()
        {
            tutorialPromptContainer.SetActive(false);
            SceneManager.Instance.modalMode = false;
        }

        private string sacrificialButton = null;
        private Altar interactingAltar;
        public void ShowSacrificePrompt(Altar altar)
        {
            interactingAltar = altar;
            SceneManager.Instance.modalMode = true;
            sacrificePromptContainer.SetActive(true);

            PaperDoll paperDoll = SceneManager.Instance.paperDoll;
            UpdateSacrificeButtons(paperDoll);
        }
        private void UpdateSacrificeButtons(PaperDoll paperDoll)
        {
            ColourSacrificeButton(sacrificePromptHeadButton, paperDoll.headSacrificed);
            ColourSacrificeButton(sacrificePromptTorsoButton, paperDoll.torsoSacrificed);
            ColourSacrificeButton(sacrificePromptLeftArmButton, paperDoll.leftArmSacrificed);
            ColourSacrificeButton(sacrificePromptRightArmButton, paperDoll.rightArmSacrificed);
            ColourSacrificeButton(sacrificePromptLeftFootButton, paperDoll.leftLegSacrificed);
            ColourSacrificeButton(sacrificePromptRightFootButton, paperDoll.rightLegSacrificed);

            sacrificePromptSacrificeButton.gameObject.SetActive(sacrificialButton != null);
        }

        private PaperDoll ProcessButtonClick(string buttonName)
        {
            // paperdoll is a struct. the variable is local only
            PaperDoll paperDoll = SceneManager.Instance.paperDoll;

            if (buttonName == "HeadButton")
            {
                paperDoll.headSacrificed = true;
            }
            else if (buttonName == "TorsoButton")
            {
                paperDoll.torsoSacrificed = true;
            }
            else if (buttonName == "LeftArmButton")
            {
                paperDoll.leftArmSacrificed = true;
            }
            else if (buttonName == "RightArmButton")
            {
                paperDoll.rightArmSacrificed = true;
            }
            else if (buttonName == "LeftLegButton")
            {
                paperDoll.leftLegSacrificed = true;
            }
            else if (buttonName == "RightLegButton")
            {
                paperDoll.rightLegSacrificed = true;
            }

            return paperDoll;
        }
        public void SetSacrificePromptPaperDollText(string buttonName)
        {
            string s;

            if (buttonName == "HeadButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-default-text"];
            }
            else if (buttonName == "TorsoButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-default-text"];
            }
            else if (buttonName == "LeftArmButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-break-walls-text"];
            }
            else if (buttonName == "RightArmButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-wall-jump-text"];
            }
            else if (buttonName == "LeftLegButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-run-speed-text"];
            }
            else if (buttonName == "RightLegButton")
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-jump-height-text"];
            }
            else
            {
                s = SceneManager.Instance.textDictionary["buff-sacrifice-default-text"];
            }

            sacrificePromptText.text = s;
        }

        private void ColourSacrificeButton(Button button, bool colourit)
        {
            ColorBlock colours = button.colors;
            colours.normalColor = (colourit) ? Color.red : Color.white;
            button.colors = colours;
        }
        private void ColourDisabledSacrificeButton(Button button, bool colourit)
        {
            ColorBlock colours = button.colors;
            colours.disabledColor = (colourit) ? Color.red : Color.white;
            button.colors = colours;
        }
        public void SacrificePaperDollButtonClicked(Button button)
        {
            sacrificialButton = button.name;

            PaperDoll paperDoll = ProcessButtonClick(sacrificialButton);
            UpdateSacrificeButtons(paperDoll);
            SetSacrificePromptPaperDollText(sacrificialButton);

            // the button wasn't rerendering
            // deactivate/reactivate to redraw
            button.gameObject.SetActive(false);
            button.gameObject.SetActive(true);
        }
        public void SacrificeButtonClicked()
        {
            if (sacrificialButton != null)
            {
                // close this sacrifice window
                // refresh the paperDoll

                PaperDoll paperDoll = ProcessButtonClick(sacrificialButton);
                SceneManager.Instance.paperDoll = paperDoll;

                HideSacrificePrompt();
                interactingAltar.SacrificePerformed = true;

                UpdatePaperDollButtons(paperDoll);
            }
            else
            {
                Debug.Log("sacrificial button not available");
            }
        }
        public void HideSacrificePrompt()
        {
            sacrificePromptContainer.SetActive(false);

            // end modalmode
            SceneManager.Instance.modalMode = false;
        }




        private void UpdatePaperDollButtons(PaperDoll paperDoll)
        {
            ColourDisabledSacrificeButton(paperDollHeadButton, paperDoll.headSacrificed);
            ColourDisabledSacrificeButton(paperDollTorsoButton, paperDoll.torsoSacrificed);
            ColourDisabledSacrificeButton(paperDollLeftArmButton, paperDoll.leftArmSacrificed);
            ColourDisabledSacrificeButton(paperDollRightArmButton, paperDoll.rightArmSacrificed);
            ColourDisabledSacrificeButton(paperDollLeftFootButton, paperDoll.leftLegSacrificed);
            ColourDisabledSacrificeButton(paperDollRightFootButton, paperDoll.rightLegSacrificed);

            //Dictionary<string, string> buffDictionary = new 

            Text[] buffTexts = paperDollBuffsVerticalLayoutContainer.GetComponentsInChildren<Text>();

            // remove all of the objects to be quick
            foreach (Text text in buffTexts)
            {
                Destroy(text.gameObject);
            }

            // add new buffs
            // this must be reflected in the ThirdPersonController
            if (paperDoll.leftArmSacrificed)
            {
                GameObject buffItem = Instantiate(paperDollBuffsItemPrefab);
                buffItem.GetComponent<Text>().text = SceneManager.Instance.textDictionary["buff-break-walls"];
                buffItem.transform.SetParent(paperDollBuffsVerticalLayoutContainer.transform);
                buffItem.transform.localScale = Vector3.one;
            }
            if (paperDoll.rightArmSacrificed)
            {
                GameObject buffItem = Instantiate(paperDollBuffsItemPrefab);
                buffItem.GetComponent<Text>().text = SceneManager.Instance.textDictionary["buff-wall-jump"];
                buffItem.transform.SetParent(paperDollBuffsVerticalLayoutContainer.transform);
                buffItem.transform.localScale = Vector3.one;
            }
            if (paperDoll.leftLegSacrificed)
            {
                GameObject buffItem = Instantiate(paperDollBuffsItemPrefab);
                buffItem.GetComponent<Text>().text = SceneManager.Instance.textDictionary["buff-run-speed"];
                buffItem.transform.SetParent(paperDollBuffsVerticalLayoutContainer.transform);
                buffItem.transform.localScale = Vector3.one;
            }
            if (paperDoll.rightLegSacrificed)
            {
                GameObject buffItem = Instantiate(paperDollBuffsItemPrefab);
                buffItem.GetComponent<Text>().text = SceneManager.Instance.textDictionary["buff-jump-height"];
                buffItem.transform.SetParent(paperDollBuffsVerticalLayoutContainer.transform);
                buffItem.transform.localScale = Vector3.one;
            }
        }

        public void UpdateBloodMeter()
        {
            PaperDoll paperDoll = SceneManager.Instance.paperDoll;
            bloodMeterForeground.fillAmount = paperDoll.bloodLevel;
        }

        public void ShowLosingConidtionPrompt()
        {
            loseConditionPrompt.SetActive(true);
            SceneManager.Instance.modalMode = true;
        }
        public void HideLosingConidtionPrompt()
        {
            loseConditionPrompt.SetActive(false);
        }
        public void RestartLevelClicked()
        {
            GameManager.Instance.RestartScene();
        }

    }
}