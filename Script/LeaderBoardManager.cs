using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static string best_level_id = "CgkI58npnusaEAIQAQ";
    static string best_score_infinity_id = "CgkI58npnusaEAIQAg";
    static string best_score_hell_id = "CgkI58npnusaEAIQAw";

    public static void LoadLeaderboard()
    {
        GooglePlayManager.ActionLeaderboardsLoaded += OnLeaderBoardsLoaded;
        GooglePlayManager.Instance.LoadLeaderBoards();
    }

   private static void OnLeaderBoardsLoaded(GooglePlayResult result)
    {
        GooglePlayManager.ActionLeaderboardsLoaded -= OnLeaderBoardsLoaded;
        if (result.IsSucceeded)
        {
            Debug.Log("Leader boards loaded");
        }
        else
        {
            Debug.Log("Leader boards loaded fail");
        }
    }

    public static void UpdateBestLevel(int level)
    {
        GooglePlayManager.Instance.SubmitScoreById(best_level_id, level);
    }

    public static void UpdateBestScoreInfinityMode(int score)
    {
        GooglePlayManager.Instance.SubmitScoreById(best_score_infinity_id, score);
    }

    public static void UpdateBestScoreHellMode(int score)
    {
        GooglePlayManager.Instance.SubmitScoreById(best_score_hell_id, score);
    }

    public static void ShowBestLevel()
    {
        GooglePlayManager.Instance.ShowLeaderBoardById (best_level_id);
    }

    public static void ShowBestScoreInfinityMode()
    {
        GooglePlayManager.Instance.ShowLeaderBoardById(best_score_infinity_id);
    }

    public static void ShowBestScoreHellMode()
    {
        GooglePlayManager.Instance.ShowLeaderBoardById(best_score_hell_id);
    }

}
