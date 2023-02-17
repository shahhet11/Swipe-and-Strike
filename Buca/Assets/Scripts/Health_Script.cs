using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health_Script : MonoBehaviour {
    public RawImage[] Life;
    public Transform Player_Respawn;
    public Transform Target_respawn;
    GameObject Target;
    public static Health_Script Instance;

	// Use this for initialization
	void Awake () {
        Instance = this;	
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
    void Die_Cycle()
    {
        
    }
    public void Respawn()
    {
        Movement.Instance.Player.position = Player_Respawn.transform.position;
        Target = GameObject.Find("Puck");
        Target.transform.position = Target_respawn.transform.position;
    }
    public void Next_Level()
    {
        Debug.Log("WentIn");
        int i = 2;
        GameObject Instance = Instantiate(Resources.Load("Level/Level"+i, typeof(GameObject))) as GameObject;
        i++;
        //        var Level = Resources.Load<GameObject>("Level/Level2") as GameObject;

    }
    public void Life_Left()
    {
        if (PlayerPrefs.GetInt("LifeLeft") == 0)
        {
            Life[2].color = new Color32(255, 255, 255, 42);
        }
        else if (PlayerPrefs.GetInt("LifeLeft") == 1)
        {
            Life[1].color = new Color32(255, 255, 255, 42);
        }
        else if (PlayerPrefs.GetInt("LifeLeft") == 2)
        {
            Life[0].color = new Color32(255, 255, 255, 42);
        }

    }
}
