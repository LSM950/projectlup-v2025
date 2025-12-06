using System.Collections;
using System.Collections.Generic;

public interface IDataSourceAdapter
{
    IEnumerator LoadData(string url, System.Action<string> onSuccess, System.Action<string> onError);

    List<T> ParseToObjects<T>(string rawData, int startRow) where T : new();
}
