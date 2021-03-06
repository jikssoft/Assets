﻿using SA.Analytics.Google;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameMode : MonoBehaviour, GameMainLogicSystem.GameMode {
    

    GameDataSystem dataSystem;
    GameMainLogicSystem system;

    public GameObject fast_nail_guide_popup;
    public CheerupGuideController cheerup_guide_controller;

    public GameObject game_ui_obj;

    // Use this for initialization
    void Start () {
        GameObject system_obj = GameObject.FindGameObjectWithTag("System");
        system = system_obj.GetComponent<GameMainLogicSystem>();
        dataSystem = system_obj.GetComponent<GameDataSystem>();
        current_level = dataSystem.GetLevel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public TextMesh level_text;
    public TextMesh clear_count_text;
    public StarPointController star_point_controller;

    public int current_level;
    public int count_clear_nail = 0;
    
    public bool CheckContinuePopup(int continue_count)
    {
        bool retval = false;

        if (current_level > 10 && continue_count == 0)// && retry_count >= 5)
        {
            if (((float)(2f * current_level) / 3f) > (float)count_clear_nail)
            {
                if (UnityEngine.Random.Range(0, 1f) > 0.2f)
                {
                    retval = true;
                }
            }
        }

        return retval;
    }

    public void UpdateGUI()
    {
        game_ui_obj.SetActive(true);

        iTween.FadeTo(clear_count_text.gameObject, 1f, 0.1f);

        count_clear_nail = current_level;
        level_text.text = string.Format("LEVEL {0}", current_level);
        clear_count_text.text = count_clear_nail.ToString();
    }

    public IEnumerator StartGame(float first_time, float second_time, float third_time)
    {
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrill();
        system.box.AdjustLevelModeBox();

        yield return new WaitForSeconds(first_time);

        system.nail_table = new ArrayList();

        count_clear_nail = current_level;
        system.builder.BuildNail(system.nail_table, count_clear_nail, current_level, system.box.box_type);
        system.nail_index = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;
        

        yield return new WaitForSeconds(second_time);

        iTween.MoveTo(system.box.gameObject, iTween.Hash("position", new Vector3(0f, -3f, 0f), "time", 0.3f, "islocal", true));
        cheerup_guide_controller.Show(current_level);

        yield return new WaitForSeconds(third_time);

        FastGuidePopup();

        if (current_level != 1 && system.current_coin > 0)
        {
            BannerController.ShowBanner();
        }
    }

    public IEnumerator ReStartGame(float first_time, float second_time, float third_time)
    {
        cheerup_guide_controller.Hide();

        yield return new WaitForSeconds(first_time);

        UpdateGUI();

        Destroy(system.drill.gameObject);
        system.box.DeleteAllBox();
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrill();
        system.box.AdjustLevelModeBox();

        star_point_controller.ArrageStar();

        system.nail_table.Clear();
        system.builder.BuildNail(system.nail_table, count_clear_nail, current_level, system.box.box_type);
        system.nail_index = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;

        yield return new WaitForSeconds(second_time);

        iTween.MoveTo(system.box.gameObject, iTween.Hash("position", new Vector3(0f, -3f, 0f), "time", 0.3f, "islocal", true));
        cheerup_guide_controller.Show(current_level);

        yield return new WaitForSeconds(third_time);

        FastGuidePopup();

        if (current_level != 1 && system.current_coin > 0)
        {
            BannerController.ShowBanner();
        }
    }

    public IEnumerator ChangeBox(bool withShutter)
    {
        system.first_nail = true;
        system.drill_time_gause.StopGauseTime();

        if (withShutter == true)
        {
            system.shutter_controller.ShutDown();
            yield return new WaitForSeconds(0.5f);

            system.DrillBoxMenuReset();

            yield return new WaitForSeconds(0.2f);
            UpdateGUI();
        }
        else
        {
            system.box.Reset(1f);
            foreach (GameObject nail in system.nail_table)
            {
                Destroy(nail.transform.parent.gameObject);
            }

            yield return new WaitForSeconds(0.3f);

            system.box.RotateBox();
            yield return new WaitForSeconds(0.5f);
        }

        //iTween.MoveTo(box.gameObject, new Vector3(0f, 0f, 0f), 0.2f);

        system.box.ResetRotateBox();
        
        //yield return new WaitForSeconds(0.3f);
        
        star_point_controller.ArrageStar();

        system.nail_table.Clear();
        system.builder.BuildNail(system.nail_table, count_clear_nail, current_level, system.box.box_type);
        system.nail_index = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;

        
    }

    public void SetDrillPosition()
    {
        system.drill.StartCoroutine(
            system.drill.SetDrillPositionToTarget(system.GetCurrentNail().transform, 0.2f, 0f, true));
    }

    public void MoveBoxAndDrill()
    {
        float time = 0.3f;
        iTween.ScaleTo(system.box.gameObject, iTween.Hash("scale", new Vector3(0.7f, 0.7f, 0.7f), "time", time, "easetype", iTween.EaseType.linear));
        system.drill.StartCoroutine(system.drill.SetDrillPositionToTarget(system.drill_clear_point.transform, time, 0f, true));
        system.box.Reset(2f);
        system.drill_time_gause.DisableGauseTime();

        UpdatePreFailClearGUI(time);
    }

    bool first_run_level_13 = true;
    public void FastGuidePopup()
    {
        if (first_run_level_13 == false)
        {
            return;
        }

        if (current_level < 13 || current_level > 15)
        {
            return;
        }

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


    public IEnumerator CreateGame(float first_time, float second_time, float third_time)
    {
        throw new NotImplementedException();
    }   

    public void TapDown()
    {
        if (system.first_nail == true)
        {
            cheerup_guide_controller.Hide();
        }
    }

    public UIPlayTween play_good_text_ani;
    public UIPlayTween play_perfect_text_ani;
    public UIPlayTween play_amazing_text_ani;

    public void TapUp(ref int point)
    {
        count_clear_nail--;

        if (system.GetCurrentNail().collision_state == Nail.NAIL_STATE.GOOD)
        {
            play_good_text_ani.gameObject.SetActive(true);
            play_good_text_ani.Play(true);
            point = 1;
            SoundManager.PlayGoodPerfect();
        }
        if (system.GetCurrentNail().collision_state == Nail.NAIL_STATE.PERFECT)
        {
            play_perfect_text_ani.gameObject.SetActive(true);
            play_perfect_text_ani.Play(true);
            point = 2;
            SoundManager.PlayGoodPerfect();
        }
        if (system.GetCurrentNail().collision_state == Nail.NAIL_STATE.AMAZING)
        {
            play_amazing_text_ani.gameObject.SetActive(true);
            play_amazing_text_ani.Play(true);
            point = 3;
            SoundManager.PlayAmazing();
        }
    }
    
    public void ProcessClearGamePoint()
    {
        clear_count_text.text = count_clear_nail.ToString();
        dataSystem.SetLevel(current_level + 1);

        int point = Mathf.RoundToInt((float)system.nail_point / (float)current_level);
        dataSystem.SetStartPoint(current_level, point);
    }

    public void UpdateFailGUI()
    {
        star_point_controller.gameObject.SetActive(true);
        star_point_controller.StartStarEffect(0);
    }

    public void UpdateClearGUI()
    {
        int point = Mathf.RoundToInt((float)system.nail_point / (float)current_level);
        star_point_controller.gameObject.SetActive(true);
        star_point_controller.StartStarEffect(point);
    }

    public void ProcessClearReward()
    {
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        LimitedDrillManager manager = selector.GetComponent<LimitedDrillManager>();

        manager.GetLimitedDrillForLevel(current_level);
    }

    public void UpdatePreFailClearGUI(float time)
    {
        iTween.FadeTo(clear_count_text.gameObject, 0f, time);
    }

    public void Continue()
    {
        count_clear_nail++;
        Manager.Client.SendEventHit("GameSequence", "Continue", current_level.ToString() + "_" + count_clear_nail.ToString());
        
        star_point_controller.ArrageStar();
    }

    public int GetAdvancedDiturbTriggerValue()
    {
        return current_level;
    }

    public void SetLevel(int level)
    {
        current_level = level;
        count_clear_nail = current_level;
    }

    public bool CheckInterstitialAD()
    {
        if (current_level >= 5)
        {
            GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
            LimitedDrillManager manager = selector.GetComponent<LimitedDrillManager>();

            if (manager.CheckLimitedDrillForLevel(current_level) < 0
                && count_clear_nail > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckNextNail()
    {
        clear_count_text.text = count_clear_nail.ToString();

        if (system.nail_index >= system.nail_table.Count)
        {
            return true;
        }
        return false;
    }

    public bool CheckClearGame()
    {
        return count_clear_nail == 0;
    }

    public void TimeGaugeOver()
    {
        count_clear_nail--;
    }

    public void CrashBox()
    {
        count_clear_nail--;
    }

    public void NextStage()
    {
        current_level = current_level + 1;
    }

    public void CleanGameMode()
    {
        game_ui_obj.SetActive(false);
        cheerup_guide_controller.Hide();
    }

    public void ShowLeaderBoard()
    {
        LeaderBoardManager.ShowBestLevel();
    }
}
