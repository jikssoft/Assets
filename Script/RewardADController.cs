using SA.Analytics.Google;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Rewardable
{
    void Reward();
}



public class RewardADController : MonoBehaviour {

#if UNITY_ANDROID
    private string[] reward_units = { "06aa6ec3e3b54c89b5c51a13b1f7c17e" };
#elif UNITY_IPHONE
	private string[] reward_units = { "df7d9cba3dc240d893fd0df997621bc1" };
#endif

    static RewardADController instance;

    // Use this for initialization
    void Start () {
        instance = this;
        /*
        if(GoogleMobileAd.IsInited == false)
        {
            GoogleMobileAd.Init();
        }

        GoogleMobileAd.OnRewarded += HandleOnRewarded;
        GoogleMobileAd.OnRewardedVideoLoaded += HandleOnRewardedVideoLoaded;
        GoogleMobileAd.OnRewardedVideoAdClosed += onRewardedVideoReceivedRewardEvent;
        GoogleMobileAd.OnRewardedVideoAdFailedToLoad += HandleOnRewardedVideoAdFailedToLoad;
        GoogleMobileAd.LoadRewardedVideo();
        */

        MoPubManager.onRewardedVideoLoadedEvent += MoPubHandleOnRewardedVideoLoaded;
        MoPubManager.onRewardedVideoReceivedRewardEvent += MoPubHandleOnRewarded;
        MoPubManager.onRewardedVideoClosedEvent += MoPubHandleOnRewardedVideoAdClosed;
        MoPubManager.onRewardedVideoFailedEvent += MoPubHandleOnRewardedVideoAdFailedToLoad;

        MoPub.initializeRewardedVideo();

		#if UNITY_ANDROID
		MoPub.loadRewardedVideoPluginsForAdUnits(reward_units);
		#elif UNITY_IPHONE
		MoPub.loadPluginsForAdUnits(reward_units);
		#endif

        
        MoPub.requestRewardedVideo(reward_units[0]);
        
        loaded = false;
        
    }

   

    private void MoPubHandleOnRewarded(MoPubManager.RewardedVideoData data)
    {
        Debug.Log("Reward Video");
        if (instance.iscontinue == false)
        {
            GameObject system = GameObject.FindGameObjectWithTag("System");
            system.GetComponent<GameDataSystem>().SetLastRewardADTime(DateTime.UtcNow);
            system.GetComponent<GameDataSystem>().SetRewardADWatingTime();
        }

        if (instance.rewardable != null)
        {
            instance.rewardable.Reward();
        }

        Manager.Client.SendEventHit("ADReward", "Watch Video");

        loaded = false;
        MoPub.requestRewardedVideo(reward_units[0]);
    }

    bool loaded = false;



    private void MoPubHandleOnRewardedVideoAdFailedToLoad(string adUnit)
    {
        loaded = false;
        StartCoroutine(LoadRewardedVideo());
    }

    IEnumerator LoadRewardedVideo()
    {
        yield return new WaitForSeconds(1f);
        //GoogleMobileAd.LoadRewardedVideo();
        MoPub.requestRewardedVideo(reward_units[0]);
    }

    
    private void MoPubHandleOnRewardedVideoAdClosed(string adUnit)
    {
        /*
        Debug.Log("Reward Video");
        if (instance.iscontinue == false)
        {
            GameObject system = GameObject.FindGameObjectWithTag("System");
            system.GetComponent<GameDataSystem>().SetLastRewardADTime(DateTime.UtcNow);
            system.GetComponent<GameDataSystem>().SetRewardADWatingTime();
        }

        if (instance.rewardable != null)
        {
            instance.rewardable.Reward();
        }
        */

        Manager.Client.SendEventHit("ADReward", "Close Video");

        loaded = false;
        //GoogleMobileAd.LoadRewardedVideo();
        MoPub.requestRewardedVideo(reward_units[0]);
    }

    

    private void MoPubHandleOnRewardedVideoLoaded(string adUnit)
    {
        loaded = true;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private Rewardable rewardable;
    private bool iscontinue;

    static public void ShowRewardAd(Rewardable reward, bool iscontinue)
    {
        //GoogleMobileAd.ShowRewardedVideo();

        MoPub.showRewardedVideo(instance.reward_units[0]);

        instance.rewardable = reward;
        instance.iscontinue = iscontinue;
        
    }

    private TimeSpan reset_delta_time = new TimeSpan(1, 0, 0);

    static public bool CheckRewardAD()
    {
        if (instance.loaded == false)
            return false;

        GameObject system = GameObject.FindGameObjectWithTag("System");
        DateTime last_reward_time = system.GetComponent<GameDataSystem>().GetLastRewardADTime();

        System.TimeSpan delta_time = System.DateTime.UtcNow - last_reward_time;

        if (delta_time > instance.reset_delta_time)
        {
            system.GetComponent<GameDataSystem>().ResetRewardADWatingTime();
        }
        
        int waiting_time = system.GetComponent<GameDataSystem>().GetRewardADWatingTime();
        
        System.TimeSpan unit_time = new TimeSpan(0, waiting_time, 0);

        if (delta_time > unit_time)
        {
            return true;
        }

        return false;
        //return true;
    }

    static public TimeSpan GetWatingTime()
    {
        if (instance.loaded == false)
        {
            return new TimeSpan(0);
        }

        GameObject system = GameObject.FindGameObjectWithTag("System");
        DateTime last_reward_time = system.GetComponent<GameDataSystem>().GetLastRewardADTime();

        System.TimeSpan delta_time = System.DateTime.UtcNow - last_reward_time;

        if (delta_time > instance.reset_delta_time)
        {
            system.GetComponent<GameDataSystem>().ResetRewardADWatingTime();
        }

        int waiting_time = system.GetComponent<GameDataSystem>().GetRewardADWatingTime();
        System.TimeSpan unit_time = new TimeSpan(0, waiting_time, 0);

        if(delta_time > unit_time)
        {
            return unit_time;
        }
        else
        {
            return unit_time - delta_time;
        } 
    }


    /*
    private void HandleOnRewarded(string arg1, int arg2)
    {

        Debug.Log("Reward Video");
        if (instance.iscontinue == false)
        {
            GameObject system = GameObject.FindGameObjectWithTag("System");
            system.GetComponent<GameDataSystem>().SetLastRewardADTime(DateTime.UtcNow);
            system.GetComponent<GameDataSystem>().SetRewardADWatingTime();
        }

        if (instance.rewardable != null)
        {
            instance.rewardable.Reward();
        }

        Manager.Client.SendEventHit("ADReward", "Watch Video");

        loaded = false;
        GoogleMobileAd.LoadRewardedVideo();

    }
    */

    /*
    private void HandleOnRewardedVideoAdFailedToLoad(int obj)
    {
    loaded = false;
    StartCoroutine(LoadRewardedVideo());
    }
    */

        /*
    private void HandleOnRewardedVideoAdClosed()
    {

    Manager.Client.SendEventHit("ADReward", "Close Video");

    loaded = false;
    GoogleMobileAd.LoadRewardedVideo();
    }
    */

        /*
    private void HandleOnRewardedVideoLoaded()
    {
        loaded = true;
    }
    */

}




