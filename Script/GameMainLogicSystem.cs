using UnityEngine;
using System.Collections;
using System;
using SA.Analytics.Google;

public class GameMainLogicSystem : MonoBehaviour, ReturnKeyProcess
{

    public ArrayList nail_table;
    public Drill drill;
    public NailBuilder builder;
    public Box box;

    public CameraTweener camera_tweener;

    public UIPlayTween play_stage_ani_text;
    int nail_index;
    int current_level;
    int current_coin;

    public float drill_speed;
    float drill_operate_speed;
    // Use this for initialization


    void Start () {
        
		Application.targetFrameRate = 60;

        drill_operate_speed = drill_speed;

        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        current_level = dataSystem.GetLevel();
        current_coin = dataSystem.GetCoin();

        SetLevelGUI();

        ReturnKeyManager.RegisterReturnKeyProcess(this);

        //Screen.SetResolution(720, 1280, true);

        continue_state = false;

        camera_size = camera_tweener.GetComponent<Camera>().orthographicSize;

    }

    public TweenAlpha red_twinkle;

    // Update is called once per frame
    void Update()
    {
        if(drill_time_gause != null && drill_time_gause.GetRemainGauseTime() < 0f)
        {
            count_clear_nail--;
            Manager.Client.SendEventHit("GameSequence", "gameover timeover", current_nail.disturb_type.ToString());
            FailRestartGame();
            drill.Stop();
            box.Stop();
            current_nail.Stop();
            time_gause_reset = true;
            SoundManager.PlayFailNailAndTimeOver();
            red_twinkle.ResetToBeginning();
            red_twinkle.PlayForward();
            VibratorController.StopVibrate();
            StopDisturb();
        }
    }

    void SetLevelGUI()
    {
        
        level_text.text = string.Format("LEVEL {0}", current_level);
    }

    bool time_gause_reset;

    bool continue_state;

    void FailRestartGame()
    {
        continue_state = true;
        if (input_system.activeSelf == false)
        {
            return;
        }
        drill.StopDrillIdleSound();
        input_system.SetActive(false);
        StopDisturb();
        drill_time_gause.StopGauseTime();
        play_fail_text_ani.gameObject.SetActive(true);
        play_fail_text_ani.Play(true);
        
        StartCoroutine(ShowContinueADPopup());
    }

    public Animation continue_popup_animation;

    int interstisialAD_count = 0;
    int retry_count = 0;
    int continue_count = 0;
    bool CheckContinuePopup()
    {
        bool retval = false;

		if(current_level > 10 && continue_count == 0)// && retry_count >= 5)
        {
            if(((float)(2f * current_level) / 3f) > (float)count_clear_nail)
            {
				if (UnityEngine.Random.Range (0, 1f) > 0.2f) 
				{
					retval = true;
				}
            }
        }

        return retval;
    }

    IEnumerator ShowContinueADPopup()
    {
        yield return new WaitForSeconds(1f);

        if (CheckContinuePopup() == true)
        {
            ReturnKeyManager.RegisterReturnKeyProcess(continue_popup_animation.GetComponent<ReturnKeyProcess>());
            continue_popup_animation.gameObject.SetActive(true);
            continue_popup_animation.GetComponent<BlurController>().Blur();
            ActiveAnimation.Play(continue_popup_animation, "ShowFailPopup", AnimationOrTween.Direction.Forward);
        }
        else
        {
            StartCoroutine(FailState());
        }

        continue_state = false;
    }
    
    public void CancelContinueAD()
    {
        continue_popup_animation.GetComponent<BlurController>().UnBlur();
        ActiveAnimation.Play(continue_popup_animation, "HideFailPopup", AnimationOrTween.Direction.Forward);
        
        StartCoroutine(FailState());
    }

    IEnumerator FailState()
    {
        ResetBoxAndDrill();

        yield return new WaitForSeconds(0.3f);

        continue_popup_animation.gameObject.SetActive(false);

        drill.StopDrillIdleSound();
        bottom_game_menu_controller.SetFailState();

        interstisialAD_count++;

        CheckInterstisialAd();

        ActiveAnimation.Play(game_ui_animation, "ShowInGameMenu", AnimationOrTween.Direction.Forward);

        star_point_controller.gameObject.SetActive(true);
        star_point_controller.StartStarEffect(0);

        yield return new WaitForSeconds(0.5f);
        
    }

    public void RetryLevel()
    {
        retry_count++;

        count_clear_nail = current_level;
        nail_point = 0;
        ChangeDrill();
    }

    public UIPlayTween clear_level_text;

    public GameObject input_system;

    void ClearLevel()
    {
        input_system.SetActive(false);
        drill.StopDrillIdleSound();
        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        clear_count_text.text = count_clear_nail.ToString();
        dataSystem.SetLevel(current_level+1);

        retry_count = 0;

        int point = Mathf.RoundToInt((float)nail_point / (float)current_level);
        GetComponent<GameDataSystem>().SetStartPoint(current_level, point);

        drill_time_gause.StopGauseTime();
        
        StartCoroutine(ClearLevelState());
    }

    public StarPointController star_point_controller;
    public Animation game_ui_animation;
    public BottomLayerController bottom_game_menu_controller;
    public GameObject drill_clear_point;

    IEnumerator ClearLevelState()
    {
        yield return new WaitForSeconds(1f);
        ResetBoxAndDrill();

        //clear_level_text.gameObject.SetActive(true);
        //clear_level_text.Play(true);

        yield return new WaitForSeconds(0.3f);
        
        int point = Mathf.RoundToInt((float)nail_point / (float)current_level);
        star_point_controller.gameObject.SetActive(true);
        star_point_controller.StartStarEffect(point);
        
        yield return new WaitForSeconds(0.3f);

        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        LimitedDrillManager manager = selector.GetComponent<LimitedDrillManager>();

        manager.GetLimitedDrillForLevel(current_level);

        drill.StopDrillIdleSound();
        bottom_game_menu_controller.SetClearState();

        interstisialAD_count++;
        CheckInterstisialAd();

        ActiveAnimation.Play(game_ui_animation, "ShowInGameMenu", AnimationOrTween.Direction.Forward);
    }

    void ResetBoxAndDrill()
    {
        float time = 0.3f;
        iTween.ScaleTo(box.gameObject, iTween.Hash("scale", new Vector3(0.7f, 0.7f, 0.7f), "time", time, "easetype", iTween.EaseType.linear));
        drill.StartCoroutine(drill.SetDrillPositionToTarget(drill_clear_point.transform, time, 0f, true));
        box.Reset(2f);
        drill_time_gause.DisableGauseTime();

        iTween.FadeTo(clear_count_text.gameObject, 0f, time);
    }

    public void UpateLevelText()
    {
        SetLevelGUI();
        count_clear_nail = current_level;
        clear_count_text.text = count_clear_nail.ToString();
    }

    bool gameover_flag = false;
    public void GameOver(Nail nail)
    {
        if(nail != current_nail)
        {
            return;
        }

        if(gameover_flag == true)
        {
            return;
        }
        VibratorController.StopVibrate();
        SoundManager.PlayFailOverAmazing();
        gameover_flag = true;
        drill_time_gause.StopGauseTime();
        StartCoroutine(CrashBox());
        StopDisturb();
    }

    
    IEnumerator CrashBox()
    {
        Debug.Log("CrashBox~~~~~~~~~~~~~~~~~~~~");
        count_clear_nail--;
        box.Crash(drill);
        drill.Stop();
        box.Stop();
        current_nail.Stop();
        time_gause_reset = true;

        yield return new WaitForSeconds(0.2f);

        red_twinkle.ResetToBeginning();
        red_twinkle.PlayForward();

        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("amount", new Vector3(2f, 2f, 0f), "time", 0.2f, "islocal", true));

        yield return new WaitForSeconds(1f);

        Manager.Client.SendEventHit("GameSequence", "gameover crash box", current_nail.disturb_type.ToString());
        FailRestartGame();
        gameover_flag = false;
    }

    public DrillTimeGause drill_time_gause;
    public float time_gause = 5f;
    int count_clear_nail = 0;
    int nail_point = 0;
    public Animation guide_panel;

    public void StartLastLevel()
    {
        current_level = current_level + 1;// dataSystem.GetLevel();
        retry_count = 0;
        nail_point = 0;

        UpateLevelText();

        ChangeDrill();
    }

    public IEnumerator StartGame()
    {
        tap_process = false;
        iTween.MoveTo(box.gameObject, new Vector3(0f, 0f, 0f), 1f);
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrill();

        yield return new WaitForSeconds(1f);

        //play_stage_ani_text.Play(true);

        //yield return new WaitForSeconds(2f);

        nail_table = new ArrayList();
        
        count_clear_nail = current_level;
        builder.BuildNail(nail_table, count_clear_nail, current_level, box.box_type);
        nail_index = 0;

        clear_count_text.text = count_clear_nail.ToString();


        current_nail = ((GameObject)(nail_table[nail_index])).GetComponent<Nail>();
        
        box.TurnBox(current_nail);

        coin_text.text = current_coin.ToString();

        iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(0f, -3f, 0f), "time", 0.3f, "islocal", true));
        SetDrillPositionToCurrentNail();
        yield return new WaitForSeconds(0.3f);
        
        tap_process = true;
        input_system.SetActive(true);
        drill.PlayDrillIdleSound();
        SoundManager.PlayBGM();

        cheerup_guide_controller.Show(current_level);

        guide_panel.GetComponent<TweenAlpha>().Play(true);
        yield return new WaitForSeconds(0.5f);
        guide_panel.GetComponent<Animation>().Play();

        continue_count = 0;

        FastGuidePopup();

        if (current_level != 1 && current_coin > 0)
        {
            BannerController.ShowBanner();
        }
    }

    void ResetBeforeGame()
    {
        drill_time_gause.EnableGauseTime();
        drill.ResetScale();
        box.Reset(3f);
        iTween.FadeTo(clear_count_text.gameObject, 1f, 0.1f);

        foreach (GameObject nail in nail_table)
        {
            Destroy(nail.transform.parent.gameObject);
        }

        ActiveAnimation.Play(game_ui_animation, "HideInGameMenu", AnimationOrTween.Direction.Forward);
        coin_text.text = current_coin.ToString();
    }

    bool tap_process = false;
    public ShutterController shutter_controller;
    IEnumerator ChangeBox(bool withShutter)
    {
        if(tap_process == false)
        {
            yield break;
        }
        tap_process = false;

        first_nail = true;
        drill_time_gause.StopGauseTime();
        
        if (withShutter == true)
        {
            shutter_controller.ShutDown();
            yield return new WaitForSeconds(0.5f);

            ResetBeforeGame();

            yield return new WaitForSeconds(0.2f);
            UpateLevelText();
        }
        else
        {
            box.Reset(1f);
            foreach (GameObject nail in nail_table)
            {
                Destroy(nail.transform.parent.gameObject);
            }

            yield return new WaitForSeconds(0.3f);

            box.RotateBox();
            yield return new WaitForSeconds(0.5f);
        }
        
        //iTween.MoveTo(box.gameObject, new Vector3(0f, 0f, 0f), 0.2f);

        box.ResetRotateBox();


        //yield return new WaitForSeconds(0.3f);

        star_point_controller.ArrageStar();
        

        nail_table.Clear();
        builder.BuildNail(nail_table, count_clear_nail, current_level, box.box_type);
        nail_index = 0;

        current_nail = ((GameObject)(nail_table[nail_index])).GetComponent<Nail>();
        box.TurnBox(current_nail);

        if (withShutter == false)
        {
            drill.StartCoroutine(drill.SetDrillPositionToTarget(current_nail.transform, 0.2f, 0f, true));
        }
        else
        {
            continue_count = 0;
            SetDrillPositionToCurrentNail();
        }

        clear_count_text.text = count_clear_nail.ToString();

        
        //iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(0f, -3f, 0f), "time", 0.3f, "islocal", true));
        

        if (withShutter == true)
        {
            shutter_controller.ShutUp();
        }
        yield return new WaitForSeconds(0.3f);
        tap_process = true;
        input_system.SetActive(true);
        drill.PlayDrillIdleSound();

        //guide_panel.GetComponent<TweenAlpha>().Play(true);
        //yield return new WaitForSeconds(0.5f);
        //guide_panel.GetComponent<Animation>().Play();

        //cheerup_guide_controller.Show(current_level);

        drill_time_gause.SetGauseTime(time_gause);

        
    }

    public UIPlayTween play_disturb_perfect_text_ani;
    public UIPlayTween play_disturb_speed_text_ani;
    public UIPlayTween play_disturb_slow_speed_text_ani;
    public void DisplayDisturbGuideText()
    {
        if(current_nail.disturb_type == Nail.DISTURB_TYPE.PERFECT)
        {
            play_disturb_perfect_text_ani.gameObject.SetActive(true);
            play_disturb_perfect_text_ani.Play(true);
        }

        if (current_nail.disturb_type == Nail.DISTURB_TYPE.TWICE_SPEED)
        {
            play_disturb_speed_text_ani.gameObject.SetActive(true);
            play_disturb_speed_text_ani.Play(true);
        }

        if (current_nail.disturb_type == Nail.DISTURB_TYPE.SLOW_SPEED)
        {
            play_disturb_slow_speed_text_ani.gameObject.SetActive(true);
            play_disturb_slow_speed_text_ani.Play(true);
        }
    }

    bool first_nail = true;

    public void SetStartState()
    {
        ActiveAnimation.Play(game_ui_animation, "StartInGameMenu", AnimationOrTween.Direction.Forward);
        //bottom_game_menu_controller.SetStartState();
    }

    public void PauseGame()
    {
        //ReturnKeyManager.RegisterReturnKeyProcess(game_ui_animation.GetComponent<ReturnKeyProcess>());
        bottom_game_menu_controller.SetPauseState();
        drill_time_gause.PauseGauseTime();
        drill.StopDrillIdleSound();

        BannerController.ShowBanner();
    }

    public void ResumeGame()
    {
        drill_time_gause.ResumeGauseTime();
        ActiveAnimation.Play(game_ui_animation, "HideInGameMenu", AnimationOrTween.Direction.Forward);
        drill.PlayDrillIdleSound();

        //BannerController.HideBanner();
    }

    public void CancelContinue()
    {
        
    }

    public void Continue()
    {
		count_clear_nail++;
        Manager.Client.SendEventHit("GameSequence", "Continue", current_level.ToString() + "_" + count_clear_nail.ToString());
        ActiveAnimation.Play(game_ui_animation, "HideInGameMenu", AnimationOrTween.Direction.Forward);
        star_point_controller.ArrageStar();
        //NextNail();
		ContinueNail ();
        retry_count = 0;
        continue_count++;
        input_system.SetActive(true);
        drill_time_gause.EnableGauseTime();
        drill_time_gause.StopGauseTime();
    }

    public void TapDown()
    {
        

        Debug.Log("------------------" + box.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        if(box.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).tagHash != 0)
        {
            return;
        }

        if (tap_process == false || gameover_flag == true || nail_index >= nail_table.Count)
        {
            return;
        }

        VibratorController.Vibrate();

        if (first_nail == true)
        {
            ActiveAnimation.Play(game_ui_animation, "HideInGameMenu", AnimationOrTween.Direction.Forward);
            drill_time_gause.SetGauseTime(time_gause);
            
            guide_panel.GetComponent<TweenAlpha>().Play(false);
            guide_panel.GetComponent<Animation>().Stop();

            cheerup_guide_controller.Hide();
            first_nail = false;
        }
        StartDisturb();

        drill.Operate();
        box.Operate();
        current_nail.Screw(drill_operate_speed);
        time_gause_reset = false;
    }
    
    public float rotate_angle = 90f;
    public float camera_zoom_out_size = 25f;
    public float camera_zoom_in_size = 3f;
    public float fast_drill_speed = 1.6666f;

    private float camera_size = 6.4f;

    void StartDisturb()
    {
        Debug.Log(current_nail.disturb_type);

        if (current_nail.disturb_type == Nail.DISTURB_TYPE.PERFECT)
        {

        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.TWICE_SPEED)
        {
            drill_operate_speed = drill_speed * fast_drill_speed;
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.SLOW_SPEED)
        {
            drill_operate_speed = drill_speed * 0.6f;
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.ZOOM_OUT)
        {
            if (current_level >= 20)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);
                camera_tweener.SetSize(camera_zoom_out_size, 2f);
            }
            else
            {
                camera_tweener.SetSize(camera_zoom_out_size, 2f);
            }
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.ZOOM_IN)
        {
            if (current_level >= 20)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);
                camera_tweener.SetSize(camera_zoom_in_size, 2f);
            }
            else
            {
                camera_tweener.SetSize(camera_zoom_in_size, 2f);
            }
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.ROTATE)
        {
            if (current_level >= 20)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);


                rand = UnityEngine.Random.Range(0, 2);
                float zoom = rand == 1 ? camera_zoom_in_size : camera_zoom_out_size;
                camera_tweener.SetSize(zoom, 2f);
                
            }
            else
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);
            }
        }
    }

    void StartPreDisturb()
    {
        

        if (current_nail.disturb_type == Nail.DISTURB_TYPE.PRE_ZOOM_OUT)
        {
            if (current_level >= 20)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);
                camera_tweener.SetSize(camera_zoom_out_size, 2f);
            }
            else
            {
                camera_tweener.SetSize(camera_zoom_out_size, 2f);
            }
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.PRE_ZOOM_IN)
        {
            if (current_level >= 20)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                float rotate_speed = rand == 1 ? rotate_angle : -rotate_angle;
                camera_tweener.SetRotation(rotate_speed, 3f);
                camera_tweener.SetSize(camera_zoom_in_size, 2f);
            }
            else
            {
                camera_tweener.SetSize(camera_zoom_in_size, 2f);
            }
        }
    }

    void StopDisturb()
    {
        if (current_nail.disturb_type == Nail.DISTURB_TYPE.PERFECT)
        {
            play_disturb_perfect_text_ani.gameObject.SetActive(false);
            if (current_nail.collision_state != Nail.NAIL_STATE.PERFECT 
                && current_nail.collision_state != Nail.NAIL_STATE.AMAZING)

            {
                current_nail.collision_state = Nail.NAIL_STATE.FAIL;
            }
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.TWICE_SPEED)
        {
            drill_operate_speed = drill_speed;
            play_disturb_speed_text_ani.gameObject.SetActive(false);
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.SLOW_SPEED)
        {
            drill_operate_speed = drill_speed;
            play_disturb_slow_speed_text_ani.gameObject.SetActive(false);
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.ZOOM_IN ||
            current_nail.disturb_type == Nail.DISTURB_TYPE.ZOOM_OUT ||
            current_nail.disturb_type == Nail.DISTURB_TYPE.PRE_ZOOM_IN ||
            current_nail.disturb_type == Nail.DISTURB_TYPE.PRE_ZOOM_OUT)
        {
            camera_tweener.SetSize(camera_size, 0.1f);
            camera_tweener.SetRotation(0f, 0.1f);
        }
        else if (current_nail.disturb_type == Nail.DISTURB_TYPE.ROTATE)
        {
            camera_tweener.SetSize(camera_size, 0.1f);
            camera_tweener.SetRotation(0f, 0.1f);
        }
    }


    public UIPlayTween play_fail_text_ani;
    public UIPlayTween play_good_text_ani;
    public UIPlayTween play_perfect_text_ani;
    public UIPlayTween play_amazing_text_ani;
    public TextMesh clear_count_text;
    
    public UILabel speed_value;
    public TextMesh level_text;
    public void ChangeSliderValue(UISlider slider)
    {
        float value = slider.value * 2f;
        drill_operate_speed = drill_speed * value;
        speed_value.text = value.ToString();
    }

    public void SetLevel(int level)
    {
        star_point_popup.Return();
        current_level = level;
        count_clear_nail = current_level;
        retry_count = 0;
        nail_point = 0;
        ChangeDrill();
        StartCoroutine(ReturnStarPointPopup());
    }

    public StarPointPopup star_point_popup;
    IEnumerator ReturnStarPointPopup()
    {
        yield return new WaitForSeconds(0.5f);
        star_point_popup.ReturnStarPointPopup(false);
    }

    public void ChangeDrill()
    {
        StartCoroutine(ChangeDrillSequence());
    }

    public GameObject fast_nail_guide_popup;
    public CheerupGuideController cheerup_guide_controller;

    IEnumerator ChangeDrillSequence()
    {
        if (tap_process == false)
        {
            yield break;
        }
        tap_process = false;

        first_nail = true;

        cheerup_guide_controller.Hide();

        drill_time_gause.StopGauseTime();

        shutter_controller.ShutDown();
        yield return new WaitForSeconds(0.5f);

        ResetBeforeGame();
        yield return new WaitForSeconds(0.2f);

        UpateLevelText();

        Destroy(drill.gameObject);
        Destroy(box.box_anchor.transform.parent.gameObject);
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrill();

		ActiveAnimation.Play(game_ui_animation, "StartInGameMenu", AnimationOrTween.Direction.Forward);

        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        current_coin = dataSystem.GetCoin();
        coin_text.text = current_coin.ToString();

        //yield return new WaitForSeconds(0.2f);        

        //iTween.MoveTo(box.gameObject, new Vector3(0f, 0f, 0f), 0.2f);

        box.ResetRotateBox();

        //yield return new WaitForSeconds(0.3f);

        star_point_controller.ArrageStar();

        nail_table.Clear();
        builder.BuildNail(nail_table, count_clear_nail, current_level, box.box_type);
        nail_index = 0;

        clear_count_text.text = count_clear_nail.ToString();

        current_nail = ((GameObject)(nail_table[nail_index])).GetComponent<Nail>();
        box.TurnBox(current_nail);

        //iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(0f, -3f, 0f), "time", 0.3f, "islocal", true));
        SetDrillPositionToCurrentNail();

        //BannerController.HideBanner();

        shutter_controller.ShutUp();

        yield return new WaitForSeconds(0.3f);
        tap_process = true;
        input_system.SetActive(true);
        drill.PlayDrillIdleSound();

        cheerup_guide_controller.Show(current_level);

        guide_panel.GetComponent<TweenAlpha>().Play(true);
        yield return new WaitForSeconds(0.5f);
        guide_panel.GetComponent<Animation>().Play();

        


        

        FastGuidePopup();

        continue_count = 0;
    }
    
    public void TapUp()
    {
        VibratorController.StopVibrate();
        if (tap_process == false || time_gause_reset == true )
        {
            return;
        }
        StopDisturb();
        drill_time_gause.SetGauseTime(time_gause);

        drill.Stop();
        box.Stop();
        current_nail.Stop();

        count_clear_nail--;
        
        int point = 0;
        if (current_nail.collision_state == Nail.NAIL_STATE.GOOD)
        {
            play_good_text_ani.gameObject.SetActive(true);
            play_good_text_ani.Play(true);
            point = 1;
            SoundManager.PlayGoodPerfect();
        }
        if (current_nail.collision_state == Nail.NAIL_STATE.PERFECT)
        {
            play_perfect_text_ani.gameObject.SetActive(true);
            play_perfect_text_ani.Play(true);
            point = 2;
            SoundManager.PlayGoodPerfect();
        }
        if(current_nail.collision_state == Nail.NAIL_STATE.AMAZING)
        {
            play_amazing_text_ani.gameObject.SetActive(true);
            play_amazing_text_ani.Play(true);
            point = 3;
            SoundManager.PlayAmazing();
        }

        nail_point += point;

        current_nail.Clear(this);

        Debug.Log("clear_nail count " + count_clear_nail.ToString());
        if (CheckGameOver() == true || CheckClearLevel() == true)
        {
            
            return;
        }

        NextNail();
    }

    void CheckInterstisialAd()
    {
        if (current_level >= 5 )
        {
            if (interstisialAD_count == 4)
            {
                InterstitialController.RequestInterstitialAd();
            }

            if( interstisialAD_count >= 6)
            {
                GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
                LimitedDrillManager manager = selector.GetComponent<LimitedDrillManager>();

                if(manager.CheckLimitedDrillForLevel(current_level) < 0
                    && count_clear_nail > 0)
                {
                    InterstitialController.ShowInterstisialsAD();
                    interstisialAD_count = 0;
                }
                
            }
        }
    }

	void ContinueNail()
	{
		
		current_nail.SetStartPos();
		SetDrillPositionToCurrentNail();
	}

    void NextNail()
    {
        clear_count_text.text = count_clear_nail.ToString();

        nail_index++;

        if (nail_index >= nail_table.Count)
        {
            StartCoroutine(ChangeBox(false));
        }
        else
        {
            Nail new_nail = ((GameObject)(nail_table[nail_index])).GetComponent<Nail>();
            if (current_nail.direction != new_nail.direction)
            {
                box.TurnBox(new_nail);
            }

            current_nail = new_nail;
            SetDrillPositionToCurrentNail();
        }
    }

    IEnumerator ClearStage()
    {
        yield return new WaitForSeconds(1f);
    }

    Nail current_nail;
    void SetDrillPositionToCurrentNail()
    {
        drill.SetDrillPositionToTarget(current_nail.transform);

        //DisplayDisturbGuideText();
        StartPreDisturb();
        current_nail.DisableGudie();
    }

    bool CheckClearLevel()
    {
        if(count_clear_nail == 0)
        {
            ClearLevel();
            
            return true;
        }

        return false;
    }

    bool CheckGameOver()
    {
        if (current_nail.collision_state == Nail.NAIL_STATE.FAIL)
        {
            Manager.Client.SendEventHit("GameSequence", "gameover fail nail", current_nail.disturb_type.ToString());
            red_twinkle.ResetToBeginning();
            red_twinkle.PlayForward();
            SoundManager.PlayFailNailAndTimeOver();
            FailRestartGame();
            //clear_count_text.text = "0";
            return true;
        }

        return false;
    }

    public TextMesh coin_text;
    public void GetCoin()
    {
        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        current_coin = dataSystem.GetCoin();

        current_coin++;
        coin_text.text = current_coin.ToString();
        dataSystem.SetCoin(current_coin, false);
    }

    public void UpdateCoin()
    {
        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        current_coin = dataSystem.GetCoin();
        coin_text.text = current_coin.ToString();
    }

    public Animation exit_popup_animation;
    public void Return()
    {
        if(continue_state == true)
        {
            return;
        }

        exit_popup_animation.gameObject.SetActive(true);
        exit_popup_animation.GetComponent<BlurController>().Blur();
        ActiveAnimation.Play(exit_popup_animation, "ShowFailPopup", AnimationOrTween.Direction.Forward);

        ReturnKeyManager.RegisterReturnKeyProcess(exit_popup_animation.GetComponent<ReturnKeyProcess>());

        drill_time_gause.PauseGauseTime();
    }

    bool first_run_level_13 = true;
    public void FastGuidePopup()
    {
        if(first_run_level_13 == false)
        {
            return;
        }

        if(current_level < 13 || current_level > 15)
        {
            return;
        }

        GameDataSystem dataSystem = GetComponent<GameDataSystem>();
        if (dataSystem.GetCheckFastNail() == true)
        {
            return;
        }

        first_run_level_13 = false;

        fast_nail_guide_popup.gameObject.SetActive(true);
        fast_nail_guide_popup.GetComponent<BlurController>().Blur();

        ActiveAnimation.Play(fast_nail_guide_popup.GetComponent<Animation>(), "ShowFailPopup", AnimationOrTween.Direction.Forward);
        ReturnKeyManager.RegisterReturnKeyProcess(fast_nail_guide_popup.GetComponent<ReturnKeyProcess>());

        dataSystem.SetCheckFastNail();
    }

    
}
