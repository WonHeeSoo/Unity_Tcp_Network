﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Text;

public class NetworkServer : MonoBehaviour
{
	TcpListener server = null;
	int port = 13000;
	IPAddress localAddr = null;

	byte[] bytes = null;
	string data = null;

	Thread th = null;

	private void Awake()
	{
		localAddr = IPAddress.Parse("127.0.0.1");
		server = new TcpListener(localAddr, port);
		server.Start();

		bytes = new byte[256];
		data = null;
		Debug.Log("Awake");

		th = new Thread(new ThreadStart(StartServerFunc))
		{
			IsBackground = true
		};
	}

	private void Start()
	{
		//StartCoroutine("StartServer");
		Debug.Log("Start");
	}

	private void Update()
	{
		if (Input.GetButtonUp("Fire1"))
		{
			th.Start();
			//StartCoroutine("StartServer");
		}

	}

	void StartServerFunc()
	{
		Debug.Log("시작");

		Debug.Log("반복문 접속");
		

		while (true)
		{
			Debug.Log("Waiting for a connection...");
			TcpClient client = server.AcceptTcpClient();
			Debug.Log("Connectied!");

			data = null;

			NetworkStream stream = client.GetStream();

			int i;

			while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
			{
				//data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
				data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
				Debug.Log("Received : " + data);

				data = data.ToUpper();

				//byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
				byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);
				stream.Write(msg, 0, msg.Length);
				Debug.Log("Sent : " + data);
			}

			client.Close();
		}
	}
}
