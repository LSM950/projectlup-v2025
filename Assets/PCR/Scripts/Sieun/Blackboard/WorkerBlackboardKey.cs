using System;
using UnityEngine;

namespace LUP.PCR
{
    public readonly struct WorkerBlackboardKey : IEquatable<WorkerBlackboardKey>
    {
        public readonly string name;
        public readonly int hashKey;

        public WorkerBlackboardKey(string keyName)
        {
            this.name = keyName;
            hashKey = ComputeHash(name);
        }

        private static int ComputeHash(string str)
        {
            unchecked
            {
                int hash = (int)2166136261;
                foreach (char c in str)
                    hash = (hash * 16777619) ^ c;
                return hash;
            }
        }

        // 해시(숫자)값 비교 로직
        public bool Equals(WorkerBlackboardKey other) => hashKey == other.hashKey;
        public override bool Equals(object obj) => obj is WorkerBlackboardKey other && Equals(other);
        public override int GetHashCode() => hashKey;
        public override string ToString() => name;

        public static bool operator ==(WorkerBlackboardKey lhs, WorkerBlackboardKey rhs) => lhs.Equals(rhs);
        public static bool operator !=(WorkerBlackboardKey lhs, WorkerBlackboardKey rhs) => !(lhs == rhs);
    }

}

