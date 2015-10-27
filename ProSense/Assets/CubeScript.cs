using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

public class CubeScript : MonoBehaviour 
{
   double[] doubles;
   int angle = 3;        // similar to doublesIndex but used in Update()
   int slowSim = 0, lineNum = 1;


   /* 
    * Open file containing degrees of rotation and store data into array of doubles "doubles[]"
    * for processing simulation.
    * 
    * RPYfinder:  Simple counter to determine the correct data points in the data file so that only
    *             the roll, pitch, yaw date points are stored into the array "doubles[]"
    * rotationFile:  The current data file being read from
    * doublesIndex:  The current position in the doubles[] array
    * filePath:  string containing the path of the specified file
    */
   void BufferRotation ()
   {      
      string filePath = @"/Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/Test_Files/TestNoSpace.out";
      string outputPath = @"/Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/Output_Files/DoublesOutput.out";
      double parseResult = 0;
      int RPYfinder = 1, doublesIndex = 0; 
      FileStream rotationFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

      try
      {
         using (TextWriter tw = new StreamWriter(outputPath))
         {
            string fileContent = File.ReadAllText(filePath);
            string[] doubleStrings = fileContent.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);
            doubles = new double[(doubleStrings.Length*3)/7];

            for (int ndx = 0; ndx < doubleStrings.Length; ndx++)
            {
               if (RPYfinder >= 1 && RPYfinder <= 3)
               {
                  RPYfinder++;
               }
               else if (RPYfinder >= 4 && RPYfinder <= 6) 
               {
                  if (Double.TryParse(doubleStrings[ndx], out parseResult))
                  {
                     doubles[doublesIndex] = Double.Parse(doubleStrings[ndx]); 
                     tw.Write(doubles[doublesIndex++] + "\n");
                     RPYfinder++;
                  }
               }
               else
               {
                  RPYfinder = 1;
               }
            }
         }
      }
      
      finally
      {
         rotationFile.Close();
      }
   }


   /*
    * Prints doubles[] in format:
    *          X, Y, Z, line number in file
    *
    * arrLength: length of array; used when EOF is reached
    * ndx: current position in the array
    * lineNumber: keeps track of the line number in the original data file
    */
   void PrintDoublesArray ()
   {
      int arrLength = doubles.Length, ndx = 0;
      int lineNumber = 1;

      while (ndx < arrLength)
      {
         Debug.Log(String.Format("x = {0}, y = {1}, z = {2}  -  line number = {3}", 
            doubles[ndx], doubles[ndx+1], doubles[ndx+2], lineNumber));
         lineNumber++;
         ndx += 3;
      }
   }


   /* 
    * Use this for initialization of EVERYTHING
    */
   void Start () 
   {
      BufferRotation();
   }


	/* 
    * Update() method is called every frame
    * Currently just rotates cube and quits when the EOF is reached.
    *
    * deltaX, deltaY, deltaZ: change in degrees of rotation; used in transform.Rotate so 
    *                         the method rotates the correct amount of degrees
    * endOfFile: variable for EOF so application quits when there's no more data
    * slowSim: variable used to slow down simulation speed by adding "waits" - lazy man style
    */
	void Update () 
	{
      float deltaX = 0, deltaY = 0, deltaZ = 0;
      int endOfFile = doubles.Length - 3;

      if (angle >= endOfFile) 
      {
         print("GAME OVER. PLAYER 1 WINS.\n");
         Application.Quit();
      }
      else
      {
         if (slowSim == 0) 
         {
            deltaX = (float)(doubles[angle] - doubles[angle-3]);
            deltaY = (float)(doubles[angle+1] - doubles[angle-2]);
            deltaZ = (float)(doubles[angle+2] - doubles[angle-1]);

            transform.Rotate(deltaX, deltaY, deltaZ);
            angle += 3;
            slowSim++;

            //Debug.Log(String.Format("x = {0}, y = {1}, z = {2},    LINE NUMBER: {3}", 
            //   doubles[angle], doubles[angle+1], doubles[angle+2], lineNum++));
         }
         else if (slowSim >= 1 && slowSim <= 7) 
         {
            slowSim++;
         }
         else 
         {
            slowSim = 0;
         }
      }
	}
}

