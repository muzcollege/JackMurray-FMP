using UnityEngine;

public class IF_AmmoCollectable : MonoBehaviour
{
    [SerializeField] int ammoCount = 1;
    [SerializeField] float magnetDistance = 4f;
    [SerializeField] float collectionDistance = 0.1f;
    [SerializeField] float collectionSpeed = 0.1f;

    float collectionTime;

    bool isWithinDistance = false;
    Transform playerTransform;

    void Awake()
    {
        playerTransform = FindAnyObjectByType<IF_ThirdPersonController>().gameObject.transform;
    }

    void Update()
    {
        if ((transform.position - playerTransform.position).magnitude < magnetDistance)
        {
            isWithinDistance = true;
        }

        if (isWithinDistance == true)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, collectionTime);
            collectionTime += collectionSpeed * Time.deltaTime;
        }

        if ((transform.position - playerTransform.position).magnitude < collectionDistance)
        {
            AmmoCollected();
        }
    }

    void AmmoCollected()
    {
        IF_GameManager.Instance.Ammunition += ammoCount;

        Destroy(this.gameObject);
    }
}
