using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
	[HideInInspector]
	public Dijkstra.Graph graph;

	[Serializable]
	public struct GraphConnection {
		public GameObject node1;
		public GameObject node2;
	};
	public GraphConnection[] connections;

	[HideInInspector]
	public List<GameObject> allNodes;

	public Material defaultMaterial;
	public Material startMaterial;
	public Material goalMaterial;
	public Material pathMaterial;

	public void SetNodes(GameObject startNode, GameObject goalNode) {
		foreach (GameObject node in allNodes) {
			node.GetComponent<MeshRenderer>().material = defaultMaterial;
		}

		startNode.GetComponent<MeshRenderer>().material = startMaterial;
		goalNode.GetComponent<MeshRenderer>().material = goalMaterial;
	}

	public void SetPath(List<Dijkstra.Connection> path) {
		for (int i = 0; i < path.Count - 1; i++) {
			path[i].toNode.GetComponent<MeshRenderer>().material = pathMaterial;
		}
	}

	public void SetupGraph(float blueCost) {
		allNodes = new List<GameObject>();
		List<Dijkstra.Connection> connections = new();

		foreach (GraphConnection connection in this.connections) {
			float distance = Vector3.Distance(connection.node1.transform.position, connection.node2.transform.position);
			float blue;

			if (connection.node2.GetComponent<CostNode>().blue) blue = blueCost;
			else blue = 1f;
			connections.Add(new Dijkstra.Connection() {
				cost = connection.node2.GetComponent<CostNode>().cost * distance * blue,
				fromNode = connection.node1,
				toNode = connection.node2,
			});
			
			if (connection.node1.GetComponent<CostNode>().blue) blue = blueCost;
			else blue = 1f;
			connections.Add(new Dijkstra.Connection() {
				cost = connection.node1.GetComponent<CostNode>().cost * distance * blue,
				fromNode = connection.node2,
				toNode = connection.node1,
			});

			if (!allNodes.Contains(connection.node1)) allNodes.Add(connection.node1);
			if (!allNodes.Contains(connection.node2)) allNodes.Add(connection.node2);
		}

		graph = new() {
			connections = connections.ToArray(),
		};
	}

	void OnDrawGizmosSelected()
	{
		foreach (GraphConnection connect in connections)
		{
			// Draws a blue line from this transform to the target
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(connect.node1.transform.position, connect.node2.transform.position);
		}
	}
}
