using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLayerController : MonoBehaviour, Rewardable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        SetADMovieButton();

        if (RewardADController.CheckRewardAD() == true)
        {
            ad_movie_sprite.GetComponent<UIButton>().enabled = true;
            ad_movie_sprite.color = new Color(1f, 1f, 1f);
            waiting_time.gameObject.SetActive(false);

            if (Time.fixedTime - (int)(Time.fixedTime) < 0.5f)
            {
                ad_movie_sprite.spriteName = "btn_getcoin";
            }
            else
            {
                ad_movie_sprite.spriteName = "btn_getcoin_red";
            }
        }
        else
        {
            ad_movie_sprite.spriteName = "btn_getcoin";
            TimeSpan delta_time = RewardADController.GetWatingTime();
            waiting_time.text = string.Format("{0}:{1}", delta_time.Minutes.ToString("D2"), delta_time.Seconds.ToString("D2"));
        }
    }

    public GameObject retry_button;
    public GameObject share_button;
    public UISprite ad_movie_sprite;

    public UILocalize next_retry_label;
    public UISprite next_retry_sprite;

    public GameObject pause_button;

    public void SetPauseState()
    {
        retry_button.SetActive(false);
        share_button.SetActive(false);
        ad_movie_sprite.gameObject.SetActive(false);
        next_retry_label.transform.parent.gameObject.SetActive(false);

        pause_button.SetActive(true);

        //GameObject systemObj = GameObject.FindGameObjectWithTag("System");
    }

    public void SetStartState()
    {
        retry_button.SetActive(false);
        share_button.SetActive(false);
        ad_movie_sprite.gameObject.SetActive(false);
        next_retry_label.transform.parent.gameObject.SetActive(false);

        pause_button.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetClearState()
    {
        retry_button.SetActive(true);
        share_button.SetActive(true);
        ad_movie_sprite.gameObject.SetActive(true);
        next_retry_label.transform.parent.gameObject.SetActive(true);

        pause_button.SetActive(false);

        SetADMovieButton();

        next_retry_sprite.spriteName = "icon-play";
        next_retry_label.key = "Next";

        UIButton button = next_retry_sprite.transform.parent.gameObject.GetComponent<UIButton>();
        button.onClick.Clear();

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        button.onClick.Add(new EventDelegate(systemObj.GetComponent<GameMainLogicSystem>(), "StartLastLevel"));
    }

    public void SetFailState()
    {
        retry_button.SetActive(false);
        share_button.SetActive(false);
        ad_movie_sprite.gameObject.SetActive(true);
        next_retry_label.transform.parent.gameObject.SetActive(true);

        pause_button.SetActive(false);

        SetADMovieButton();

        next_retry_sprite.spriteName = "img-retry";
        next_retry_label.key = "Retry";

        UIButton button = next_retry_sprite.transform.parent.gameObject.GetComponent<UIButton>();
        button.onClick.Clear();

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        button.onClick.Add(new EventDelegate(systemObj.GetComponent<GameMainLogicSystem>(), "RetryLevel"));
    }

    public UILabel waiting_time;
    public void SetADMovieButton()
    {
        if(RewardADController.CheckRewardAD() == true)
        {
            ad_movie_sprite.spriteName = "btn_getcoin";
            ad_movie_sprite.GetComponent<UIButton>().enabled = true;
            ad_movie_sprite.color = new Color(1f, 1f, 1f);
            waiting_time.gameObject.SetActive(false);
            waiting_time.text = null;
        }
        else
        {
            ad_movie_sprite.spriteName = "btn_getcoin";
            ad_movie_sprite.GetComponent<UIButton>().enabled = false;
            ad_movie_sprite.color = new Color(0.3f, 0.3f, 0.3f);
            waiting_time.gameObject.SetActive(true);
        }
        
    }

    public void ShowRewardAD()
    {
        RewardADController.ShowRewardAd(this, false);
    }

    public RewardCoinController coin_controller;
    public void Reward()
    {
        ad_movie_sprite.spriteName = "btn-ad-none";
        ad_movie_sprite.GetComponent<UIButton>().enabled = false;

        GameObject target = GameObject.FindGameObjectWithTag("CoinIcon");
        coin_controller.CreateRewardCoin(target.transform);
    }
}
