using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelSpinner : MonoBehaviour
{
    public GameObject wheelOfLuck;
    public GameObject triangl;
    public GameObject[] items;
    public GameObject spinButton;
    public GameObject wheelOfFortuneSelection;
    public GameObject ShopMenuGameObject;

    private Vector3 originalVector3;
    private Quaternion originalQuaternion;
    private int index;
    private int indexHiarchy;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void spin()
    {
        triangl.transform.SetParent(wheelOfFortuneSelection.transform, true);
        StartCoroutine(wheelTurned());
    }

    IEnumerator wheelTurned()
    {
        spinButton.GetComponent<Button>().enabled = false;
        int i = Random.Range(5, 10);
        do
        {
            wheelOfLuck.GetComponent<Rigidbody2D>().AddTorque(10);
            //wheelOfLuck.GetComponent<RectTransform>().transform.Rotate(0f,0f,10f);//RotateAround(wheelOfLuck.transform.position,Vector3.back,10);
            i++;
            yield return new WaitForSeconds(0.01f);
        } while (i < 50);

        yield return new WaitForSeconds(1f);
        do
        {
            wheelOfLuck.GetComponent<Rigidbody2D>().angularVelocity -= 2;
            yield return new WaitForSeconds(0.001f);
        } while (wheelOfLuck.GetComponent<Rigidbody2D>().angularVelocity > 0);

        float distence = 0;
        index = 0;
        for (int j = 0; j < items.Length; j++)
        {
            if (j == 0) distence = Vector3.Distance(triangl.transform.position, items[j].transform.position);
            if (Vector3.Distance(triangl.transform.position, items[j].transform.position) < distence)
            {
                distence = Vector3.Distance(triangl.transform.position, items[j].transform.position);
                index = j;
            }
        }

        StartCoroutine(animationOfWin());
    }

    IEnumerator animationOfWin()
    {
        originalVector3 = items[index].transform.position;
        originalQuaternion = items[index].transform.localRotation;
        triangl.transform.SetParent(wheelOfLuck.transform, true);
        float i = 1;
        indexHiarchy = items[index].transform.GetSiblingIndex();
        items[index].transform.SetAsLastSibling();
        do
        {
            items[index].transform.localPosition = Vector3.Lerp(items[index].transform.localPosition,
                new Vector3(0, 0, 0), 3f * Time.deltaTime);
            i += 0.03f;
            if (i < 2.5f) items[index].transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.01f);
        } while ((int) items[index].transform.localPosition.x != 0);
        items[index].GetComponent<Button>().onClick.AddListener(call: rewardCollectedCall);
        do
        {
            items[index].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(Vector3.Lerp(
                items[index].GetComponent<RectTransform>().transform.rotation.eulerAngles,
                new Vector3(0, 0, 0), 3f * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        } while ((int) items[index].GetComponent<RectTransform>().transform.rotation.eulerAngles.z != 0);
        
    }

    void rewardCollectedCall()
    {
        PlayerPrefs.SetInt("money" , PlayerPrefs.GetInt("money") + 20000);
        GameObject.Find("Cash text").GetComponent<TextMeshProUGUI>().text = "$" + PlayerPrefs.GetInt("money");
        items[index].transform.SetSiblingIndex(indexHiarchy);
        items[index].transform.position = originalVector3;
        items[index].transform.localRotation = originalQuaternion;
        items[index].transform.localScale = new Vector3(1,1,1);
        spinButton.GetComponent<Button>().enabled = true;
    }
}