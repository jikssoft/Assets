using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPointPopup : MonoBehaviour, ReturnKeyProcess {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(call_return)
        {
            StartCoroutine(CloseStarPointPopup());
            call_return = false;
        }
        
    }

    public GameObject list_item;
    public UIGrid scroll_grid;

    GameDataSystem data_system;

    ArrayList data;

    public void ShowStarPointPopup()
    {
        call_return = false;
        data = new ArrayList();

        gameObject.SetActive(true);

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        data_system = systemObj.GetComponent<GameDataSystem>();
        int last_level = data_system.GetLevel();

        int count = (last_level / 12) + 1;

        for (int i = 0; i < count; i++)
        {
            GameObject level_list_item = Instantiate(list_item, scroll_grid.transform);
            LevelListItem list_item_table = level_list_item.GetComponent<LevelListItem>();
            level_list_item.transform.localScale = new Vector3(1f, 1f, 1f);
            level_list_item.transform.localPosition = new Vector3(0f, 0f, 0f);
            //level_list_item.transform.parent = scroll_grid.transform;

            data.Add(level_list_item);

            for (int j = 0; j < list_item_table.star_point_table.Length; j++)
            {
                int level = i * 12 + j + 1;
                int point = data_system.GetStartPoint(level);

                list_item_table.star_point_table[j].SetPoint(level, point);
            }
            
        }

        scroll_grid.enabled = true;
        ActiveAnimation.Play(gameObject.GetComponent<Animation>(), "ShowStarMenu", AnimationOrTween.Direction.Forward);

        Register();
        //BannerController.HideBanner();
    }
    bool call_return = false;
    public void ReturnStarPointPopup(bool disableBanner)
    {
        call_return = true;
        if (disableBanner == true)
        {
            BannerController.ShowBanner();
        }
    }

    IEnumerator CloseStarPointPopup()
    {
        ActiveAnimation.Play(gameObject.GetComponent<Animation>(), "HideStarMenu", AnimationOrTween.Direction.Forward);

        yield return new WaitForSeconds(0.3f);

        foreach (GameObject go in data)
        {
            DestroyObject(go);
        }

        data.Clear();

        gameObject.SetActive(false);
    }

    public void Register()
    {
        ReturnKeyManager.RegisterReturnKeyProcess(this);
    }

    public void Return()
    {
        ReturnStarPointPopup(true);
        //ReturnKeyManager.RegisterReturnKeyProcess(GetParentReturnKeyProcess());
        ReturnKeyManager.RetrunProcess();
        
    }


    public GameMenuUI menu_ui;
   
}
