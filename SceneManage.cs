using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void CsvWriter()
    {
        SceneManager.LoadSceneAsync("AudioSelect");
    }

}
