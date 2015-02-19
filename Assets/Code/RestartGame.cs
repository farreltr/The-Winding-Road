using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour
{

		void OnMouseDown ()
		{
				GameController gameController = GameObject.FindObjectOfType<GameController> ();
				PlayerManager playerController = GameObject.FindObjectOfType<PlayerManager> ();
				playerController.Reset ();
				if (gameController != null) {
						for (int i=gameController.transform.childCount-1; i>=0; i--) {
								Destroy (gameController.transform.GetChild (i));
						}
						Destroy (gameController.gameObject);
				}
				Application.LoadLevel (0);

		}
}
