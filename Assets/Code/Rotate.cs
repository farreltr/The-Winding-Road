using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
		public Transform onClickSprite;
		public Transform defaultSprite;
		public int index;

		void Start ()
		{
				defaultSprite = GetComponent<SpriteRenderer> ().transform;
		}

		void OnMouseDown ()
		{
				GetComponent<SpriteRenderer> ().sprite = onClickSprite.GetComponent<SpriteRenderer> ().sprite;
		}

		void OnMouseUp ()
		{
				GetComponent<SpriteRenderer> ().sprite = defaultSprite.GetComponent<SpriteRenderer> ().sprite;		
				//Vector3 localPos = transform.position;
				//Quaternion localRot = transform.rotation;
				//transform.parent.GetComponent<Tile> ().RotateLeft ();
				GameObject.FindObjectOfType<PlayerDeck> ().GetTileFromIndex (index).RotateLeft ();
				//transform.position = localPos;
				//transform.rotation = localRot;
		}
}
