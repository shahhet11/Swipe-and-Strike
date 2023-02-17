using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy1 : MonoBehaviour {
    public float a = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
    public void DeactivateInSecs()
    {
        Invoke("ABC",a);
    }
    void ABC()
    {
        gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
