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
