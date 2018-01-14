using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Camera target_camera;
	public GameObject[] disable_target_table;

	public void ScreenShot()
	{
		StartCoroutine (ScreenShotSequence ());
	}

	public Texture2D screen_shot;
	public RenderTexture screen_shot_2;

	public IEnumerator ScreenShotSequence()
	{
		screen_shot = new Texture2D(Screen.width, Screen.height);
        //screen_shot_2 = new RenderTexture (720, 1280, 0);

        BannerController.HideBanner();

		foreach (GameObject go in disable_target_table) {
			go.SetActive (false);
		}


		//target_camera.targetTexture = screen_shot_2;
		//target_camera.Render ();

		yield return new WaitForFixedUpdate ();
        yield return new WaitForFixedUpdate();
        
        screen_shot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screen_shot.Apply();

        foreach (GameObject go in disable_target_table)
        {
            go.SetActive(true);
        }

        //Application.CaptureScreenshot("screen_shot.png");

        //target_camera.targetTexture = null;

        yield return new WaitForEndOfFrame();
        
        BannerController.ShowBanner();

        //RenderTexture.active = screen_shot_2;

        //GameObject system_go = GameObject.FindWithTag ("GameSystem");
        //GameDataSystem data_system = system_go.GetComponent<GameDataSystem> ();
        //int level = data_system.GetLevel ();

        UM_ShareUtility.ShareMedia ("", "", screen_shot);
		yield return null;
	}

}
