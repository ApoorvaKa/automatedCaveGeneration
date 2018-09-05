using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;
	List<Vector3> vertices;
	List<int> traingles;

	public void GenerateMesh(int[,] map, float squareSize){
		squareGrid = new SquareGrid (map, squareSize);

		for(int x = 0; x < squareGrid.squares.GetLength(0); x ++){
			for(int y = 0; y< squareGrid.squares.GetLength(1); y++){
				TriangulateSquare(squareGrid.squares[x,y];
			}
		}
	}

	void TriangulateSquare(Square square){
		switch (square.configuration) {
		
		case 0:
			break;

		// 1 Points:
		case 1:
			MeshFromPoints (square.centerBottom, square.bottomLeft, square.centerLeft);
			break;
		case 2:
			MeshFromPoints (square.centerRight, square.bottomRight, square.centerBottom);
			break;
		case 4:
			MeshFromPoints (square.centerTop, square.topRight, square.centerRight);
			break;
		case 8:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerLeft);
			break;

		//2 Points:
		case 3:
			MeshFromPoints (square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 6:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
			break;
		case 9:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints (square.topLeft, square.centerRight, square.centerRight, square.centerLeft);
			break;

		//Diagonal 2 points so it has an extra two points
		case 5:
			MeshFromPoints (square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
			break;
		case 10:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

		//3 points
		case 7:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 11:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.bottomRight, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

		//4 points
		case 12:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			break;					
		}
	}

	// recieving some points
	void MeshFromPoints(params Node[] points) {
		AssignVertices(points);
	}

	void AssignVertices(Node[] points){
		for(int i = 0; i< points.Length; i ++){
			if(points[i].vertexIndex == -1){
				points[i].vertexIndex = vertices.Count;
				vertices.Add(points[i]);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c){
		
	}



	void OnDrawGizmos() {
		if (squareGrid != null) {
			for (int x = 0; x < squareGrid.squares.GetLength (0); x++) {
				for (int y = 0; y < squareGrid.squares.GetLength (1); y++) {

					Gizmos.color = (squareGrid.squares [x, y].topLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topLeft.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].topRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topRight.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomLeft.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomRight.position, Vector3.one * .4f);

					Gizmos.color = Color.grey;
					Gizmos.DrawCube (squareGrid.squares [x, y].centerTop.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerBottom.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerRight.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerLeft.position, Vector3.one * .15f);
		
				}
			}
		}
	}

	//will store array of squares
	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(int[,] map, float squareSize){
			int nodeCountX = map.GetLength (0);
			int nodeCountY = map.GetLength (1);
			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for(int x = 0; x< nodeCountX; x++){
				for(int y = 0; y< nodeCountY; y++){
					Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos,map[x,y] == 1, squareSize);
				}
			}

			squares = new Square[nodeCountX - 1, nodeCountY - 1];
			for(int x = 0; x< nodeCountX-1; x++){
				for(int y = 0; y< nodeCountY - 1; y++){
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes [x+1,y+1], controlNodes [x+1,y], controlNodes [x,y]);
				}
			}
		}
	}

	public class Square {
		
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centerTop, centerRight, centerBottom, centerLeft;
		public int configuration;

		public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft) {
			topLeft = _topLeft;
			topRight = _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;

			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;

			//Configuration will show the nodes that are on
			if(topLeft.active)
				configuration += 8;
			if (topRight.active)
				configuration +=4;
			if(bottomRight.active)
				configuration +=2;
			if (bottomLeft.active)
				configuration += 1;

		}
	}

	public class Node{
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 _pos){
			position = _pos;
		}
	}

	public class ControlNode : Node {

		//active is wall not active is a cave
		public bool active;
		public Node above, right;

		public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) {
			active = _active;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize/2f);
		}
			
	}
}
