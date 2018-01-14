using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {

    public Nail.NAIL_STATE state;

    Nail nail;

    // Use this for initialization
    void Start () {
        nail = gameObject.transform.parent.GetComponent<Nail>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    string box_name = "Box";
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(box_name))
        {
            nail.CollisionEnter(state);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(box_name))
        {
            nail.CollisionExit(state);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("impact ==== " + col.gameObject.name);
        if (col.gameObject.layer == LayerMask.NameToLayer(box_name))
        {
            nail.CollisionEnter(state);
        }
    }
}
