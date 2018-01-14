using UnityEngine;
using System.Collections;

public class Drill : MonoBehaviour {

    public SpriteAnimation drill_sprite_ani;
    public SpriteAnimation drill_body_sprite_ani;
    
    public enum DRILL_TYPE { SPRITE_ANI, POSITION, ROTATION }
    public enum SOUND_TYPE { SCREW, KNOCK, ROLLING}

    public SOUND_TYPE drill_sound_type = SOUND_TYPE.SCREW;
    public DRILL_TYPE drill_type = DRILL_TYPE.SPRITE_ANI;
    public Vector3 operate_position;
    public Vector3 operate_rotation;

    public Box.BOX_TYPE box_type = Box.BOX_TYPE.RECTANGLE;

    public AudioClip addition_idle = null;
    public AudioClip addition_operate = null;
    public AudioClip addition_environment = null;

    GameObject trans_pos_obj;
    public Vector3 shake_pos;

    // Use this for initialization
    void Start () {

        trans_pos_obj = gameObject.transform.GetChild(0).gameObject;
        if (drill_sprite_ani != null)
        {
            drill_sprite_ani.SetScale(0f);
        }

        if(drill_body_sprite_ani != null)
        {
            drill_body_sprite_ani.SetScale(0f);
        }
    }

    
    GameObject target_nail = null;

	// Update is called once per frame
	void Update () {
        /*
        if (operate_speed < 0f)
        {
            gameObject.transform.Translate( 0f, operate_speed, 0f, Space.Self);
        }
        */
        
        if(target_nail != null)
        {
            gameObject.transform.position = new Vector3(target_nail.transform.position.x,
                target_nail.transform.position.y,
                -10f);
            gameObject.transform.rotation = new Quaternion(target_nail.transform.rotation.x,
                target_nail.transform.rotation.y,
                target_nail.transform.rotation.z,
                target_nail.transform.rotation.w);
        }
    }
    public ParticleSystem drill_paricle;
    public ParticleSystem current_particle;
    string[] layer_mask_strings = { "Box" };

    public void ChaseTargetNail(GameObject nail)
    {
        target_nail = nail;
    }

    public bool only_show_operate_drill = false;
    public bool drill_body_sprite = false;

    public void Operate()
    {
        OperateDrill();
        //operate_speed = speed;

        if(only_show_operate_drill == true)
        {
            drill_sprite_ani.gameObject.SetActive(true);
        }

        

        int layermask = LayerMask.GetMask(layer_mask_strings);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 4f, layermask);
        if (hit.collider != null)
        {
            Debug.Log("Point of contact: " + hit.ToString() +  "collider :" + transform.eulerAngles.z.ToString());
            Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.collider.transform.position.z - 0.1f);
            ParticleSystem particle = (ParticleSystem)Instantiate(drill_paricle, position, 
                Quaternion.Euler(0f, 0f, transform.eulerAngles.z));
            particle.Play();
            current_particle = particle;
        }

    }
    
    public void Stop()
    {
        iTween.Stop(trans_pos_obj);
        trans_pos_obj.transform.localPosition = new Vector3(0f, 0f, 0f);
        StopDrill();

        if (only_show_operate_drill == true)
        {
            drill_sprite_ani.gameObject.SetActive(false);
        }

        //operate_speed = 0f;
        target_nail = null;
        if (current_particle != null)
        {
            current_particle.Stop();
            DestroyObject(current_particle.gameObject, 1f);
        }
    }
    
    public float operate_time = 0.5f;
    public GameObject rotation_anchor_obj;
    void OperateDrill()
    {
        if(drill_type == DRILL_TYPE.SPRITE_ANI)
        {
            iTween.ShakePosition(trans_pos_obj, iTween.Hash("amount", shake_pos, "time", float.MaxValue, "islocal", true));
            drill_sprite_ani.StartAnimation();
        }
        else if(drill_type == DRILL_TYPE.POSITION)
        {
            iTween.PunchPosition(trans_pos_obj, iTween.Hash("amount", operate_position, "time", operate_time, "looptype", iTween.LoopType.loop));
        }
        else if(drill_type == DRILL_TYPE.ROTATION)
        {
            iTween.PunchRotation(rotation_anchor_obj, iTween.Hash("amount", operate_rotation, "time", operate_time, "looptype", iTween.LoopType.loop));
        }

        if (drill_body_sprite == true)
        {
            drill_body_sprite_ani.StartAnimation();
        }

        if (addition_operate != null)
        {
            SoundManager.PlayDrillAdditionOperation(addition_operate);
        }
        else
        {
            if (drill_sound_type == SOUND_TYPE.SCREW)
            {
                SoundManager.PlayDefaultDrill();
            }
            else if (drill_sound_type == SOUND_TYPE.KNOCK)
            {
                SoundManager.PlayknockDrill();
            }
            else if (drill_sound_type == SOUND_TYPE.ROLLING)
            {
                SoundManager.PlayRollingDrill();
            }
        }
    }

    public void PlayDrillIdleSound()
    {
        if (addition_idle != null)
        {
            SoundManager.PlayDillIdleSound(addition_idle);
        }
        
        if (addition_environment != null)
        {
            SoundManager.PlayDillEnvironmentSound(addition_environment);
        }
    }

    public void StopDrillIdleSound()
    {
        SoundManager.StopDrillIdleAndEnvironmentSound();
    }

    void StopDrill()
    {
        if (drill_type == DRILL_TYPE.SPRITE_ANI)
        {
            drill_sprite_ani.SetScale(0f);
            drill_sprite_ani.ResetAni();
        }
        else if (drill_type == DRILL_TYPE.ROTATION)
        {
            iTween.Stop(rotation_anchor_obj);
            rotation_anchor_obj.transform.localRotation = Quaternion.identity;
        }

        if (drill_body_sprite == true)
        {
            drill_body_sprite_ani.SetScale(0f);
            drill_body_sprite_ani.ResetAni();
        }

        SoundManager.StopDrillSound();
    }

    public void SetGray()
    {
        foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(0.4f, 0.4f, 0.4f);
        }
    }

    public void SetBlack()
    {
        foreach (SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(0f, 0f, 0f);
        }
    }

    public void SetDefaultColor()
    {
        foreach (SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(1f, 1f, 1f);
        }
    }

    public void SetDrillPositionToTarget(Transform target)
    {
        Debug.Log("================= ah~~~~~~~~~~~~~~~");
        transform.position = new Vector3(target.transform.position.x,
            target.transform.position.y,
            target.transform.position.z);
        transform.rotation = new Quaternion(target.transform.rotation.x,
            target.transform.rotation.y,
            target.transform.rotation.z,
            target.transform.rotation.w);

        ChaseTargetNail(target.gameObject);
    }
    
    public IEnumerator SetDrillPositionToTarget(Transform target, float time, float delay, bool rotation)
    {
        yield return new WaitForSeconds(delay);

        iTween.MoveTo(gameObject, iTween.Hash("position", target, "time", time, "easetype", iTween.EaseType.easeInOutQuad));
        if (rotation == true)
        {
            iTween.RotateTo(trans_pos_obj, iTween.Hash("rotation", target, "time", time, "easetype", iTween.EaseType.easeInOutQuad));
            ChaseTargetNail(target.gameObject);
        }
        iTween.ScaleBy(gameObject, iTween.Hash("amount", target.transform.localScale, "time", time, "easetype", iTween.EaseType.easeInOutQuad));

        

        yield return new WaitForSeconds(time);
        yield return new WaitForFixedUpdate();
        
        transform.position = new Vector3(target.transform.position.x,
            target.transform.position.y,
            target.transform.position.z);

        
        if (rotation == true)
        {
            trans_pos_obj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            transform.rotation = new Quaternion(target.transform.rotation.x,
            target.transform.rotation.y,
            target.transform.rotation.z,
            target.transform.rotation.w);
        }
            
    }

    public void ResetScale()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
