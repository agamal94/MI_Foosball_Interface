using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    public Slider alphaslider;
    public Slider betaslider;
    public Slider epsilonslider;

    public Text alphatext;
    public Text betatext;
    public Text epsilontext;

	public Text HostIptext;


    public Image loadingimg;
    public GameObject PlayMenu;

    public void Update()
    {
        alphatext.text = alphaslider.value.ToString();
        betatext.text = betaslider.value.ToString();
        epsilontext.text = epsilonslider.value.ToString();

    }


    public void HideMenuRight(Animation anim1)
    {
        anim1.Play("LeaveMainRight", PlayMode.StopAll);
    }

    public void ShowMenuLeft(Animation anim1)
    {
        anim1.Play("ShowMenuLeft", PlayMode.StopAll);
    }

    public void EnterSettings(Animation anim1)
    {
        anim1.Play("EnterSettings", PlayMode.StopAll);
    }

    public void LeaveSettings(Animation anim1)
    {
        anim1.Play("LeaveSettings", PlayMode.StopAll);
    }

    public void EnterPlayMenu(Animation anim1)
    {
        anim1.Play("EnterPlayMenu", PlayMode.StopAll);
    }

    public void HideMenuLeft(Animation anim1)
    {
        anim1.Play("LeaveMainLeft", PlayMode.StopAll);

    }

    public void ShowMenuRight(Animation anim1)
    {
        anim1.Play("ShowMenuRight", PlayMode.StopAll);
    }

    public void LeavePlayMenu(Animation anim1)
    {
        anim1.Play("LeavePlayMenu", PlayMode.StopAll);
    }

    public void HostGame(Animation anim1)
    {
        anim1.Play("loading", PlayMode.StopAll);
        PlayMenu.SetActive(false);
        loadingimg.gameObject.SetActive(true);
		Invoke ("LoadLevel", 2);
    }




    public void JoinGame(Animation anim1)
    {
        anim1.Play("loading", PlayMode.StopAll);
        PlayMenu.SetActive(false);
        loadingimg.gameObject.SetActive(true);
		Debug.Log ("Ip is: " + getHostIp());
		Invoke ("LoadLevel", 2);
    }


    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

	public string getHostIp()
	{
		return HostIptext.text.ToString ();
	}

}
