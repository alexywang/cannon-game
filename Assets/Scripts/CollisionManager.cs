using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionManager : MonoBehaviour
{
	public static CollisionManager Instance = null;
	List<Collidable> collidables;

	KeyValuePair<Collidable, Collidable> collisionPairs;

    void Start()
    {
		if (Instance == null) Instance = this;
		else Destroy(this);

		collidables = new List<Collidable>();
		collisionPairs = new KeyValuePair<Collidable, Collidable>();
    }

    // Update is called once per frame
    void Update()
    {
		CheckCollisions();
    }

	// Check entire scene for collisions
	void CheckCollisions()
	{
		// Every pair of collidables
		for (int i = 0; i < collidables.Count - 1; i++)
		{
			for (int j = i+1; j < collidables.Count; j++)
			{
				Collidable member1 = collidables[i].gameObject.GetComponent<Collidable>();
				Collidable member2 = collidables[j].GetComponent<Collidable>();
				LineRenderer l1 = member1.GetComponent<LineRenderer>();
				LineRenderer l2 = member2.GetComponent<LineRenderer>();
				if(IsOverlapping(l1, l2))
				{					
					if(member1 && member2)
					{
						// Call collision method for collided, unless previous collision has not yet been resolved
						if(!member1.collidingWith.Contains(member2) && !member2.collidingWith.Contains(member1))
						{
							Debug.Log("Collision Detected " + member1.gameObject.name + " "  + member2.gameObject.name);

							member1.MyOnCollisionEnter(new MyCollision(member2.gameObject));
							member2.MyOnCollisionEnter(new MyCollision(member1.gameObject));
							member1.collidingWith.Add(member2);
							member2.collidingWith.Add(member1);
						}

					}
				}
				else 
				{
					member1.collidingWith.Remove(member2);
					member2.collidingWith.Remove(member1);
				}
			}
		}
	}

	// Check if two line renderers are overlapping
	bool IsOverlapping(LineRenderer l1, LineRenderer l2)
	{
		// Check overlap for all adjacent pairs of vertices
		for (int i = 0; i < l1.positionCount - 1; i++)
		{
			for (int j = 0; j < l2.positionCount - 1; j++)
			{ 
				// First Check
				Vector2 B = l2.GetPosition(j) - l2.GetPosition(j+1);
				Vector2 a0 = l2.GetPosition(j) - l1.GetPosition(i);
				Vector2 a1 = l2.GetPosition(j) - l1.GetPosition(i + 1);
				bool diff1 = false;

				if ( Vector2Cross(B, a0) * Vector2Cross(B, a1) <= 0)
				{
					diff1 = true;
				}
				
				B = l1.GetPosition(i) - l1.GetPosition(i+1);
				a0 = l1.GetPosition(i) - l2.GetPosition(j);
				a1 = l1.GetPosition(i + 1) - l2.GetPosition(j + 1);
				bool diff2 = false;

				if (Vector2Cross(B, a0) * Vector2Cross(B, a1) <= 0)
				{
					diff2 = true;
				}

				if(diff1 && diff2)
				{
					return true;
				}
			}
		}

		return false;
	}

	float Vector2Cross(Vector2 v, Vector2 u)
	{
		return v.x * u.y - v.y * u.x;
	}

	// Register the line segments of a collidable 
	public void RegisterCollidable(Collidable c)
	{
		collidables.Add(c);
		Debug.Log("Adding " + c);
	}

	public void DeregisterCollidable(Collidable c)
	{
		collidables.Remove(c);
		Debug.Log("Removing " + c);
	}
}

public class MyCollision
{
	public GameObject gameObject;
	public MyCollision(GameObject collided)
	{
		gameObject = collided;
	}
}

