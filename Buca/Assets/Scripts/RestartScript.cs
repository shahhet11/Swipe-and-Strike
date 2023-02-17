using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartScript : MonoBehaviour 
{
	void Start()
	{
		this.GetComponent <Button>().onClick.AddListener (Restart);
	}

	void Restart()
	{
		SceneManager.LoadScene ("Main", LoadSceneMode.Single);
	}

}
