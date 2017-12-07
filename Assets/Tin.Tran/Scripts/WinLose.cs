using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    public static WinLose Global;
    public GameObject Lose;
    public GameObject Win;
    private void Awake()
    {
        Global = this;
    }
    public void ShowResult(bool result)
    {
        if (result)
            StartCoroutine(WaitForShow(Win));
        else
            StartCoroutine(WaitForShow(Lose));
    }
    IEnumerator WaitForShow(GameObject go)
    {
        yield return new WaitForSeconds(2);
        go.SetActive(true);
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
