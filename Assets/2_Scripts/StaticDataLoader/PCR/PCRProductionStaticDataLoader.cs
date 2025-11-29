using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "PCRProductionStaticData", menuName = "Scriptable Objects/PCRProductionStaticData")]
public class PCRProductionStaticDataLoader : BaseStaticDataLoader<PCRProductionStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1_8bIM_IAx1r9BhxDEX2Pn3jaf5O42iigmWJmDKKXs6k/export?format=csv&gid=1913495394#gid=1913495394";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[PCRProductionStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}