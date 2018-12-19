using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tool : NetworkBehaviour{
    public MyController controller;
    public bool getData = false;
	public float timeDelay;
	public float x_t, y_t,velX,velY;

    //public Text test;
    void Start(){

        //toHide = GameObject.FindGameObjectsWithTag("Hide");
        //for (int i = 0;i<)

        //for (int i = 0; i < 3; i++)
            //toAA[i].SetActive(false);
        controller = GameObject.Find("Controller").GetComponent<MyController>();
        Debug.Log(isServer+gameObject.name);
        //test.text = "";
        //test.text = isLocalPlayer.ToString();
        if (isServer)
        {
            for (int i = 0; i < 4; i++)
                controller.toHide[i].SetActive(true);

            for (int i = 0; i < 3; i++)
                controller.toAA[i].SetActive(false);

        }
        else {
            for (int i = 0; i < 4; i++)
                controller.toHide[i].SetActive(false);

            for (int i = 0; i < 3; i++)
                controller.toAA[i].SetActive(true);
        }
		timeDelay = 0;
	}
	void Update(){
		
		if(timeDelay>0)
		timeDelay -= Time.deltaTime;
		if (getData) {
			if (timeDelay <= 0) {
				if (controller.count < 1) {
					controller.DataX [controller.count] = x_t;
					controller.DataY [controller.count] = y_t;
                    controller.velY[controller.count] = velY;
                    controller.velX[controller.count] = velX;
                    if(controller.nameTool=="")
                    controller.nameTool = gameObject.name;
					//controller.getP = true;
					controller.count++;
                    controller.Trajectory(velX, velY, x_t, y_t, Mathf.Atan(velY / velX) * 180 / Mathf.PI);

                    Debug.Log (gameObject.name + ":" + x_t + "," + y_t);
				} 
				timeDelay = 0.4f;

				//getData = false;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		getData=true;
		x_t = col.gameObject.transform.position.x;
		y_t = col.gameObject.transform.position.y;
        velX = col.GetComponent<Rigidbody2D>().velocity.x;
        velY = col.GetComponent<Rigidbody2D>().velocity.y;
    }
	void OnTriggerStay2D(Collider2D col){
		x_t = col.gameObject.transform.position.x;
		y_t = col.gameObject.transform.position.y;
        velX = col.GetComponent<Rigidbody2D>().velocity.x;
        velY = col.GetComponent<Rigidbody2D>().velocity.y;

    }
	void OnTriggerExit2D(Collider2D col){
		getData=false;
        if (controller.count >= 3)
        {
            for (int i = 0; i < 1; i++)
            {
                //Debug.Log("Res:" + controller.DataX[i] + "-" + controller.DataY[i] + "-> Speed: " + controller.velX[i] + "," + controller.velY[i] + "->Angle:" + Mathf.Atan(controller.velY[i] / controller.velX[i]) * 180 / Mathf.PI);
            }
        }
    }
}
