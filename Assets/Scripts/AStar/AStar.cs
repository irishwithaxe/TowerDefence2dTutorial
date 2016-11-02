using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AStar {

	private static Node[,] nodes;
	private static int colsCnt;
	private static int rowsCnt;

	private static void CreateNodes() {
		colsCnt = LevelManager.Instance.Tiles.GetLength(0);
		rowsCnt = LevelManager.Instance.Tiles.GetLength(1);

		nodes = new Node[colsCnt, rowsCnt];

		for (int r = 0; r < rowsCnt; r++)
			for (int c = 0; c < colsCnt; c++) {
				nodes[c, r] = new Node(LevelManager.Instance.Tiles[c, r]);
			}
	}

	public static event ChangeLists OnChangeLists = (ol) => { };
	public delegate void ChangeLists(HashSet<Node> openList);

	public static void GetPath(Point startPos) {
		if (nodes == null)
			CreateNodes();

		var openList = new HashSet<Node>();
		var start = nodes[startPos.X, startPos.Y];

		openList.Add(start);

		OnChangeLists(openList);

		for (int xdelta = -1; xdelta <= 1; xdelta++)
			for (int ydelta = -1; ydelta <= 1; ydelta++) {
				if (xdelta == 0 && ydelta == 0)
					continue;

				var newx = startPos.X + xdelta;
				var newy = startPos.Y + ydelta;

				if (newx < 0 || newy < 0 || newx > colsCnt || newy > rowsCnt)
					continue;

				if (!LevelManager.Instance.Tiles[newx, newy].Walkable)
					continue;

				var neighbor = nodes[newx, newy];
				if (!openList.Contains(neighbor)) {
					openList.Add(neighbor);
				}

				neighbor.CalcValues(start);

			}

		OnChangeLists(openList);
	}
}
