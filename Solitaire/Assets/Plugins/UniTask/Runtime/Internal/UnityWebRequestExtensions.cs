using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Cysharp.Threading.Tasks.Internal
{
    internal static class UnityWebRequestResultExtensions
    {
#if UNITY_WEB_REQUEST_ENABLED
#if ENABLE_UNITYWEBREQUEST
        public static bool IsError(this UnityWebRequest unityWebRequest)
        {
#if UNITY_2020_2_OR_NEWER
            var result = unityWebRequest.result;
            return (result == UnityWebRequest.Result.ConnectionError)
                || (result == UnityWebRequest.Result.DataProcessingError)
                || (result == UnityWebRequest.Result.ProtocolError);
#else
            return unityWebRequest.isHttpError || unityWebRequest.isNetworkError;
#endif
        }
#endif
#endif
    }
}