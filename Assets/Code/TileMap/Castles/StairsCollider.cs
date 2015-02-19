using UnityEngine;
using System.Collections;

public class StairsCollider : MonoBehaviour
{

		private GameController controller;
		private bool gameOver = false;

		void Start ()
		{
				controller = GameObject.FindObjectOfType<GameController> ();
	
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
				if (!gameOver) {
						GameObject colliderObject = collider.gameObject;
						string myName = GetFormattedName (gameObject);
						PlayerController playerController = colliderObject.transform.GetComponent<PlayerController> ();
						if (playerController != null) {
								if (myName == playerController.GetName ()) {
										playerController.isWinner = true;
										controller.GameOver ();
										gameOver = true;
								} else if (myName == playerController.GetRespawnCastle () && playerController.isRespawn) {
										//do nothing
								} else {
										playerController.ChangeDirection ();
								}
						}

				}

					
		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}

}
