using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;

        public void SetActivePanel(int index)
        {

            switch (index)
            {
                case 0: SceneManager.LoadScene("MainScene"); break;
                case 1: SceneManager.LoadScene("StartScene"); break;
                case 2: Application.Quit(); break;
                default: return;
            }
            /*
            for (int i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
            */
        }

        void OnEnable()
        {
            panels[0].SetActive(true);
        }

    }
}