using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	public static event ChangeLists OnChangeLists = (ol, cl) => { };
	public delegate void ChangeLists(HashSet<Node> openList, HashSet<Node> closedList);

	public static void GetPath(Point currentPos) {
		if (nodes == null)
			CreateNodes();

		var openList = new HashSet<Node>();
		var closedList = new HashSet<Node>();

		var currentNode = nodes[currentPos.X, currentPos.Y];

		openList.Add(currentNode);

		for (int xdelta = -1; xdelta <= 1; xdelta++)
			for (int ydelta = -1; ydelta <= 1; ydelta++) {
				if (xdelta == 0 && ydelta == 0)
					continue;

				var newx = currentPos.X + xdelta;
				var newy = currentPos.Y + ydelta;

				if (newx < 0 || newy < 0 || newx > colsCnt || newy > rowsCnt)
					continue;

				if (!LevelManager.Instance.Tiles[newx, newy].Walkable)
					continue;

				int gCost = 0;

				if (Math.Abs(xdelta - ydelta) == 1)
					gCost = 10;
				else
					gCost = 14;

				var neighbor = nodes[newx, newy];
				if (!openList.Contains(neighbor)) {
					openList.Add(neighbor);
				}

				neighbor.CalcValues(currentNode, gCost);
			}

		openList.Remove(currentNode);
		closedList.Add(currentNode);

		OnChangeLists(openList, closedList);
	}
}
