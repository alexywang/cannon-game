using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Collidable
{
	bool hit = false; // If the ghost was hit by a cannon ball or not
	float rightY; // Minimum y position where the ghost should start moving right
	float vx;
	float vy;
	float maxVelocity = 0.1f;
	// Velocity modifier from hitting cannon ball
	float vxMod = 0;
	float vyMod = 0;

	// Spawn bounds
	float xSpawnMin;
	float xSpawnMax;
	float ySpawnMax;
	float ySpawnMin;

	// Color once moved to right position
	Color color = Color.gray;

	public override void MyOnCollisionEnter(MyCollision c)
	{

		// Adjust the velocity of the cannon ball
		if (c.gameObject.GetComponent<CannonBall>())
		{
			if(gameObject.GetComponent<Renderer>().material.color.a > 0)
			{
				CannonBall cb = c.gameObject.GetComponent<CannonBall>();
				cb.vx -= vx * 30;
				cb.vy += vy * 30;
				cb.GhostDestroy();
			}
			
		}
	}

	// Start is called before the first frame update
	void Start()
    {

		// Choose a velocity
		maxVelocity = Random.Range(0.05f, maxVelocity);
		// Get min y where ghost should move right
		rightY = GameObject.Find("Stonehenge").transform.position.y + Random.Range(0f, 4f);
		ySpawnMax = rightY;
		ySpawnMin = GameObject.Find("Ground").transform.position.y;
		xSpawnMax = GameObject.Find("Stonehenge").transform.position.x - 4f;
		xSpawnMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
		base.Start();  

		transform.position = new Vector3(Random.Range(xSpawnMin, xSpawnMax), Random.Range(ySpawnMin, ySpawnMax), 0);

		// Make visible
		LineRenderer r = gameObject.GetComponent<LineRenderer>();
		r.material.color = color;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		HandleMove();
		CheckDestroy();
		base.Update();
    }
	 
	void HandleMove()
	{
		// Calculate velocity
		if(transform.position.y < rightY)
		{
			vx = 0;
			vy = maxVelocity;
		}
		else
		{
			vy = 0;
			vx = maxVelocity;
		}

		// Move
		Vector3 deltaPos = new Vector3(vx, vy, 0);
		transform.position += deltaPos;
		
	}

	void CheckDestroy()
	{
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		if(pos.x > 1)
		{
			DestroySelf();
		}
	}

	void DestroySelf()
	{
		GhostSpawner.Instance.SpawnGhost();
		CollisionManager.Instance.DeregisterCollidable(this);
		Destroy(gameObject);
	}
}
