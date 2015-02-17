using UnityEngine;
using System.Collections;

public class MoveDownOnStart : MonoBehaviour
{
		public bool move = false;
		private Vector3 endPosition = new Vector3 (0f, 0f, -1f);
		private float duration = 10f;
		private PlayerManager playerManager;
		private bool clickable = false;
		//private float time = 0.0f;
		public Sprite startJourney;

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
				//time += Time.deltaTime;
				//if (time > 3.0f) {
				//		move = true;
				//}
		}

		void ChangeText ()
		{
				StartCoroutine ("WaitAndDo", 1);
		}

		IEnumerator WaitAndDo (int secs)
		{
				yield return new WaitForSeconds (secs);
				GetComponent<SpriteRenderer> ().sprite = startJourney;
		}


		void MakeClickable ()
		{
				clickable = true;
				CharacterSelect[] charSelect = GameObject.FindObjectsOfType<CharacterSelect> ();
				foreach (CharacterSelect sel in charSelect) {
						sel.joinGame = true;
				}
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
