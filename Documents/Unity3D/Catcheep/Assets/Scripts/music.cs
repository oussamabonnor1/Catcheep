using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class music : MonoBehaviour
    {
        public static music Instance;
       
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {

            if (Instance)
                DestroyImmediate(gameObject);
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }
    }
}
