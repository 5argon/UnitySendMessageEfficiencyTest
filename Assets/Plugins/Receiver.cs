using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {

	public static int checker = 0;
	public static void ResetChecker()
	{
		checker = 0;
	}

	public void OtherMethods1()
	{
	}

	public void OtherMethods2()
	{
	}

	public void OtherMethods3()
	{
	}

    public void F(string message)
	{
		checker++;
		//Debug.Log("Call Success");
		//Debug.Log(checker);
	}

    public void ReceiverFunction(string message)
	{
		checker++;
		//Debug.Log("Call Success");
		//Debug.Log(checker);
	}

    public void OtherMethods4()
	{
	}

}
