﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillSelector : MonoBehaviour {

    public GameObject[] drill_table;
    public GameObject[] box_table;
    public Texture[] texture_table;
    public int[] limited_drill_index;
    public int[] limited_drill_level;

    

    public int select_index;

    public GameObject box_parent;

	// Use this for initialization
	void Start () {

        
    }

    int get_limeted_drill_index = -1;

    public void GetLimetedDirll(int index)
    {
        get_limeted_drill_index = index;
    }

    public void ChangeDrill()
    {
        GameObject gameSystem = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = gameSystem.GetComponent<GameMainLogicSystem>();
        GameDataSystem data_system = gameSystem.GetComponent<GameDataSystem>();
        select_index = data_system.GetLastDrill();
        
        if(select_index == -1)
        {
            ArrayList collected_drill_table = new ArrayList();
            for(int i = 0; i < drill_table.Length; i++)
            {
                if(data_system.IsCollectDrill(i))
                {
                    collected_drill_table.Add(i);
                }
            }

            int random_index = Random.Range(0, collected_drill_table.Count);
            Debug.Log("------------ random:" + drill_table.Length + " " + random_index);
            select_index = (int)(collected_drill_table[random_index]);
        }

        if (get_limeted_drill_index > 0)
        {
            select_index = get_limeted_drill_index;
            if(data_system.GetLastDrill() != -1)
            {
                data_system.SelectDrill(get_limeted_drill_index);
            }

            get_limeted_drill_index = -1;
        }

        GameObject drill = Instantiate(drill_table[select_index]);

        system.drill = drill.GetComponent<Drill>();
        system.drill_time_gause = drill.GetComponentInChildren<DrillTimeGause>();

        GameObject box = Instantiate(box_table[select_index]);
        system.box.box_anchor = box.GetComponentInChildren<BoxAnchor>().gameObject;
        box.transform.parent = box_parent.transform;
        box.transform.rotation = new Quaternion(0f,0f,0f,0f);
        box.transform.localPosition = new Vector3(0f, 0f, 0f);
        box.GetComponentInChildren<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        system.box.box_type = system.drill.box_type;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
