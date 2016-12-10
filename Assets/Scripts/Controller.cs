using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour
{
    public GameObject blue_attack;
    public GameObject blue_defence;
    public GameObject red_attack;
    public GameObject red_defence;
    public GameObject ball;
    public GameObject temp;
    public GameObject red_score_obj;
    public GameObject blue_score_obj;
    bool kick = false;
    bool translate = false;
    Quaternion[] rot = new Quaternion[4]; 
    private float rotation_count = 0;
    private float translation_count = 0;
    private float prev_time;
    private float current_time;
    private float ball_x_step_size = 12F;
    private float ball_y_step_size = 12F;
    private int rod_index,scorered,scoreblue,ball_prevx, ball_prevy;
    private int ball_x_mapping,ball_y_mapping;
    private int ball_x, ball_y; // the coordinates of the ball should be changed with the input.
    public Text red_score;
    public Text blue_score;
    Vector3 ball_postion;
    private float input_check_freq = 1; // this is the freq by which the controller checks for the input, 1 is 1 second
    private float kick_animation_speed = 300 * Time.deltaTime;
    private float translate_animation_speed = 200 * Time.deltaTime;
    // Use this for initialization
    void Start()
    {
        load_objects();
        original_rotations();
        prev_time = Time.time;
        scorered = 1;
        scoreblue = 0;
        red_score = GetComponent<Text>();
        blue_score = GetComponent<Text>();
    }
    // just loading objects
    void load_objects()
    {
        blue_attack = GameObject.Find("TS_Blue_Team_Attack");
        blue_defence = GameObject.Find("TS_Blue_Team_Deffence");
        red_attack = GameObject.Find("TS_Red_Team_Attack");
        red_defence = GameObject.Find("TS_Red_Team_Defence");
        ball = GameObject.Find("TS_Ball");
        red_score_obj = GameObject.Find("Red_Score");
        blue_score_obj = GameObject.Find("Blue_Score");
    }
    // ignore
    void original_rotations()
    {
        rot[0] = blue_attack.transform.rotation;
        rot[1] = blue_defence.transform.rotation;
        rot[2] = red_attack.transform.rotation;
        rot[3] = red_defence.transform.rotation;
    }
    //rod kick, by changing the value of kick_animation_speed, you can control the speed of the kick animation
    void kick_animation(GameObject animate, int original_rotation_index)
    {

        if (rotation_count < 45)
        {
            if (animate == blue_attack || animate == blue_defence)
                animate.transform.Rotate(kick_animation_speed, 0, 0);
            else
                animate.transform.Rotate(-kick_animation_speed, 0, 0);
            rotation_count += kick_animation_speed;
        }
        else if (rotation_count < 135)
        {
            if (animate == blue_attack || animate == blue_defence)
                animate.transform.Rotate(-kick_animation_speed, 0, 0);
            else
                animate.transform.Rotate(kick_animation_speed, 0, 0);
            rotation_count += kick_animation_speed;
        }
        else if (rotation_count > 135 && rotation_count < 180)
        {
            if (animate == blue_attack || animate == blue_defence)
                animate.transform.Rotate(kick_animation_speed, 0, 0);
            else
                 animate.transform.Rotate(-kick_animation_speed, 0, 0);
            rotation_count += kick_animation_speed;
        }
        else
        {
            rotation_count = 0;
            animate.transform.rotation = rot[original_rotation_index];
            kick = false;
        }
    }
    //translate a rod, by changing the value of translate_animation_speed, you can control the speed of the translation animation
    void translation(GameObject translate_obj, int up)
    {
        if (up == 1 && translation_count < 10)
        {
            translate_obj.transform.Translate(translate_animation_speed, 0, 0);
            translation_count += translate_animation_speed;
        }
        if (up == 0 && translation_count < 10)
        {
            translate_obj.transform.Translate(-translate_animation_speed, 0, 0);
            translation_count += translate_animation_speed;
        }
        if (translation_count >= 10)
        {
            translate = false;
            translation_count = 0;
        }

    }
    //translate the ball (ignore)
    void translate_ball(int y, int x)
    {
        ball_postion.x = y * ball_y_step_size; 
        ball_postion.y = 0;
        ball_postion.z = x * ball_x_step_size;
        ball.transform.localPosition = ball_postion;
    }
    // inputs for translation and rotation of rods, should be changed to the inputs by integration team
    void check_inputs()
    {
        if (Input.GetKey("a"))  // blue attack kicks
        {
            temp = blue_attack;
            rod_index = 0;
            kick = true;
        }
        if (Input.GetKey("b")) // blue_defence kicks
        {
            temp = blue_defence;
            rod_index = 1;
            kick = true;
        }
        if (Input.GetKey("d")) // red defence kicks
        {
            temp = red_defence;
            rod_index = 3;
            kick = true;
        }
        if (Input.GetKey("1")) // blue attack moves up
        {
            temp = blue_attack;
            rod_index = 1;
            translate = true;
        }
        if (Input.GetKey("2")) // blue attack moves down
        {
            temp = blue_attack;
            rod_index = 0;
            translate = true;
        }
        if (Input.GetKey("3")) // blue defence moves up
        {
            temp = blue_defence;
            rod_index = 1;
            translate = true;
        }
        if (Input.GetKey("4")) // blue defence moves down
        {
            temp = blue_defence;
            rod_index = 0;
            translate = true;
        }
        if (Input.GetKey("7")) // red defence moves up
        {
            temp = red_defence;
            rod_index = 1;
            translate = true;
        }
        if (Input.GetKey("8")) // red defence moves down
        {
            temp = red_defence;
            rod_index = 0;
            translate = true;
        }
            
       

    }

    // Update is called once per frame
    void Update()
    {
        current_time = Time.time;
        if (current_time >= prev_time + 1)
        {
            prev_time = current_time;
            check_inputs();
            ball_y = 3;
            ball_x = 5;
            ball_y_mapping = ball_y - 3;
            ball_x_mapping = -ball_x + 5;
            translate_ball(ball_y_mapping, ball_x_mapping);
          
        }
        if (kick)
            kick_animation(temp, rod_index);
        if (translate)
            translation(temp, rod_index);


    }
}

