using UnityEngine;
using System.Collections;
using System;
using CielaSpike;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Globalization;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    // Use this for initialization
    public Animation Rod1Roll;
    public GameObject Rod1;

    FileStream logFile;
    TextWriter tw;

    public Animation Rod2Roll;
    public GameObject Rod2;

    public Animation Rod3Roll;
    public GameObject Rod3;

    public Animation Rod4Roll;
    public GameObject Rod4;

    public Animation[] RodRolls;


    public Animation BallKick;
    public GameObject Ball;

    public GameObject UIManager;


    public GameObject EscapeMenu;
    public string myTeam;

	public Text RedScore;
	public Text BlueScore;



    private string[] rodActions = { "1 NoAction", "2 NoAction", "3 NoAction", "4 NoAction" };
    private string[] lastStepRodActions = { "1 NoAction", "2 NoAction", "3 NoAction", "4 NoAction" };
    private Vector3 upVector = new Vector3(0.0f, 0.0f, -12.0f);
    Vector3 targetPosition1 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 targetPosition2 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 targetPosition3 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 targetPosition4 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 ballTargetPosition = new Vector3(0.0f, 0.0f, 0.0f);

    float marginX = (137.0f / 11.0f) / 2.0f;
    float marginY = (83.2f / 7.0f) / 2.0f;
    float stepX = (137.0f / 11.0f);
    float stepY = (83.2f / 7.0f);

    int rl = 5;
    int ud = 3;
	int reds = 0;
	int blues = 0;

    void Start() {

        targetPosition1 = Rod1.transform.position;
        targetPosition2 = Rod2.transform.position;
        targetPosition3 = Rod3.transform.position;
        targetPosition4 = Rod4.transform.position;
        ballTargetPosition = Ball.transform.position;
        print("MI Project Test");
        Rod1Roll = Rod1.GetComponent<Animation>();
        Rod2Roll = Rod2.GetComponent<Animation>();
        Rod3Roll = Rod3.GetComponent<Animation>();
        Rod4Roll = Rod4.GetComponent<Animation>();
        RodRolls = new Animation[4];
        RodRolls[0] = Rod1Roll;
        RodRolls[1] = Rod2Roll;
        RodRolls[2] = Rod3Roll;
        RodRolls[3] = Rod4Roll;

    }

    // Update is called once per frame
    void Update() {
        Rod1.transform.position = Vector3.Lerp(Rod1.transform.position, targetPosition1, Time.deltaTime * 10);
        Rod2.transform.position = Vector3.Lerp(Rod2.transform.position, targetPosition2, Time.deltaTime * 10);
        Rod3.transform.position = Vector3.Lerp(Rod3.transform.position, targetPosition3, Time.deltaTime * 10);
        Rod4.transform.position = Vector3.Lerp(Rod4.transform.position, targetPosition4, Time.deltaTime * 10);
        Ball.transform.position = Vector3.Lerp(Ball.transform.position, ballTargetPosition, Time.deltaTime * 10);
		ballMovement (rl, ud);

		checkInput ();


    }


    void RodMovement(int rodNumber, int direction) {
        switch (rodNumber)
        {
            case 1:
                if (direction == 1) {
                    targetPosition1 += upVector;
                } else if (direction == -1) {
                    targetPosition1 -= upVector;
                }
                break;
            case 2:
                if (direction == 1) {
                    targetPosition2 += upVector;
                } else if (direction == -1) {
                    targetPosition2 -= upVector;
                }
                break;
            case 3:
                if (direction == 1) {
                    targetPosition3 += upVector;
                } else if (direction == -1) {
                    targetPosition3 -= upVector;
                }
                break;
            case 4:
                if (direction == 1) {
                    targetPosition4 += upVector;
                } else if (direction == -1) {
                    targetPosition4 -= upVector;
                }
                break;
        }

    }

    void ballMovement(int x, int y) {
        ballTargetPosition = new Vector3((-1 * x * stepX - marginX) + 68.5f, 0, (-1 * y * stepY - marginY) + 41.6f);
    }
		

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowMenu()
    {
        EscapeMenu.SetActive(true);
    }

    public void HideMenu()
    {
        EscapeMenu.SetActive(false);
    }

	public void setScore(int red, int blue)
	{
		RedScore.text = red.ToString();
		BlueScore.text = blue.ToString();
	}

	public void checkInput()
	{

		if (Input.GetKeyDown(KeyCode.A))
		{
			RodRolls [0].Play ();
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			RodRolls [1].Play ();
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			RodRolls [2].Play ();
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			RodRolls [3].Play ();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			RodMovement (1, 1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			RodMovement (1, -1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			RodMovement (2, 1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			RodMovement (2, -1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			RodMovement (3, 1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			RodMovement (3, -1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			RodMovement (4, 1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			RodMovement (4, -1);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			ud++;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			ud--;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			rl++;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			rl--;
		}
		else if (Input.GetKeyDown(KeyCode.Equals))
		{
			setScore (reds, ++blues);
		}
		else if (Input.GetKeyDown(KeyCode.Minus))
		{
			setScore (reds, --blues);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			setScore (++reds, blues);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			setScore (--reds, blues);
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowMenu();
		}
		
	}

		
  
    
}
