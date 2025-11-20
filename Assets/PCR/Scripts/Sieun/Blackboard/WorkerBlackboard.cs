using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerBlackboard : MonoBehaviour
    {
        private Dictionary<WorkerBlackboardKey, object> data = new (); // 실제 데이터 저장소

        private Dictionary<string, WorkerBlackboardKey> keyRegistry = new (); // 키 캐싱 ( 같은 문자열인 키 생성 방지 )


        //Flyweight
        // 키가 이미 존재하면 기존 키 반환, 없으면 새로 생성하여 등록 후 반환
        public WorkerBlackboardKey GetOrRegisterKey(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var existingKey))
            {
                return existingKey;
            }
            var newKey = new WorkerBlackboardKey(keyName);
            keyRegistry[keyName] = newKey;

            return newKey;
        }

        public void SetValue<T>(string keyName, T value)
        {
            var key = GetOrRegisterKey(keyName);

            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        // 이미 Key 객체가 있다면 더 빠르게 처리
        public void SetValue<T>(WorkerBlackboardKey key, T value)
        {
            if (data.ContainsKey(key)) data[key] = value;
            else data.Add(key, value);
        }


        public T GetValue<T>(string keyName)
        {
            var key = GetOrRegisterKey(keyName);
            return GetValue<T>(key);
        }

        public T GetValue<T>(WorkerBlackboardKey key)
        {
            if (data.TryGetValue(key, out object val))
            {
                // 타입 캐스팅 (저장된게 int인데 float로 달라고 하면 에러나거나 기본값)
                if (val is T castedVal) return castedVal;
            }
            return default(T);
        }

        public void Remove(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var key))
                data.Remove(key);
        }

        public bool HasKey(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var key))
            { return data.ContainsKey(key); }
            
            return false;
        }
    }

}

