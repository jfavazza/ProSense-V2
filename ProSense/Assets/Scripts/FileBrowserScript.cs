/*
 * /Users/Johnny/Documents/ProSense/GitHub/ProSense-V2/ProSense/Assets/Test_Files 
 */
using UnityEngine;
using System.Collections;

public class FileBrowserScript : MonoBehaviour 
{
   public static string dataFilePath;
   public static int pathFlag = 0;
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
               Application.LoadLevel("Simulation");
            }
         }
      }
   }

	// Use this for initialization
	void Start () 
   {
      fb = new FileBrowser(@"/Users/Johnny/Google Drive/Documents/ProSense/ProSense/Unity Rotation Test Files/");
      fb.guiSkin = skins[0];
      fb.fileTexture = file; 
      fb.directoryTexture = folder;
      fb.backTexture = back;
      fb.driveTexture = drive;
      fb.showSearch = true;
      fb.searchRecursively = true;
	}
	
	// Update is called once per frame
	void Update () 
   {
	
	}
}
