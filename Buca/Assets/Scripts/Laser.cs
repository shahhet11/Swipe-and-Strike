using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    LineRenderer LaserLight;
    public GameObject[] hitEffects;
    public bool makeHitEffects;
    public float range;
    public float beamPower;
    public Transform raycastStartSpot;   
    public bool reflect = true;                         // Whether or not the laser beam should reflect off of certain surfaces
    public Material reflectionMaterial;                 // The material that reflects the laser.  If this is null, the laser will reflect off all surfaces
    public int maxReflections = 5;                      // The maximum number of times the laser beam can reflect off surfaces.  Without this limit, the system can possibly become stuck in an infinite loop
    public string beamTypeName = "laser_beam";          // This is the name that will be used as the name of the instantiated beam effect.  It is not necessary.
    public float maxBeamHeat = 1.0f;                    // The time, in seconds, that the beam will last
    public bool infiniteBeam = false;                   // If this is true the beam will never overheat (equivalent to infinite ammo)
    public Material beamMaterial;                       // The material that will be used in the beam (line renderer.)  This should be a particle material
    public Color beamColor = Color.red;                 // The color that will be used to tint the beam material
    public float startBeamWidth = 0.5f;                 // The width of the beam on the starting side
    public float endBeamWidth = 1.0f;                   // The width of the beam on the ending side
    private float beamHeat = 0.0f;                      // Timer to keep track of beam warmup and cooldown
    private bool coolingDown = false;                   // Whether or not the beam weapon is currently cooling off.  This is used to make sure the weapon isn't fired when it's too close to the maximum heat level
    private GameObject beamGO;                          // The reference to the instantiated beam GameObject
    private bool beaming = false;
	// Use this for initialization
	void Start () {
        LaserLight = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
//        Laser_Ray();
        if (ControlFreak2.CF2Input.GetMouseButton(0))
        {
//            Beam();
        }
    }
   
    void Laser_Ray () {
        Ray ray = new Ray (transform.position, Vector3.up);
        RaycastHit hit;
        if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
        {
            LaserLight.SetPosition(0, ray.origin);
        }
        else if (ControlFreak2.CF2Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                LaserLight.SetPosition(1, Movement.Instance.LaserMove);
            }
            else
            {
                LaserLight.SetPosition(1, Movement.Instance.LaserMove);
            }
//            if (hit.collider.tag == "wallR" || hit.collider.tag == "puck" || hit.collider.tag == "wallG") {
                LaserLight.SetPosition (2, hit.point +  Vector3.Reflect (transform.forward, hit.normal));
//            }
        }
//        if (ControlFreak2.CF2Input.GetMouseButtonUp(0))
//        {
       
    }
    MeshRenderer FindMeshRenderer(GameObject go)
    {
        MeshRenderer hitMesh;

        // Use the MeshRenderer directly from this GameObject if it has one
        if (go.GetComponent<Renderer>() != null)
        {
            hitMesh = go.GetComponent<MeshRenderer>();
        }

        // Try to find a child or parent GameObject that has a MeshRenderer
        else
        {
            // Look for a renderer in the child GameObjects
            hitMesh = go.GetComponentInChildren<MeshRenderer>();

            // If a renderer is still not found, try the parent GameObjects
            if (hitMesh == null)
            {
                GameObject curGO = go;
                while (hitMesh == null && curGO.transform != curGO.transform.root)
                {
                    curGO = curGO.transform.parent.gameObject;
                    hitMesh = curGO.GetComponent<MeshRenderer>();
                }
            }
        }

        return hitMesh;
    }
    void Beam()
    {
        Debug.Log("Went ni");
        // Send a messsage so that users can do other actions whenever this happens
        SendMessageUpwards("OnEasyWeaponsBeaming", SendMessageOptions.DontRequireReceiver);

        // Set the beaming variable to true
        beaming = true;

        // Make the beam weapon heat up as it is being used
        if (!infiniteBeam)
            beamHeat += Time.deltaTime;

        // Make the beam effect if it hasn't already been made.  This system uses a line renderer on an otherwise empty instantiated GameObject
        if (beamGO == null)
        {
            beamGO = new GameObject(beamTypeName, typeof(LineRenderer));
            beamGO.transform.parent = transform;        // Make the beam object a child of the weapon object, so that when the weapon is deactivated the beam will be as well   - was beamGO.transform.SetParent(transform), which only works in Unity 4.6 or newer;
        }
        LineRenderer beamLR = beamGO.GetComponent<LineRenderer>();
        beamLR.material = beamMaterial;
        beamLR.material.SetColor("_TintColor", beamColor);
        beamLR.SetWidth(startBeamWidth, endBeamWidth);

        // The number of reflections
        int reflections = 0;

        // All the points at which the laser is reflected
        List<Vector3> reflectionPoints = new List<Vector3>();
        // Add the first point to the list of beam reflection points
        reflectionPoints.Add(raycastStartSpot.position);

        // Hold a variable for the last reflected point
        Vector3 lastPoint = raycastStartSpot.position;

        // Declare variables for calculating rays
        Vector3 incomingDirection;
        Vector3 reflectDirection;

        // Whether or not the beam needs to continue reflecting
        bool keepReflecting = true;

        // Raycasting (damgage, etc)
        Ray ray = new Ray(lastPoint, raycastStartSpot.forward);
        RaycastHit hit;



        do
        {
            // Initialize the next point.  If a raycast hit is not returned, this will be the forward direction * range
            Vector3 nextPoint = ray.direction * range;

            if (Physics.Raycast(ray, out hit, range))
            {
                // Set the next point to the hit location from the raycast
                nextPoint = hit.point;

                // Calculate the next direction in which to shoot a ray
                incomingDirection = nextPoint - lastPoint;
                reflectDirection = Vector3.Reflect(incomingDirection, hit.normal);
                ray = new Ray(nextPoint, reflectDirection);

                // Update the lastPoint variable
                lastPoint = hit.point;

                // Hit Effects
                if (makeHitEffects)
                {
                    foreach (GameObject hitEffect in hitEffects)
                    {
                        if (hitEffect != null)
                            Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }
                }

                // Damage
                hit.collider.gameObject.SendMessageUpwards("ChangeHealth", -beamPower, SendMessageOptions.DontRequireReceiver);

                // Shooter AI support
//                if (shooterAIEnabled)
//                {
//                    hit.transform.SendMessageUpwards("Damage", beamPower / 100, SendMessageOptions.DontRequireReceiver);
//                }

                // Bloody Mess support
//                if (bloodyMessEnabled)
//                {
//                    //call the ApplyDamage() function on the enenmy CharacterSetup script
//                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Limb"))
//                    {
//                        Vector3 directionShot = hit.collider.transform.position - transform.position;
//
//                        //  Remove the comment marks from the following section of code for Bloody Mess support
//                        /*
//                        if (hit.collider.gameObject.GetComponent<Limb>())
//                        {
//                            GameObject parent = hit.collider.gameObject.GetComponent<Limb>().parent;
//                            
//                            CharacterSetup character = parent.GetComponent<CharacterSetup>();
//                            character.ApplyDamage(beamPower, hit.collider.gameObject, weaponType, directionShot, Camera.main.transform.position);
//                        }
//                        */
//
//                    }
//                }


                // Increment the reflections counter
                reflections++;
            }
            else
            {

                keepReflecting = false;
            }

            // Add the next point to the list of beam reflection points
            reflectionPoints.Add(nextPoint);

        } while (keepReflecting && reflections < maxReflections && reflect && (reflectionMaterial == null || (FindMeshRenderer(hit.collider.gameObject) != null && FindMeshRenderer(hit.collider.gameObject).sharedMaterial == reflectionMaterial)));

        // Set the positions of the vertices of the line renderer beam
        beamLR.SetVertexCount(reflectionPoints.Count);
        for (int i = 0; i < reflectionPoints.Count; i++)
        {
            beamLR.SetPosition(i, reflectionPoints[i]);

            // Muzzle reflection effects
//            if (makeMuzzleEffects && i > 0)     // Doesn't make the FX on the first iteration since that is handled later.  This is so that the FX at the muzzle point can be parented to the weapon
//            {
//                GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
//                if (muzfx != null)
//                {
//                    Instantiate(muzfx, reflectionPoints[i], muzzleEffectsPosition.rotation);
//                }
//            }
        }

        // Muzzle flash effects
//        if (makeMuzzleEffects)
//        {
//            GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
//            if (muzfx != null)
//            {
//                GameObject mfxGO = Instantiate(muzfx, muzzleEffectsPosition.position, muzzleEffectsPosition.rotation) as GameObject;
//                mfxGO.transform.parent = raycastStartSpot;
//            }
//        }

      
    }
}
