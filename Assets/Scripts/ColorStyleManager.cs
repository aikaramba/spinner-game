using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public static class ColorStyleManager {
    private static string colorSchemeResourcePath = "colors";
    private static List<Color[]> colorsDB = new List<Color[]>();
    private static bool isInitialized = false;
    public static void Init () {
        LoadDB(colorSchemeResourcePath);
        isInitialized = true;
    }
    public static void LoadDB(string path) {
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        string fs = textAsset.text;
        string[] fLines = Regex.Split(fs, "\n|\r|\r\n");
        for (int i = 0; i < fLines.Length; i++) {
            
            string[] fValues = fLines[i].Split(' ');
            if (fValues.Length == 4)
            {
                Color[] colorBlock = new Color[4];
                colorBlock[0] = ParseColor(fValues[0]);
                colorBlock[1] = ParseColor(fValues[1]);
                colorBlock[2] = ParseColor(fValues[2]);
                colorBlock[3] = ParseColor(fValues[3]);
                colorsDB.Add(colorBlock);
            }
        }
    }
    public static Color GetRandomObstacleColor() {
        if (!isInitialized) Init();

        Color[] randColorBlock = colorsDB[Random.Range(0, colorsDB.Count)];
        Color backgroundColor = randColorBlock[3];
        Color obstacleColor = randColorBlock[2];
        Color playerColor = randColorBlock[1];
        Color textColor = randColorBlock[0];

        TextColorSwitcher.ChangeColor(textColor);
        PlayerController.ChangeColor(playerColor);
        CameraController.SetNewBackgroundColor(backgroundColor);
        return obstacleColor;
    }
    //------------------------------------
    public static void ParseRawHTML(TextAsset htmlCode) {
        string[] split1 = htmlCode.text.Split( new string[] { "<span class=\"tran\">" }, System.StringSplitOptions.None);
        List<string> colors = new List<string>();
       
        for (int i = 1; i < split1.Length; i++) {
            string[] split2 = split1[i].Split(new string[] { "</span>" }, System.StringSplitOptions.None);
            string colorString = split2[0];
            if (colorString.Length > 0 && colorString[0] == '#')
                colors.Add(colorString);
        }
        //-------------------
        List<string[]> sortedColors = new List<string[]>();
        string[] colorStringBlock = new string[] { "", "", "", "" };
        int c = 0;
        foreach (string colorString in colors) {
            colorStringBlock[c] = colorString;
            c++;
            if (c > 3){
                c = 0;

                sortedColors.Add(colorStringBlock);

                colorStringBlock = new string[] { "", "", "", "" };
            }
        }

        //------------------------
        using (StreamWriter sw = new StreamWriter("ColorsFiltered.txt"))
        {

            foreach (string[] blockOfStrings in sortedColors)
            {
                sw.WriteLine(
                    blockOfStrings[0] + " " +
                    blockOfStrings[1] + " " +
                    blockOfStrings[2] + " " +
                    blockOfStrings[3]);
            }
        }
    }

    //-------------------------
    public static Color ParseColor(string hexstring)
    {
        if (hexstring.StartsWith("#"))
        {
            hexstring = hexstring.Substring(1);
        }

        if (hexstring.StartsWith("0x"))
        {
            hexstring = hexstring.Substring(2);
        }

        if (hexstring.Length != 6)
        {
            return Color.white;
           // throw new Exception(string.Format("{0} is not a valid color string.", hexstring));
        }

        byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);
        
        Color32 color32 = new Color32(r, g, b, 1);
        Color outputColor = color32;
        outputColor.a = 1f;
        //return new Color32(r, g, b, 1);
        return outputColor;
    }
}
