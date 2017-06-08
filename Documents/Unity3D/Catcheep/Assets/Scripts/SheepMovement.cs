using UnityEngine;

    public class SheepMovement : MonoBehaviour
    {
        [Range(0f,10)]
        public int Speed;

        public sheepDestroyer SheepDestroyer;

        private Vector2 destination;

        // Use this for initialization
        void Start()
        {
            destination = new Vector2(0f,-1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!SheepDestroyer.caught)
            {
                transform.Translate(destination * Time.deltaTime * Speed);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            }
        }
    }
