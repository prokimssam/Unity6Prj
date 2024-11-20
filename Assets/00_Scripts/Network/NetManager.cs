using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class NetManager : MonoBehaviour
{
	//Lobby -> �÷��̾ ���ϴ� ������ ã�ų�, �� ������ ����� ����� �� �ִ� (�� �����. �� ������ <= ȣ��Ʈ)
	//Relay -> ��Ī�� �÷��̾���� Replay�� Join Code�� ����Ǿ�,
	//         ȣ��Ʈ-Ŭ���̾�Ʈ ������� �ǽð� ��Ƽ�÷��� ȯ���� ����
	private Lobby CurrentLobby;
	public Button StartMatchButton;

	private async void Start()
	{
		await UnityServices.InitializeAsync();
		if (!AuthenticationService.Instance.IsSignedIn)
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}

		StartMatchButton.onClick.AddListener(() => StartMatchMaking());
	}

	public async void StartMatchMaking()
	{
		if (!AuthenticationService.Instance.IsSignedIn)
		{
			Debug.Log("�α��ε��� �ʾҽ��ϴ�.");
			return;
		}

		CurrentLobby = await FindAvailableLobby();
		if (CurrentLobby == null)
		{
			await CreateLobby();
		}
		else
		{
			await JoinLobby(CurrentLobby.Id);
		}
	}

	private async Task<Lobby> FindAvailableLobby()
	{
		try
		{
			QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
			if (queryResponse.Results.Count > 0)
			{
				return queryResponse.Results[0];
			}
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError("�κ� ã�� ���� " + e);
		}
		return null;
	}

	private async Task CreateLobby()
	{
		try
		{
			CurrentLobby = await LobbyService.Instance.CreateLobbyAsync("������Ī��", 2);
			Debug.Log("���ο� �� ������: " + CurrentLobby.Id);

			await AllocateRelayServerAndJoin(CurrentLobby);
			StartHost();
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError("�κ� ���� ���� " + e);
		}
	}

	private async Task JoinLobby(string lobbyId)
	{
		try
		{
			CurrentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
			Debug.Log("�濡 ���ӵǾ����ϴ�. " + CurrentLobby.Id);
			StartClient();
		}
		catch (LobbyServiceException e)
		{
			Debug.Log("�κ� ���� ���� : " + e);
		}
	}

	private async Task AllocateRelayServerAndJoin(Lobby lobby)
	{
		try
		{
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log("Relay ���� �Ҵ� �Ϸ�. Join Code: " + joinCode);
		}
		catch (RelayServiceException e)
		{
			Debug.Log("Relay ���� �Ҵ� ���� : " + e);
		}
	}

	private void StartHost()
	{
		NetworkManager.Singleton.StartHost();
		Debug.Log("ȣ��Ʈ�� ���۵Ǿ����ϴ�.");
	}

	private void StartClient()
	{
		NetworkManager.Singleton.StartClient();
		Debug.Log("Ŭ���̾�Ʈ�� ����Ǿ����ϴ�.");
	}
}
