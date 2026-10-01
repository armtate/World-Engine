using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public static DamageNumberSpawner Instance;

    [SerializeField]
    private GameObject damageNumberPrefab;

    private void Awake()
    {
        if (
            Instance != null &&
            Instance != this
        )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Spawn(
        float damage,
        Vector3 position)
    {
        if (damageNumberPrefab == null)
        {
            Debug.LogWarning(
                "DamageNumberSpawner: " +
                "Damage Number Prefab is missing!"
            );

            return;
        }

        GameObject numberObject =
            Instantiate(
                damageNumberPrefab,
                position,
                Quaternion.identity
            );

        DamageNumber number =
            numberObject.GetComponent<DamageNumber>();

        if (number == null)
        {
            Debug.LogError(
                "DamageNumber prefab is missing " +
                "the DamageNumber script!"
            );

            Destroy(numberObject);
            return;
        }

        number.Setup(damage);
    }
}