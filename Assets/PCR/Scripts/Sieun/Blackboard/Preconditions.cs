using System;
/*
public static bool operator ==(Object x, Object y)
        {
            // 1) C# 참조가 null인지 확인
            // 2) Unity 엔진 내부에서 '파괴된 객체'인지 확인
            // 3) 둘 중 하나라도 null 또는 destroyed라면 true 반환
        }
 */

namespace LUP.PCR.BlackboardSystem
{
    public class Preconditions
    {
        Preconditions() { }

        public static T CheckNotNull<T>(T reference)
        {
            return CheckNotNull(reference, null);
        }

        public static T CheckNotNull<T>(T reference, string message)
        {
            if (reference == null)
                throw new ArgumentNullException(message);

            return reference;
        }

        public static void CheckState(bool expression)
        {
            CheckState(expression, null);
        }

        public static void CheckState(bool expression, string messageTemplate, params object[] messageArgs)
        {
            CheckState(expression, string.Format(messageTemplate, messageArgs));
        }

        public static void CheckState(bool expression, string message)
        {
            if (expression)
            {
                return;
            }

            throw message == null ? new InvalidOperationException() : new InvalidOperationException(message);
        }
    }
}
