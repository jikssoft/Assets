using UnityEngine;
using System.Collections;

public class NailBuilder : MonoBehaviour {

    public Box box;
    public GameObject nail;
    
    
    // Use this for initialization
    void Start() {


    }

    // Update is called once per frame
    void Update() {

    }
    
    float init_x_pos = 2f;
    int count = 3;

    float min_y = 4f;
    float fast_min_y = 4.4f;
    float max_y = 4.8f;

    public void BuildNailCircle(ArrayList nail_table)
    {
        Vector3 pos = new Vector3();
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        float angle = -30f;
        float unit_angle = 30f;

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, angle);
            angle += unit_angle;

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.UP;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, angle);
            angle += unit_angle;

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.LEFT;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, angle);
            angle += unit_angle;

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.DOWN;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, angle);
            angle += unit_angle;

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.RIGHT;
            nail_table.Add(nail_new);
        }
    }

    float unit_y = 6.62f;
    float unit_x = 6.62f;

    public void BuildNailRectangle(ArrayList nail_table)
    {
        Vector3 pos = new Vector3();
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");
        
        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();

        BoxCollider2D box_collider = system.box.GetCurrentBox().GetComponentInChildren<BoxCollider2D>();

        float adjust_distance_vertical = 0f;
        float adjust_y_vertical = 0f;

        if (box_collider.size.y < unit_y)
        {
            //unit_y : min_y = box_collider.size.y : ??
            float new_min_y = (min_y * box_collider.size.y) / unit_y;
            adjust_y_vertical = new_min_y - min_y;
            adjust_y_vertical /= 1.5f;

            //unit_y : init_x_pos = box_collider.size.y : ??
            float new_distance = (init_x_pos * box_collider.size.y) / unit_y;
            adjust_distance_vertical = new_distance - init_x_pos;
        }

        float adjust_distance_horizontal = 0f;
        float adjust_y_horizontal = 0f;

        if (box_collider.size.x < unit_x)
        {
            float new_min_y = (min_y * box_collider.size.x) / unit_x;
            adjust_y_horizontal = new_min_y - min_y;
            adjust_y_horizontal /= 1.5f;

            float new_distance = (init_x_pos * box_collider.size.x) / unit_x;
            adjust_distance_horizontal = new_distance - init_x_pos;
        }

        float distance_vertical = (-4f + (-2f * adjust_distance_vertical)) / (float)(count - 1);
        float distance_horizontal = (-4f + (-2f * adjust_distance_horizontal)) / (float)(count - 1);

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y , max_y) + adjust_y_vertical;
            pos.Set(init_x_pos + adjust_distance_horizontal + (distance_horizontal * i), y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 0f);

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.UP;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y) + adjust_y_horizontal;
            pos.Set(init_x_pos + adjust_distance_vertical + (distance_vertical * i), y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 90f);

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.LEFT;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y) + adjust_y_vertical;
            pos.Set(init_x_pos + adjust_distance_horizontal + (distance_horizontal * i), y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 180f);

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.DOWN;
            nail_table.Add(nail_new);
        }

        for (int i = 0; i < count; i++)
        {
            float y_pos = Random.Range(min_y, max_y) + adjust_y_horizontal;
            pos.Set(init_x_pos + adjust_distance_vertical + (distance_vertical * i), y_pos, 10f);
            GameObject nail_new = (GameObject)Instantiate(nail, pos + box.transform.position, rotation);

            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = box.transform.position;
            nail_new.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 270f);

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.RIGHT;
            nail_table.Add(nail_new);
        }
    }

    Box.BOX_TYPE setting_box_type;
    public void BuildNail(ArrayList nail_table, int remainNail, int level, Box.BOX_TYPE box_type)
    {
        Debug.Log("BuildNail level: " + remainNail.ToString());
        setting_box_type = box_type;


        nail_table.Clear();

        if(box_type == Box.BOX_TYPE.RECTANGLE)
        {
            BuildNailRectangle(nail_table);
        }
        else if (box_type == Box.BOX_TYPE.CIRCLE)
        {
            BuildNailCircle(nail_table);
        }

        ArrayList index_table = new ArrayList();
        for (int i = 1; i < nail_table.Count; i++)
        {
            index_table.Add(i);
        }

		foreach (GameObject nail in nail_table) {
			nail.GetComponent<Nail>().SaveStartPos ();
		}
        
        /* Random Index
        if (remainNail % 4 == 0)
        {
            for (int t = 0; t < nail_table.Count; t++)
            {
                GameObject tmp = (GameObject)(nail_table[t]);
                int r = Random.Range(t, nail_table.Count);
                nail_table[t] = nail_table[r];
                nail_table[r] = tmp;
            }
        }
        */

        if (level >= 13 && level < 20)
        {
            BuildRandomSpeedSlowDisturb(nail_table, index_table);
            BuildRandomRotateZoomPerfectDisturb(nail_table, index_table, 1);
        }
        else if (level >= 20)
        {
            BuildRandomSpeedSlowDisturb(nail_table, index_table);
            BuildRandomRotateZoomPerfectDisturb(nail_table, index_table, 2);
        }
        //BuildRandomDisturb(nail_table);

        BuildCoinNail(nail_table);

        if(remainNail / (count * 4) <= 0)
        {
            ArrangeNailByLevel(nail_table, remainNail);
        }
    }

    public void BuildNailInfinityMode(ArrayList nail_table, int remainNail, Box box)
    {
        setting_box_type = box.box_type;

        nail_table.Clear();

        foreach (GameObject box_obj in box.box_table)
        {
            if (setting_box_type == Box.BOX_TYPE.RECTANGLE)
            {
                BuildNailInfinityModeRectangle(nail_table, box_obj);
            }
            else if (setting_box_type == Box.BOX_TYPE.CIRCLE)
            {
                BuildNailInfinityModeCircle(nail_table, box_obj);
            }
        }

        ArrayList index_table = new ArrayList();
        for (int i = 1; i < nail_table.Count; i++)
        {
            index_table.Add(i);
        }

        foreach (GameObject nail in nail_table)
        {
            nail.GetComponent<Nail>().SaveStartPos();
        }

        // slow 못 하나만
        int slow1 = Random.Range(1, nail_table.Count);
        ((GameObject)nail_table[slow1]).GetComponent<Nail>().SetDisturbType(Nail.DISTURB_TYPE.SLOW_SPEED);

        BuildCoinNail(nail_table);
    }

    public void BuildNailHellMode(ArrayList nail_table, int remainNail, Box box)
    {
        setting_box_type = box.box_type;

        nail_table.Clear();

        foreach (GameObject box_obj in box.box_table)
        {
            if (setting_box_type == Box.BOX_TYPE.RECTANGLE)
            {
                BuildNailInfinityModeRectangle(nail_table, box_obj);
            }
            else if (setting_box_type == Box.BOX_TYPE.CIRCLE)
            {
                BuildNailInfinityModeCircle(nail_table, box_obj);
            }
        }

        ArrayList index_table = new ArrayList();
        for (int i = 1; i < nail_table.Count; i++)
        {
            index_table.Add(i);
        }

        foreach (GameObject nail in nail_table)
        {
            nail.GetComponent<Nail>().SaveStartPos();
            nail.GetComponent<Nail>().SetDisturbType(Nail.DISTURB_TYPE.PERFECT);
        }

        int speed1 = Random.Range(1, nail_table.Count);
        int speed2 = Random.Range(1, nail_table.Count);
        int slow1 = Random.Range(1, nail_table.Count);

        ((GameObject)nail_table[speed1]).GetComponent<Nail>().SetDisturbType(Nail.DISTURB_TYPE.PERFECT_SPEED);
        ((GameObject)nail_table[speed2]).GetComponent<Nail>().SetDisturbType(Nail.DISTURB_TYPE.PERFECT_SPEED);
        ((GameObject)nail_table[slow1]).GetComponent<Nail>().SetDisturbType(Nail.DISTURB_TYPE.PERFECT_SLOW);


        BuildCoinNail(nail_table);
    }

    float infinity_mode_init_x_pos = -2f;
    public void BuildNailInfinityModeRectangle(ArrayList nail_table, GameObject anchor)
    {
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();

        BoxCollider2D box_collider = system.box.GetCurrentBox().GetComponentInChildren<BoxCollider2D>();

        float adjust_distance_vertical = 0f;
        float adjust_y_vertical = 0f;

        Vector3 scale = box_collider.transform.parent.localScale;
        if (box_collider.size.y < unit_y)
        {
            //unit_y : min_y = box_collider.size.y : ??
            float new_min_y = (min_y * box_collider.size.y) / unit_y;
            adjust_y_vertical = new_min_y - min_y;
            adjust_y_vertical /= 1.5f;

            //unit_y : init_x_pos = box_collider.size.y : ??
            float new_distance = (infinity_mode_init_x_pos * box_collider.size.y) / unit_y;
            adjust_distance_vertical = new_distance - infinity_mode_init_x_pos; 
        }

        float adjust_distance_horizontal = 0f;
        float adjust_y_horizontal = 0f;

        if (box_collider.size.x < unit_x)
        {
            float new_min_y = (min_y * box_collider.size.x) / unit_x;
            adjust_y_horizontal = new_min_y - min_y;
            adjust_y_horizontal /= 1.5f;

            float new_distance = (infinity_mode_init_x_pos * box_collider.size.x) / unit_x;
            adjust_distance_horizontal = new_distance - infinity_mode_init_x_pos;
        }

        float distance_horizontal = (4f + (-2f * adjust_distance_horizontal)) / (float)(count - 1);


        for (int i = 0; i < count; i++)
        {
            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = anchor.transform.position;
            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 0f);
            
            //GameObject nail_new = (GameObject)Instantiate(nail, pos, rotation, nail_anchor.transform);
            GameObject nail_new = (GameObject)Instantiate(nail, nail_anchor.transform, true);

            Vector3 local_pos = new Vector3();
            float y_pos = Random.Range(min_y * 0.8f, max_y * 0.8f) + (adjust_y_vertical);
            local_pos.Set(infinity_mode_init_x_pos + adjust_distance_horizontal + (distance_horizontal * i), y_pos, 10f);
            local_pos.x *= scale.x;

            nail_new.transform.localPosition = local_pos;

            //nail_new.transform.parent = nail_anchor.transform;


            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.UP;
            nail_table.Add(nail_new);
        }
    }

    public void ReBuildNailInfinityModeRectangle(ArrayList nailTable, int index, GameObject anchor)
    {
        Quaternion rotation = Quaternion.identity;

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();

        BoxCollider2D box_collider = system.box.GetCurrentBox().GetComponentInChildren<BoxCollider2D>();

        float adjust_distance_vertical = 0f;
        float adjust_y_vertical = 0f;

        Vector3 scale = box_collider.transform.parent.localScale;
        if (box_collider.size.y < unit_y)
        {
            //unit_y : min_y = box_collider.size.y : ??
            float new_min_y = (min_y * box_collider.size.y) / unit_y;
            adjust_y_vertical = new_min_y - min_y;
            adjust_y_vertical /= 1.5f;

            //unit_y : init_x_pos = box_collider.size.y : ??
            float new_distance = (infinity_mode_init_x_pos * box_collider.size.y) / unit_y;
            adjust_distance_vertical = new_distance - infinity_mode_init_x_pos;
        }

        float adjust_distance_horizontal = 0f;
        float adjust_y_horizontal = 0f;

        if (box_collider.size.x < unit_x)
        {
            float new_min_y = (min_y * box_collider.size.x) / unit_x;
            adjust_y_horizontal = new_min_y - min_y;
            adjust_y_horizontal /= 1.5f;

            float new_distance = (infinity_mode_init_x_pos * box_collider.size.x) / unit_x;
            adjust_distance_horizontal = new_distance - infinity_mode_init_x_pos;
        }

        float distance_horizontal = (4f + (-2f * adjust_distance_horizontal)) / (float)(count - 1);

        GameObject remove_nail = ((GameObject)nailTable[index]);
        GameObject nail_new = (GameObject)Instantiate(nail, remove_nail.transform.parent);
        Destroy(remove_nail);
        nailTable[index] = nail_new;
        GameObject nail_anchor = nail_new.transform.parent.gameObject;
        nail_anchor.transform.position = anchor.transform.position;
        nail_anchor.transform.Rotate(0f, 0f, 0f);
        
        Vector3 local_pos = new Vector3();
        float y_pos = Random.Range(min_y * 0.8f, max_y * 0.8f) + (adjust_y_vertical);
        local_pos.Set(infinity_mode_init_x_pos + 
            adjust_distance_horizontal + 
            (distance_horizontal * (index % 3)), 
            y_pos, 
            10f);
        local_pos.x *= scale.x;

        nail_new.transform.localPosition = local_pos;

        nail_new.GetComponent<Nail>().ResetCollision();

    }

    public void BuildNailInfinityModeCircle(ArrayList nail_table, GameObject anchor)
    {
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        float angle = 30f;
        float unit_angle = -30f;

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();
        
        for (int i = 0; i < count; i++)
        {
            GameObject nail_anchor = new GameObject();
            nail_anchor.transform.position = anchor.transform.position;
            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 0f);

            GameObject nail_new = (GameObject)Instantiate(nail, nail_anchor.transform, true);

            Vector3 local_pos = new Vector3();
            float y_pos = Random.Range(min_y * 0.8f, max_y * 0.8f);
            local_pos.Set(0f, y_pos, 10f);
            nail_new.transform.localPosition = local_pos;

            nail_anchor.transform.Rotate(0f, 0f, angle);
            angle += unit_angle;

            nail_new.GetComponent<Nail>().direction = Nail.DIRECTION.UP;
            nail_table.Add(nail_new);
        }
    }

    public void ReBuildNailInfinityModeCircle(ArrayList nailTable, int index, GameObject anchor)
    {
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        float angle = 30f;
        float unit_angle = -30f;

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();
        CircleCollider2D box_collider = system.box.GetCurrentBox().GetComponentInChildren<CircleCollider2D>();

        GameObject remove_nail = ((GameObject)nailTable[index]);
        GameObject nail_new = (GameObject)Instantiate(nail, remove_nail.transform.parent);
        Destroy(remove_nail);
        nailTable[index] = nail_new;
        GameObject nail_anchor = nail_new.transform.parent.gameObject;

        nail_anchor.transform.position = anchor.transform.position;
        nail_anchor.transform.localRotation = Quaternion.identity;


        Vector3 local_pos = new Vector3();
        float y_pos = Random.Range(min_y * 0.8f, max_y * 0.8f);
        local_pos.Set(0f, y_pos, 10f);

        nail_new.transform.localPosition = local_pos;
        
        nail_anchor.transform.Rotate(0f, 0f, angle + ((index % 3) * unit_angle));
    }

    public void ScrollNails(ArrayList nail_table, float distance, float time)
    {
        foreach (GameObject nail in nail_table)
        {
            //GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");
            iTween.MoveBy(nail, iTween.Hash("amount", new Vector3(distance, 0f), "time", time, "space", Space.World));
        }
    }

    public void SetRePositionNail(ArrayList nail_table, int startIndex, GameObject targetBox, bool hellmode)
    {
        for(int i = 0; i < 3; i++)
        {
            if (setting_box_type == Box.BOX_TYPE.RECTANGLE)
            {
                ReBuildNailInfinityModeRectangle(nail_table, startIndex + i, targetBox);
            }
            else if (setting_box_type == Box.BOX_TYPE.CIRCLE)
            {
                ReBuildNailInfinityModeCircle(nail_table, startIndex + i, targetBox);
            }

            float random = Random.Range(0f, 1f);

            Nail nail = ((GameObject)nail_table[startIndex + i]).GetComponent<Nail>();

            if(hellmode == true)
            {
                nail.SetDisturbType(Nail.DISTURB_TYPE.PERFECT);
            }
            
            if (random < 0.15f)
            {
                if (hellmode == true)
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.PERFECT_SLOW);
                }
                else
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.SLOW_SPEED);
                }
            }
            else if (random < 0.3f)
            {
                if (hellmode == true)
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.PERFECT_SPEED);
                }
                else
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);
                }
            }

            if (random < 0.1f)
            {
                nail.SetCoin(true);
            }
        }
    }
    
    void ArrangeNailByLevel(ArrayList nail_table, int remainNail)
    {
        int arrange_count = (count * 4) - (remainNail % (count * 4));

        while(arrange_count > 0)
        {
            int index = nail_table.Count - 1;
            GameObject nail = (GameObject)(nail_table[index]);
            DestroyObject(nail.transform.parent.gameObject);
            nail_table.RemoveAt(nail_table.Count - 1);
            arrange_count--;
        }
    }

    void BuildCoinNail(ArrayList nail_table)
    {
        int index = Random.Range(0, nail_table.Count);
        Nail coinNail = ((GameObject)(nail_table[index])).GetComponent<Nail>();
        coinNail.SetCoin(true);
    }

    void BuildRandomSpeedSlowDisturb(ArrayList nail_table, ArrayList index_table)
    {
        int rand = Random.Range(0, index_table.Count);
        int index1 = (int)index_table[rand];
        index_table.RemoveAt(rand);

        rand = Random.Range(0, index_table.Count);
        int index2 = (int)index_table[rand];
        index_table.RemoveAt(rand);

        //int index3 = Random.Range(0, index_table.Count);
        //index_table.RemoveAt(index3);

        rand = Random.Range(0, index_table.Count);
        int index4 = (int)index_table[rand];
        index_table.RemoveAt(rand);

        
        //1~3 fast nail
        Nail fastNail = ((GameObject)(nail_table[index1])).GetComponent<Nail>();
        fastNail.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);
        ArrangeFastNail(fastNail, index1);

        fastNail = ((GameObject)(nail_table[index2])).GetComponent<Nail>();
        fastNail.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);
        ArrangeFastNail(fastNail, index2);

        //fastNail = ((GameObject)(nail_table[index3])).GetComponent<Nail>();
        //fastNail.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);

        //4 slow nail
        Nail slowNail = ((GameObject)(nail_table[index4])).GetComponent<Nail>();
        slowNail.SetDisturbType(Nail.DISTURB_TYPE.SLOW_SPEED);
    }

    public void ArrangeFastNail(Nail nail, int index)
    {
        if (setting_box_type == Box.BOX_TYPE.RECTANGLE)
        {
            ReBuildFastNailRectangle(nail, index);
        }
        else if (setting_box_type == Box.BOX_TYPE.CIRCLE)
        {
            ReBuildFastNailCircle(nail, index);
        }
    }

    public void ReBuildFastNailCircle(Nail nail, int index)
    {
        Vector3 pos = new Vector3();
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        if (index >= 0 && index < 3)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;
            nail.transform.rotation = Quaternion.identity;

            float y_pos = Random.Range(fast_min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, -30 + (30f * index));
        }

        if (index >= 3 && index < 6)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;
            nail.transform.rotation = Quaternion.identity;

            float y_pos = Random.Range(fast_min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, -30 + (30f * index));

        }

        if (index >= 6 && index < 9)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;
            nail.transform.rotation = Quaternion.identity;

            float y_pos = Random.Range(fast_min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, -30 + (30f * index));

        }

        if (index >= 9 && index < 12)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;
            nail.transform.rotation = Quaternion.identity;

            float y_pos = Random.Range(fast_min_y, max_y);
            pos.Set(0f, y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor.transform;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, -30 + (30f * index));
        }
    }

    public void ReBuildFastNailRectangle(Nail nail, int index)
    {
        Vector3 pos = new Vector3();
        Quaternion rotation = Quaternion.identity;

        GameObject nail_table_obj = GameObject.FindGameObjectWithTag("NailTable");

        GameObject systemObj = GameObject.FindGameObjectWithTag("System");
        GameMainLogicSystem system = systemObj.GetComponent<GameMainLogicSystem>();

        BoxCollider2D box_collider = system.box.GetCurrentBox().GetComponentInChildren<BoxCollider2D>();

        float adjust_distance_vertical = 0f;
        float adjust_y_vertical = 0f;

        if (box_collider.size.y < unit_y)
        {
            //unit_y : min_y = box_collider.size.y : ??
            float new_min_y = (min_y * box_collider.size.y) / unit_y;
            adjust_y_vertical = new_min_y - min_y;
            adjust_y_vertical /= 1.5f;

            //unit_y : init_x_pos = box_collider.size.y : ??
            float new_distance = (init_x_pos * box_collider.size.y) / unit_y;
            adjust_distance_vertical = new_distance - init_x_pos;
        }

        float adjust_distance_horizontal = 0f;
        float adjust_y_horizontal = 0f;

        if (box_collider.size.x < unit_x)
        {
            float new_min_y = (min_y * box_collider.size.x) / unit_x;
            adjust_y_horizontal = new_min_y - min_y;
            adjust_y_horizontal /= 1.5f;

            float new_distance = (init_x_pos * box_collider.size.x) / unit_x;
            adjust_distance_horizontal = new_distance - init_x_pos;
        }

        float distance_vertical = (-4f + (-2f * adjust_distance_vertical)) / (float)(count - 1);
        float distance_horizontal = (-4f + (-2f * adjust_distance_horizontal)) / (float)(count - 1);

        if(index >= 0 && index < 3)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;

            float y_pos = Random.Range(fast_min_y, max_y) + adjust_y_vertical;
            pos.Set(init_x_pos + adjust_distance_horizontal + (distance_horizontal * (index%3)), y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 0f);

        }

        if (index >= 3 && index < 6)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;

            float y_pos = Random.Range(fast_min_y, max_y) + adjust_y_horizontal;
            pos.Set(init_x_pos + adjust_distance_vertical + (distance_vertical * (index % 3)), y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 90f);

        }

        if (index >= 6 && index < 9)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;

            float y_pos = Random.Range(fast_min_y, max_y) + adjust_y_vertical;
            pos.Set(init_x_pos + adjust_distance_horizontal + (distance_horizontal * (index % 3)), y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 180f);

        }

        if (index >= 9 && index < 12)
        {
            Transform nail_anchor = nail.transform.parent;
            nail_anchor.transform.rotation = Quaternion.identity;
            nail.transform.parent = null;

            float y_pos = Random.Range(fast_min_y, max_y) + adjust_y_horizontal;
            pos.Set(init_x_pos + adjust_distance_vertical + (distance_vertical * (index % 3)), y_pos, 10f);
            nail.transform.position = pos + box.transform.position;

            nail_anchor.transform.position = box.transform.position;
            nail.transform.parent = nail_anchor;

            nail_anchor.transform.parent = nail_table_obj.transform;
            nail_anchor.transform.Rotate(0f, 0f, 270f);

        }
    }

    void BuildRandomRotateZoomPerfectDisturb(ArrayList nail_table, ArrayList index_table, int count)
    {
        for(int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, index_table.Count);
            int index = (int)index_table[rand];
            index_table.RemoveAt(rand);

            rand = Random.Range(0, 4);
            Nail nail = ((GameObject)(nail_table[index])).GetComponent<Nail>();

            //camera rotate
            if (rand == 0)
            {
                nail.SetDisturbType(Nail.DISTURB_TYPE.ROTATE);
            }
            //camera zoom out
            else if (rand == 1)
            {
                if(Random.Range(0f, 1f) > 0.5f)
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_OUT);
                }
                else
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.PRE_ZOOM_OUT);
                }                
            }
            //zoom in
            else if (rand == 2)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_IN);
                }
                else
                {
                    nail.SetDisturbType(Nail.DISTURB_TYPE.PRE_ZOOM_IN);
                }                
            }
            //perfect only
            else if (rand == 3)
            {
                nail.SetDisturbType(Nail.DISTURB_TYPE.PERFECT);
            }
            
        }
    }

    void BuildRandomDisturb(ArrayList nail_table)
    {
        int rand = Random.Range(0, 6);

        int rand_index1 = Random.Range(0, 15);
        int rand_index2 = Random.Range(0, 15);

        Nail nail1 = ((GameObject)(nail_table[rand_index1])).GetComponent<Nail>();
        Nail nail2 = ((GameObject)(nail_table[rand_index2])).GetComponent<Nail>();
        
        Debug.Log(rand + " " + rand_index1 + " " + rand_index2);

        //camera rotate
        if (rand == 0)
        {
            nail1.SetDisturbType(Nail.DISTURB_TYPE.ROTATE);
            nail2.SetDisturbType(Nail.DISTURB_TYPE.ROTATE);
        }
        //2x speed
        else if (rand == 1)
        {
            nail1.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);
            nail2.SetDisturbType(Nail.DISTURB_TYPE.TWICE_SPEED);
        }
        //camera zoom out
        else if (rand == 2)
        {
            nail1.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_OUT);
            nail2.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_OUT);
        }
        //random index
        else if(rand == 3)
        {
            for (int t = 0; t < nail_table.Count; t++)
            {
                GameObject tmp = (GameObject)(nail_table[t]);
                int r = Random.Range(t, nail_table.Count);
                nail_table[t] = nail_table[r];
                nail_table[r] = tmp;
            }
        }
        //perfect only
        else if(rand == 4)
        {
            nail1.SetDisturbType(Nail.DISTURB_TYPE.PERFECT);
            nail2.SetDisturbType(Nail.DISTURB_TYPE.PERFECT);
        }
        //zoom in
        else if(rand == 5)
        {
            nail1.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_IN);
            nail2.SetDisturbType(Nail.DISTURB_TYPE.ZOOM_IN);
        }
    }
}
