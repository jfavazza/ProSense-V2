using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScript : MonoBehaviour {

	public Canvas startupMenu;
	public Canvas loginMenu;
	public Canvas createAccountMenu;
	public Button login;

	// Use this for initialization
	void Start () 
	{
		loginMenu = loginMenu.GetComponent<Canvas>(); 
		startupMenu = startupMenu.GetComponent<Canvas>();
		createAccountMenu = createAccountMenu.GetComponent<Canvas>();
		login = login.GetComponent<Button>();
		loginMenu.enabled = false;
		createAccountMenu.enabled = false;

	}

	public void SignInPressed() 
	{
		loginMenu.enabled = true;
		createAccountMenu.enabled = false;
		login.enabled = false;
		startupMenu.enabled = false;
	}

	public void CreateAccountPressed()
	{
		createAccountMenu.enabled = true;
		loginMenu.enabled = false;
		login.enabled = false;
		startupMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
