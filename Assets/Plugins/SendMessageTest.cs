using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using AOT;

public class SendMessageTest : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void _NativeSendMessage(string gameObjectName, string methodName, string message);


    private static readonly string shortNameDeep = "D";
    private static readonly string shortNameShallow = "S";

    private static readonly string shortFunctionName = "F";
    private static readonly string normalFunctionName = "ReceiverFunction";

    private static readonly string normalName = "SendMessageReceiver";
    private static readonly string deep = "_d";
    private static readonly string shallow = "_s";

    private static readonly string shortMessage = "m";
    private static readonly string normalMessage = "437.32|324.84|5argon|44389,313";
    private static readonly string longMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In id pulvinar nisi, ac iaculis ipsum. Ut lectus turpis, fringilla sed velit ut, convallis pretium risus. Praesent interdum blandit bibendum. Ut venenatis risus diam, eget vulputate ex imperdiet non. Nunc mattis commodo dui, at lacinia leo posuere nec. Nullam sollicitudin dolor non neque euismod maximus. Maecenas molestie hendrerit luctus. Maecenas fermentum, lacus quis accumsan finibus, dolor nulla ultrices justo, sed auctor quam justo sit amet ipsum. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur ut rutrum magna. Suspendisse potenti. Pellentesque hendrerit neque lacus, vel viverra nisi condimentum ut.";

    Stopwatch stopwatch;

    [DllImport("__Internal")]
    private static extern void _PointerHackTest(ActionDelegate actionDelegate, string message);

    static int counter;

    [MonoPInvokeCallback(typeof(ActionDelegate))]
    public static void PointerHackTargetStatic(string message)
    {
        counter++;
    }

    public void PointerHackTarget(string message)
    {
        counter++;
    }

    public delegate void ActionDelegate(string message);

    public void StartPointerHackTest()
    {
        UnityEngine.Debug.Log("Stopwatch Frequency : " + Stopwatch.Frequency);

        stopwatch = new Stopwatch();

        counter = 0;
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < 5000000; i++)
        {
            _PointerHackTest(PointerHackTargetStatic, longMessage);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log(counter);
        UnityEngine.Debug.Log("Time for POINTER : " + (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency));

        counter = 0;
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < 5000000; i++)
        {
            _NativeSendMessage(gameObject.name,"PointerHackTarget", longMessage);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log(counter);
        UnityEngine.Debug.Log("Time for UNITY SEND MESSAGE: " + (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency));

    }

    public void StartTest()
    {
        UnityEngine.Debug.Log("Stopwatch Frequency : " + Stopwatch.Frequency);
        //Test(shortNameShallow,shortFunctionName,shortMessage);
        //UnityEngine.Debug.Log(Receiver.checker);
        StartCoroutine(StartTestRoutine());
    }

    IEnumerator StartTestRoutine()
    {
        stopwatch = new Stopwatch();
        yield return BothTest(shortNameShallow,shortFunctionName,shortMessage);
        yield return BothTest(shortNameShallow,normalFunctionName,shortMessage);
        yield return BothTest(shortNameShallow,shortFunctionName,normalMessage);
        yield return BothTest(shortNameShallow,normalFunctionName,normalMessage);
        yield return BothTest(shortNameShallow,shortFunctionName,longMessage);
        yield return BothTest(shortNameShallow,normalFunctionName,longMessage);
        yield return BothTest(normalName+shallow,shortFunctionName,shortMessage);
        yield return BothTest(normalName+shallow,normalFunctionName,shortMessage);
        yield return BothTest(normalName+shallow,shortFunctionName,normalMessage);
        yield return BothTest(normalName+shallow,normalFunctionName,normalMessage);
        yield return BothTest(normalName+shallow,shortFunctionName,longMessage);
        yield return BothTest(normalName+shallow,normalFunctionName,longMessage);
        yield return BothTest(shortNameDeep,shortFunctionName,shortMessage);
        yield return BothTest(shortNameDeep,normalFunctionName,shortMessage);
        yield return BothTest(shortNameDeep,shortFunctionName,normalMessage);
        yield return BothTest(shortNameDeep,normalFunctionName,normalMessage);
        yield return BothTest(shortNameDeep,shortFunctionName,longMessage);
        yield return BothTest(shortNameDeep,normalFunctionName,longMessage);
        yield return BothTest(normalName+deep,shortFunctionName,shortMessage);
        yield return BothTest(normalName+deep,normalFunctionName,shortMessage);
        yield return BothTest(normalName+deep,shortFunctionName,normalMessage);
        yield return BothTest(normalName+deep,normalFunctionName,normalMessage);
        yield return BothTest(normalName+deep,shortFunctionName,longMessage);
        yield return BothTest(normalName+deep,normalFunctionName,longMessage);
    }

    private IEnumerator BothTest(string gameObjectName, string methodName, string message)
    {
        //Avg100Test(gameObjectName,methodName,message);
        UnityEngine.Debug.Log("Running test : " + gameObjectName + " " + methodName + " " + message);
        yield return null;
        FiveMillionTest(gameObjectName,methodName,message);
        yield return null;
    }

    private void Avg100Test(string gameObjectName, string methodName, string message)
    {
        float timeAccumulate = 0;
        for (int i = 0; i < 100; i++)
        {
            stopwatch.Reset();
            stopwatch.Start();
            Test(gameObjectName, methodName, message);
            stopwatch.Stop();
            //UnityEngine.Debug.Log(stopwatch.ElapsedTicks);
            timeAccumulate += (stopwatch.ElapsedTicks/(float)Stopwatch.Frequency);
        }
        Log(gameObjectName, methodName, message, timeAccumulate, true);
    }

    private void FiveMillionTest(string gameObjectName, string methodName, string message)
    {
        float timeAccumulate = 0;
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < 5000000; i++)
        {
            Test(gameObjectName, methodName, message);
        }
        stopwatch.Stop();
        //UnityEngine.Debug.Log(stopwatch.ElapsedTicks);
        timeAccumulate += (stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);
        Log(gameObjectName, methodName, message, timeAccumulate, false);
    }

    private void Test(string gameObjectName, string methodName, string message)
    {
#if UNITY_EDITOR
        GameObject.Find(gameObjectName).SendMessage(methodName, message);
#else
		_NativeSendMessage(gameObjectName,methodName,message);
#endif
    }

    private void Log(string gameObjectName, string methodName, string message, float time, bool isAvg100)
    {
        if(Receiver.checker == 0)
        {
            UnityEngine.Debug.LogError("Checker is... " + Receiver.checker);
            //throw new System.Exception("Error!");
        }
        else
        {
            UnityEngine.Debug.Log("Checker is " + Receiver.checker);
        }
        UnityEngine.Debug.Log(time);
        //UnityEngine.Debug.Log(time + " ms " + (isAvg100 ? "(Avg. 100)" : "(5M)") + " | " + gameObjectName + " " + methodName + " " + message.Substring(0, message.Length > 10 ? 10 : message.Length));
        Receiver.ResetChecker();
    }

}
