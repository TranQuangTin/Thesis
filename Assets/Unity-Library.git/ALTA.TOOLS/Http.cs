using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace Alta.Tools
{
    /// <summary>
    /// Multil Http request suppot restful api
    /// </summary>
    public static class HttpRequest
    {
        public static void GET(this GameObject go, string url)
        {
            Func<UnityWebRequest, bool> callback = (www)=> {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };
            REQUEST(url, null, null, callback);
        }
        public static void GET(string url, Func<UnityWebRequest, bool> callback)
        {
            REQUEST(url, null, null, callback);
        }

        public static void POST(string url, WWWForm postForm, Func<UnityWebRequest, bool> callback)
        {
            REQUEST(url, postForm, null, callback);
        }

        public static void POST(this GameObject go, string url, WWWForm postForm)
        {
            Func<UnityWebRequest, bool> callback = (www) => {
                go.SendMessage("httpCallback", www);
                return string.IsNullOrEmpty(www.error);
            };
            REQUEST(url, postForm, null, callback);
        }
        private static void REQUEST(string url, WWWForm form, Dictionary<string,string> h , Func<UnityWebRequest, bool> c)
        {
            if (RequestManager.Global == null)
            {
                GameObject managerRequest = new GameObject();
                managerRequest.AddComponent<RequestManager>();
#if UNITY_EDITOR
                managerRequest.name = "[HTTTP] MANAGER";
#endif
            }
            RequestManager.Global.REQUEST(url, form,h,c);
        }
    }

    public class RequestManager : MonoBehaviour
    {
        public static RequestManager Global;
        private Dictionary<string, Func<UnityWebRequest, bool>> queue;
        private Coroutine httpCoroutine;
        private void Awake()
        {
            if (Global != null)
            {
                Destroy(this);
                return;
            }
            Global = this;
            DontDestroyOnLoad(gameObject);
            queue = new Dictionary<string,Func<UnityWebRequest, bool>>();
        }

        public void REQUEST(string url, WWWForm form, Dictionary<string,string> header, Func<UnityWebRequest,bool> c)
        {
            UnityWebRequest www;
            if (form == null)
            {
                www = UnityWebRequest.Get(url);
            }
            else
            {
                www = UnityWebRequest.Post(url, form);
            }
            queue.Add(www.url, c);
            if (header != null)
            {
                foreach(string k in header.Keys)
                {
                    www.SetRequestHeader(k, header[k]);
                }
            }
            httpCoroutine = StartCoroutine(WaitForRequest(www));
        }

        private IEnumerator WaitForRequest(UnityWebRequest www)
        {
            yield return www.Send();
            if (this.queue.ContainsKey(www.url))
            {
                this.queue[www.url](www);
                this.queue.Remove(www.url);
            }
            else
            {
                Debug.LogError("HTTP: " + www.url);
            }
        }
    }    

}