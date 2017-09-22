using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class music : MonoBehaviour
    {
        public static music Instance;
        public float[] timer;
       
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            updatingTimers();
        }

        void Awake()
        {
            loadingTimeData();
            if (Instance)
                DestroyImmediate(gameObject);
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }

        void loadingTimeData()
        {
            timer = new float[7];
            for (int i = 0; i < 7; i++)
            {
                timer[i] = PlayerPrefs.GetFloat("time" + i);
            }
        }

        void updatingTimers()
        {
            for (int i = 0; i < timer.Length; i++)
            {
                if (timer[i] > 0)
                {
                    timer[i] -= Time.deltaTime;
                    PlayerPrefs.SetFloat("time" + i,timer[i]);
                }
            }
        }
    }
}
