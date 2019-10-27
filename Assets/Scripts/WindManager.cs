using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
	public static WindManager Instance = null;

	float[] magnitudeList = { 0.25f, 0.5f, 1f, 1.5f};
	public float magnitude; // Magnitude of wind accessible at all times
	GameObject stonehenge; // To tell what y-level wind should be applied to

    // Start is called before the first frame update
    void Start()
    {
		if (Instance == null) Instance = this;
		else Destroy(this);

		StartCoroutine(SetMagnitude());
    }

	// Coroutine for selecting new wind
	IEnumerator SetMagnitude()
	{
		int[] directions = { -1, 1 };
		while (true)
		{
			int d = Random.Range(0, directions.Length);
			int m = Random.Range(0, magnitudeList.Length);
			magnitude = magnitudeList[m] * directions[d];
			yield return new WaitForSeconds(2);
		}
	}

}