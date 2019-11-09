using TMPro;
using UnityEngine;

public class destroyerCollider : MonoBehaviour
{
    private int kills;
    public GameObject finishPanel;

    void Start()
    {
        kills = 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("sheepy") || other.gameObject.tag.Equals("blacky"))
        {
            kills++;
        }
        other.gameObject.GetComponent<sheepDestroyer>().Destruction();
        if (kills >= 20)
        {
            gameManager.gameOver = true;
            finishPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
                "You Missed Too Many Sheeps !";
        }
    }
}
