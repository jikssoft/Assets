using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratorController : MonoBehaviour {

    static private VibratorController instance;
    private VibratorPlugin vibratorPlugin;
    private bool isVibrate = false;

    // Use this for initialization
    void Start () {
        instance = this;
        vibratorPlugin = VibratorPlugin.GetInstance();
        vibratorPlugin.SetDebug(0);
        vibratorPlugin.Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void Vibrate()
    {
        Debug.Log("=====================Vibrate");
        GameObject system = GameObject.FindGameObjectWithTag("System");
        int vibration = system.GetComponent<GameDataSystem>().GetVibration();

        if(vibration == 0)
        {
            return;
        }

        StopVibrate();

		if (RuntimePlatform.Android == Application.platform) {
			if (!instance.isVibrate) {
				instance.vibratorPlugin.Vibrate (10000);
				instance.isVibrate = true;
			}
		} else {
			if (!instance.isVibrate) {
				instance.StartCoroutine (instance.OtherPlatformLoopingVibrate ());
				instance.isVibrate = true;
			}
		}
			
    }

    static public void StopVibrate()
    {
        Debug.Log("=====================stop Vibrate");

		if (RuntimePlatform.Android == Application.platform) {
			if (instance.isVibrate) {
				instance.vibratorPlugin.StopVibrate ();
				instance.isVibrate = false;
			}
		} else {
			if (instance.isVibrate) {
				instance.isVibrate = false;
			}
		}
    }

	IEnumerator OtherPlatformLoopingVibrate()
	{
		Handheld.Vibrate ();
		yield return new WaitForSeconds (0.25f);
		if (isVibrate == true) {
			StartCoroutine (OtherPlatformLoopingVibrate ());
		}
	}

    static private void OnApplicationQuit()
    {
        StopVibrate();
    }
}
