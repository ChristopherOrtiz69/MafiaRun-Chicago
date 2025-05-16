using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;
    public GameObject prefabVisualArma;  // Aquí asignas el prefab del arma visible
}
