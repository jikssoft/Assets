using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePopup : MonoBehaviour, ReturnKeyProcess, Rewardable
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool continue_popup = true;

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }
    
    public void Return()
    {
        GetComponent<BlurController>().UnBlur();
        ActiveAnimation.Play(GetComponent<Animation>(), "HideFailPopup", AnimationOrTween.Direction.Forward);

        Debug.Log("================= hide ===================");

        // contnue popup 과 exit popup 이 같은 script 사용해서 분리함.
        if (continue_popup == true)
        {
            system.CancelContinueAD();
        }

        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public GameMainLogicSystem system;


    public void Continue()
    {
        RewardADController.ShowRewardAd(this, true);
    }

    public void Reward()
    {
        GetComponent<BlurController>().UnBlur();
        ActiveAnimation.Play(GetComponent<Animation>(), "HideFailPopup", AnimationOrTween.Direction.Forward);
        system.Continue();
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();

    }
}


