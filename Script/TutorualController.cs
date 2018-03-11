
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorualController : MonoBehaviour {

    public UITexture tutorial_1;
    public UITexture tutorial_2;
    public UITexture tutorial_3;

    public UIButton hold_button;
    public UIButton release_button;
    public UIButton confirm_button;

    public Drill drill;
    public GameObject down_anchor;
    
    // Use this for initialization
    void Start () {
        is_success = false;
    }

    public float operate_speed = 0f;

    // Update is called once per frame
    void Update () {
        if (drill_speed < 0f)
        {
            down_anchor.transform.Translate(0f, drill_speed, 0f);
        }
    }


    bool is_success = false;

    public void Success()
    {
        is_success = true;
        drill_speed = 0f;
        drill.Stop();

        touch_obj.SetActive(false);
        release_obj.SetActive(true);
    }

    float drill_speed;
    public void OperateDrill()
    {
        // 구글 플레이 로딩 중에 튜토리얼 동작 하는 문제 있어 시간으로 막음)
        if (Time.fixedTime < 3f)
        {
            drill.Operate();
            drill_speed = operate_speed;
        }
    }

    public GameObject touch_obj;
    public GameObject release_obj;
    public GameObject good_obj;
    public GameObject nail_guide_obj;
    public GameObject success_obj;
    public TweenAlpha red_twinkle;
    public GameObject touch_panel;

    public void StopDrill()
    {
        drill_speed = 0f;
        drill.Stop();
        if(is_success == true)
        {
            good_obj.SetActive(true);
            foreach(UITweener tween in good_obj.GetComponentsInChildren<UITweener>())
            {
                tween.ResetToBeginning();
                tween.PlayForward();
            }
            release_obj.SetActive(false);
            success_obj.SetActive(false);
            StartCoroutine(ShowNailGuide());
            touch_panel.SetActive(false);
        }
        else
        {
            down_anchor.transform.localPosition = new Vector3(0f, 0f, 0f);
            red_twinkle.ResetToBeginning();
            red_twinkle.PlayForward();
        }
    }
    
    IEnumerator ShowNailGuide()
    {
        yield return new WaitForSeconds(2.5f);
        nail_guide_obj.SetActive(true);
        down_anchor.SetActive(false);
        good_obj.SetActive(false);
        
    }

    public void ShowTutorial()
    {
        gameObject.SetActive(true);

        touch_obj.SetActive(true);
        release_obj.SetActive(false);
        good_obj.SetActive(false);
        nail_guide_obj.SetActive(false);
        success_obj.SetActive(true);
        touch_panel.SetActive(true);
        down_anchor.SetActive(true);
        down_anchor.transform.localPosition = new Vector3(0f, 0f, 0f);
        is_success = false;
        /*
        gameObject.SetActive(true);
        tutorial_1.gameObject.SetActive(true);
        tutorial_2.gameObject.SetActive(false);
        tutorial_3.gameObject.SetActive(false);

        hold_button.gameObject.SetActive(true);
        release_button.gameObject.SetActive(false);
        confirm_button.gameObject.SetActive(false);
        */
    }

    public void PressHold()
    {
        tutorial_1.gameObject.SetActive(false);
        tutorial_2.gameObject.SetActive(true);
        tutorial_3.gameObject.SetActive(false);

        hold_button.gameObject.SetActive(false);
        release_button.gameObject.SetActive(true);
        confirm_button.gameObject.SetActive(false);
    }

    public void PressRelease()
    {
        tutorial_1.gameObject.SetActive(false);
        tutorial_2.gameObject.SetActive(false);
        tutorial_3.gameObject.SetActive(true);

        hold_button.gameObject.SetActive(false);
        release_button.gameObject.SetActive(false);
        confirm_button.gameObject.SetActive(true);
    }

    public void PressConfirm()
    {
        gameObject.SetActive(false);
    }

    public RewardCoinController coin_controller;
    public bool get_coin = false;
    public void ExitTutorial()
    {
        gameObject.SetActive(false);

        if (get_coin == true)
        {
            GameObject target = GameObject.FindGameObjectWithTag("CoinIcon");
            coin_controller.CreateCoin(target.transform, 100, 0.02f);
            get_coin = false;
        }

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameDataSystem dataSystem = systemObj.GetComponent<GameDataSystem>();

        SoundManager.OnOffBGM(dataSystem.GetBGM() == 1 ? true : false);

        BannerController.ShowBanner();
    }
}
