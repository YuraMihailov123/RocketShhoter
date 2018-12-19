using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyController : NetworkBehaviour {
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
    public float t,delay;
    public GameObject bullet,spawn;
    public Vector3 dirBull;
    public float speedBull = 40;
    GameObject bullet_ = null;
    ///public GameObject[] Tool;
    ///
    public GameObject bb;
    public string nameTool="";
    public bool firstFire=true;
    public GameObject[] toHide = new GameObject[4];
    public GameObject[] toAA = new GameObject[3];
    public InputField x, y;
    public GameObject toolSp;
    public GameObject[] tranTemp;
    public Transform[] tran;
    void Start(){
        GameObject.Find("NetworkObject").GetComponent<NetworkManager>().startPositions.Clear();
        //Transform tran=null;

        //tran.position = new Vector3(0, 26, 0);
        tranTemp = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < tranTemp.Length; i++)
        {
            tran[i] = tranTemp[i].transform;
            GameObject.Find("NetworkObject").GetComponent<NetworkManager>().startPositions.Add(tran[i]);
        }




        Debug.Log(GameObject.Find("NetworkObject").GetComponent<NetworkManager>().startPositions.Count);
        //Debug.Log(GameObject.Find("NetworkObject").GetComponent<NetworkManager>().startPositions[0].position);
        toAA[0] = GameObject.Find("x");
        toAA[1] = GameObject.Find("y");
        toAA[2] = GameObject.Find("spawn");
        toHide[0] = GameObject.Find("InputField");
        toHide[1] = GameObject.Find("InputField (1)");
        toHide[2] = GameObject.Find("Button");
        toHide[3] = GameObject.Find("Button (1)");
        firstFire = true;
        //Time.timeScale = 0.14f;
        speedBull = 40;
        if (isServer)
        {
            for (int i = 0; i < 4; i++)
                toHide[i].SetActive(true);

            for (int i = 0; i < 3; i++)
                toAA[i].SetActive(false);

        }
        else
        {
            for (int i = 0; i < 4; i++)
                toHide[i].SetActive(false);

            for (int i = 0; i < 3; i++)
                toAA[i].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update () {


        if (delay > 0) {
            delay -= Time.deltaTime;
        }
        if (delay < 0 && firstFire)
        {
            bullet_ = Instantiate(bullet, spawn.transform.position, Quaternion.identity);
            NetworkServer.Spawn(bullet_);
            firstFire = false;
        }
        //if(bullet_.transform.position.y==dirBull.y)
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
        float y_max = y_array[0],y_m=0,x_max=0;
        float dist = 0;
        int index = 0;
        GameObject rocket = GameObject.Find("Rocket");
        for (int i = 0; i < y_array.Length; i++)
        {
            if (y_max < y_array[i])
            {
                y_max = y_array[i];
                index = i;
            }
        }
        y_m = y_max;
        for (int i = 1; i < y_array.Length; i++)
        {
            y_m /= 2;
            for (int j = 0; j < y_array.Length - 1; j++)
            {
                if (y_m > y_array[j + 1] && y_m < y_array[j])
                {
                    Debug.Log("fuckinDuckin");
                    y_m = y_array[j];
                    x_max = x_array[j];
                }
            }
            dist = Vector3.Distance(spawn.transform.position, new Vector3(x_max, y_m, 0));
            float time_rocket = (float)(Mathf.Abs(x0 - x_max) / vx);
            float time_bullet = dist / speedBull;
            Debug.Log(x_max+"--");
            if (time_rocket >= time_bullet)
            {
                dirBull = new Vector3(x_max, y_m, 0);
                delay = time_rocket - time_bullet;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    public void SpawnTool()
    {
        if (isServer) return;

        float xt, yt;
        xt = float.Parse(x.text);
        yt = float.Parse(y.text);
        var temp = Instantiate(toolSp, new Vector3(xt, yt, 0), Quaternion.identity);

        //NetworkServer.Spawn(temp);
        //Network.Connect("localhost", 7777);
        //RegisterHostMessage msg = new RegisterHostMessage();

        //msg.x = xt;
        //msg.y = yt;
        //cl.Send(RegisterHostMsgId, msg);
    }

}
