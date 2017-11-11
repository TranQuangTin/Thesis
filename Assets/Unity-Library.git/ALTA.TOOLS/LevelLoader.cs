using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Alta.Tools
{
    public class LevelLoader : MonoBehaviour
    {
        public GameObject LoaddingProgess;
        public static LevelLoader Global;

        private void Awake()
        {
            Global = this;
        }

        public void LoadLevel(int index)
        {
            SceneManager.LoadScene(index);
        }
        public void LoadLevel(string name)
        {
            StartCoroutine(LoadAsynchronously(name));
        }
        IEnumerator LoadAsynchronously(string name)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);
            operation.priority = 1;
            if (LoaddingProgess)
                LoaddingProgess.SetActive(true);
            yield return new WaitUntil(() => operation.isDone);
            if (LoaddingProgess)
                LoaddingProgess.SetActive(false);
        }
    }
}
