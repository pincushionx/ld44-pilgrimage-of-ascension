using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class StartMenuController : MonoBehaviour
    {
        public void StartGameClicked()
        {
            GameManager.Instance.StartGame();
        }
    }
}