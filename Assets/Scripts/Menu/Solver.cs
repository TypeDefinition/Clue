using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SolverState {
    None = -1,

    Overview,
    SuspectSelection,
    WeaponSelection,

    Num,
}

public class Solver : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject overview;
    [SerializeField] private GameObject suspectSelection;
    [SerializeField] private GameObject weaponSelection;
    [SerializeField] private GameObject selectSuspectButton;
    [SerializeField] private GameObject selectWeaponButton;
    [SerializeField] private Sprite[] itemSprites = new Sprite[(int)ItemName.Num];

    private FiniteStateMachine fsm = new FiniteStateMachine();
    private ItemName selectedSuspect = ItemName.None;
    private ItemName selectedWeapon = ItemName.None;

    private void Awake() {
        fsm.SetNumStates((int)SolverState.Num);

        fsm.SetStateEntry((int)SolverState.Overview, EnterOverview);
        fsm.SetStateUpdate((int)SolverState.Overview, UpdateOverview);
        fsm.SetStateExit((int)SolverState.Overview, ExitOverview);

        fsm.SetStateEntry((int)SolverState.SuspectSelection, EnterSuspectSelection);
        fsm.SetStateUpdate((int)SolverState.SuspectSelection, UpdateSuspectSelection);
        fsm.SetStateExit((int)SolverState.SuspectSelection, ExitSuspectSelection);

        fsm.SetStateEntry((int)SolverState.WeaponSelection, EnterWeaponSelection);
        fsm.SetStateUpdate((int)SolverState.WeaponSelection, UpdateWeaponSelection);
        fsm.SetStateExit((int)SolverState.WeaponSelection, ExitWeaponSelection);
    }

    private void Start() {
        fsm.ChangeState((int)SolverState.None);
    }

    private void Update() {
        fsm.Update();
    }

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    // **************** Public Interface ****************
    public void ToggleVisibility() {
        if (fsm.GetCurrentState() == (int)SolverState.None) {
            fsm.ChangeState((int)SolverState.Overview);
        } else {
            fsm.ChangeState((int)SolverState.None);
        }
    }

    public void GoToSuspectSelection() { fsm.ChangeState((int)SolverState.SuspectSelection); }

    public void GoToWeaponSelection() { fsm.ChangeState((int)SolverState.WeaponSelection); }

    public void SelectSuspect(ItemName suspect) {
        selectedSuspect = suspect;
        selectSuspectButton.GetComponent<Image>().sprite = itemSprites[(int)suspect];
        fsm.ChangeState((int)SolverState.Overview);
    }

    public void SelectWeapon(ItemName weapon) {
        selectedWeapon = weapon;
        selectWeaponButton.GetComponent<Image>().sprite = itemSprites[(int)weapon];
        fsm.ChangeState((int)SolverState.Overview);
    }

    // Button.OnClick does not accept functions with enums as arguments, so this is a stupid workaround.
    public void SelectSuspectDrGreen() { SelectSuspect(ItemName.DrGreen); }
    public void SelectSuspectProfPlum() { SelectSuspect(ItemName.ProfPlum); }
    public void SelectSuspectMrScarlet() { SelectSuspect(ItemName.MrScarlet); }

    public void SelectWeaponMeasuringTape() { SelectWeapon(ItemName.MeasuringTape); }
    public void SelectWeaponBook() { SelectWeapon(ItemName.Book); }
    public void SelectWeaponScissors() { SelectWeapon(ItemName.Scissors); }
    public void SelectWeaponBillardBall() { SelectWeapon(ItemName.BillardBall); }

    // **************** Overview State ****************
    private void EnterOverview() {
        background.SetActive(true);
        overview.SetActive(true);
    }

    private void UpdateOverview() {

    }

    private void ExitOverview() {
        background.SetActive(false);
        overview?.SetActive(false);
    }

    // **************** Suspect Selection State ****************
    private void EnterSuspectSelection() {
        background.SetActive(true);
        suspectSelection.SetActive(true);
    }

    private void UpdateSuspectSelection() { }

    private void ExitSuspectSelection() {
        background.SetActive(false);
        suspectSelection.SetActive(false);
    }

    // **************** Weapon Selection State ****************
    private void EnterWeaponSelection() {
        background.SetActive(true);
        weaponSelection.SetActive(true);
    }

    private void UpdateWeaponSelection() { }

    private void ExitWeaponSelection() {
        background.SetActive(false);
        weaponSelection.SetActive(false);
    }
}