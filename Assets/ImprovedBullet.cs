using UnityEngine;
using System.Collections;

public class ImprovedBullet : MonoBehaviour 
{
    //RigidBody version of bullet
	private Player player;
	public float damage , lifeTime, speed;
	//DO NOT REMOVE [speed], used by RigidSentry.cs

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
        //limits th bullet's lifetime in flight
        Destroy(this.gameObject, lifeTime);
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		//we hit a smol rock, destroy it
		if(col.tag == "asteroid")
			Destroy (this.gameObject);
        //we hit the player, hurt it
        if (col.tag == "Player")
        {
            player.takeDamage(damage);
            Destroy(this.gameObject);
        }
        if (col.tag == "medium" || col.tag == "large")
        {
            Destroy(this.gameObject);
        }

    }
		
}
