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
	//Lobby -> 플레이어가 원하는 게임을 찾거나, 새 게임을 만들고 대기할 수 있는 (방 만들기. 방 만든사람 <= 호스트)
	//Relay -> 매칭된 플레이어들의 Replay의 Join Code로 연결되어,
	//         호스트-클라이언트 방식으로 실시간 멀티플레이 환경을 유지
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
			Debug.Log("로그인되지 않았습니다.");
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
			Debug.LogError("로비 찾기 실패 " + e);
		}
		return null;
	}

	private async Task CreateLobby()
	{
		try
		{
			CurrentLobby = await LobbyService.Instance.CreateLobbyAsync("랜덤매칭방", 2);
			Debug.Log("새로운 방 생성됨: " + CurrentLobby.Id);

			await AllocateRelayServerAndJoin(CurrentLobby);
			StartHost();
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError("로비 생성 실패 " + e);
		}
	}

	private async Task JoinLobby(string lobbyId)
	{
		try
		{
			CurrentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
			Debug.Log("방에 접속되었습니다. " + CurrentLobby.Id);
			StartClient();
		}
		catch (LobbyServiceException e)
		{
			Debug.Log("로비 참가 실패 : " + e);
		}
	}

	private async Task AllocateRelayServerAndJoin(Lobby lobby)
	{
		try
		{
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log("Relay 서버 할당 완료. Join Code: " + joinCode);
		}
		catch (RelayServiceException e)
		{
			Debug.Log("Relay 서버 할당 실패 : " + e);
		}
	}

	private void StartHost()
	{
		NetworkManager.Singleton.StartHost();
		Debug.Log("호스트가 시작되었습니다.");
	}

	private void StartClient()
	{
		NetworkManager.Singleton.StartClient();
		Debug.Log("클라이언트가 연결되었습니다.");
	}
}
