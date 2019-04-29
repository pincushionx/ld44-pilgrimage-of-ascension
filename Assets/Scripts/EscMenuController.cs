using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class EscMenuController : MonoBehaviour
    {
        public void ResumeClicked()
        {
            SceneManager.Instance.EnableEscMenu(false);
        }
        public void RestartLevelClicked()
        {
            GameManager.Instance.RestartScene();
        }
        public void ExitClicked()
        {
            GameManager.Instance.GoToStartScreen();
        }
    }
}