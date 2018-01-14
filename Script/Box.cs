using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

    public SpriteRenderer box_body;
    public enum BOX_TYPE { CIRCLE, RECTANGLE};

    public BOX_TYPE box_type;


    // Use this for initialization
    void Start () {
	
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

    public GameObject box_anchor;
    public Vector3 shake_pos;
    public void Operate( )
    {
        iTween.ShakePosition(box_anchor, iTween.Hash("amount", shake_pos, "time", float.MaxValue, "islocal", true));

    }

    public void Stop()
    {
        iTween.Stop(box_anchor);
        box_anchor.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void RotateBox()
    {
        //iTween.PunchRotation(box_anchor.gameObject, new Vector3(0f, 0f, 180f), 0.5f);
        iTween.RotateAdd(box_anchor.gameObject, new Vector3(0f, 0f, 360f), 0.4f);
    }

    public void ResetRotateBox()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = 1f;
        box_anchor.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
