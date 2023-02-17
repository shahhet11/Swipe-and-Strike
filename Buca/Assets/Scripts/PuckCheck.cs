using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuckCheck : MonoBehaviour {
//    public GameObject Plane;


    public static PuckCheck Instance;
    Rigidbody puckRB;
	// Use this for initialization
    void Awake()
    {
        Instance = this;

    }
	void Start () {
        puckRB = transform.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("holein"))
        {
            
            Debug.Log("holeCs");
//            Plane.GetComponent<MeshCollider>().convex = true;
//            Plane.GetComponent<MeshCollider>().isTrigger = true;
//            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            PlayerPrefs.SetInt("LevelFinished", 10);
            Movement.Instance.a = true;
            Winning_PE.Instance.ParticleEffectsWin();

            //LevelFinished
            GameObject.FindGameObjectWithTag("holein").GetComponent<BoxCollider>().enabled = false;
        }
        if (collision.gameObject.CompareTag("wallR"))
        {
            Movement.Instance.Sparks(collision);
            this.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("wallG"))
        {
            Movement.Instance.Sparks(collision);
        }
    }


     IEnumerator RedLoadLevel()
    {
        yield return new WaitForSeconds(2f);
        Health_Script.Instance.Next_Level();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel()
    {
        
        StartCoroutine(RedLoadLevel()); 
    }
//    protected void PlayEffectOnce(ParticleSystem prefab, Vector3 position) {
//       
//        ParticleSystem ps = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;
//        Destroy(ps.gameObject, 1f);
//    }  
//    void OnTriggerEnter(Collider col)
//    {
//        if (col.gameObject.CompareTag("hole"))
//        {
//            Debug.Log("holeT");
//            Plane.GetComponent<MeshCollider>().convex = true;
//            Plane.GetComponent<MeshCollider>().isTrigger = true;
//            PlayerPrefs.SetInt("LevelFinished", 10);
//            Movement.Instance.a = true;
//            //LevelFinished
//
//        }
//    }


}
