using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "DeckCharacterModelStaticData", menuName = "Scriptable Objects/DeckCharacterModelStaticData")]
public class DeckCharacterModelStaticDataLoader : BaseStaticDataLoader<DeckCharacterModelStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=56059235#gid=56059235";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[DeckCharacterModelStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;
        ParseSheet(csvData);
    }
}