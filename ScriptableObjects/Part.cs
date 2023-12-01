using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Ships/Part", order = 0)]
public class Part : ScriptableObject {
    // strind id ("sid") use to reference the part in the save-system
    // or in game
    public string sid;
    
    // displayed name of the part
    public string name;

    // actual in game Part
    public GameObject prefab;

    // mass (to compute total mass of the vehicule)
    public float mass;

    // audio clips to play when placing the part
    // pitch may vary in game (via random pitch system to reduce repetitivness)
    public AudioClip[] placing_sounds;

    // can you rotate this piece in build mode ?
    public bool can_rotate = true;
}