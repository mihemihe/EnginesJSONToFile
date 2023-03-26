using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace EnginesJSONToFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\mihe\Dropbox\PC (2)\Desktop\jsonengines\";
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
            string outputFilePath = @"C:\Users\mihe\Dropbox\PC (2)\Desktop\jsonengines\a\result.tsv";

            // Overwrite the output file if it exists
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            File.WriteAllText(outputFilePath, "Title\tManufacturer\tDescription\tOriginalMass\tLiteralZeroIgnitions\tEngineType\tDefaultConfig\tIsGimbaled\tRange\tConfigName\tConfigDescription\tMaxThrust\tMinThrust\tMassMult\tUllage\tPressureFed\tIspVacuum\tIspSeaLevel\tMinThrottle\tAirLightable\tIgnitionNumber\tIgnitionResources\tPropellants\tRatedBurnTime\tIgnitionReliabilityStart\tIgnitionReliabilityEnd\tCycleReliabilityStart\tCycleReliabilityEnd\n");


            foreach (string file in jsonFiles)
            {
                // Read the content of the JSON file
                string jsonContent = File.ReadAllText(file);

                // Parse the JSON content
                JObject jsonObj = JObject.Parse(jsonContent);

                // Extract the required attributes
                string title = jsonObj["Title"].ToString();
                string manufacturer = jsonObj["Manufacturer"].ToString();
                string description = jsonObj["Description"].ToString();
                double originalMass = jsonObj["OriginalMass"].ToObject<double>();
                bool literalZeroIgnitions = jsonObj["LiteralZeroIgnitions"].ToObject<bool>();
                string engineType = jsonObj["EngineType"].ToString();
                string defaultConfig = jsonObj["DefaultConfig"].ToString();

                JObject gimbalObj = jsonObj["Gimbal"].ToObject<JObject>();
                bool isGimbaled = gimbalObj["IsGimbaled"].ToObject<bool>();
                double range = gimbalObj["Range"].ToObject<double>();


                // Extract the EngineConfigs section
                JObject engineConfigs = jsonObj["EngineConfigs"].ToObject<JObject>();

                // Iterate through each EngineConfig
                foreach (KeyValuePair<string, JToken> engineConfigPair in engineConfigs)
                {
                    JObject engineConfig = engineConfigPair.Value.ToObject<JObject>();

                    string configName = engineConfig["ConfigName"].ToString();
                    string configDescription = engineConfig["ConfigDescription"].ToString();
                    double maxThrust = engineConfig["MaxThrust"].ToObject<double>();
                    double minThrust = engineConfig["MinThrust"].ToObject<double>();
                    double massMult = engineConfig["MassMult"].ToObject<double>();
                    bool ullage = engineConfig["Ullage"].ToObject<bool>();
                    bool pressureFed = engineConfig["PressureFed"].ToObject<bool>();
                    double ispVacuum = engineConfig["IspVacuum"].ToObject<double>();
                    double ispSeaLevel = engineConfig["IspSeaLevel"].ToObject<double>();
                    double minThrottle = engineConfig["MinThrottle"].ToObject<double>();
                    bool airLightable = engineConfig["AirLightable"].ToObject<bool>();

                    JObject ignition = engineConfig["Ignition"].ToObject<JObject>();
                    int ignitionNumber = ignition["number"].ToObject<int>();
                    //JObject ignitionResources = ignition["resources"].ToObject<JObject>();
                    Dictionary<string, double> ignitionResources = ignition["resources"].ToObject<Dictionary<string, double>>();


                    Dictionary<string, double> propellants = engineConfig["Propellants"].ToObject<Dictionary<string, double>>();

                    JObject reliability = engineConfig["Reliability"].ToObject<JObject>();
                    int ratedBurnTime = reliability["RatedBurnTime"].ToObject<int>();
                    double ignitionReliabilityStart = reliability["IgnitionReliabilityStart"].ToObject<double>();
                    double ignitionReliabilityEnd = reliability["IgnitionReliabilityEnd"].ToObject<double>();
                    double cycleReliabilityStart = reliability["CycleReliabilityStart"].ToObject<double>();
                    double cycleReliabilityEnd = reliability["CycleReliabilityEnd"].ToObject<double>();


                    StringBuilder tsvLine = new StringBuilder();

                    // Add attributes to the TSV line
                    tsvLine.Append(title + "\t");
                    tsvLine.Append(manufacturer + "\t");
                    tsvLine.Append(description + "\t");
                    tsvLine.Append(originalMass + "\t");
                    tsvLine.Append(literalZeroIgnitions + "\t");
                    tsvLine.Append(engineType + "\t");
                    tsvLine.Append(defaultConfig + "\t");
                    tsvLine.Append(isGimbaled + "\t");
                    tsvLine.Append(range + "\t");

                    tsvLine.Append(configName + "\t");
                    tsvLine.Append(configDescription + "\t");
                    tsvLine.Append(maxThrust + "\t");
                    tsvLine.Append(minThrust + "\t");
                    tsvLine.Append(massMult + "\t");
                    tsvLine.Append(ullage + "\t");
                    tsvLine.Append(pressureFed + "\t");
                    tsvLine.Append(ispVacuum + "\t");
                    tsvLine.Append(ispSeaLevel + "\t");
                    tsvLine.Append(minThrottle + "\t");
                    tsvLine.Append(airLightable + "\t");
                    //tsvLine.Append(ignitionNumber + "\t");
                    tsvLine.Append(ignitionNumber + "\t");
                    string ignitionResourcesString = string.Join("|", ignitionResources.Select(r => r.Key + ":" + r.Value));
                    tsvLine.Append(ignitionResourcesString + "\t");



                    // Add propellants to the TSV line
                    string propellantsString = string.Join("|", propellants.Select(p => p.Key + ":" + p.Value));
                    tsvLine.Append(propellantsString + "\t");

                    tsvLine.Append(ratedBurnTime + "\t");
                    tsvLine.Append(ignitionReliabilityStart + "\t");
                    tsvLine.Append(ignitionReliabilityEnd + "\t");
                    tsvLine.Append(cycleReliabilityStart + "\t");
                    tsvLine.Append(cycleReliabilityEnd);

                    // Output the TSV line to the console
                    Console.WriteLine(tsvLine.ToString());
                    using (StreamWriter sw = File.AppendText(outputFilePath))
                    {
                        sw.WriteLine(tsvLine.ToString());
                    }

                }
                    Console.WriteLine($"Processing file: {Path.GetFileName(file)}");
                // TODO: Add your JSON processing code here
            }

            Console.WriteLine("All JSON files have been processed.");
        }
    }
}