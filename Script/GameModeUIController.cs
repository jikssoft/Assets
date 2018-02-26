using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeUIController : MonoBehaviour {

    public GameObject l_level_mode_btn;
    public GameObject r_level_mode_btn;
    public GameObject hell_mode_btn;
    public GameObject infinity_mode_btn;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInfiniteMode()
    {
        l_level_mode_btn.SetActive(true);
        hell_mode_btn.SetActive(true);

        r_level_mode_btn.SetActive(false);
        infinity_mode_btn.SetActive(false);
    }

    public void SetHellMode()
    {
        infinity_mode_btn.SetActive(true);
        r_level_mode_btn.SetActive(true);

        l_level_mode_btn.SetActive(false);
        hell_mode_btn.SetActive(false);        
    }

    public void SetLevelMode()
    {
        infinity_mode_btn.SetActive(true);
        hell_mode_btn.SetActive(true);

        l_level_mode_btn.SetActive(false);
        r_level_mode_btn.SetActive(false);
    }
}
