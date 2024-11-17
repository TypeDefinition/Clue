using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine;

public enum RoomState {
    None = -1,

    EnterRoom = 0,
    PointAndClick,
    StartConversation,
    Listen,
    Interrogate,
    EndConversation,
    LeaveRoom,

    Num,
}

public class Room : MonoBehaviour {
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip enterRoomClip;
    [SerializeField] private VideoClip pointAndClickClip;
    [SerializeField] private VideoClip leaveRoomClip;

    [Header("UI")]
    [SerializeField] private GameObject pointAndClickUI;
    [SerializeField] private GameObject interrogateUI;

    private FiniteStateMachine fsm = new FiniteStateMachine();

    private Conversation conversation;
    private Dialogue dialogue;

    private void Awake() {
        // Initialise number of states.
        fsm.SetNumStates((int)RoomState.Num);

        // Enter Room
        fsm.SetStateEntry((int)RoomState.EnterRoom, OnEnterEnterRoom);

        // Point & Click
        fsm.SetStateEntry((int)RoomState.PointAndClick, OnEnterPointAndClick);
        fsm.SetStateExit((int)RoomState.PointAndClick, OnExitPointAndClick);

        // Start Conversation
        fsm.SetStateEntry((int)RoomState.StartConversation, OnEnterStartConversation);

        // Listen
        fsm.SetStateEntry((int)RoomState.Listen, OnEnterListen);

        // Interrogate
        fsm.SetStateEntry((int)RoomState.Interrogate, OnEnterInterrogate);
        fsm.SetStateExit((int)RoomState.Interrogate, OnExitInterrogate);

        // End Conversation
        fsm.SetStateEntry((int)RoomState.EndConversation, OnEnterEndConversation);

        // Leave Room
        fsm.SetStateEntry((int)RoomState.LeaveRoom, OnEnterLeaveRoom);
    }

    private void OnEnable() {
        videoPlayer.loopPointReached += OnVideoFinish;

        GameEventSystem.GetInstance().SubscribeToEvent<Conversation>(nameof(GameEventName.StartConversation), OnStartConversation);
        GameEventSystem.GetInstance().SubscribeToEvent(nameof(GameEventName.EndConversation), OnEndConversation);
    }

    private void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoFinish;

        GameEventSystem.GetInstance().UnsubscribeFromEvent<Conversation>(nameof(GameEventName.StartConversation), OnStartConversation);
        GameEventSystem.GetInstance().UnsubscribeFromEvent(nameof(GameEventName.EndConversation), OnEndConversation);
    }

    private void Start() {
        pointAndClickUI.SetActive(false);
        interrogateUI.SetActive(false);

        // Update Finite State Machine
        fsm.ChangeState((int)RoomState.EnterRoom);
    }

    private void Update() {
        fsm.Update();
    }

    // Enter Room State
    private void OnEnterEnterRoom() {
        // Play video.
        videoPlayer.clip = enterRoomClip;
        videoPlayer.Play();
    }

    // Point & Click State
    private void OnEnterPointAndClick() {
        videoPlayer.isLooping = true;
        videoPlayer.clip = pointAndClickClip;
        videoPlayer.Play();

        pointAndClickUI.SetActive(true);
    }

    private void OnExitPointAndClick() {
        videoPlayer.Stop();

        pointAndClickUI.SetActive(false);
    }

    // Start Conversation State
    private void OnEnterStartConversation() {
        // Play video.
        videoPlayer.clip = conversation.GetStartVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // End Conversation State
    private void OnEnterEndConversation() {
        // Play video.
        videoPlayer.clip = conversation.GetEndVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // Dialog State
    private void OnEnterListen() {
        // Play video.
        videoPlayer.clip = dialogue.GetVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();

        // Are there any clues? If yes, update our notes.
        if (0 != dialogue.GetClues().Length) {
            GameEventSystem.GetInstance().TriggerEvent<Clue[]>(nameof(GameEventName.FoundClues), dialogue.GetClues());
        }
    }

    // Interrogate State
    private void OnEnterInterrogate() {
        interrogateUI.SetActive(true);

        // Display dialog options.
        DialogueOption[] options = dialogue.GetOptions();
        for (int i = 0; i < options.Length; ++i) {
            string text = options[i].text;
            Dialogue nextDialogue = options[i].nextDialogue;
            interrogateUI.GetComponent<QuestionList>().AddButton(text,
                () => {
                    this.dialogue = nextDialogue;
                    fsm.ChangeState((int)RoomState.Listen);
                });
        }
    }

    private void OnExitInterrogate() {
        interrogateUI.GetComponent<QuestionList>().ClearButtons();
        interrogateUI.SetActive(false);
    }

    // Leave Room State
    private void OnEnterLeaveRoom() {
        // Play video.
        videoPlayer.clip = leaveRoomClip;
        videoPlayer.Play();
    }

    // Video Player Callback
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        RoomState currentState = (RoomState)fsm.GetCurrentState();
        switch (currentState) {
            case RoomState.EnterRoom:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.StartConversation:
                fsm.ChangeState((int)RoomState.Listen);
                break;
            case RoomState.Listen:
                fsm.ChangeState((int)RoomState.Interrogate);
                break;
            case RoomState.EndConversation:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.LeaveRoom:
                SceneManager.LoadScene("MapScene");
                break;
            default:
                break;
        }
    }

    // Event Callbacks
    private void OnStartConversation(Conversation conversation) {
        this.conversation = conversation;
        this.dialogue = conversation.GetInitialDialogue();
        fsm.ChangeState((int)RoomState.StartConversation);
    }

    private void OnEndConversation() {
        fsm.ChangeState((int)RoomState.EndConversation);
    }
}