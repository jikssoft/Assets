using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (target_spread > 0f && blur.blurSpread < target_spread)
        {
            blur.blurSpread += Time.deltaTime * (target_spread / time);
        }

        if (target_spread < 0f && blur.blurSpread > target_spread)
        {
            blur.blurSpread += Time.deltaTime * (target_spread / time);
        }

        if (target_spread <= 0f)
        {
            blur.blurSpread = 0f;
            blur.enabled = false;
        }
    }

    float start_time;
    static float target_spread;

    public UnityStandardAssets.ImageEffects.Blur blur;
    public float time = 0.5f;

    public void Blur()
    {
        blur.enabled = true;
        blur.blurSpread = 0.001f;
        target_spread = 0.5f;
        Debug.Log("====================== Blur");
    }

    public void UnBlur()
    {
        blur.blurSpread = 0.5f;
        target_spread = -0.5f;
    }
}
