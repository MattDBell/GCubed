using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
class IniLoader
{
    bool successfullyInitilized = false;
    Dictionary<string, string> iniContents;

    List<string> wholeThing;    

    public IniLoader(string iniPath)
    {
        if (!File.Exists(iniPath))
        {
            return;
        }

        iniContents = new Dictionary<string, string>();
        StreamReader sReader = File.OpenText(iniPath);
        wholeThing = new List<string>();


        //Open the file and shove into dictionary
        while (!sReader.EndOfStream)
        {
            string wholeLine = sReader.ReadLine();

            string[] lineParts = wholeLine.Split(';');
            foreach (string line in lineParts)
            {
                char[] splitOn = { ' ', '=', '\t', '\n' };
                string[] contents = line.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

                if (contents.Length <= 0)  //Sanity Check
                    continue;

                if (contents[0].StartsWith("#")) //Line is a comment!
                {
                    wholeThing.Add(line);
                    continue;
                }

                string varName = contents[0];

                if (iniContents.ContainsKey(varName))//Can't add things twice!
                    continue;

                string varValue = "";
                //Treat the first word as the name ov a variable and the rest of the line as the result
                for (int i = 1; i < contents.Length; ++i)
                {
                    varValue += contents[i] + " ";  //Put the whitespace back in that we stripped out above
                }
                varValue = varValue.Remove(varValue.Length - 1, 1);    //Strip out the extra whitespace at the end of the line

                //Now shove it into the dictionary
                iniContents.Add(varName, varValue);              
                wholeThing.Add("~" + varName);
            }
        }
        sReader.Close();
        successfullyInitilized = true;
    }

    public void WriteOutModified(string iniPath)
    {
        StreamWriter sWriter;
        if (File.Exists(iniPath))
            sWriter = new StreamWriter(File.OpenWrite(iniPath));
        else
            sWriter = File.CreateText(iniPath);

        foreach (string entry in wholeThing)
        {
            string output = entry;
            if (entry.Length > 1 && entry[0] == '~')
            {
                //First char is ~ so it's an entry in the dictionary!
                string key = entry.Remove(0, 1);
                iniContents.TryGetValue(key, out output);
                sWriter.WriteLine(key + " = " + output + ";");
            }
            else
                sWriter.WriteLine(output);
        }

        sWriter.Close();
    }

    public void ModifyValue(string key, string valAsString)
    {
        if (iniContents.ContainsKey(key))
        {
            iniContents.Remove(key);
        }
        else
        {
            wholeThing.Add("~" + key);
        }

        iniContents.Add(key, valAsString);
    }

    public void GetFloat(string name, out float var, float defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        string value;

        if (iniContents.TryGetValue(name, out value))
        {
            //Strip out the 'f' if it's there, as tryparse fails otherwise!
            value = value.Replace('f', ' ');
            if (float.TryParse(value, out var))
                return;
        }
        var = defaultVal;
    }

    public void GetInt(string name, out int var, int defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        string value;

        if (!iniContents.TryGetValue(name, out value))
            return;

        if (int.TryParse(value, out var))
            return;
        var = defaultVal;
    }

    public void GetUnsignedInt32(string name, out UInt32 var, UInt32 defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        string value;

        if (!iniContents.TryGetValue(name, out value))
            return;

        int asInt;  //Read it as an int to that we can properly treat negative numbers the way they should be treated
        if (int.TryParse(value, out asInt))
        {
            var = (UInt32)asInt;
            return;
        }
        var = defaultVal;
    }

    public void GetBool(string name, out bool var, bool defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        string value;

        if (!iniContents.TryGetValue(name, out value))
            return;

        if (bool.TryParse(value, out var))
            return;
        var = defaultVal;
    }

    public void GetString(string name, out string var, string defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        if (iniContents.TryGetValue(name, out var))
            return;

        var = defaultVal;
    }

    public void GetChar(string name, out char var, char defaultVal)
    {
        var = defaultVal;

        if (!successfullyInitilized)
            return;

        string value;

        if (iniContents.TryGetValue(name, out value))
            return;

        if (char.TryParse(value, out var))
            return;
        var = defaultVal;
    }
}

