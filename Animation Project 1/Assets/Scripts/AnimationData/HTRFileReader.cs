using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public enum currentHTRMode
{
    HEADER = 0,
    SEGMENTHIERARCHY,
    BASE_POSITION,
    FRAMING,
    END

}

public class HTRFileReader : EditorWindow
{
    AnimationDataHierarchal animData;
    string path = "Assets/";
    currentHTRMode curMode;
    [MenuItem("Window/HTR Input")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(HTRFileReader));
    }

    private void OnGUI()
    {
        curMode = currentHTRMode.HEADER;
        animData = EditorGUILayout.ObjectField("AnimationData", animData, typeof(AnimationDataHierarchal), true) as AnimationDataHierarchal;
        path = EditorGUILayout.TextField("AssestPath", path);
        if(GUILayout.Button("Process"))
        {
            if(animData != null)
            {
                processFile();
            }
            else
            {
                Debug.Log("scriptable object missing!");
            }
        }
    }

    void processFile()
    {
        StreamReader reader = new StreamReader(path);
        //read header
        curMode = 0;
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(readToSpaceInt(reader.ReadLine()));
       // animData.createBase(int.Parse(reader.ReadLine()));
        /*
        while (!reader.EndOfStream)
        {

            string textLine = "";
            if (readLine(ref textLine,ref reader))
            {

            }
        }
        */
    }

    void checkBracket()
    {

    }

    int readToSpaceInt(string text)
    {
        string newText = "";
        bool spaceEncountered = false;
        for(int i = 0; i < text.Length; i++)
        {
            char dat = text[i];

            if(spaceEncountered)
            {
                newText += dat;
            }
            if(dat == ' ')
            {
                spaceEncountered = true;
            }
        }
        return int.Parse(newText);
    }

    //checks the line if it has # or [
    bool readLine(ref string textLine,ref StreamReader inReader)
    {
        textLine = inReader.ReadLine();

        if(textLine[0] == '#') // is a comment can ignore
        {
            return false;
        }
        if(textLine[0] == '[') //conext or chapter currently
        {
            if(textLine == "[Header]")
            {
                curMode = 0;
            }
            else if(textLine == "[SegmentNames&Hierarchy]")
            {
                curMode = currentHTRMode.SEGMENTHIERARCHY;
            }
            else if(textLine == "[BasePosition]")
            {
                curMode = currentHTRMode.BASE_POSITION;
            }
            else if(textLine == "[EndOfFile]")
            {
                curMode = currentHTRMode.END;
            }
            else
            {
                curMode = currentHTRMode.FRAMING;
            }
            return false;
        }
        return true;
    }
}
