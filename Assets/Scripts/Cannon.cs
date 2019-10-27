using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	public GameObject projectilePrefab;
	GameObject curProjectile = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		HandleMove();
    }



	void HandleMove()
	{
		// Rotate barrel
		if (Input.GetKey(KeyCode.DownArrow) && transform.localEulerAngles.z >= 270)
		{
			transform.Rotate(Vector3.forward * 100 * Time.deltaTime);
			if (transform.localEulerAngles.z < 270) transform.localEulerAngles = new Vector3(0, 0, 359);

		}
		if(Input.GetKey(KeyCode.UpArrow) && transform.localEulerAngles.z >= 270)
		{
			transform.Rotate(Vector3.back * 100 * Time.deltaTime);
			if (transform.localEulerAngles.z < 270) transform.localEulerAngles = new Vector3(0, 0, 270);
		}

		// Fire projectile
		if (Input.GetKey(KeyCode.Space) && !curProjectile)
		{
			float angle = 360 - transform.localEulerAngles.z;
			Debug.Log(angle);
			curProjectile = FireProjectile(angle);
		}
	}

	// Instantiate and fire projectile
	GameObject FireProjectile(float angle)
	{
		GameObject proj = Instantiate(projectilePrefab);
		proj.GetComponent<CannonBall>().shotAngle = angle;
		proj.transform.position = transform.position;
		proj.GetComponent<CannonBall>().ready = true;
		return proj;
	}
}
