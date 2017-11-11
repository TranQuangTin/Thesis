using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Alta.Tools
{
    public class LogController : MonoBehaviour
    {
        public static LogController Global;
        public bool dont_destroy = true;
        public string url = "http://logs.altamedia.vn/api/log/insert";
        private Queue<WWWForm> queue;
        [Range(0, 10)]
        public float TimeDelay;
        private float _time;

        public void Awake()
        {
            Global = this;
            queue = new Queue<WWWForm>();
            if (this.dont_destroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Log(string code, string type, string content)
        {
            WWWForm f = new WWWForm();
            f.AddField("code", code);
            f.AddField("type", type);
            f.AddField("content", content);
            this.queue.Enqueue(f);
        }

        public void httpCallback(UnityWebRequest www)
        {
            Debug.Log("[LOG CODE]:" + www.responseCode);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= TimeDelay)
            {
                _time = 0;
                if (this.queue.Count > 0)
                    this.gameObject.POST(this.url, this.queue.Dequeue());
            }
        }
    }

}