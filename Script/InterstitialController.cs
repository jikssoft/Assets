using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialController : MonoBehaviour {

    static InterstitialController instance;
    bool loaded = false;

#if UNITY_ANDROID
    private string[] interstitial_units = { "b73a26be878644719785aba743eec396" };
#elif UNITY_IPHONE
	private string[] interstitial_units = { "da4b968e940a47d692a5ee4166d2043b" };
#endif

    // Use this for initialization
    void Start () {
        instance = this;
        /*
        if (GoogleMobileAd.IsInited == false)
        {
            GoogleMobileAd.Init();
        }

        GoogleMobileAd.OnInterstitialLoaded += OnInterstisialsLoaded;
        GoogleMobileAd.OnInterstitialOpened += OnInterstisialsOpen;
        GoogleMobileAd.OnInterstitialClosed += OnInterstisialsClosed;
        */
        //loadin ad:

#if UNITY_ANDROID
        MoPub.loadInterstitialPluginsForAdUnits(interstitial_units);
#elif UNITY_IPHONE
		MoPub.loadPluginsForAdUnits(interstitial_units);
#endif

        

        MoPubManager.onInterstitialLoadedEvent += OnInterstisialsLoaded;

        MoPub.requestInterstitialAd(instance.interstitial_units[0]);

        loaded = false;
    }

    private void OnInterstisialsClosed()
    {
        
    }

    private void OnInterstisialsOpen()
    {

    }

    private void OnInterstisialsLoaded(string adUnit)
    {
        //GoogleMobileAd.ShowInterstitialAd();
        Debug.Log("OnInterstisialsLoaded==========");
    }

    // Update is called once per frame
    void Update () {
		
	}

    static public void RequestInterstitialAd()
    {
        MoPub.requestInterstitialAd(instance.interstitial_units[0]);
    }

    static public void ShowInterstisialsAD()
    {
        //GoogleMobileAd.LoadInterstitialAd();
        MoPub.showInterstitialAd(instance.interstitial_units[0]);
    }
}
