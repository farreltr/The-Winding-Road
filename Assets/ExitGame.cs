using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour
{
	
		void OnMouseDown ()
		{
				// warning first? Are you sure you want to quit?
				Application.Quit (); 
		}
	
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) 
						Application.Quit (); 
		}
}
