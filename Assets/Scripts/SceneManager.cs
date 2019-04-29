using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pincushion.LD44
{
    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {
        public OverlayController overlayController;
        public EscMenuController escMenuController;
        public PaperDoll paperDoll;
        public bool modalMode = false;

        private float hemorrhageRateModifier = 0.0025f;

        public Dictionary<string, string> textDictionary = new Dictionary<string, string>();

        private bool paused;
        private bool resumeFromPausedAsModal = false;
        public bool Paused {
            get
            {
                return paused;
            }
            set
            {
                // only handle switches
                if (value != paused) {
                    // 
                    if (paused)
                    {
                        modalMode = resumeFromPausedAsModal;
                    }
                    else
                    {
                        resumeFromPausedAsModal = modalMode;
                        modalMode = true;
                    }
                    
                    paused = value;
                }
            }
        }

        private void Awake()
        {
            paperDoll.InitializeEmpty();
            InitializeText();
            EnableEscMenu(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EnableEscMenu(!Paused);
            }
            else if (!Paused)
            {
                float hemorrhageRate = 0;
                if (paperDoll.leftArmSacrificed)
                {
                    hemorrhageRate += 1f;
                }
                if (paperDoll.rightArmSacrificed)
                {
                    hemorrhageRate += 1f;
                }
                if (paperDoll.leftLegSacrificed)
                {
                    hemorrhageRate += 1f;
                }
                if (paperDoll.rightLegSacrificed)
                {
                    hemorrhageRate += 1f;
                }

                if (hemorrhageRate > 0f)
                {
                    paperDoll.bloodLevel -= hemorrhageRate * hemorrhageRateModifier * Time.deltaTime;
                    overlayController.UpdateBloodMeter();
                }

                if (paperDoll.bloodLevel < 0f)
                {
                    // losing condition
                    PlayerDied();
                }
            }
        }

        public void PlayerDied()
        {
            overlayController.ShowLosingConidtionPrompt();
        }

        public void ResetScene()
        {
            paperDoll = new PaperDoll();
            paperDoll.InitializeEmpty();
            EnableEscMenu(false);
        }

        public void EnableEscMenu(bool enable)
        {
            Paused = enable;
            overlayController.gameObject.SetActive(!Paused);
            escMenuController.gameObject.SetActive(Paused);
        }

        public void InitializeText()
        {
            textDictionary.Clear();

            textDictionary.Add("tutorial-altar1", "...and so the Pilgrimage of Ascension begins.\n\nThey say that the Ascended One worshiped the Altar God in this very temple, where the Ascended One is said to have sacrificed his limbs, one by one. One limb was sacrificed at each altar he visited, and in return, the Altar God blessed the Ascended One with great powers to aid in his asension.\n\nThe first altar is before you, as is quite the climb. Perhaps a sacrifice is required.");

            textDictionary.Add("prompt-grand-altar-next-level", "Your limbs begin to heal immediately as you touch the Grand Altar. You feel an overwhelming presense, as though the Altar God is guiding you toward ascension.");
            textDictionary.Add("prompt-altar-interaction", "Press 'E' to interact with the altar");

            textDictionary.Add("buff-jump-height", "Jump Height");
            textDictionary.Add("buff-run-speed", "Run Speed");
            textDictionary.Add("buff-break-walls", "Wall-breaking Strength");
            textDictionary.Add("buff-wall-jump", "Wall Jumping");

            textDictionary.Add("buff-sacrifice-default-text", "Click on a limb to sacrifice it!");
            textDictionary.Add("buff-sacrifice-jump-height-text", "You will be blessed with increased jump height.");
            textDictionary.Add("buff-sacrifice-run-speed-text", "You will be blessed with superior running speed.");
            textDictionary.Add("buff-sacrifice-break-walls-text", "You will be blessed with the ability to break [thin] walls! Don't forget to press 'E' when you find a thin wall!");
            textDictionary.Add("buff-sacrifice-wall-jump-text", "You will be blessed with the ability to jump against walls.");
        }
    }
}