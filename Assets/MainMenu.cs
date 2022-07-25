using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void Exit()
   {
      Application.Quit();
   }

   public void PlayTheGame()
   {
      SceneManager.LoadScene("Scenes/SampleScene");
   }
}
