using System;
using System.Collections;
using UnityEngine;

    public class UTime : MonoBehaviour
    {
        private const string TimeServer = "https://script.google.com/macros/s/AKfycbyal_mx91_jytjMzr_ykoP3NfZXBVMNRNXCX7qmt0QpTj6mAHg/exec";
        private static UTime Instance;

        void Awake()
        {
            if (!Instance) { 
               Instance = this;
            }
        }

        /// <summary>
        /// Get time from server asynchronically with callback (bool success, string error, DateTime time)
        /// </summary>
        public void GetUtcTimeAsync(Action<bool, string, DateTime> callback)
        {
            Debug.Log("Requesting network time from: " + TimeServer);
            Instance.StartCoroutine(Download(TimeServer, www =>
                {
                    if (www.error == null)
                    {
                        try
                        {
                            callback(true, null, DateTime.Parse(www.text).ToUniversalTime());
                        }
                        catch (Exception e)
                        {
                            callback(false, e.Message, DateTime.MinValue);
                        }
                    }
                    else
                    {
                        callback(false, www.error, DateTime.MinValue);
                    }
                }));
            
        }

        /// <summary>
        /// Check network connection asynchronically with callback (bool success)
        /// </summary>
        public void HasConnection(Action<bool> callback)
        {
            
                Instance.StartCoroutine(Download(TimeServer, www => { callback(www.error == null); }));
            
        }

        private IEnumerator Download(string url, Action<WWW> callback)
        {
            var www = new WWW(url);

            yield return www;

            callback(www);
        }
    }
