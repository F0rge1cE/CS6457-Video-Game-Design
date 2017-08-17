using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement; // neded in order to load scenes

public class menu : MonoBehaviour 
{
	public GameObject difficulty;
	public GameObject container;

	RectTransform submenuOpen;

	void Start ()

	{
		submenuOpen = container.GetComponent<RectTransform> ();
		submenuOpen.localScale = new Vector3(1, 0, 1);

		DontDestroyOnLoad(difficulty);
	}
		

	public void CreditsLevel () //this function will be used on our Play button

	{
		SceneManager.LoadScene ("Credits"); //this will load our first level from our build settings. "1" is the second scene in our game

	}

	public void NewGame()
	{
		submenuOpen.localScale = new Vector3(1, 1, 1);
//		SceneManager.LoadScene("City");
	}

	public void Easy()
	{
		difficulty.GetComponent<DiffRecord> ().difficulty = 0;
		SceneManager.LoadScene("City");
	}
	public void Medium()
	{
		difficulty.GetComponent<DiffRecord> ().difficulty = 1;
		SceneManager.LoadScene("City");
	}
	public void Hard()
	{
		difficulty.GetComponent<DiffRecord> ().difficulty = 2;
		SceneManager.LoadScene("City");
	}

	public void MainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void Tutorial()
	{
		SceneManager.LoadScene ("Tutorial");
	}

	public void Quit()
	{
		Application.Quit ();
	}
		

}
