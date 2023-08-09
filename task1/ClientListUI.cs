using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class ClientListUI : MonoBehaviour
{
    public Dropdown filterDropdown;
    public Transform clientListContainer;
    public GameObject clientListItemPrefab;
    public Text popupNameText;
    public Text popupPointsText;
    public Text popupAddressText;
    public RectTransform popupPanel;
    
    private List<Client> allClients;
    private List<Client> displayedClients = new List<Client>();

    private void Start()
    {
        APIService apiService = GetComponent<APIService>();
        StartCoroutine(apiService.FetchClients(OnClientsFetched));

        filterDropdown.onValueChanged.AddListener(OnFilterDropdownChanged);
    }

    private void OnClientsFetched(List<Client> clients)
    {
        allClients = clients;
        displayedClients = allClients;

        PopulateClientList();
    }

    private void OnFilterDropdownChanged(int index)
    {
        switch (index)
        {
            case 0: // All clients
                displayedClients = allClients;
                break;
            case 1: // Managers only
                displayedClients = allClients.FindAll(client => client.points > 100); // Adjust condition as needed
                break;
            case 2: // Non-managers
                displayedClients = allClients.FindAll(client => client.points <= 100); // Adjust condition as needed
                break;
        }

        PopulateClientList();
    }

    private void PopulateClientList()
    {
        ClearClientList();

        foreach (Client client in displayedClients)
        {
            GameObject clientListItem = Instantiate(clientListItemPrefab, clientListContainer);
            clientListItem.transform.GetChild(0).GetComponent<Text>().text = client.label;
            clientListItem.transform.GetChild(1).GetComponent<Text>().text = client.points.ToString();

            Button button = clientListItem.GetComponent<Button>();
            button.onClick.AddListener(() => ShowPopup(client));
        }
    }

    private void ClearClientList()
    {
        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowPopup(Client client)
    {
        popupNameText.text = client.label;
        popupPointsText.text = client.points.ToString();
        popupAddressText.text = client.address;

        popupPanel.gameObject.SetActive(true);
        popupPanel.DOScale(Vector3.one, 0.3f);
    }

    public void ClosePopup()
    {
        popupPanel.DOScale(Vector3.zero, 0.3f).OnComplete(() => popupPanel.gameObject.SetActive(false));
    }
}
