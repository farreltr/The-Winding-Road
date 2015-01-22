using UnityEngine;
using System.Collections;

public class Castles : MonoBehaviour
{
	
		public static Castles castles;
	
		void Awake ()
		{
				if (castles == null) {
						DontDestroyOnLoad (castles);
						castles = this;
				} else if (castles != this) {
						Destroy (gameObject);
				}
		}
	
		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				GameObject[] castles = Resources.LoadAll<GameObject> ("Castles/Prefabs/");
				foreach (GameObject castle in castles) {
						GameObject s = (GameObject)Instantiate (castle);
						s.transform.parent = this.transform;	
						s.layer = this.gameObject.layer;
						SpriteRenderer renderer = s.GetComponent<SpriteRenderer> ();
						renderer.sortingLayerName = "Castle";
						renderer.sortingOrder = 0;
				}
		
		}
	
}
