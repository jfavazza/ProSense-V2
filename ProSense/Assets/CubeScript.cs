using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

public class CubeScript : MonoBehaviour {

   double[] doubles;
   int angle = 0;
   int bob = 0;

   // Open file and store data into array for processing
   void BufferRotation ()
   {
      FileStream rotationFile = new FileStream(@"/Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/60sec.out", FileMode.Open, FileAccess.Read);
      double parseResult = 0;
      int counter = 1, doublesIndex = 0; //, i = 0, arrlength = 0;

      try
      {
         /*-------------------------------------------------*/
         using (TextWriter tw = new StreamWriter(@"out.txt"))
         {
            string fileContent = File.ReadAllText(@"/Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/60sec.out");
            string[] doubleStrings = fileContent.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            doubles = new double[(doubleStrings.Length*3)/7];

            for (int n = 0; n < doubleStrings.Length; n++)
            {
               if (Double.TryParse(doubleStrings[n], out parseResult)) 
               {
                  if (counter >= 1 && counter <= 3)
                  {
                     counter++;
                  }
                  else if (counter >= 4 && counter <= 6) 
                  {
                     doubles[doublesIndex] = Double.Parse(doubleStrings[n]); //, out parseResult);
                     tw.Write(doubles[doublesIndex] + " ");
                     counter++;
                     doublesIndex++;
                  }
                  else
                  {
                     counter = 1;
                  }
               }
            }

            /*arrlength = doubles.Length;
            while (i < arrlength)
            {
               Debug.Log(String.Format("{0}", doubles[i]));
               i++;
            }*/
         }
         /*-------------------------------------------------*/
      }
      
      finally
      {
         rotationFile.Close();
      }

   }

   // Use this for initialization
   void Start () 
   {
      BufferRotation();
   }

	// Update is called once per frame
	void Update () 
	{
      if (bob == 0) {
         transform.Rotate((float)doubles[angle], (float)doubles[angle+1], (float)doubles[angle+2]);
         angle += 3;
         bob++;
      }
      else if (bob >= 1 && bob <= 7) {
         bob++;
      }
      else {
         bob = 0;
      }
	}
}

