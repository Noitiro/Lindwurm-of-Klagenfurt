using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Rogue/Upgrade Card")]
public class UpgradeCardSO : ScriptableObject {
    [Header("Informacje")]
    public string cardName;
    [TextArea] public string description;
    public Sprite icon;
    public int cost = 100;

    [Header("Co ulepszamy?")]
    public UpgradeTarget target;
    public UpgradeType type;
    public float value;          

    public enum UpgradeTarget { Claw, Ice, Fire, Tail, Player }

    public enum UpgradeType {
        Damage,      // Obrazenia
        AreaSize,    // Wiekos ataku 
        Knockback,   // Sila odrzutu 
        CritChance,  // Szansa na krytyka 
        MaxHealth,   // Zycie gracza
        Regen,       // Regeneracja gracza
        MoveSpeed    // Prêdkosc gracza 
    }
}