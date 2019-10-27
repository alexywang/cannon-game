using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Collidable
{
	public override void MyOnCollisionEnter(MyCollision c)
	{
		// Do nothing to myself on collisions
	}

	// Start is called before the first frame update
	void Start()
    {
		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
    }
}
