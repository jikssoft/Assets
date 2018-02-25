using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : MonoBehaviour {

    public SpriteRenderer box_body;
    public enum BOX_TYPE { CIRCLE, RECTANGLE};

    public BOX_TYPE box_type;


    // Use this for initialization
    void Start () {
        box_table = new Queue<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    
    public GameObject crash_image;
    public void Crash(Drill drill)
    {
        Vector3 pos = new Vector3(drill.transform.position.x, drill.transform.position.y, transform.position.z - 2f);
        GameObject crash = (GameObject)Instantiate(crash_image, pos, drill.transform.rotation);
        crash.transform.parent = gameObject.transform;
        Destroy(crash, 1.4f);
    }

    public void TurnBox(Nail nail)
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = 1f;
        if (nail.direction == Nail.DIRECTION.LEFT)
        {
            TurnLeft();
        }
        else if (nail.direction == Nail.DIRECTION.RIGHT)
        {
            TurnRight();
        }
        else if (nail.direction == Nail.DIRECTION.UP)
        {
            Reset(1f);
        }
        else if( nail.direction == Nail.DIRECTION.DOWN)
        {
            LiftUp();
        }
    }

    void TurnLeft()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("TurnLeft");
    }
    
    void TurnRight()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("TurnRight");
    }

    void LiftUp()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("LiftUp");
    }

    public void Reset(float speed)
    {
        Animator animator = GetComponent<Animator>();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reset") == false)
        {
            animator.SetTrigger("Reset");
            animator.speed = speed;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public Queue<GameObject> box_table;
    GameObject current_box_anchor;
    GameObject last_box;

    public void AddBox(GameObject box)
    {
        box_table.Enqueue(box);
        current_box_anchor = box_table.Peek();
        last_box = box;
    }

    public GameObject GetLastBox()
    {
        return last_box;
    }

    public GameObject GetCurrentBox()
    {
        return current_box_anchor;
    }

    public Vector3 shake_pos;
    public void Operate( )
    {
        iTween.ShakePosition(current_box_anchor, iTween.Hash("amount", shake_pos, "time", float.MaxValue, "islocal", true));
    }

    public void Stop()
    {
        iTween.Stop(current_box_anchor);
        current_box_anchor.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void RotateBox()
    {
        //iTween.PunchRotation(box_anchor.gameObject, new Vector3(0f, 0f, 180f), 0.5f);
        iTween.RotateAdd(current_box_anchor.gameObject, new Vector3(0f, 0f, 360f), 0.4f);
    }

    public void DeleteBox()
    {
        Destroy(box_table.Dequeue());
    }

    public void ResetRotateBox()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = 1f;
        current_box_anchor.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void ScrollBox()
    {
        float time = 0.2f;
        foreach(GameObject box in box_table)
        {
            iTween.MoveBy(box, new Vector3(-distance_box, 0f), time);
        }
    }

    public GameObject infinity_mode_adjust_target_obj;
    public void AdjustInfinityModeBoxPositionForMultiResolution()
    {
        
        float adjust_distance = 0f;

        if (box_type == Box.BOX_TYPE.RECTANGLE)
        {
            adjust_distance = current_box_anchor.GetComponentInChildren<BoxCollider2D>().size.x *
                current_box_anchor.transform.localScale.x;
            adjust_distance /= 2f;
        }
        else
        {
            adjust_distance = current_box_anchor.GetComponentInChildren<CircleCollider2D>().radius *
                current_box_anchor.transform.localScale.x;
        }

        adjust_distance = infinity_mode_adjust_target_obj.transform.position.x + adjust_distance;

        current_box_anchor.transform.parent.localPosition = new Vector3(adjust_distance, 0f, 0f);

        /*
        foreach (GameObject box in box_table)
        {
            box.transform.Translate(-adjust_distance, 0f, 0f);
        }
        */
    }

    public void SetFrontBoxToLast()
    {
        GameObject box_front = box_table.Dequeue();
        box_front.transform.localPosition = new Vector3(distance_box * box_table.Count, 0f, 0f);

        AddBox(box_front);
    }
    
    float distance_box;
    public void SetBoxDistance(float distance)
    {
        distance_box = distance;
    }

    public float GetDistanceBox()
    {
        return distance_box;
    }
}
