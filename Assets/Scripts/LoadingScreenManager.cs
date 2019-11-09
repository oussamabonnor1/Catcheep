
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{

    [Header("Backgrounds")]
    public Sprite farm;
    public Sprite snow;
    public Sprite city;
    public Sprite alien;

	[Header("Loading Visuals")]
	public Image loadingDoneIcon;
	public Image loadingText;
	public Image progressBar;
	public Image fadeOverlay;
    public Image background;

	[Header("Timing Settings")]
	public float waitOnLoadEnd;
	public float fadeDuration;

	[Header("Loading Settings")]
	public LoadSceneMode loadSceneMode;
	public ThreadPriority loadThreadPriority;

	[Header("Other")]
	// If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
	public AudioListener audioListener;

	AsyncOperation operation;
	Scene currentScene;

	public static int sceneToLoad;
	// IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
	static int loadingSceneIndex;

	public static void LoadScene(int levelNum) {				
		Application.backgroundLoadingPriority = ThreadPriority.High;
		sceneToLoad = levelNum;
		SceneManager.LoadScene(sceneToLoad);
	}

	void Start() {
		if (sceneToLoad < 0)
			return;
	    switch (sceneToLoad)
	    {
	        case 2:
	            background.sprite = snow;
	            break;
	        case 3:
	            background.sprite = farm;
                break;
	        case 5:
	            background.sprite = city;
                break;
	        case 6:
	            background.sprite = alien;
	            break;
            default:
                print("default");
	            break;
        }
		//fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
		currentScene = SceneManager.GetActiveScene();
		StartCoroutine(LoadAsync(sceneToLoad));
	}

	private IEnumerator LoadAsync(int levelNum) {
		ShowLoadingVisuals();

		yield return null; 

		FadeIn();
		StartOperation(levelNum);

		float lastProgress = 0f;

		// operation does not auto-activate scene, so it's stuck at 0.9
		while (DoneLoading() == false) {

			if (Mathf.Approximately(operation.progress, lastProgress) == false) {
				progressBar.fillAmount = operation.progress;
				lastProgress = operation.progress;
			    yield return new WaitForSeconds(0.1f);
            }
		}

		if (loadSceneMode == LoadSceneMode.Additive)
			audioListener.enabled = false;

		ShowCompletionVisuals();

		yield return new WaitForSeconds(waitOnLoadEnd);

		FadeOut();

		yield return new WaitForSeconds(fadeDuration);

		if (loadSceneMode == LoadSceneMode.Additive)
			SceneManager.UnloadSceneAsync(currentScene.name);
		else
			operation.allowSceneActivation = true;
	}

	private void StartOperation(int levelNum) {
		Application.backgroundLoadingPriority = loadThreadPriority;
		operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


		if (loadSceneMode == LoadSceneMode.Single)
			operation.allowSceneActivation = false;
	}

	private bool DoneLoading() {
		return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f); 
	}

	void FadeIn() {
		fadeOverlay.CrossFadeAlpha(0, 0f, true);
	}

	void FadeOut() {
		fadeOverlay.CrossFadeAlpha(1, 0f, true);
	}

	void ShowLoadingVisuals() {
		//loadingDoneIcon.gameObject.SetActive(false);
		progressBar.fillAmount = 0f;
	}

	void ShowCompletionVisuals() {
		//loadingDoneIcon.gameObject.SetActive(true);
		progressBar.fillAmount = 1f;
	}

}