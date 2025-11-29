using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "RoguelikeStaticData", menuName = "Scriptable Objects/RoguelikeStaticData")]
public class RoguelikeStaticDataLoader : BaseStaticDataLoader<RoguelikeStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1OcIRuu5efxWr63dNE0e9DQ6OD5gPnNWCoPh_nN8Oe6A/export?format=csv&gid=926345530#gid=926345530";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[RoguelikeStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}

