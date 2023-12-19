using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCollider : MonoBehaviour
{
    //シーンを変わる為に、ボタンや別ものを関連して
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
    }
    //ゲームオーバーボタンに関連して
    public void GameOver()
    {
        Application.Quit();
    }

    public void ClearSceneChange()
    {
        SceneManager.LoadScene("Clear");
    }
}
