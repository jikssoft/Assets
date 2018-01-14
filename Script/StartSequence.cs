using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequence : MonoBehaviour {

    public GameObject splash_image;
    public GameObject game_ui;
    public ShutterController shutter;
    public GameObject under_ui;
	// Use this for initialization
	void Start () {
        game_ui.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void DownShutter()
    {
        StartCoroutine(DownShutterCoroutine());
    }

    IEnumerator DownShutterCoroutine()
    {
        shutter.ShutDown();
        yield return new WaitForSeconds(0.5f);

        SA.Analytics.Google.Manager.StartTracking();

        game_ui.SetActive(true);

        under_ui.SetActive(true);
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();
        system.SetStartState();
        system.StartCoroutine(system.StartGame());

        splash_image.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        shutter.ShutUp();
    }
}
