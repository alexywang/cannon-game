using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
	public static GhostSpawner Instance = null;
	int ghostCount = 4;
	public GameObject ghostPrefab;
	// Start is called before the first frame update
	void Start()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);

		InitSpawn();
	}

	void InitSpawn()
	{
		for(int i = 0; i < ghostCount; i++)
		{
			SpawnGhost();
		}
	}
	public void SpawnGhost()
	{
		Instantiate(ghostPrefab);
	}
}
