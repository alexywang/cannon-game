using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Collidable
{

	// Start is called before the first frame update
	public void Start()
    {
		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
    }

	
	
	public override void MyOnCollisionEnter(MyCollision c)
	{
		Debug.Log(gameObject + " Collided with" + c.gameObject);
	}

}
