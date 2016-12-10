using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlueScore : MonoBehaviour
{

    public static int bluescores;
    public Text text;
    private float input_check_freq = 1;
    private float prev_time;
    private float current_time;
    // Use this for initialization
    void Start()
    {
        prev_time = Time.time;
        bluescores = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        text.text = "" + bluescores;
    }
}
