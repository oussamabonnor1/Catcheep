using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class music : MonoBehaviour
    {
        public AudioClip[] spaceShipSounds;
        public AudioClip[] uiSounds;
        public AudioClip[] MusicClip;
        public static music Instance;
        public float[] timer;

        // Use this for initialization
        void Start()
        {
             if (SceneManager.GetActiveScene().name.Equals("Start"))
             {
                if (PlayerPrefs.GetInt("maxHearts") == 0)
                {
                    PlayerPrefs.SetInt("maxHearts", 5);
                    PlayerPrefs.SetInt("hearts", 3);
                }
            }
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

        public void loadingTimeData()
        {
            timer = new float[7];
            for (int i = 0; i < 7; i++)
            {
                timer[i] = PlayerPrefs.GetFloat("time" + i);
            }
        }

        void updatingTimers()
        {
            if (PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
            {
                if (PlayerPrefs.GetFloat("heartTime") > 0)
                {
                    PlayerPrefs.SetFloat("heartTime", PlayerPrefs.GetFloat("heartTime") - Time.deltaTime);
                }
                else
                {
                    if (PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
                    {
                        PlayerPrefs.SetFloat("heartTime", 150);
                        PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + 1);
                        if (SceneManager.GetActiveScene().name.Equals("Start"))
                        {
                            GameObject.Find("hearts").GetComponent<TextMeshProUGUI>().text =
                                "x" + PlayerPrefs.GetInt("hearts");
                        }
                    }
                }
            }
            for (int i = 0; i < timer.Length; i++)
            {
                if (timer[i] > 0)
                {
                    timer[i] -= Time.deltaTime;
                    PlayerPrefs.SetFloat("time" + i, timer[i]);
                }
            }
        }

        public void spaceShipSound(int index)
        {
            if (PlayerPrefs.GetInt("SFX") == 0)
            {
                GetComponents<AudioSource>()[2].clip = spaceShipSounds[index];
                GetComponents<AudioSource>()[2].Play();
            }
        }
        public void BackgroundMusic(int index)
        {
            if (PlayerPrefs.GetInt("music") == 0)
            {
                GetComponents<AudioSource>()[0].clip = MusicClip[index];
                GetComponents<AudioSource>()[0].Play();
            }
            else
            {
                GetComponents<AudioSource>()[0].Pause();
            }
        }
        public void UISFX(int index)
        {
            if (PlayerPrefs.GetInt("SFX") == 0)
            {
                GetComponents<AudioSource>()[1].clip = uiSounds[index];
                GetComponents<AudioSource>()[1].Play();
            }
        }
    }
}