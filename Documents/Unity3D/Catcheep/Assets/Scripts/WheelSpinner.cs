using System;
using System.Collections;
using Assets.Scripts;
using Assets.SimpleAndroidNotifications;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelSpinner : MonoBehaviour
{
    public GameObject wheelOfLuck;
    public GameObject triangl;
    public GameObject[] items;
    public GameObject spinButton;
    public GameObject wheelOfFortuneSelection;
    public GameObject spinText;
    public GameObject noAdsPanel;

    private Vector3 originalVector3;
    private Quaternion originalQuaternion;
    private int index;
    private int indexHiarchy;
    private music musicManager;

    // Use this for initialization
    void Start()
    {
        musicManager = GameObject.Find("Music Manager").GetComponent<music>();
        dailySpinTime();
    }
    
    public void spin()
    {
        if (PlayerPrefs.GetInt("spin") > 0)
        {
            musicManager.UISFX(3);
            PlayerPrefs.SetString("spinTime", DateTime.Now.ToString());
            PlayerPrefs.SetInt("spin", PlayerPrefs.GetInt("spin") - 1);
            triangl.transform.SetParent(wheelOfFortuneSelection.transform, true);
            StartCoroutine(wheelTurned());
            dailySpinTime();
            if (PlayerPrefs.GetInt("notifications") == 0)
            {
                var notificationParams = new NotificationParams
                {
                    Id = Random.Range(0, int.MaxValue),
                    Delay = TimeSpan.FromDays(1),
                    Title = "Daily Spin :3",
                    Message = "Your Daily Spin is Ready, Go Catch sheeps !",
                    Ticker = "Ticker",
                    Sound = true,
                    Vibrate = true,
                    Light = true,
                    SmallIcon = NotificationIcon.Heart,
                    SmallIconColor = new Color(0, 0.5f, 0),
                    LargeIcon = "app_icon"
                };

                NotificationManager.CancelAll();
                NotificationManager.SendCustom(notificationParams);
            }
        }
    }

    IEnumerator wheelTurned()
    {
        spinButton.GetComponent<Button>().enabled = false;
        int i = Random.Range(5, 15);
        do
        {
            wheelOfLuck.GetComponent<Rigidbody2D>().AddTorque(10);
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
        do
        {
            items[index].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(Vector3.Lerp(
                items[index].GetComponent<RectTransform>().transform.rotation.eulerAngles,
                new Vector3(0, 0, 0), 3f * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        } while ((int) items[index].GetComponent<RectTransform>().transform.rotation.eulerAngles.z != 0);
        rewardCollectedCall();
    }

    void rewardCollectedCall()
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetInt("netStock", PlayerPrefs.GetInt("netStock") + 1);
                break;
            case 1:
                if (PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
                {
                    PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + 1);
                }
                break;
            case 2:
                PlayerPrefs.SetInt("loveStock", PlayerPrefs.GetInt("loveStock") + 1);
                break;
            case 3:
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 100000);
                PlayerPrefs.SetInt("mostMoney", PlayerPrefs.GetInt("mostMoney") + 100000);
                break;
            case 4:
                PlayerPrefs.SetInt("hayStackStock", PlayerPrefs.GetInt("hayStackStock") + 1);
                break;
            case 5:
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 50000);
                PlayerPrefs.SetInt("mostMoney", PlayerPrefs.GetInt("mostMoney") + 50000);
                break;
        }
        GameObject.Find("Cash text").GetComponent<TextMeshProUGUI>().text = "$" + PlayerPrefs.GetInt("money");
        items[index].transform.SetSiblingIndex(indexHiarchy);
        items[index].transform.position = originalVector3;
        items[index].transform.localRotation = originalQuaternion;
        items[index].transform.localScale = new Vector3(1, 1, 1);
        spinButton.GetComponent<Button>().enabled = true;
        wheelOfLuck.transform.Rotate(0, 0, -wheelOfLuck.transform.rotation.eulerAngles.z);
        triangl.transform.SetParent(wheelOfLuck.transform, true);
    }

    void dailySpinTime()
    {
        if (PlayerPrefs.GetString("spinTime").Equals("")) PlayerPrefs.SetString("spinTime", DateTime.Now.ToString());
        if (PlayerPrefs.GetInt("spin") == 0)
        {
            GameObject.Find("Music Manager").GetComponent<UTime>().GetUtcTimeAsync(OnTimeReceived);
            GameObject.Find("Music Manager").GetComponent<UTime>().HasConnection(connection => print(""));
        }
        else
        {
            spinText.GetComponent<TextMeshProUGUI>().text = "Spin !";
        }
    }

    private void OnTimeReceived(bool success, string error, DateTime time)
    {
        if (SceneManager.GetActiveScene().name.Equals("Start"))
        {
            if (success)
            {
                rewardSpin(time.ToLocalTime().ToString(), PlayerPrefs.GetString("spinTime"));
            }
            else
            {
                rewardSpin(DateTime.Now.ToString(), PlayerPrefs.GetString("spinTime"));
            }
        }
    }

    void rewardSpin(String now, String spinTime)
    {
        DateTime n = Convert.ToDateTime(now);
        DateTime s = Convert.ToDateTime(spinTime);
        TimeSpan result = n - s;
        if (result.Days >= 1)
        {
            spinText.GetComponent<TextMeshProUGUI>().text = "spin!";
            PlayerPrefs.SetString("spinTime", now);
            if (PlayerPrefs.GetInt("spin") == 0) PlayerPrefs.SetInt("spin", PlayerPrefs.GetInt("spin") + 1);
        }
        else
        {
            spinText.GetComponent<TextMeshProUGUI>().text = "Time left: " + (24 - result.Hours) + "h";
        }
    }

    public void watchAdSpin()
    {
        ShowRewardedVideo(0);
    }

    void ShowRewardedVideo(int index)
    {
        ShowOptions options = new ShowOptions();
        switch (index)
        {
            case 0:
                options.resultCallback = spinAd;
                break;
        }
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            noAdsPanel.SetActive(true);
        }
    }

    void spinAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            spinText.GetComponent<TextMeshProUGUI>().text = "spin!";
            PlayerPrefs.SetString("spinTime", DateTime.Now.ToString());
            if (PlayerPrefs.GetInt("spin") == 0) PlayerPrefs.SetInt("spin", PlayerPrefs.GetInt("spin") + 1);
        }
    }
}