using System;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
	public class Connection
	{
		// The non-negative cost of this connection.
		public float cost = 1f;

		// The node that this connection came from.
		public GameObject fromNode;

		// The node that this connection leads to.
		public GameObject toNode;
	}

	// This structure is used to keep track of the information we need
	// for each node.
	public struct NodeRecord {
		public GameObject node;
		public Connection connection;
		public float costSoFar;
	}

	public class Graph
	{
		public Connection[] connections;

		// An array of connections outgoing from the given node.
		public List<Connection> getConnections(GameObject fromNode)
		{
			List<Connection> list = new();

			foreach (Connection item in connections) {
				if (item.fromNode.Equals(fromNode)) list.Add(item);
			}

			return list;
		}
		public List<Connection> getConnections(NodeRecord fromNode)
		{
			List<Connection> connections = getConnections(fromNode.node);
			connections.Remove(fromNode.connection);
			return connections;
		}
	}

	public class PathfindingList : List<NodeRecord>
	{
		public static PathfindingList operator +(PathfindingList list, NodeRecord record)
		{
			list.Add(record);
			return list;
		}

		public static PathfindingList operator -(PathfindingList list, NodeRecord record)
		{
			list.Remove(record);
			return list;
		}

		public bool Contains(GameObject item)
		{
			foreach (NodeRecord record in this) {
				if (record.node.Equals(item)) return true;
			}
			return false;
		}
		
		public NodeRecord Find(GameObject match)
		{
			foreach (NodeRecord record in this) {
				if (record.node.Equals(match)) return record;
			}
			throw new Exception("Match Not Found");
			//return this[0];
		}

		public NodeRecord SmallestElement()
		{
			NodeRecord selected = this[0];
			//if (Count == 1) return selected;

			for (int i = 1; i < Count; i++) {
				if (this[i].costSoFar < selected.costSoFar) selected = this[i];
			}
			return selected;
		}
	}

	public static List<Connection> PathfindDijkstra(Graph graph, GameObject start, GameObject goal)
	{
        // Initialize the record for the start node.
        NodeRecord current = new NodeRecord {
            node = start,
            connection = null,
            costSoFar = 0,
        };

		// Initialize the open and closed lists.
		PathfindingList open = new();
		open += current;
		PathfindingList closed = new();

		// Iterate through processing each node.
		while (open.Count > 0) {
			// Find the smallest element in the open list.
			current = open.SmallestElement();

			// If it is the goal node, then terminate.
			if (current.node == goal) break;

			// Otherwise get its outgoing connections.
			List<Connection> connections = graph.getConnections(current);

			// Loop through each connection in turn.
			foreach (Connection connection in connections) {
				// Get the cost estimate for the end node.
				GameObject endNode = connection.toNode;
				float endNodeCost = current.costSoFar + connection.cost;
				NodeRecord endNodeRecord;

				// Skip if the node is closed.
				if (closed.Contains(endNode)) continue;

				// .. or if it is open and we’ve found a worse route.
				else if (open.Contains(endNode)) {
					// Here we find the record in the open list
					// corresponding to the endNode.
					endNodeRecord = open.Find(endNode);
					if (endNodeRecord.costSoFar <= endNodeCost) continue;
				}

				// Otherwise we know we’ve got an unvisited node, so make a
				// record for it.
				else {
                    endNodeRecord = new NodeRecord {
                        node = endNode,
                    };
                }

				// We’re here if we need to update the node. Update the
				// cost and connection.
				endNodeRecord.costSoFar = endNodeCost;
				endNodeRecord.connection = connection;

				// And add it to the open list.
				if (!open.Contains(endNode)) open += endNodeRecord;
			}

			// We’ve finished looking at the connections for the current
			// node, so add it to the closed list and remove it from the
			// open list.
			open -= current;
			closed += current;
		}

		// We’re here if we’ve either found the goal, or if we’ve no more
		// nodes to search, find which.
		if (current.node != goal) {
			// We’ve run out of nodes without finding the goal, so there’s
			// no solution.
			Debug.Log("No Solution");
			return null;
		}
		Debug.Log("Found Solution");

        // Compile the list of connections in the path.
		List<Connection> path = new();

		// Work back along the path, accumulating connections.
		while (current.node != start) {
			path.Add(current.connection);
			current = closed.Find(current.connection.fromNode);
		}

		// Reverse the path, and return it.
		path.Reverse();
        return path;
	}
}
