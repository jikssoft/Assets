using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SA.Analytics.Google;

public class GameDataSystem : MonoBehaviour {
    

	// Use this for initialization
	void Start () {

        temp_texture = new Texture2D(128, 128);

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    int level_data;
    int coin_data;
    public char[] star_point_data;
    public bool[] drill_data;
    public int bgm_on_off_data;
    public int sound_on_off_data;
    public int vibration_on_off_data;
    public int language_data;
    public int last_drill = 0;
    public System.DateTime last_reward_ad_time;
    public int reward_waiting_time;
    public bool check_fast_nail;
    public bool[] drill_purchase_data;
    public bool[] drill_used_data;
    public int infinity_score_data;
    public int hell_score_data;

    public void SetLevel(int level)
    {
		if (level >= star_point_data.Length) {
			return;
		}

        if(level > level_data)
        {
            Manager.Client.SendEventHit("set level", "set level", level.ToString());
            level_data = level;
            Save();
        }
    }

    public int GetLevel()
    {
        if(level_data == 0)
        {
            Load();
        }
        return level_data;
    }

    public void SetStartPoint(int level, int point)
    {
        if (star_point_data[level] < point)
        {
            star_point_data[level] = (char)point;
        }
        Save();
        Manager.Client.SendEventHit("star point", "set star", level.ToString() + "_" + point.ToString());
    }

    public int GetStartPoint(int level)
    {
        return (star_point_data[level]);
    }

    public void CollectDrill(int index)
    {
        Manager.Client.SendEventHit("collect drill", "collect", index.ToString());
        drill_data[index] = true;
        drill_used_data[index] = false;
        Save();
    }

	public void ResetPurchaseDrill()
	{
		for (int i = 0; i < 1000; i++)
		{
			drill_purchase_data[i] = false;
		}
	}

    public void PurchaseDrill(int index)
    {
        Manager.Client.SendEventHit("collect drill", "purchases", index.ToString());
        drill_purchase_data[index] = true;
        drill_used_data[index] = false;
    }

    public void RestoreDrill(int index)
    {
        Manager.Client.SendEventHit("restore drill", "purchases", index.ToString());
        drill_purchase_data[index] = true;
    }

    public bool GetDrillUsed(int index)
    {
        return drill_used_data[index];
    }
    
    public void SetBGM(int onoff)
    {
        bgm_on_off_data = onoff;
        Save();
        SoundManager.OnOffBGM(bgm_on_off_data == 1 ? true : false);
        
    }

    public int GetBGM()
    {
        return bgm_on_off_data;
    }

    public void SetSound(int onoff)
    {
        sound_on_off_data = onoff;
        Save();
        SoundManager.OnOffSound(sound_on_off_data == 1 ? true : false);
    }

    public int GetSound()
    {
        return sound_on_off_data;
    }

    public void SetVibration(int onoff)
    {
        vibration_on_off_data = onoff;
        Save();
        Manager.Client.SendEventHit("Set Vibration", "vibratoin", onoff.ToString());
    }

    public int GetVibration()
    {
        return vibration_on_off_data;
    }

    public void SetLanguage(int index)
    {
        language_data = index;
        Localization.language = Localization.knownLanguages[language_data];
        Save();
        Manager.Client.SendEventHit("SetLanguage", "Language", index.ToString());
    }

    public int GetLanguage()
    {
        return language_data;
    }

    public bool IsCollectDrill(int index)
    {
        return drill_data[index] || drill_purchase_data[index];
    }

    public void SetCoin(int coin, bool saveCloud)
    {
        coin_data = coin;
        Save(saveCloud);
    }

    public void SaveCloud()
    {
        Save(true);
    }

    public int GetCoin()
    {
        return coin_data;
    }
    
    public void SelectDrill(int index)
    {
        last_drill = index;
        if (index > 0)
        {
            drill_used_data[index] = true;
        }
        Save();
        Manager.Client.SendEventHit("SelectDrill", "selectdrill", index.ToString());
    }

    public int GetLastDrill()
    {
        return last_drill;
    }

    public void SetCheckFastNail()
    {
        check_fast_nail = true;
        Save();
    }

    public bool GetCheckFastNail()
    {
        return check_fast_nail;
    }

    public void SetLastRewardADTime(System.DateTime last_time)
    {
        last_reward_ad_time = last_time;
        Save(false);
    }

    public System.DateTime GetLastRewardADTime()
    {
        return last_reward_ad_time;
    }

    public int GetRewardADWatingTime()
    {
        return reward_waiting_time;
    }

    public void SetRewardADWatingTime()
    {
        reward_waiting_time++;
        if(reward_waiting_time > 10)
        {
            reward_waiting_time = 10;
        }
        Save(false);
    }

    public void ResetRewardADWatingTime()
    {
		if (reward_waiting_time > 1) 
		{
			reward_waiting_time = 1;
			Save ();
		}
    }

    public void SetInfinityScore(int score)
    {
        if (score > infinity_score_data)
        {
            infinity_score_data = score;
            Save();
        }
        
    }

    public int GetInifityScore()
    {
        return infinity_score_data;
    }

    public void SetHellScore(int score)
    {
        if (score > hell_score_data)
        {
            hell_score_data = score;
        }
        Save();
    }

    public int GetHellScore()
    {
        return hell_score_data;
    }

void SetDefaultLanguage()
    {
        language_data = 0;
        if (Application.systemLanguage == SystemLanguage.English)
        {
            language_data = 0;
        }
        else if (Application.systemLanguage == SystemLanguage.Arabic)
        {
            language_data = 1;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            language_data = 2;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            language_data = 3;
        }
        else if (Application.systemLanguage == SystemLanguage.Dutch)
        {
            language_data = 4;
        }
        else if (Application.systemLanguage == SystemLanguage.French)
        {
            language_data = 5;
        }
        else if (Application.systemLanguage == SystemLanguage.German)
        {
            language_data = 6;
        }
        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            language_data = 7;
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            language_data = 8;
        }
        else if (Application.systemLanguage == SystemLanguage.Korean)
        {
            language_data = 9;
        }
        else if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            language_data = 10;
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            language_data = 11;
        }
        else if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            language_data = 12;
        }
        else if (Application.systemLanguage == SystemLanguage.Turkish)
        {
            language_data = 13;
        }
    }

    public void ResetDrill()
    {
        for (int i = 0; i < 1000; i++)
        {
            drill_data[i] = false;
        }
        Save();
    }

    public void AllDrill()
    {
        for (int i = 0; i < 1000; i++)
        {
            drill_data[i] = true;
        }
        Save();
    }

    public void Save(bool saveCloud = true)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamedata.dat", FileMode.OpenOrCreate);

        GameData data = new GameData();
        data.level = level_data;
        data.coin = coin_data;
        data.star_point = star_point_data;
        data.drill = drill_data;
        data.bgm_on_off = bgm_on_off_data;
        data.sound_on_off = sound_on_off_data;
        data.vibration_on_off = vibration_on_off_data;
        data.language = language_data;
        data.last_drill = last_drill;
        data.check_fast_nail = check_fast_nail;
        data.last_reward_ad_time = last_reward_ad_time;
        data.reward_waiting_time = reward_waiting_time;
		data.drill_purchase = drill_purchase_data;
        data.drill_used = drill_used_data;
        data.infinity_score = infinity_score_data;
        data.hell_score = hell_score_data;

        MemoryStream memory = new MemoryStream ();
        bf.Serialize(file, data);
		bf.Serialize (memory, data);
        file.Close();

        if (saveCloud == true)
        {
            Debug.Log("Save Cloud!!!!!!!!!!!!!!!!");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                SaveGameIPhone(memory);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                SaveGameAndroid(memory);
            }
        }
    }

	void SaveGameIPhone (MemoryStream ms) {
		ISN_GameSaves.ActionGameSaved += HandleActionGameSaved;
		ISN_GameSaves.Instance.SaveGame(ms.ToArray(), "savedgame");
		Debug.Log("Save Call");
	}

    private Texture2D temp_texture;
    void SaveGameAndroid(MemoryStream ms)
    {
        GooglePlaySavedGamesManager.ActionGameSaveResult += AndroidActionGameSaveResult;
        GooglePlaySavedGamesManager.Instance.CreateNewSnapshot("com.twocm.taptapdrill", "savedata", temp_texture, ms.ToArray(), System.DateTime.UtcNow.Ticks);
        Debug.Log("Save Call");
    }

    private void AndroidActionGameSaveResult(GP_SpanshotLoadResult result)
    {
        GooglePlaySavedGamesManager.ActionGameSaveResult -= AndroidActionGameSaveResult;
        Debug.Log("ActionGameSaveResult: " + result.Message);
    }

    void HandleActionGameSaved (GK_SaveResult res) {
		if(res.IsSucceeded) {
			Debug.Log("Saved");
		} else {
			Debug.Log("Failed: " + res.Error.ToString());
		}
	}

	void FetchIPhoneGame()
	{
		Debug.Log("FetchIphoneGame");
		ISN_GameSaves.ActionSavesFetched += HandleActionSavesFetched;
		ISN_GameSaves.Instance.FetchSavedGames();
	}

    void FetchAndroidGame()
    {
        Debug.Log("FetchAndroidGame");
        GooglePlayConnection.ActionPlayerConnected += OnPlayerConnected;
        GooglePlayConnection.ActionConnectionResultReceived += OnConnectionResult;

        GooglePlayConnection.Instance.Connect();
        

    }

    private void OnPlayerConnected()
    {
        
        Debug.Log("google play service connect");

        GooglePlayConnection.ActionPlayerConnected -= OnPlayerConnected;

        GooglePlaySavedGamesManager.ActionAvailableGameSavesLoaded += Android_ActionAvailableGameSavesLoaded;
        GooglePlaySavedGamesManager.Instance.LoadAvailableSavedGames();
    }

    private void OnConnectionResult(GooglePlayConnectionResult result)
    {
        GooglePlayConnection.ActionConnectionResultReceived -= OnConnectionResult;
        Debug.Log(result.code.ToString());
    }

    private void Android_ActionAvailableGameSavesLoaded(GooglePlayResult res)
    {
        Debug.Log("Android_ActionAvailableGameSavesLoaded " + res.ToString());
        GooglePlaySavedGamesManager.ActionAvailableGameSavesLoaded -= Android_ActionAvailableGameSavesLoaded;
        if (res.IsSucceeded)
        {
            foreach (GP_SnapshotMeta meta in GooglePlaySavedGamesManager.Instance.AvailableGameSaves)
            {
                Debug.Log("Meta.Title: " + meta.Title);
                Debug.Log("Meta.Description: " + meta.Description);
                Debug.Log("Meta.CoverImageUrl): " + meta.CoverImageUrl);
                Debug.Log("Meta.LastModifiedTimestamp: " + meta.LastModifiedTimestamp);

                if (meta.Title.Equals("com.twocm.taptapdrill") == true)
                {
                    GooglePlaySavedGamesManager.ActionGameSaveLoaded += Android_ActoinGameSaveLoaded;
                    GooglePlaySavedGamesManager.Instance.LoadSpanshotByName(meta.Title);
                    
                    // GooglePlaySavedGamesManager.Instance.AvailableGameSaves[0].CoverImageUrl;
                    Debug.Log(meta.Title + " saves loaded");
                }
            }
        }
        else
        {
            Debug.Log("Fail Available Game Saves Load failed");
        }
    }

    private void Android_ActoinGameSaveLoaded(GP_SpanshotLoadResult spaneshot)
    {
        Debug.Log("Load SaveData spanshot : " + spaneshot.Snapshot.bytes.Length);
        
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(spaneshot.Snapshot.bytes);
        GameData data = (GameData)bf.Deserialize(ms);
        FetchFromCloud(data);
    }


    void HandleActionSavesFetched (GK_FetchResult res) {
		if(res.IsSucceeded) {
			Debug.Log(res.SavedGames.Count + " saves loaded");
			LoadIPhone (res.SavedGames [0]);
		} else {
			Debug.Log("Failed: " + res.Error.ToString());
		}
	}

	void LoadIPhone(GK_SavedGame saved_game) {
		Debug.Log("LoadIPhone");
		saved_game.ActionDataLoaded += HandleActionDataLoaded;
		saved_game.LoadData();
	}

	void HandleActionDataLoaded (GK_SaveDataLoaded res) {
		if(res.IsSucceeded) {
			Debug.Log("Data loaded. data Length: " + res.SavedGame.Data.Length);
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream (res.SavedGame.Data);
			GameData data = (GameData)bf.Deserialize(ms);
            FetchFromCloud(data);

		} else {
			Debug.Log("Failed: " + res.Error.ToString());
		}
	}

    void FetchFromCloud(GameData data)
    {
        if (data.level > level_data)
        {
            level_data = data.level;
        }

        if (data.coin > coin_data)
        {
            coin_data = data.coin;
        }

        star_point_data = data.star_point;
        for (int i = 0; i < 1000; i++)
        {
            if (star_point_data[i] > 3)
            {
                star_point_data[i] = (char)3;
            }

            if (star_point_data[i] < 1)
            {
                star_point_data[i] = (char)1;
            }
        }

        for (int i = 0; i < 1000; i++)
        {
            drill_data[i] = drill_data[i] | data.drill[i];
        }
        bgm_on_off_data = data.bgm_on_off;
        sound_on_off_data = data.sound_on_off;
        vibration_on_off_data = data.vibration_on_off;
        language_data = data.language;
        last_drill = data.last_drill;
        check_fast_nail = data.check_fast_nail;
        last_reward_ad_time = data.last_reward_ad_time;
        reward_waiting_time = data.reward_waiting_time;

        DrillSelector drill_selector = GameObject.FindGameObjectWithTag("DrillSelector").GetComponent<DrillSelector>();
        for (int i = 0; i < drill_selector.limited_drill_level.Length; i++)
        {
            if (drill_selector.limited_drill_level[i] < level_data)
            {
                drill_data[drill_selector.limited_drill_index[i]] = true;
            }
        }
    }

	void LoadData(GameData data)
	{
		for (int i = 0; i < 1000; i++)
		{
			if(star_point_data[i] > 3)
				star_point_data[i] = (char)3;
		}

		level_data = data.level;
        coin_data = data.coin;
		star_point_data = data.star_point;
		drill_data = data.drill;
		bgm_on_off_data = data.bgm_on_off;
		sound_on_off_data = data.sound_on_off;
		vibration_on_off_data = data.vibration_on_off;
		language_data = data.language;
		last_drill = data.last_drill;
        check_fast_nail = data.check_fast_nail;
        last_reward_ad_time = data.last_reward_ad_time;
		reward_waiting_time = data.reward_waiting_time;
		drill_purchase_data = data.drill_purchase;
        if(drill_purchase_data == null)
        {
            drill_purchase_data = new bool[1000];
        }
        
        if(data.drill_used != null)
        {
            drill_used_data = data.drill_used;
        }

        infinity_score_data = data.infinity_score;
        hell_score_data = data.hell_score;

        if (level_data == 0)
		{
			level_data = 1;
			coin_data = 0;
            infinity_score_data = 0;
            hell_score_data = 0;
        }
	}

    public TutorualController tutorial_controller;
    public void Load()
    {
        level_data = 0;
        coin_data = 0;
        star_point_data = new char[1000];
        drill_data = new bool[1000];
		drill_purchase_data = new bool[1000];
        drill_used_data = new bool[1000];
        for (int i = 0; i < 1000; i++)
        {
            star_point_data[i] = (char)0;
            drill_data[i] = false;
			drill_purchase_data [i] = false;
            drill_used_data[i] = true;
        }
        drill_data[0] = true;
        bgm_on_off_data = 1;
        sound_on_off_data = 1;
        vibration_on_off_data = 1;
        SetDefaultLanguage();
        last_drill = 0;
        check_fast_nail = false;
        last_reward_ad_time = System.DateTime.MinValue;
        reward_waiting_time = 1; // 2 min
        infinity_score_data = 0;
        hell_score_data = 0;

        if (Application.platform == RuntimePlatform.IPhonePlayer) {
			FetchIPhoneGame ();
		}
        else if(Application.platform == RuntimePlatform.Android) { 
            FetchAndroidGame();
        }

        if (File.Exists(Application.persistentDataPath + "/gamedata.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamedata.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

			LoadData (data);
            SoundManager.OnOffBGM(bgm_on_off_data == 1 ? true : false);
        }
        else
        {
            level_data = 1;
            coin_data = 0;
            tutorial_controller.get_coin = true;
            tutorial_controller.ShowTutorial();
            SoundManager.OnOffBGM(false);
            infinity_score_data = 0;
            hell_score_data = 0;
        }

        Localization.language = Localization.knownLanguages[language_data];

        
        SoundManager.OnOffSound(sound_on_off_data == 1 ? true : false);
    }
    
}

[System.Serializable]
class GameData
{
    public int level;
    public int coin;
    public char[] star_point = {};
    public bool[] drill = {};
    public int bgm_on_off;
    public int sound_on_off;
    public int vibration_on_off;
    public int language;
    public int last_drill;
    public System.DateTime last_reward_ad_time;
    public int reward_waiting_time;
    public bool check_fast_nail;
	public bool[] drill_purchase = {};
    public bool[] drill_used = {};
    public int infinity_score;
    public int hell_score;
}

