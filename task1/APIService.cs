using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APIService : MonoBehaviour
{
    private const string apiURL = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    public IEnumerator FetchClients(System.Action<List<Client>> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching API data: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                List<Client> clients = JsonUtility.FromJson<List<Client>>(response);
                callback?.Invoke(clients);
            }
        }
    }
}
