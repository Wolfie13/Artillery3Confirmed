using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {
	private TerrainMeshGenerator generator;

	// Use this for initialization
	void Start () {
		this.generator = this.gameObject.AddComponent<TerrainMeshGenerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dirty) {
			this.generator.UpdateMesh();
			this.dirty = false;
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Projectile")) {
			ProjectileController pc = col.gameObject.GetComponent<ProjectileController>();
			DamageTerrain(col.gameObject.transform.position, pc.damage / 2);
			Destroy (pc.gameObject);
		}
	}

	public bool isLoaded()
	{
		return this.generator.isLoaded ();
	}

	public const int HORIZONTAL_RES = 1920;
	public const int VERTICAL_RES = 1392;

	private bool dirty = false;

	private static Color CLEAR = new Color(255, 255, 255, 0);

	private void DamageTerrain(Vector3 pos, float radius)
	{
		Vector3 p = TerrainMeshGenerator.WorldToTerrain(pos);
		int pX = (int) Mathf.Round (p.x);
		int pY = (int) Mathf.Round (p.y);
		int rad = (int) radius;

		int minX = Mathf.Min (pX - rad, 2);
		int maxX = Mathf.Max (pX + rad, HORIZONTAL_RES - 2);

		int minY = Mathf.Min (pY - rad, 2);
		int maxY = Mathf.Max (pY + rad, VERTICAL_RES - 2);

		Texture2D image = this.generator.getTerrainMapInstance ();

		for (int y = minY; y != maxY; y++) {
			//Set up the state machine used for the mesh generator.
			for (int x = minX; x != maxX; x++) {
				Vector3 vertPos = TerrainMeshGenerator.WorldToTerrain(new Vector3(x, y, 0));
				float dist = Vector3.Distance(vertPos, pos);
				if (dist > radius) {
					continue;
				}

				image.SetPixel(x, y, CLEAR);
			}
		}

		image.Apply ();
		

		this.dirty = true;
	}
}
