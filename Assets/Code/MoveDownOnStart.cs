using UnityEngine;
using System.Collections;

public class MoveDownOnStart : MonoBehaviour
{
		private bool move = false;
		private Vector3 endPosition = Vector3.zero;
		private float duration = 10f;
		private PlayerManager playerManager;
		private bool clickable = false;
		private float time = 0.0f;

		void Start ()
		{
				playerManager = GameObject.FindObjectOfType<PlayerManager> ();
		}

		void Update ()
		{
				if (move && !Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						Move ();
				} else if (move && Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						move = false;
						ChangeText ();
						MakeClickable ();
				}
				time += Time.deltaTime;
				if (time > 3.0f) {
						move = true;
				}
		}

		void ChangeText ()
		{

		}

		void MakeClickable ()
		{
				clickable = true;
		}

		void OnMouseUp ()
		{
				if (clickable) {
						playerManager.StartGame ();
				}
		}

		void Move ()
		{
				gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, endPosition, 1 / (duration * (Vector3.Distance (gameObject.transform.position, endPosition))));
		}


}
