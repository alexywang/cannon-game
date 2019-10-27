using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	float width;

    // Start is called before the first frame update
    void Start()
    {
		width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		MoveCloud();
    }

	void MoveCloud()
	{
		// Get wind magnitude
		float wind = WindManager.Instance.magnitude;
		Vector3 displacement = new Vector3(wind, 0, 0);
		transform.position += displacement * Time.deltaTime;

		// Check if wrapping is needed
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		if(pos.x < 0) // Left of edge
		{
			Vector3 right = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
			transform.position = new Vector3(right.x, transform.position.y, 0); // Send to right
		}else if(pos.x > 1)
		{
			Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)); // Send to left
			transform.position = new Vector3(left.x, transform.position.y, 0);
		}
	}

}
