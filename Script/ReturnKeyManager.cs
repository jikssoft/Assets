using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ReturnKeyProcess
{
    void Return();
    
}

public class ReturnKeyManager : MonoBehaviour {

    static ReturnKeyProcess return_key_process;
    static ReturnKeyManager instance;
    static Stack<ReturnKeyProcess> return_process_statck;

    // Use this for initialization

    void Awake()
    {
        instance = this;
        return_process_statck = new Stack<ReturnKeyProcess>();
        instance.regist_time = float.MinValue;
    }

    void Start () {
        return_time = Time.fixedTime;
        
    }

    // Update is called once per frame

    float return_time;
    float regist_time;
    public LoadingController loading_controller;
    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if(loading_controller.IsLoading() == true || instance.pause == true)
            {
                return;
            }

            if (return_key_process != null && Time.fixedTime - return_time > 0.5f)
            {
                return_time = Time.fixedTime;
                return_key_process.Return();
                Debug.Log("================= Return object: " + gameObject.name);
            }
        }
	}
    
    static public void RegisterReturnKeyProcess(ReturnKeyProcess returnKeyProcess)
    {
        if(Time.fixedTime - instance.regist_time < 0.5f)
        {
            return;
        }

        Debug.Log("================= register");
        instance.return_time = Time.fixedTime;
        instance.regist_time = Time.fixedTime;
        return_key_process = returnKeyProcess;
        return_process_statck.Push(returnKeyProcess);
        instance.pause = false;
    }

    bool pause = false;
    static public void PauseReturnProcess()
    {
        Debug.Log("================= pause");
        instance.pause = true;
    }

    static public void RetrunProcess()
    {
        instance.return_time = Time.fixedTime;
        return_process_statck.Pop();

        Debug.Log("================= return size : " + return_process_statck.Count.ToString());
        return_key_process = return_process_statck.Peek();
        instance.pause = false;

        
    }

}
