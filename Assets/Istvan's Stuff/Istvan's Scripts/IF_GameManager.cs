using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IF_GameManager : MonoBehaviour
{
    public static IF_GameManager Instance;

    Transform playerTransform;

    [Header("Ammo"), SerializeField]
    private int ammunition;
    public int Ammunition
    {
        get
        {
            return ammunition;
        }
        set
        {
            ammunition = value;
            UpdateAmmoCounter();
        }
    }

    public TextMeshProUGUI ammoText;

    [Header("Score"), SerializeField]
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UpdateScoreCounter();
        }
    }

    public TextMeshProUGUI scoreText;

    [Header("Health"), SerializeField]
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            UpdateHealthCounter();
        }
    }

    public TextMeshProUGUI healthText;

    // InteractableItems
    [Header("ItemCollection"), SerializeField] private float maxInteractionDistance;
    [SerializeField] private RectTransform callToAction;

    [SerializeField] private InteractableItem[] interactableItemsInSceneList; // TODO: Remove [SerializeField]
    private List<Key> collectedKeysList = new List<Key>();

    [Header("Input Actions")]
    public InputActionReference interactAction;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UpdateAmmoCounter();
        UpdateScoreCounter();
        UpdateHealthCounter();
        FindAllInteractablesInCurrentScene();
        FindPlayer();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void LateUpdate()
    {
        CallToActionUpdate();
    }

    private void UpdateAmmoCounter()
    {
        if (ammoText != null)
        {
            ammoText.text = Ammunition.ToString();
        }
    }

    private void UpdateScoreCounter()
    {
        if (scoreText != null)
        {
            scoreText.text = Score.ToString();
        }
    }

    private void UpdateHealthCounter()
    {
        if (healthText != null)
        {
            healthText.text = Health.ToString();
        }
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        FindPlayer();
        FindAllInteractablesInCurrentScene();
    }

    private void FindPlayer()
    {
        playerTransform = FindAnyObjectByType<IF_ThirdPersonController>().gameObject.transform;
    }

    private void FindAllInteractablesInCurrentScene()
    {
        // Find all keys in the scene
        interactableItemsInSceneList = (InteractableItem[])GameObject.FindObjectsByType(typeof (InteractableItem), FindObjectsSortMode.None);
    }

    private InteractableItem FindClosestInteractable()
    {
        if (interactableItemsInSceneList.Length <= 0)
        {
            if (callToAction != null)
            {
                // Hide the CTA
                callToAction.gameObject.SetActive(false);
            }
            
            return null;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < interactableItemsInSceneList.Length; i++)
        {
            float distanceToInteractable = (interactableItemsInSceneList[i].transform.position - playerTransform.transform.position).magnitude;
            if (distanceToInteractable < closestDistance)
            {
                closestDistance = distanceToInteractable;
                closestIndex = i;
            }
        }

        return interactableItemsInSceneList[closestIndex];
    }

    private void CallToActionUpdate()
    {
        InteractableItem closestInteractableItem = FindClosestInteractable();
        if (closestInteractableItem == null)
        {
            return;
        }

        float distanceToClosestInteractableItem = (closestInteractableItem.transform.position - playerTransform.transform.position).magnitude;
        if (distanceToClosestInteractableItem < maxInteractionDistance)
        {
            if (callToAction != null)
            {
                // Show CTA
                callToAction.gameObject.SetActive(true);

                // Update the position of the CTA to make sure that it shows on the collectible item.
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(closestInteractableItem.transform.position);
                callToAction.position = screenPosition;
            }

            if (interactAction.action.WasPressedThisFrame())
            {
                if (closestInteractableItem.GetType() == typeof(Key)) // If the closestInteractableItem is a Key...
                {
                    CollectKey((Key)closestInteractableItem);
                    FindAllInteractablesInCurrentScene(); // We update the interactable objects in the scene to make sure there isnt a null object in the array
                }
                else if (closestInteractableItem.GetType() == typeof(IF_Door))
                {
                    IF_Door myIfDoor = (IF_Door)closestInteractableItem;
                    myIfDoor.OpenDoor();
                    FindAllInteractablesInCurrentScene(); // We update the interactable objects in the scene to make sure there isnt a null object in the array
                }
            }
        }
        else if (callToAction != null)
        {
            // Hide the CTA
            callToAction.gameObject.SetActive(false);
        }
    }

    public void CollectKey(Key keyToAdd)
    {
        collectedKeysList.Add(keyToAdd);

        DestroyImmediate(keyToAdd.gameObject);
    }

    public bool ContainsKey(Key keyToCheck)
    {
        if (collectedKeysList.Contains(keyToCheck))
        {
            return true;
        }

        return false;
    }
}
