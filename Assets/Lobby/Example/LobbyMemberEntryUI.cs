using System;
using Lobby;
using Lobby.LobbyInstance;
using PlayFab.MultiplayerModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMemberEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private TextMeshProUGUI ownerText;
    [SerializeField] private Button kickButton;
    
    public Member Member { get; private set; }
    private ObservableLobby Lobby { get; set; }

    private void Awake()
    {
        kickButton.onClick.AddListener(OnKickButtonClicked);
    }

    private void OnKickButtonClicked()
    {
        Lobby.KickMember(Member, false);
    }

    public void Initialise(ObservableLobby lobby, Member member)
    {
        if (Member != null)
            throw new Exception("LobbyMemberEntryUI already initialised");
        
        Member = member;
        Lobby = lobby;
        Lobby.OnLobbyOwnerChanged += OnLobbyOwnerChanged;
        
        UpdateUI();
    }

    private void OnLobbyOwnerChanged(EntityKey _)
    {
        UpdateUI();
    }

    public void UpdateMember(Member member)
    {
        if (Member == null)
            throw new Exception("LobbyMemberEntryUI not initialised");
        
        if (Member.MemberEntity.Id != member.MemberEntity.Id)
            throw new Exception("LobbyMemberEntryUI initialised with different member");
        
        Member = member;
        UpdateUI();
    }
    
    public void HandleMemberRemoved()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Lobby != null)
            Lobby.OnLobbyOwnerChanged -= OnLobbyOwnerChanged;
    }

    private void UpdateUI()
    {
        displayNameText.text = Member.MemberEntity.Id;
        ownerText.gameObject.SetActive(Lobby.LobbyOwner.Id == Member.MemberEntity.Id);
        kickButton.gameObject.SetActive(Lobby.IsOwner && Lobby.LobbyOwner.Id != Member.MemberEntity.Id);
    }

}
