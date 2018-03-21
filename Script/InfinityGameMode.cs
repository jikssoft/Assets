using SA.Analytics.Google;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityGameMode : MonoBehaviour, GameMainLogicSystem.GameMode
{
    GameDataSystem dataSystem;
    GameMainLogicSystem system;

    public GameObject fast_nail_guide_popup;
    public CheerupGuideController cheerup_guide_controller;

    public GameObject game_ui_obj;

    // Use this for initialization
    void Start()
    {
        GameObject system_obj = GameObject.FindGameObjectWithTag("System");
        system = system_obj.GetComponent<GameMainLogicSystem>();
        dataSystem = system_obj.GetComponent<GameDataSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public TextMesh clear_count_text;

    public int count_clear_nail = 0;

    public bool CheckContinuePopup(int continue_count)
    {
        bool retval = false;

        int score = dataSystem.GetInifityScore();

        if (score < count_clear_nail)
        {
            return false;
        }

        if (count_clear_nail > 10 && continue_count == 0)// && retry_count >= 5)
        {
            if ((score / 2) < count_clear_nail)
            {
                if (UnityEngine.Random.Range(0, 1f) > 0.2f)
                {
                    retval = true;
                }
            }
        }

        return retval;
    }

    public TextMesh score_text;
    public void UpdateGUI()
    {
        game_ui_obj.SetActive(true);
        bg.SetActive(true);

        result_game_point.GetComponentInChildren<TweenAlpha>().ResetToBeginning();

        int score = dataSystem.GetInifityScore();
        score_text.text = string.Format("BEST {0}", score);
        iTween.FadeTo(clear_count_text.gameObject, 1f, 0.1f);

        count_clear_nail = 0;
        clear_count_text.text = count_clear_nail.ToString();
    }

    public IEnumerator StartGame(float first_time, float second_time, float third_time)
    {
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrillInfiniteMode();
        system.box.AdjustInfinityModeBoxPositionForMultiResolution();

        yield return new WaitForSeconds(first_time);

        system.nail_table = new ArrayList();

        count_clear_nail = 0;
        system.builder.BuildNailInfinityMode(system.nail_table, count_clear_nail, system.box);
        system.nail_index = 0;
        unit_clear_count_nail = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;


        yield return new WaitForSeconds(second_time);

        iTween.MoveTo(system.box.gameObject, iTween.Hash("position", new Vector3(0f, -2.58f, 0f), "time", 0.3f, "islocal", true));

        yield return new WaitForSeconds(third_time);

        FastGuidePopup();

    }

 
    public IEnumerator ReStartGame(float first_time, float second_time, float third_time)
    {
        yield return new WaitForSeconds(first_time);

        UpdateGUI();

        Destroy(system.drill.gameObject);
        system.box.DeleteAllBox();
        GameObject selector = GameObject.FindGameObjectWithTag("DrillSelector");
        selector.GetComponent<DrillSelector>().ChangeDrillInfiniteMode();
        system.box.AdjustInfinityModeBoxPositionForMultiResolution();


        system.nail_table.Clear();
        count_clear_nail = 0;
        system.builder.BuildNailInfinityMode(system.nail_table, count_clear_nail, system.box);
        system.nail_index = 0;
        unit_clear_count_nail = 0;

        clear_count_text.text = count_clear_nail.ToString();

        system.build_nail_table = true;

        yield return new WaitForSeconds(second_time);
        iTween.MoveTo(system.box.gameObject, iTween.Hash("position", new Vector3(0f, -2.58f, 0f), "time", 0.3f, "islocal", true));

        yield return new WaitForSeconds(third_time);

        FastGuidePopup();
    }

    public void MoveBoxAndDrill()
    {
        float time = 0.3f;
        
        system.drill.StartCoroutine(system.drill.SetDrillPositionToTarget(system.drill_clear_point.transform, time, 0f, true));
        system.box.Reset(2f);
        system.drill_time_gause.DisableGauseTime();

        UpdatePreFailClearGUI(time);
    }

    public void SetDrillPosition()
    {
        system.SetDrillPositionToCurrentNail();
    }

    public IEnumerator ChangeBox(bool withShutter)
    {
        //system.first_nail = true;
        system.drill_time_gause.StopGauseTime();

        system.box.ScrollBox();
        system.builder.ScrollNails(system.nail_table, -system.box.GetDistanceBox(), 0.1f);
        
        system.build_nail_table = true;

        if (system.nail_index >= system.nail_table.Count)
        {
            system.nail_index = 0;
        }

        system.SetNextNail();
        system.box.NextBox();
        system.SetSystemTapProcess();

        bg.GetComponent<_2dxFX_Offset>()._AutoScrollSpeedX = 100f;

        yield return new WaitForSeconds(0.1f);

        bg.GetComponent<_2dxFX_Offset>()._AutoScrollSpeedX = 0f;

        int re_position_start_nail_index = system.nail_index - unit_max_count_nail;
        if(re_position_start_nail_index < 0)
        {
            re_position_start_nail_index = system.nail_table.Count - unit_max_count_nail;
        }

        yield return new WaitForSeconds(0.1f);

        system.box.SetFrontBoxToLast();
        system.builder.SetRePositionNail(system.nail_table,
                re_position_start_nail_index,
                system.box.GetLastBox(),
                false);
        
        
        clear_count_text.text = count_clear_nail.ToString();

        

        yield return null;
    }
    
    public void FastGuidePopup()
    {
        if (dataSystem.GetCheckFastNail() == true)
        {
            return;
        }

        fast_nail_guide_popup.gameObject.SetActive(true);
        fast_nail_guide_popup.GetComponent<BlurController>().Blur();

        ActiveAnimation.Play(fast_nail_guide_popup.GetComponent<Animation>(), "ShowFailPopup", AnimationOrTween.Direction.Forward);
        ReturnKeyManager.RegisterReturnKeyProcess(fast_nail_guide_popup.GetComponent<ReturnKeyProcess>());

        dataSystem.SetCheckFastNail();
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

        count_clear_nail += point;
    }

    public void ProcessClearGamePoint()
    {
        //To do save best point
    }


    public UIPlayTween result_game_point;
    public void UpdateFailGUI()
    {
        //count_clear_nail--;

        result_game_point.gameObject.SetActive(true);
        result_game_point.Play(true);
        result_game_point.GetComponentInChildren<UILabel>().text = count_clear_nail.ToString();

        dataSystem.SetInfinityScore(count_clear_nail);
    }

    public void UpdateClearGUI()
    {
        //To show best point
    }

    public void ProcessClearReward()
    {

    }

    public void UpdatePreFailClearGUI(float time)
    {
        iTween.FadeTo(clear_count_text.gameObject, 0f, time);
    }

    public void Continue()
    {
        //count_clear_nail--;
        Manager.Client.SendEventHit("GameSequence", "Continue-InfinityMode", count_clear_nail.ToString() + "_" + count_clear_nail.ToString());
    }

    public int GetAdvancedDiturbTriggerValue()
    {
        return count_clear_nail;
    }

    public void SetLevel(int level)
    {
    }

    public bool CheckInterstitialAD()
    {
        if (count_clear_nail >= 60)
        {
            return true;
        }
        return false;
    }

    int unit_clear_count_nail;
    int unit_max_count_nail = 3;
    public bool CheckNextNail()
    {
        unit_clear_count_nail++;
        clear_count_text.text = count_clear_nail.ToString();

        if (unit_clear_count_nail >= unit_max_count_nail)
        {
            unit_clear_count_nail = 0;
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
        //count_clear_nail++;
    }

    public void CrashBox()
    {
        //count_clear_nail++;
    }

    public void NextStage()
    {
        
    }

    public GameObject bg;
    public void CleanGameMode()
    {
        bg.SetActive(false);
        game_ui_obj.SetActive(false);
    }

    public void ShowLeaderBoard()
    {
        LeaderBoardManager.ShowBestScoreInfinityMode();
    }
}
