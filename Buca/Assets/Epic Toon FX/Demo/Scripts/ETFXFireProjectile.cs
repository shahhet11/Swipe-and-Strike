using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace EpicToonFX
{
public class ETFXFireProjectile : MonoBehaviour 
{
    RaycastHit hit;
    public GameObject[] projectiles;
    public Transform spawnPosition;
    [HideInInspector]
    public int currentProjectile = 0;
	public float speed = 1000;

//    MyGUI _GUI;
	ETFXButtonScript selectedProjectileButton;

	void Start () 
	{
		selectedProjectileButton = GameObject.Find("Button").GetComponent<ETFXButtonScript>();
	}

	void Update () 
	{
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextEffect();
        }

		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.D))
		{
			nextEffect();
		}

		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.A))
		{
			previousEffect();
		}
        else if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.LeftArrow))
        {
            previousEffect();
        }

        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Mouse0))
        {

			if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition), out hit, 100f))
                {
                    GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
                    projectile.transform.LookAt(hit.point);
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
                    projectile.GetComponent<ETFXProjectileScript>().impactNormal = hit.normal;
                }  
            }

        }
        Debug.DrawRay(Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition).origin, Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition).direction*100, Color.yellow);
	}

    public void nextEffect()
    {
        if (currentProjectile < projectiles.Length - 1)
            currentProjectile++;
        else
            currentProjectile = 0;
		selectedProjectileButton.getProjectileNames();
    }

    public void previousEffect()
    {
        if (currentProjectile > 0)
            currentProjectile--;
        else
            currentProjectile = projectiles.Length-1;
		selectedProjectileButton.getProjectileNames();
    }

	public void AdjustSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
}