using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
	
		public static int size_x = 9;
		public static int size_y = 9;
		public static float tileSize = 1.1f;
		public static TileMap tileMap;

		void Awake ()
		{
				if (tileMap == null) {
						DontDestroyOnLoad (tileMap);
						tileMap = this;
				} else if (tileMap != this) {
						Destroy (gameObject);
				}
		}

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				BuildMesh ();
		}
	
		public void BuildMesh ()
		{
		
				int numTiles = size_x * size_y;
				int numTris = numTiles * 2;
		
				int vsize_x = size_x + 1;
				int vsize_y = size_y + 1;
				int numVerts = vsize_x * vsize_y;

				Vector3[] vertices = new Vector3[ numVerts ];
				Vector3[] normals = new Vector3[numVerts];
				Vector2[] uv = new Vector2[numVerts];
		
				int[] triangles = new int[ numTris * 3 ];

				int x, y;
				for (y=0; y < vsize_y; y++) {
						for (x=0; x < vsize_x; x++) {
								vertices [y * vsize_x + x] = new Vector3 (x * tileSize, y * tileSize, 0);
								normals [y * vsize_x + x] = Vector3.up;
								uv [y * vsize_x + x] = new Vector2 ((float)x / vsize_x, (float)y / vsize_y);
						}
				}
		
				for (y=0; y < size_y; y++) {
						for (x=0; x < size_x; x++) {
								int squareIndex = y * size_x + x;
								int triOffset = squareIndex * 6;
								triangles [triOffset + 0] = y * vsize_x + x + 0;
								triangles [triOffset + 1] = y * vsize_x + x + vsize_x + 0;
								triangles [triOffset + 2] = y * vsize_x + x + vsize_x + 1;
				
								triangles [triOffset + 3] = y * vsize_x + x + 0;
								triangles [triOffset + 4] = y * vsize_x + x + vsize_x + 1;
								triangles [triOffset + 5] = y * vsize_x + x + 1;
						}
				}

				Mesh mesh = new Mesh ();
				mesh.vertices = vertices;
				mesh.triangles = triangles;
				mesh.normals = normals;
				mesh.uv = uv;

				MeshFilter mesh_filter = GetComponent<MeshFilter> ();
				MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
				MeshCollider mesh_collider = GetComponent<MeshCollider> ();
		
				mesh_filter.mesh = mesh;
				mesh_collider.sharedMesh = mesh;
				mesh_renderer.enabled = false;
		}
	
	
}
