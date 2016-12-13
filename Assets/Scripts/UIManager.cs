using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CielaSpike;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Pipes;


public class UIManager : MonoBehaviour
{

    public Slider alphaslider;
    public Slider betaslider;
    public Slider epsilonslider;

    public Text alphatext;
    public Text betatext;
    public Text epsilontext;

	public Text HostIptext;
	public Text myIp;


    public Image loadingimg;
    public GameObject PlayMenu;

	public void Start()
	{
		myIp.text = GetLocalIPAddress ();
	}

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
	private void ConnectWithImplementation()
	{
		SharedObjects.AgentProcess = System.Diagnostics.Process.Start(@"Implementation.exe");
		SharedObjects.PipeUpServer = new NamedPipeServerStream(SharedObjects.UP_PIPE);



		SharedObjects.PipeUpServer.WaitForConnection();
		SharedObjects.PipeDownServer = new NamedPipeServerStream(SharedObjects.DOWN_PIPE);

		SharedObjects.PipeDownServer.WaitForConnection();
		SharedObjects.Reader = new StreamReader(SharedObjects.PipeUpServer);
		SharedObjects.Writer = new StreamWriter(SharedObjects.PipeDownServer);
	}
	public  string GetLocalIPAddress()
	{
		return Network.player.ipAddress.ToString();
	}
    public void HostGame(Animation anim1)
    {
        anim1.Play("loading", PlayMode.StopAll);
        PlayMenu.SetActive(false);
		loadingimg.gameObject.SetActive(true);
		this.StartCoroutineAsync(InitHostGame());
    }

	private IEnumerator InitHostGame()
	{
		SharedObjects.Host = true;
		ConnectWithImplementation();
        yield return Ninja.JumpToUnity;
        string alpha = alphatext.ToString();
        string beta = betatext.ToString();
        string epsilon = epsilontext.ToString();
        yield return Ninja.JumpBack;
        SharedObjects.Writer.Write("RED " + alpha + " " + beta + " " + epsilon);
		SharedObjects.Writer.Flush();

		yield return Ninja.JumpToUnity;
		Debug.Log(GetLocalIPAddress());
		IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
		yield return Ninja.JumpBack;
		try
		{
			// Set the TcpListener on port 13000.
			Int32 port = 3000;



			SharedObjects.TcpServer = new TcpListener(localAddr, port);

			// Start listening for client requests.
			SharedObjects.TcpServer.Start();

			Debug.Log("Waiting for a connection... ");

			// Perform a blocking call to accept requests.
			// You could also user server.AcceptSocket() here.
			SharedObjects.TcpClient = SharedObjects.TcpServer.AcceptTcpClient();
			Debug.Log("Connected!");


            // Get a stream object for reading and writing

            SharedObjects.TcpStream = SharedObjects.TcpClient.GetStream();
        }
		catch (SocketException e)
		{
			Debug.Log("SocketException: " + e);
			yield break;
		}

		yield return Ninja.JumpToUnity;
		LoadLevel();
		yield break;
	}

    public void JoinGame(Animation anim1)
    {
        anim1.Play("loading", PlayMode.StopAll);
        PlayMenu.SetActive(false);
        loadingimg.gameObject.SetActive(true);
		Debug.Log ("Ip is: " + getHostIp());
		this.StartCoroutineAsync(InitGuestGame());
    }

	private IEnumerator InitGuestGame()
	{
		SharedObjects.Host = false;
		ConnectWithImplementation();
		SharedObjects.Writer.Write("BLUE");
		SharedObjects.Writer.Flush();
		try
		{
			Int32 port = 3000;
            //String localIPAddress;
            SharedObjects.TcpClient = new TcpClient(getHostIp(), port);
			SharedObjects.TcpStream = SharedObjects.TcpClient.GetStream();
		}
		catch (SocketException e)
		{
			Debug.Log("SocketException: " + e);
			yield break;
		}

		yield return Ninja.JumpToUnity;
		LoadLevel();
		yield break;
	}

    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

	public string getHostIp()
	{
		return HostIptext.text.ToString ();
	}
	void OnApplicationQuit()
	{
		StopAllCoroutines();
		if (SharedObjects.Writer != null)
		{
			SharedObjects.Writer.Close();
			SharedObjects.Writer = null;
		}
		if (SharedObjects.Reader != null)
		{
			SharedObjects.Reader.Close();
			SharedObjects.Reader = null;
		}
		if (SharedObjects.AgentProcess != null)
		{
			SharedObjects.AgentProcess.Kill();
			SharedObjects.AgentProcess = null;
		}
		if (SharedObjects.PipeUpServer != null)
		{
			SharedObjects.PipeUpServer.Close();
			SharedObjects.PipeUpServer.Dispose();
			SharedObjects.PipeUpServer = null;
		}
		if (SharedObjects.PipeDownServer != null)
		{
			SharedObjects.PipeDownServer.Close();
			SharedObjects.PipeDownServer.Dispose();
			SharedObjects.PipeDownServer = null;
		}
		if (SharedObjects.TcpStream != null)
		{
			SharedObjects.TcpStream.Close();
			SharedObjects.TcpStream = null;
		}
		if (SharedObjects.TcpClient != null)
		{
			SharedObjects.TcpClient.Close();
			SharedObjects.TcpClient = null;
		}
		if (SharedObjects.TcpServer != null)
		{
			SharedObjects.TcpServer.Stop();
			SharedObjects.TcpServer = null;
		}
	}
}
public class SharedObjects
{
	public static NamedPipeServerStream PipeUpServer;
	public static NamedPipeServerStream PipeDownServer;
	public static StreamWriter Writer;
	public static StreamReader Reader;
	public static bool Host;
	public static TcpListener TcpServer;
	public static NetworkStream TcpStream;
	public static TcpClient TcpClient;
	public static System.Diagnostics.Process AgentProcess;
	public const string UP_PIPE = "ImplementationToIntegrationUpPipe";
	public const string DOWN_PIPE = "ImplementationToIntegrationDownPipe";
}