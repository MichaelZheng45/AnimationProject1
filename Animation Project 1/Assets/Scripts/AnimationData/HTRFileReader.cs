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

public struct DataInput
{
    public string name;
    public Vector3 transform;
    public Vector3 rotation;
    public float boneLength;
    public float scaleFactor;
    public int frame;
}


public class HTRFileReader : EditorWindow
{
    AnimationDataHierarchal animData;
    string path = "Assets/";
    currentHTRMode curMode;

    bool done = false;

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
        done = false;

        StreamReader reader = new StreamReader(path);
        //read header
        curMode = 0;
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        Debug.Log(reader.ReadLine());
        animData.createBase(readToSpaceInt(reader.ReadLine())); // number of segments
        animData.generateFrames(readToSpaceInt(reader.ReadLine())); //number of frames
        animData.setFramePerSecond(readToSpaceInt(reader.ReadLine())); //frame rate
        Debug.Log(reader.ReadLine()); //rotation order
        animData.setCalibrationUnit(readToSpaceString(reader.ReadLine())); //calibration units
        Debug.Log(reader.ReadLine()); //rotation units
        Debug.Log(reader.ReadLine()); //globalaxisofGravity
        Debug.Log(reader.ReadLine()); //bone length axis
        animData.scaleFactor = readToSpaceFloat(reader.ReadLine()); //scale factor
        //end of header

        //for setting hierarchy
        int jointCount = 0; //keep track of the where the index it is putting in the basepose
        List<string> jointIndexList = new List<string>(); //keeps track of the string names for easier hierarchy building/checking
        string currentJoint = ""; //for keyframing

        while (!done)
        {
            string textLine = "";
            if (readLine(ref textLine,ref reader, ref currentJoint))
            {
                if(curMode == currentHTRMode.SEGMENTHIERARCHY)
                {
                    string first = "", second = "";
                    parseTextToTwoBetweenTab(ref first, ref second, textLine); //find the two strings

                    //add and update joint
                    animData.poseBase[jointCount].name = first;
                    jointIndexList.Add(first);

                    if(second != "GLOBAL")
                    {
                        animData.poseBase[jointCount].parentNodeIndex = jointIndexList.IndexOf(second);
                    }
                    else
                    {
                        animData.poseBase[jointCount].parentNodeIndex = -1;
                    }

                    jointCount++;
                }
                else if(curMode == currentHTRMode.BASE_POSITION)
                {
                    DataInput data = superParseDataIntoInputBase(textLine);
                    int index = jointIndexList.IndexOf(data.name);
                    animData.poseBase[index].basePosition = data.transform;
                    animData.poseBase[index].baseRotation = data.rotation;
                    animData.poseBase[index].boneLength = data.boneLength;
                }
                else if(curMode == currentHTRMode.FRAMING)
                {
                    DataInput data = superParseDataIntoInputKeyFrame(textLine);
                    int index = jointIndexList.IndexOf(currentJoint);
                    Debug.Log(currentJoint);
                    animData.poseBase[index].keyFrames[data.frame].keyPosition = data.transform;
                    animData.poseBase[index].keyFrames[data.frame].keyRotation = data.rotation;
                    animData.poseBase[index].keyFrames[data.frame].scale = data.scaleFactor;
                }
            }
        }
        
    }

    DataInput superParseDataIntoInputKeyFrame(string textBlock)
    {
        DataInput newInput = new DataInput();
        int indexer = 0;
        //0 = frame
        // 1-3 = transform xyz
        // 4-7 = rotation xyz euler
        // 8 = scale
        int charCount = 0;
        string newText = "";
        while (charCount < textBlock.Length)
        {
            char dat = textBlock[charCount];
            if (dat == '\t')
            {
                //encountered tab, determine where the text it will go into based on index
                switch (indexer)
                {
                    case 0:
                        newInput.frame = int.Parse(newText);
                        break;
                    case 1:
                        newInput.transform += new Vector3(float.Parse(newText), 0, 0);
                        break;
                    case 2:
                        newInput.transform += new Vector3(0, float.Parse(newText), 0);
                        break;
                    case 3:
                        newInput.transform += new Vector3(0, 0, float.Parse(newText));
                        break;
                    case 4:
                        newInput.rotation += new Vector3(float.Parse(newText), 0, 0);
                        break;
                    case 5:
                        newInput.rotation += new Vector3(0, float.Parse(newText), 0);
                        break;
                    case 6:
                        newInput.rotation += new Vector3(0, 0, float.Parse(newText));
                        break;
                }
                //update to next index and reset text
                indexer++;
                newText = "";
            }
            else
            {
                newText += dat;
            }

            charCount++;
        }

        newInput.scaleFactor = float.Parse(newText); //add the last text
        return newInput;
    }

    DataInput superParseDataIntoInputBase(string textBlock)
    {
        DataInput newInput = new DataInput();
        int indexer = 0; //keeps track of which block to put it in
        // 0 = name
        // 1-3 = transform xyz
        // 4-7 = rotation xyz euler
        // 8 = bone length

        int charCount = 0;
        string newText = "";
        while(charCount < textBlock.Length)
        {
            char dat = textBlock[charCount];
            if(dat == '\t')
            {
                //encountered tab, determine where the text it will go into based on index
                switch(indexer)
                {
                    case 0:
                        newInput.name = newText;
                        break;
                    case 1:
                        newInput.transform += new Vector3(float.Parse(newText), 0, 0);
                        break;
                    case 2:
                        newInput.transform += new Vector3(0, float.Parse(newText), 0);
                        break;
                    case 3:
                        newInput.transform += new Vector3(0, 0, float.Parse(newText));
                        break;
                    case 4:
                        newInput.rotation += new Vector3(float.Parse(newText), 0, 0);
                        break;
                    case 5:
                        newInput.rotation += new Vector3(0, float.Parse(newText), 0);
                        break;
                    case 6:
                        newInput.rotation += new Vector3(0, 0, float.Parse(newText));
                        break;
                }
                //update to next index and reset text
                indexer++;
                newText = "";
            }
            else
            {
                newText += dat;
            }

            charCount++;
        }

        newInput.boneLength = float.Parse(newText); //add the last text
        return newInput;
    }

    void parseTextToTwoBetweenTab(ref string first, ref string second, string main)
    {
        bool spaceEncountered = false;
        for(int i = 0; i<main.Length; i++)
        {
            char dat = main[i];
            if(dat == '\t')
            {
                spaceEncountered = true;
            }
            else if(spaceEncountered)
            {
                second += dat;
            }
            else
            {
                first += dat;
            }
        }
    }

    //reads in an int, ignores the first text "name: " then gets the int after.
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

    float readToSpaceFloat(string text)
    {
        string newText = "";
        bool spaceEncountered = false;
        for (int i = 0; i < text.Length; i++)
        {
            char dat = text[i];

            if (spaceEncountered)
            {
                newText += dat;
            }
            if (dat == ' ')
            {
                spaceEncountered = true;
            }
        }
        return float.Parse(newText);
    }

    string readToSpaceString(string text)
    {
        string newText = "";
        bool spaceEncountered = false;
        for (int i = 0; i < text.Length; i++)
        {
            char dat = text[i];

            if (spaceEncountered)
            {
                newText += dat;
            }
            if (dat == ' ')
            {
                spaceEncountered = true;
            }
        }
        return newText;
    }

    //checks the line if it has # or [
    bool readLine(ref string textLine,ref StreamReader inReader, ref string joint)
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
                done = true;
            }
            else
            {
                //read in [joingName] for processing
                joint = "";
                curMode = currentHTRMode.FRAMING;
                for(int i = 1; i < textLine.Length-1; i++)
                {
                    joint += textLine[i];
                }
            }
            return false;
        }
        return true;
    }
}
