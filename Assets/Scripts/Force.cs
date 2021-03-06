﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Force : NetworkBehaviour {
	public InputField inpField,inpSpeed;
  
    public float angle;
    public float speed;
    public GameObject loseText;
    private Rigidbody2D rgb2;

    [SyncVar]
    public bool gamePlay = false;


    // Use this for initialization
    void Start () {
        Time.timeScale = 0.5f;
        rgb2 = GetComponent<Rigidbody2D>();
		rgb2.bodyType = RigidbodyType2D.Static;
        gamePlay = false;	

	}
	
	// Update is called once per frame
	void Update () {


        string a = inpField.text;
      
        if (a != "" && a!="-" )
        {
            angle = float.Parse(a);
            angle = angle * Mathf.PI / 180;
        }

        string b = inpSpeed.text;
    
        if (b != "")
        {
            speed = float.Parse(b);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
        if (gamePlay)
            transform.Rotate(0, 0, -Time.deltaTime * speed);


    }
    public void Play()
    {
        rgb2.bodyType = RigidbodyType2D.Dynamic;
        rgb2.AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed, ForceMode2D.Impulse);
        gamePlay = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Rocket", LoadSceneMode.Single);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Floor")
        {
            rgb2.bodyType = RigidbodyType2D.Static;
            gamePlay = false;
        }
        if (col.name == "Bullet(Clone)") 
        {
            loseText.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
