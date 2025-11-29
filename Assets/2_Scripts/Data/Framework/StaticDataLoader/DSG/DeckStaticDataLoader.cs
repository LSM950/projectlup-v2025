using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "DeckStaticData", menuName = "Scriptable Objects/DeckStaticData")]
public class DeckStaticDataLoader : BaseStaticDataLoader<DeckStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=1593366084#gid=1593366084";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[DeckStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}
