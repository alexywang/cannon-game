using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
	/**
	 * Put this on the parent object of multiple line renderers
	 **/
	// Default configurations
	int numOctaves = 3;
	float movement = 0.2f; // Displacement for the first octave
	float divideBy = 3; // Divide by x for next octave displacement

	LineRenderer[] lines;
    // Start is called before the first frame update
    void Start()
    {
		lines = gameObject.GetComponentsInChildren<LineRenderer>();
		foreach(LineRenderer line in lines)
		{
			AddNoise(line);
		}
    }

	void AddNoise(LineRenderer line)
	{
		bool horizontal = line.GetPosition(0).y == line.GetPosition(1).y;
		List<int> candidates = new List<int>();
		for(int i = 1; i < line.positionCount-1; i++)
		{
			candidates.Add(i);
		}
		// Keep adding noise to points until there are no more vertices or max octaves is reached
		int octave = 1;
		while(octave <= numOctaves)
		{
			if (candidates.Count == 0) // No candidates left for noise
			{
				return;
			}

			List<int> currPoints = new List<int>();
			
			// Choose octave points to add noise to
			for(int i = 0; i < Mathf.Min(candidates.Count, octave); i++)
			{
				int index = UnityEngine.Random.Range(0, candidates.Count);
				currPoints.Add(candidates[index]);
				candidates.RemoveAt(index);
			}

			// Add noise
			foreach(int point in currPoints)
			{
				Vector3 prevPos = line.GetPosition(point);
				Vector3 newPos;
				int up = UnityEngine.Random.Range(0, 2);
				float moveBy = movement / (divideBy * octave);
				if (up == 1) // Move in negative direction
				{
					moveBy *= -1;
				}

				if (horizontal) // Along y axis
				{
					newPos = new Vector3(prevPos.x, prevPos.y + moveBy, prevPos.z);
				}
				else // Along x axis
				{
					newPos = new Vector3(prevPos.x + moveBy, prevPos.y, prevPos.z);
				}

				line.SetPosition(point, newPos);

			}
			octave++;
		}
	}

    
}
