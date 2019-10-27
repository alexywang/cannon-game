using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Collidable
{
	public float shotAngle = 45;
	public bool ready = false;
	float gravity = 9.8f;
	float velocity = 13f;
	public float vx;
	public float vy;
	float restitution = 0.5f;
	float elapsedTime;
	float windY;
	public Material hitboxMaterial;

	public override void MyOnCollisionEnter(MyCollision c)
	{
		if (c.gameObject.GetComponent<Ground>())
		{
			Debug.Log("I hit the ground");
			DestroySelf();
		}

		if (c.gameObject.GetComponent<Stonehenge>())
		{
			Stonehenge stonehenge = c.gameObject.GetComponent<Stonehenge>();
			if (transform.position.y > c.gameObject.transform.position.y+0.2f) // Top collision
			{
				Debug.Log("Top collision");
				vy = Mathf.Abs(vy - (gravity * elapsedTime)) * restitution;
				elapsedTime = 0;
			}
			else // Side collision
			{
				vx = -vx * restitution;
			}
		}

	}

	// Start is called before the first frame update
	void Start()
    {
		// Calculate velocity components
		vx = velocity * Mathf.Cos(shotAngle * Mathf.PI / 180);
		vy = velocity * Mathf.Sin(shotAngle * Mathf.PI / 180);
		
		// Give myself a hitbox
		LineRenderer hitbox = gameObject.AddComponent<LineRenderer>();
		hitbox.startWidth = 0.1f;
		hitbox.endWidth = 0.1f;
		hitbox.positionCount = 5;
		hitbox.SetPosition(0, new Vector3(transform.position.x - 0.6f, transform.position.y + 0.6f, 0));
		hitbox.SetPosition(1, new Vector3(transform.position.x + 0.6f, transform.position.y + 0.6f, 0));
		hitbox.SetPosition(2, new Vector3(transform.position.x + 0.6f, transform.position.y - 0.6f, 0));
		hitbox.SetPosition(3, new Vector3(transform.position.x - 0.6f, transform.position.y - 0.6f, 0));
		hitbox.SetPosition(4, new Vector3(transform.position.x - 0.6f, transform.position.y + 0.6f, 0));
		hitbox.material = hitboxMaterial;
		// Find out wind level
		windY = GameObject.Find("Stonehenge").transform.position.y;

		base.Start();


	}

	// Update is called once per frame
	void Update()
    {
		UpdatePosition();
		CheckOutsideScreen();
		base.Update();

	}


	// Update the position
	void UpdatePosition()
	{
		if (ready)
		{
			float windForce = 0;
			if (transform.position.y >= windY) // Apply wind force
			{
				windForce = WindManager.Instance.magnitude;
			}

			// Allow the ball to roll if colliding with another with 0 vy
			bool roll = false;
			elapsedTime += Time.deltaTime;
			foreach(Collidable c in collidingWith)
			{
				if(Mathf.Abs(vy) < 1)
				{
					roll = roll || true;
				}
			}

			if (roll)
			{
				transform.position += new Vector3(-(vx - windForce) * Time.deltaTime, 0, 0);

			}
			else
			{
				transform.position += new Vector3(-(vx - windForce) * Time.deltaTime, (vy - gravity * (elapsedTime)) * Time.deltaTime, 0);

			}

		}

	}

	// Destroy if outside of screen
	void CheckOutsideScreen()
	{
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		if(pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
		{
			DestroySelf();
		}
	}

	void DestroySelf()
	{
		CollisionManager.Instance.DeregisterCollidable(this);
		Destroy(gameObject);
	}

	// Fade away and destroy self when hit by ghost
	public void GhostDestroy()
	{
		StartCoroutine(Fade());
	}
	IEnumerator Fade()
	{
		Renderer r = GetComponent<Renderer>();
		while(r.material.color.a > 0)
		{
			r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, r.material.color.a - 0.1f);
			yield return new WaitForSeconds(0.1f);
		}
		DestroySelf();
	}

}
