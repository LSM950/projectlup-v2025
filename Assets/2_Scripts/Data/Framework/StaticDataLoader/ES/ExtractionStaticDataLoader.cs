using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ExtractionStaticData", menuName = "Scriptable Objects/ExtractionStaticData")]
public class ExtractionStaticDataLoader : BaseStaticDataLoader<ExtractionStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1R-q7I41tJMOg7_Melx8lQYMLczS27u-pGseCk-DLxeE/export?format=csv&gid=1480693328#gid=1480693328";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[ExtractionStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}
