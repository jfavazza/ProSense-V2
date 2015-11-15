using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

public class CubeScript : MonoBehaviour 
{
   /*  public variables  */
   public double[] doubles;
   public int angle, pathFlag;   
   public string dataFilePath;
   public FileBrowser fb;
   public GUISkin[] skins;
   public Texture2D file,folder,back,drive;

void OnGUI()
{
   if (fb.drawWindow == true) 
   {
      if (fb.draw())
      {
         if (fb.outputFile == null)
         {
            fb.drawWindow = false;
         }
         else
         {
            dataFilePath = fb.outputFile.ToString();
            pathFlag = 1;
            fb.drawWindow = false;
         }
      }
   }
}

  /* 
   * Open file containing degrees of rotation and store data into array of doubles "doubles[]"
   * for processing simulation.
   * 
   * RPYfinder:  Simple counter to determine the correct data points in the data file so that only
   *             the roll, pitch, yaw date points are stored into the array "doubles[]"
   * rotationFile:  The current data file being read from
   * doublesIndex:  The current position in the doubles[] array
   * filePath:  String containing the path of the specified input file
   * outputPath:  String containing the path for the Output File which contains data from doubles[] 
   *              separated by new lines
   */
   void BufferRotation (string path)
   {      
      string filePath = path; 
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
   *          Roll, Pitch, Yaw, line number in file
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
         Debug.Log(String.Format("roll = {0},  pitch = {1},  yaw = {2}  -  line number = {3}", 
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
      angle = 3;
      pathFlag = 0;
      dataFilePath = null;

      fb = new FileBrowser(@"/Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/Test_Files");
      /*fb.guiSkin = skins[0];
      fb.fileTexture = file; 
      fb.directoryTexture = folder;
      fb.backTexture = back;
      fb.driveTexture = drive;
      fb.showSearch = true;
      fb.searchRecursively = true;*/
   }


 /* 
  * Update() method is called every frame
  * Currently just rotates cube and quits when the EOF is reached.
  *
  * deltaX, deltaY, deltaZ: change in degrees of rotation; used in transform.Rotate so 
  *                         the method rotates the correct amount of degrees
  * endOfFile: variable for EOF so application quits when there's no more data
  * slowSim: variable used to slow down simulation speed by adding "waits" - lazy man style
  *
  * transform.Roate takes data as PITCH, YAW, ROLL
  */
	void Update () 
	{
      float deltaX = 0, deltaY = 0; //, deltaZ = 0;
      int endOfFile; 

      if (pathFlag == 1)
      {
         BufferRotation(dataFilePath);
         pathFlag = 0;
      }

      if (dataFilePath != null) 
      {
         endOfFile = doubles.Length - 3;

         if (angle >= endOfFile) 
         {
            print("GAME OVER. PLAYER 1 WINS.\n");
            Application.Quit();
         }
         else
         {
            deltaX = (float)(doubles[angle] - doubles[angle-3]);
            deltaY = (float)(doubles[angle+1] - doubles[angle-2]);

            transform.Rotate(deltaY, 0, deltaX);
            angle += 3;
         }
      }
	}
}

