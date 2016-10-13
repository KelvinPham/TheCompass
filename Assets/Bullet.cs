﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	GameObject player;
	public GameObject parent;
	public float speed;

    Vector3 parentDir;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
        parentDir = parent.transform.right;
    }
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += ( parentDir* Time.deltaTime * speed);
		StartCoroutine (destroy ());
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player" || col.tag == "small")
			Destroy (gameObject);
	}

	IEnumerator destroy()
	{
		yield return new WaitForSeconds (1);

		Destroy (gameObject);
	}
}
