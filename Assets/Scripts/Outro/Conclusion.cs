using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Conclusion", menuName = "Outro/Conclusion")]
public class Conclusion : ScriptableObject {
    [SerializeField] private ItemName murderer = ItemName.ProfPlum;
    [SerializeField] private ItemName murderWeapon = ItemName.Book;

    [SerializeField] private VideoClip deathClip = null;
    [SerializeField] private VideoClip[] arrestClips = new VideoClip[0];
    [SerializeField] private VideoClip solveMurderClip = null;
    [SerializeField] private VideoClip wrongSuspectClip = null;
    [SerializeField] private VideoClip wrongWeaponClip = null;

    public ItemName GetMurderer() { return murderer; }
    public ItemName GetMurderWeapon() { return murderWeapon; }

    public VideoClip GetDeathClip() { return deathClip; }
    public VideoClip GetArrestClip(ItemName suspect) { return arrestClips[(int)suspect]; }
    public VideoClip GetSolveMurderClip() { return solveMurderClip; }
    public VideoClip GetWrongSuspectClip() { return wrongSuspectClip; }
    public VideoClip GetWrongWeaponClip() { return wrongWeaponClip; }
}