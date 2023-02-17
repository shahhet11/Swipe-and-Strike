using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Movement : MonoBehaviour 
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
//    public Transform originalObject;
//    public Transform reflectedObject;
    public List<Pool> pools;
    public Dictionary<string , Queue<GameObject>> poolDictionary;
//    public Light InholeLight;
//    public Transform DustParticles;
    public Transform MagicCircle;

    public Transform SparkPrefabTarget;
    public Transform SparkPrefabRed;
    public Transform SparkPrefabGreen;
    public GameObject FingerTouchPE;
//    public GameObject PlayerPE;//Player Particle Position
    public Camera camera;
//    public GameObject Plane;
    public GameObject Collisionlight;
    public GameObject PlayerLight;
    public Transform Player;
    GameObject go;
	Rigidbody rb;
	Vector2 fingerStart;
	Vector2 fingerEnd;
	float swipeDistanceOnX;
	float swipeDistanceOnY;
	public float ForceMultiplier;
	LineRenderer Trail;
    Vector3 TrailMove;
    public Vector3 LaserMove;
    float speed = 0.02f;
    public bool a = false;
    bool fingertouch = false;
    bool IsDustParticles = false;
    public Health_Script Health_Obj;
    public static Movement Instance;
    int Life_Left_value = 1;
    void Awake()
    {
        Instance = this;
        PlayerPrefs.SetInt("Stopped",0);
    }
    public GameObject SpawnFromPool(string tag,Vector3 position, Quaternion rotation)
    {
        
        if (!poolDictionary.ContainsKey(tag))
        { 
           Debug.LogWarning("Pool with tag"+tag+"dosent exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.GetComponent<Destroy1>().DeactivateInSecs();
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
	void Start () 
	{
        
        PlayerPrefs.DeleteAll();
//        GameObject Instance = Instantiate(Resources.Load("Level/Level1", typeof(GameObject))) as GameObject;
        Physics.gravity = new Vector3(0, -10f, 0);
        poolDictionary = new Dictionary<string , Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectpool);
        }

		rb = transform.GetComponent <Rigidbody> ();
		Trail = transform.GetComponent <LineRenderer> ();
        Trail.enabled = false;
        Trail.SetPosition (0 ,Player.transform.position);
        Trail.SetPosition (1 ,Player.transform.position);

        InvokeRepeating("FingerTouchParticles", 1f , 0.1f);
        InvokeRepeating("DustParticlesTrail", .01f , .05f);

	}
    void FingerTouchParticles()
    {
        if (fingertouch != false)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);

            if (Physics.Raycast(ray, out hit) )
            {
                SpawnFromPool("FingerParticle",hit.point,Quaternion.identity);
//             go = Instantiate(FingerTouchPE, hit.point, Quaternion.identity);
            }
        }
    }
    void DustParticlesTrail()
    {
        if (IsDustParticles != false)
        {
            Debug.Log("Received");
            SpawnFromPool("DustParticle",Player.transform.position,Quaternion.identity);
            
        }

    }

	void Update () 
	{
//        Debug.Log(rb.velocity+" Velocity");
//        reflectedObject.position = Vector3.Reflect(originalObject.position, Vector3.right);
//        if (fingertouch != false)
//        {
//            RaycastHit hit;
//            Ray ray = camera.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);
//
//            if (Physics.Raycast(ray, out hit) )
//            {
//                SpawnFromPool("FingerParticle",hit.point,Quaternion.identity);
//                //             go = Instantiate(FingerTouchPE, hit.point, Quaternion.identity);
//            }
//        }

      //  Debug.Log(swipeDistanceOnX + " Xdis " + swipeDistanceOnY + " YdisS");
//        Debug.Log(fingerEnd);
//        Debug.Log(rb.velocity +"Velocity");

        if(rb.velocity == Vector3.zero)
        {
            if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                
                fingertouch = true;
                IsDustParticles = false;
                fingerStart = ControlFreak2.CF2Input.mousePosition;
                fingerEnd = ControlFreak2.CF2Input.mousePosition;
                Trail.SetPosition(0, Player.transform.position);

                Trail.enabled = true;

            }
            else if (ControlFreak2.CF2Input.GetMouseButton(0))
            {
                float maxDist = 200f;

                fingertouch = true;
                IsDustParticles = false;
                fingerEnd = ControlFreak2.CF2Input.mousePosition;
//                fingerEnd = new Vector2(0f, Mathf.Clamp(Time.time, 1.0f, 3.0f));
                Vector2 dir = fingerEnd - fingerStart;
                float dist = Mathf.Clamp(Vector2.Distance(fingerStart, fingerEnd), 0, maxDist);
                fingerEnd = fingerStart + (dir.normalized * dist);
                swipeDistanceOnX = (fingerEnd.x - fingerStart.x);
                swipeDistanceOnY = (fingerEnd.y - fingerStart.y);
//                TrailMove = new Vector3(-swipeDistanceOnX, 1f, -swipeDistanceOnY);
                TrailMove = new Vector3(-swipeDistanceOnX * 0.02f, 1f, -swipeDistanceOnY * 0.02f);
                LaserMove = new Vector3(-swipeDistanceOnX , 1f, -swipeDistanceOnY );
                Trail.SetPosition(1, TrailMove);
//                DustParticles.gameObject.SetActive(true);
//                MagicCircle.Rotate(new Vector3(-180f, Mathf.Atan2(swipeDistanceOnY, swipeDistanceOnX) * Mathf.Rad2Deg, 0f));
//                MagicCircle.RotateAround(Player.position,Vector3.up,Mathf.Atan2(-swipeDistanceOnY, -swipeDistanceOnX) * Mathf.Rad2Deg);
            }
            if (ControlFreak2.CF2Input.GetMouseButtonUp(0) && (swipeDistanceOnX == 0f || swipeDistanceOnY == 0f))
            {
                fingertouch = false;
                Trail.enabled = false;
            }
            if (ControlFreak2.CF2Input.GetMouseButtonUp(0) && (swipeDistanceOnX != 0f || swipeDistanceOnY != 0f))
            {
                
                    PlayerLight.SetActive(true);
//                MagicCircle.position = new Vector3(-swipeDistanceOnX * 0.02f, 1f, -swipeDistanceOnY * 0.02f);
                transform.LookAt( new Vector3(-swipeDistanceOnX, 1f, -swipeDistanceOnY)); 
                MagicCircle.gameObject.SetActive(true);
                IsDustParticles = true;
                fingertouch = false;
                Debug.Log("OK");
                Trail.enabled = false;
                rb.AddForce(new Vector3(-swipeDistanceOnX * ForceMultiplier, 1f, -swipeDistanceOnY * ForceMultiplier), ForceMode.Force);

                fingerStart = Vector2.zero;
                fingerEnd = Vector2.zero;
                TrailMove = Player.transform.position;

                StartCoroutine(LoadLevel()); 
            }


            if (rb.velocity == Vector3.zero && a == true)
            {
                IsDustParticles = false;
                GameObject[] dust = GameObject.FindGameObjectsWithTag("Dust");
                for (int i = 0; i < dust.Length; i++)
                {
                    
                    dust[i].SetActive(false);
                }

                PlayerLight.SetActive(false);
//                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                a = false;

                if (PlayerPrefs.GetInt("LevelFinished") == 10)
                {
                    Debug.Log("Not-dead");
//                    Health_Obj.Respawn();
                    Health_Obj.Next_Level();
                    PlayerPrefs.SetInt("LevelFinished",0);

                }
                else
                {
                    
                    Debug.Log("dead");
//                    Health_Obj.Respawn();
                    Health_Obj.Next_Level();
                    PlayerPositionReset();
                    Health_Obj.Life_Left();
                    PlayerPrefs.SetInt("LifeLeft",Life_Left_value);
                    Life_Left_value++;
                    if (PlayerPrefs.GetInt("LifeLeft") == 3)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                }
            }

           // Debug.Log(swipeDistanceOnX + " Xdis1 " + swipeDistanceOnY + " YdisS1");
//



        //Touch Aim

//        if (rb.velocity == Vector3.zero)
//        {
//            if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
//            {
//
//                fingerStart = Input.GetTouch(0).position;
//                fingerEnd = Input.GetTouch(0).position;
//                Trail.SetPosition(0, Player.transform.position);
//                PlayerLight.SetActive(true);
//                Trail.enabled = true;
//
//            }
//            else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved)
//            {
//
//                fingerEnd = Input.GetTouch(0).position;
//
//                swipeDistanceOnX = (fingerEnd.x - fingerStart.x);
//                swipeDistanceOnY = (fingerEnd.y - fingerStart.y);
//                //                TrailMove = new Vector3(-swipeDistanceOnX, 1f, -swipeDistanceOnY);
//                TrailMove = new Vector3(-swipeDistanceOnX * speed, 1f, -swipeDistanceOnY * speed);
//                Trail.SetPosition(1, TrailMove);
//                PlayerLight.SetActive(true);
//
//            }
//             if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended)
//            {
//                  PlayerLight.SetActive(true);
//                Debug.Log("OK");
//                Trail.enabled = false;
//                rb.AddForce(new Vector3(-swipeDistanceOnX * ForceMultiplier, 1f, -swipeDistanceOnY * ForceMultiplier), ForceMode.Force);
//                PlayerLight.SetActive(true);
//                fingerStart = Vector3.zero;
//                fingerEnd = Vector3.zero;
//
//                swipeDistanceOnX = Player.transform.position.x;
//                swipeDistanceOnY = Player.transform.position.z;
//                //                Reset(Trail);
//                StartCoroutine(LoadLevel()); 
//            }
//
//            if (rb.velocity == Vector3.zero && a == true)
//            {
//
//                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//            }
//
//        }
            }
       
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wallR"))
        {
            Debug.Log("wall");
            Collisionlight.SetActive(true);
            StartCoroutine(CollisionlightStop());
            Sparks(collision);
           this.gameObject.SetActive(false);
            PuckCheck.Instance.LoadLevel();


        }
        if (collision.gameObject.CompareTag("wallG"))
        {
            Debug.Log("wall");
            Collisionlight.SetActive(true);
            StartCoroutine(CollisionlightStop());
            Sparks(collision);
        }
        if (collision.gameObject.CompareTag("puck"))
        {
            Sparks(collision);

        }
        if (collision.gameObject.CompareTag("hole"))
        {
            Debug.Log("hole");
//            Plane.GetComponent<MeshCollider>().convex = true;
//            Plane.GetComponent<MeshCollider>().isTrigger = true;
//            PlayerPrefs.SetInt("LevelFinished", 10);
//            StartCoroutine(VelocityZero());
//            a = true;
            //LevelFinished
//            rb.velocity == rb.velocity - Vector3()
//            rb.AddForce(new Vector3(0f,-50f,0f), ForceMode.VelocityChange);
            GameObject.FindGameObjectWithTag("holein").GetComponent<BoxCollider>().enabled = false;
            rb.AddForce(Vector3.down);
            //            rb.AddForce(new Vector3(0f, -100f, 0f), ForceMode.Acceleration);
            //            rb.AddForce(new Vector3(0f, -30f, 0f), ForceMode.Force);
            //            rb.AddForce(new Quaternion(50f,0f,0f), ForceMode.Acceleration);
              Physics.gravity = new Vector3(0, -100.0f, 0);
            
            //            InholeLight.enabled = true;
            StartCoroutine(LightingONOFF());
//            rb.velocity = new Vector3(0f,10f,0f);
//            rb.mass = 20;
        }
        if (collision.gameObject.CompareTag("holein"))
        {
            
        }

    }
//    IEnumerator 

    IEnumerator LightingONOFF()
    {
//        InvokeRepeating("Light_ON", 0.1f , 0.25f);
        yield return new WaitForSeconds(0.01f);
//        InvokeRepeating("Light_OFF", 0.1f , 0.25f);


//        rb.rotation = Quaternion.Euler(25f, 0, 0);

        yield return new WaitForSeconds(2f);
//        InholeLight.enabled = false;
    }
//    public void Light_ON()
//    {
//        Debug.Log("ON");
//        InholeLight.intensity++;
//    
//    }
//    public void Light_OFF()
//    {Debug.Log("OFF");
//        InholeLight.intensity--;
//    }
    public void Sparks(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if(collision.gameObject.CompareTag("wallR"))
            Instantiate(SparkPrefabRed, pos, rot);

        if (collision.gameObject.CompareTag("wallG"))
        {
            Instantiate(SparkPrefabGreen, pos, rot);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if (collision.gameObject.CompareTag("puck"))
        {
            Instantiate(SparkPrefabTarget, pos, rot);    

                                                         
        }
    }
    IEnumerator CollisionlightStop()
    {
        yield return new WaitForSeconds(0.08f);
        Collisionlight.SetActive(false);
    }
    IEnumerator LoadLevel()
    {
        
        yield return new WaitForSeconds(1f);
        a = true;      
    }

//    IEnumerator VelocityZero()
//    {
//        yield return new WaitForSeconds(0.05f);
//                    rb.velocity = Vector3.zero;
//    }
//    public void Reset(LineRenderer lr)
//    {
//
//        Debug.Log("Reset");
//        Vector3[] positions = new Vector3[2];
//        positions[0] = new Vector3(0f, 0f, 0f);
//        positions[1] = new Vector3(0f, 0f, 0f);
//        lr.positionCount = positions.Length;
//        lr.SetPositions(positions);
//    }

//    IEnumerator Wait()
//    {
//        yield return new WaitForSeconds(.2f);
//        Trail.enabled = true;
//    }

    public void PlayerPositionReset()
    {
        transform.position = new Vector3(0f, 0.03f, 0f);
        transform.rotation = Quaternion.EulerAngles(0f, 0f, 0f);
    }
}
