using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ShootingStaticData", menuName = "Scriptable Objects/ShootingStaticData")]
public class ShootingStaticDataLoader : BaseStaticDataLoader<ShootingStaticData>
{
    protected override string URL => "https://docs.google.com/spreadsheets/d/1rwLdR5cOTk5i262bj6VY-WPTY3YJRJB28WbkxAzpcHE/export?format=csv&gid=839444384#gid=839444384";

    public override IEnumerator LoadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[ShootingStaticData] Failed to load sheet: {www.error}");
            yield break;
        }

        string csvData = www.downloadHandler.text;

        ParseSheet(csvData);
    }
}
