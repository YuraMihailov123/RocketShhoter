using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyController : MonoBehaviour {
	public float[] DataX = new float[3];
	public float[] DataY= new float[3];
    public float[] velX = new float[3];
    public float[] velY = new float[3];
	public bool getP=false;
	public int count = 0;
    public float[] x_array=new float[100];
    public float[] y_array= new float[100];
    public Vector3[] xyz = new Vector3[100];
    public Vector3[] circle=new Vector3[36];
    public float t;
    public GameObject bullet,spawn;
    public Vector3 dirBull;
    public float speedBull;
    GameObject bullet_ = null;
    ///public GameObject[] Tool;
    ///
    public GameObject bb;
    public string nameTool="";
    void Start(){
        //Time.timeScale = 0.14f;
    }
    // Update is called once per frame
    void Update () {
        if (bullet_ != null)
        {
            bullet_.transform.position += (dirBull - bullet_.transform.position).normalized * speedBull * Time.deltaTime;
        }
        //DrawCircle();
        //DataX.Length
        if (getP) {
			Debug.Log (gameObject.name + "-:Rocket: "+ DataX[0] +","+DataY[0]);
			getP = false;
		}
	}
    public void Trajectory(double vx, double vy, float x0, float y0, double alpha) {
        float temp = (float)(vy * vy + 2 * -Physics.gravity.y * y0);
        t = (float)(-vy - Mathf.Sqrt(temp))/ Physics.gravity.y;

        for (int i = 0; i < x_array.Length; i++)
        {
            x_array[i] = (float)(x0 + vx * t * (i + 1) / 100);
            y_array[i] = (float)(y0 + vy * t * (i + 1) / 100 + Physics.gravity.y* ((t * (i + 1) / 100)* (t * (i + 1) / 100) )/ 2);
            xyz[i].x = x_array[i];
            xyz[i].y = y_array[i];
            xyz[i].z = 0;
        }
        GetComponent<LineRenderer>().positionCount = 100;
        GetComponent<LineRenderer>().SetPositions(xyz);
        Fire(t,y0,x0,vx,alpha);
    }
    public void DrawCircle()
    {
        int j = 0;
        for (float i = 0.0f; i < 2*Mathf.PI; i += Mathf.PI / 18)
        {
            if (j < 36)
            {
                circle[j].x = 24 + 25f * Mathf.Sin(i);
                circle[j].y = 25f * Mathf.Cos(i);
                circle[j].z = 0;
                j++;

            }
            Debug.Log(j+"-"+i);
        }

        Camera.main.GetComponent<LineRenderer>().positionCount = 36;
        Camera.main.GetComponent<LineRenderer>().SetPositions(circle);
    }
    public void Fire(float t,float y0, float x0, double vx, double a)
    {
        int v0 = 30;
        float y_max = y_array[0],y_m=0,x_max=0;
        float dist = 0,time=0;
        int index = 0;
        GameObject rocket = GameObject.Find("Rocket");
        for (int i = 1; i < y_array.Length; i++)
        {
            if (y_max < y_array[i])
            {
                y_max = y_array[i];
                index = i;
            }
        }
        x_max = x_array[index];
        if (y0 >= y_max )
        {
            y_m = y0 / 2;
            for (int i = 1; i < y_array.Length - 1; i++)
            {

                if (y_m > y_array[i] && y_m < y_array[i + 1])
                {
                    y_m = y_array[i];
                    index = i;
                }
            }
            x_max = x_array[index];
            //find y between two numbers of array i&i+1
            Debug.Log("1");
        }
        else if (y0 < y_max )
        {
            y_m = y_max;
            x_max = x_array[index];//main case when the rocket is under the tool. need to be f*kicng fixeddd
            Debug.Log("2");
        }      

        time = (float)(Mathf.Abs(x0-x_max) / vx);
        //Debug.Log(y0+"-"+y_m + ":" + x_max+"Time rocket:"+time+"velx"+vx+"full time"+t);
        dist = Vector3.Distance(spawn.transform.position, new Vector3(x_max,y_m,0));
        speedBull = (dist / time);
        Debug.Log(x_max + ":" + y_m + "->" + dist+"->"+speedBull+"->"+time+"Speed"+vx+"x0:"+x0+"y0"+y0);
        bullet_= Instantiate(bullet, spawn.transform.position, Quaternion.identity);
        //bullet_.GetComponent<Rigidbody2D>().AddForce(new Vector2(x_max, y_m)*15, ForceMode2D.Force);
        dirBull = new Vector3(x_max, y_m, 0);
        //if ((targetBox.position - transform.position).sqrMagnitude < 0.01f) Destroy(gameObject);
    }
}
