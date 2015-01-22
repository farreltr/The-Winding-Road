using UnityEngine;
using System.Collections;

public class WaitAndClick : MonoBehaviour
{

		private float time = 0.0f;
	
		void Update ()
		{
				time += Time.deltaTime;
				if (time > 30.0f || Input.anyKey) {
						Application.LoadLevel (1);
				}
	
		}

}
