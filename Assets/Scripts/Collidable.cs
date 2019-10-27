using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public abstract class Collidable : MonoBehaviour
{
	public HashSet<Collidable> collidingWith; // Keep track of objects that this object has not yet resolved their collision with yet
	Vector3 startingPos;
	LineRenderer line;
	public virtual void Start()
	{
		line = GetComponent<LineRenderer>();
		collidingWith = new HashSet<Collidable>();
		CollisionManager.Instance.RegisterCollidable(this);
		startingPos = gameObject.transform.position;
	}

	public virtual void Update()
	{
		MoveWithObject();
	}

	void MoveWithObject()
	{

		for (int i = 0; i < line.positionCount; i++)
		{
			Vector3 deltaPos = transform.position - startingPos;
			line.SetPosition(i, line.GetPosition(i) + deltaPos);
		}
		startingPos = transform.position;

	}


	public abstract void MyOnCollisionEnter(MyCollision c);

}
