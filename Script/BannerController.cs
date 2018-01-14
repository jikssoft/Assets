using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerController : MonoBehaviour {

    GoogleMobileAdBanner banner;
    static BannerController instance;
    // Use this for initialization

#if UNITY_ANDROID
    static private string[] banner_units = { "12b6c8d0036241b3bd6058783cf6962c" };
#elif UNITY_IPHONE
	static private string[] banner_units = { "cbe28cd72f0f44b3b8efb5645a73e1c6" };
#endif

    void Start () {
        /*
        if (GoogleMobileAd.IsInited == false)
        {
            GoogleMobileAd.Init();
        }
        //GoogleMobileAd.AddKeyword("game");
        banner = GoogleMobileAd.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
        banner.ShowOnLoad = false;
        //banner.Hide();

        //banner.Show();
        */


#if UNITY_ANDROID
        MoPub.loadBannerPluginsForAdUnits(banner_units);
#elif UNITY_IPHONE
		MoPub.loadPluginsForAdUnits(banner_units);
#endif
        
        //MoPub.createBanner(banner_units[0], MoPubAdPosition.BottomCenter);


        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void HideBanner()
    {
        /*
        Debug.Log("================= Hide Banner");
        instance.banner.Hide();
        */
        MoPub.showBanner(banner_units[0], false);
        instance.showed = false;
    }

    private bool showed = false;
    private bool created = false;

    static public void ShowBanner()
    {
        Debug.Log("================= Show Banner");
        //instance.banner.Show();

        if(instance.created == false)
        {
            MoPub.createBanner(banner_units[0], MoPubAdPosition.BottomCenter);
            instance.created = true;
        }
        
        //MoPub.showBanner(instance.banner_units[0], true);

        if(instance.showed == true)
        {
            return;
        }

        MoPub.showBanner(banner_units[0], true);
        instance.showed = true;
    }
}
