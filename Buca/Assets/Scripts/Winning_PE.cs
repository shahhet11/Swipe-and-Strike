using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winning_PE : MonoBehaviour {
    public GameObject[] ParticlesHole;
    public static Winning_PE Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ParticleEffectsWin()
    {
        for (int i = 0; i < ParticlesHole.Length; i++)
        {

            ParticlesHole[i].SetActive(true);
        }
        StartCoroutine(ParticleDisable()); 
    }
    IEnumerator ParticleDisable()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < ParticlesHole.Length; i++)
        {

            ParticlesHole[i].SetActive(false);
        }
    }
}
