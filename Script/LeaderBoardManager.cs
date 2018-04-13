using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (RuntimePlatform.IPhonePlayer == Application.platform) {
			
			GameCenterManager.OnAuthFinished += GameCenterManager_OnAuthFinished;
			GameCenterManager.Init ();

		}
	}

	void GameCenterManager_OnAuthFinished (SA.Common.Models.Result obj)
	{
		if (obj.IsSucceeded) {
			Debug.Log ("GameCenter Init");
		} else {
			Debug.Log ("GameCenter Init fail");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    static string best_level_id = "CgkI58npnusaEAIQAQ";
    static string best_score_infinity_id = "CgkI58npnusaEAIQAg";
    static string best_score_hell_id = "CgkI58npnusaEAIQAw";

	static string best_level_id_ios = "com.taptapdrill.leaderboard.levelranking";
	static string best_score_infinity_id_ios = "com.taptapdrill.leaderboard.infinityranking";
	static string best_score_hell_id_ios = "com.taptapdrill.leaderboard.hellranking";


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
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.SubmitScoreById (best_level_id, level);
    
		} else {
			GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;
			GameCenterManager.ReportScore(level, best_level_id_ios); 
		}
	}

	static void OnScoreSubmitted(GK_LeaderboardResult result)
	{
		GameCenterManager.OnScoreSubmitted -= OnScoreSubmitted;
		if (result.IsSucceeded) {
			Debug.Log ("succeeded submit score");
		}
	}

    public static void UpdateBestScoreInfinityMode(int score)
    {
        
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.SubmitScoreById(best_score_infinity_id, score);

		} else {
			GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;
			GameCenterManager.ReportScore (score, best_score_infinity_id_ios); 
		}
    }

    public static void UpdateBestScoreHellMode(int score)
    {
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.SubmitScoreById(best_score_hell_id, score);

		} else {
			GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;
			GameCenterManager.ReportScore (score, best_score_hell_id_ios); 
		}
    }

    public static void ShowBestLevel()
    {
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.ShowLeaderBoardById (best_level_id);
		} else {
			GameCenterManager.ShowLeaderboard (best_level_id_ios);
		}
    }

    public static void ShowBestScoreInfinityMode()
    {
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.ShowLeaderBoardById (best_score_infinity_id);
		} else {
			GameCenterManager.ShowLeaderboard (best_score_infinity_id_ios);
		}
    }

    public static void ShowBestScoreHellMode()
    {
		if (RuntimePlatform.Android == Application.platform) {
			GooglePlayManager.Instance.ShowLeaderBoardById (best_score_hell_id);
		} else {
			GameCenterManager.ShowLeaderboard (best_score_hell_id_ios);
		}
    }

}
