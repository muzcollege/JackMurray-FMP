using UnityEngine;

public class IF_Door : InteractableItem
{
    [SerializeField] private Key keyToOpen;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] objectsToSpawnOnUnlock;

    public void OpenDoor()
    {
        if (IF_GameManager.Instance.ContainsKey(keyToOpen))
        {
            for (int i = 0; i < objectsToSpawnOnUnlock.Length; i++)
            {
                Instantiate(objectsToSpawnOnUnlock[i], door.transform.position, Quaternion.identity);
            }

            DestroyImmediate(door);
            DestroyImmediate(this.gameObject);
        }
    }
}
