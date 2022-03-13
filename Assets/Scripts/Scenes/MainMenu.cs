using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPlayPVEButtonClicked()
        {
            PVE.Load((PlayerColor.Black, new AlphaBetaBot(5)));
        }


        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}