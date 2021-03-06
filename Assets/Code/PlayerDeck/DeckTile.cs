﻿using UnityEngine;
using System.Collections;

public class DeckTile : MonoBehaviour
{

		private Color originalColour;
		private Color transparentColour;
		private Vector3 slotPosition = Vector3.zero;
		private Vector3 screenPoint;
		private Vector3 offset;
		private GameController gameController;
		private PlayerDeck parent;
		private bool released = false;
		private bool hit = false;
		private bool draggable = true;
		private int slotIndex;
		private Transform child;
		public bool disabled = false;
		private bool cpuMoving = false;
		private Vector3 endLocation;
		private float duration = 8f;
	
		void Start ()
		{
				originalColour = gameObject.renderer.material.color;
				transparentColour = originalColour;
				transparentColour.a = 0.8f;
				slotPosition = transform.position;
				if (GameObject.FindObjectOfType<PlayerDeck> () != null) {
						this.transform.parent = GameObject.FindObjectOfType<PlayerDeck> ().transform;
						this.gameController = GameObject.FindObjectOfType<GameController> ();
						this.parent = gameObject.GetComponentInParent<PlayerDeck> ();
				}
				disabled = false;
		}
	
		void OnMouseDrag ()
		{
				doMouseDrag ();
		}

		void doMouseDrag ()
		{
				if (draggable && !disabled) {
						Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
						transform.position = curPosition;
						SetDragging (true);
						gameObject.transform.localScale = Vector3.one / 2f * 1.1f;
				}
		}

		void OnMouseUp ()
		{
				doMouseUp ();

		}

		void doMouseUp ()
		{
				if (!disabled) {
						GetComponent<SpriteRenderer> ().sortingLayerName = "Deck Tile";
						gameObject.transform.localScale = Vector3.one / 2f;
						float radius = this.transform.localScale.x / 2;
						Vector2 point = new Vector2 ();
						point.x = this.transform.position.x + radius;
						point.y = this.transform.position.y + radius;
						Collider2D[] colliders = Physics2D.OverlapCircleAll (point, radius, Physics.AllLayers, -Mathf.Infinity, Mathf.Infinity);
						Vector3 position = Vector3.zero;
						foreach (Collider2D collider in colliders) {
								bool notThis = collider.gameObject != this.gameObject;
								bool isArrow = collider.gameObject.GetComponent<Arrow> () != null;
								if (notThis && isArrow) {
										position = collider.transform.position;
										gameObject.transform.position = position;
										gameController.SetCurrentX (Mathf.RoundToInt (position.x / TileMap.tileSize) + 4);
										gameController.SetCurrentY (Mathf.RoundToInt (position.y / TileMap.tileSize) + 4);
										Arrow arrow = collider.gameObject.GetComponent<Arrow> ();
										gameController.SetDirection (arrow.GetDirection ());
										this.draggable = false;
										SetHit (true);
								}
						}
						SetReleased (true);
						if (!IsHit ()) {
								gameObject.transform.position = slotPosition;
						}
			
				}
		}


		void OnMouseDown ()
		{
				doMouseDown ();
		}  

		void doMouseDown ()
		{
				if (draggable && !disabled) {
						parent.SetSelected (this);
						GetComponent<SpriteRenderer> ().sortingLayerName = "Top Layer";
						screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);			
						offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				}
		}

		public Vector3 GetSlotPosition ()
		{
				return slotPosition;
		}
	
		public void SetSlotPosition (Vector3 slotPosition)
		{
				this.slotPosition = slotPosition;
		}

		public void SetDragging (bool dragging)
		{
				gameController.SetDragging (dragging);
				if (dragging) {
						gameController.SetDraggingTile (this);
						this.transform.DetachChildren ();
				} else {
						gameController.SetDraggingTile (null);
				}
				
		}

		public void SetReleased (bool released)
		{
				this.released = released;
		}

		public bool IsReleased ()
		{
				return released;
		}


		public void SetHit (bool hit)
		{
				this.hit = hit;
				DeckTile[] dts = GameObject.FindObjectsOfType<DeckTile> ();
				foreach (DeckTile dt in dts) {
						dt.disabled = true;
						dt.draggable = false;
				}
		}
				
		public bool IsHit ()
		{
				return hit;
		}

		public int GetSlotIndex ()
		{
				return slotIndex;
		}

		public void SetSlotIndex (int index)
		{
				this.slotIndex = index;
		}

	
		public string Serialize ()
		{
				int rot = Mathf.FloorToInt (gameObject.transform.rotation.eulerAngles.z);
				Tile tile = gameObject.GetComponent<Tile> ();
				return Tile.getTileString (tile.type) + ":" + rot + ":" + slotIndex;
		}
	
		public static DeckTile Deserialize (string tileString)
		{
				GameObject tile = null;
				DeckTile dt = null;
				if (tileString != null) {
						string[] split = tileString.Split (':');
						tile = Resources.Load<GameObject> ("Tiles/" + split [0]);
						if (tile == null) {
								GameObject[] tilePrefabs = Resources.LoadAll<GameObject> ("Tiles/");
								GameObject tilePrefab = tilePrefabs [Random.Range (0, tilePrefabs.Length)];
								tile = Instantiate (tilePrefab) as GameObject;
								tile.transform.rotation = PlayerDeck.GetUpRandomRotation ();
				
						} else {
								tile = Instantiate (tile) as GameObject;
								tile.transform.rotation = Tile.GetQuaternion (int.Parse (split [1]));
						}
						dt = tile.AddComponent<DeckTile> ();
						dt.SetSlotIndex (int.Parse (split [2]));
				}

				return dt;
		}

		public void MoveToRandomLocation ()
		{
				cpuMoving = true;
				Arrow[] arrows = GameObject.FindObjectsOfType<Arrow> ();
				endLocation = arrows [Random.Range (0, arrows.Length)].transform.position;
				if (draggable && !disabled) {
						parent.SetSelected (this);
						GetComponent<SpriteRenderer> ().sortingLayerName = "Top Layer";
				}
				// doMouseDown ();
				// get random location
				// set move to 
				// on arrival set release
		}

		void Update ()
		{
				if (cpuMoving && !Mathf.Approximately (gameObject.transform.position.magnitude, endLocation.magnitude)) {
						if (draggable && !disabled) {
								SetDragging (true);
								gameObject.transform.localScale = Vector3.one / 2f * 1.1f;
						}
						MoveTile ();
				} else if (cpuMoving && Mathf.Approximately (gameObject.transform.position.magnitude, endLocation.magnitude)) {
						cpuMoving = false;
						doMouseUp ();
				}
		}

		void MoveTile ()
		{
				gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, endLocation, 1 / (duration * (Vector3.Distance (gameObject.transform.position, endLocation))));
		}

}
