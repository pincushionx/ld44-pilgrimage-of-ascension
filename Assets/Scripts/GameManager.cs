using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pincushion.LD44
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private string currentScene = "Level1";

        public void RestartScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
            SceneManager.Instance.ResetScene();
        }

        public void GoToStartScreen()
        {
            currentScene = "StartScreen";
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }

        public void StartGame()
        {
            currentScene = "TutorialLevel";
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }

        public void GoToNextLevel()
        {
            if (currentScene == "StartScreen")
            {
                currentScene = "TutorialLevel";
            }
            else if (currentScene == "TutorialLevel")
            {
////////////////////////////////
/// There is no level 1, go straight to level 2
                currentScene = "Level2";
            }
            else if (currentScene == "Level1")
            {
                currentScene = "Level2";
            }
            else if (currentScene == "Level2")
            {
                currentScene = "Level3";
            }
            else if (currentScene == "Level3")
            {
// There's no ending
                currentScene = "StartScreen";
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }
    }
}