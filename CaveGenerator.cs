using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;

	void Start()
	{
		GenerateMap ();
	}

	//update will give a way to generate maps easily
	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			GenerateMap ();
		}
	}
		
	// calls randomFillMap and generates the map
	void GenerateMap()
	{
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (map, 1);
	}

	void RandomFillMap()
	{
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		// Fake Random number generator
		System.Random psuedoRandom = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
					// wall for the border of the cave map
				}
				else{
				map [x, y] = (psuedoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
				// 1 is a wall and 0 is the cave space
				
				}
			}
		}
	}

	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighborWallTiles = GetSurroundingWallCount (x, y);

				if (neighborWallTiles > 4)
					map [x, y] = 1;
				else if (neighborWallTiles < 4)
					map [x, y] = 0;
			}
		}
	}

	// 3x3 grid centered on gridX and grid Y
	int GetSurroundingWallCount(int gridX, int gridY){
		int wallCount = 0;
		for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++) {
			for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++) {
				// if statemnet checks that the value is within the map and will cause no errors
				if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height){
					// if statment makes sure that we are not looking at the original tiles
					if (neighborX != gridX || neighborY != gridY) {
					wallCount += map [neighborX, neighborY];
					}
				}
				else{
					wallCount ++;
				}
			}
		}
		return wallCount;
	}

	// This method draws everything based on the previour parameters from the program.	
	void OnDrawGizmos()
	{
	/*
		if (map != null)
		{
		
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Gizmos.color = (map[x,y] == 1)? Color.black:Color.white;
					Vector3 pos = new Vector3(-width/2 + x + .5f ,0, - height/2 + y+.5f);
					Gizmos.DrawCube(pos,Vector3.one);
				}
			}
		}
		*/
	
	}		

}
