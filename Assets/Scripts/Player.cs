using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int Health = 100;
    public int Hunger = 300;
    private int ScrollPos = 0;

    private MoveDirection MoveDirection;

    private MoveDirection MouseDirection;

    public List<ItemStack> InventoryItems;

    public bool DisableMovement = false;

    [Header("Settings")]
    public int MaxHealth = 100;
    public int MaxHunger = 300;
    public float Speed = 4;
    public float SprintMultiplier;
    public float MouseSensitivity;
    public float Reach;
    public float TurnThreshold;
    private bool OverrideAnimDirection;

    public Vector2 SpawnPos;

    public AudioClip FailPlace;
    public AudioClip Place;

    [Header("Required Objects")]
    public RectTransform HealthBar;
    public RectTransform HungerBar;
    public GameObject Inventory;
    public GameObject Cursor;
    private MouseStack Mouse;
    public GameObject Crafting;
    public GameObject Canvas;
    public Transform Menus;
    public CraftingController CC;
    private bool MenuOpen;
    private bool ExternalMenu;
    private Rigidbody2D Body;
    private Item NoItem;

    public GameObject Hotbar;
    public LayerMask HarvestableMask;
    public LayerMask InteractableMask;
    private List<HotbarSlot> HotbarSlots;
    private ItemUtility ItemUtility;
    private ItemActionLibrary ItemActionLibrary;
    private Animator TorsoAnim;
    private Animator LegAnim;
    private AudioSource Audio;
    public GameObject Pause;
    public Item FinishObject;
    

    public void HarvestResource(int Power, int Strength, HarvestTool Type, string AnimName) {
        HarvestPower = Power;
        HarvestStrength = Strength;
        HarvestToolType = Type;
        HarvestDirection = MouseDirection;

        OverrideAnimDirection = true;

        LegAnim.SetFloat("Direction", (int) HarvestDirection);

        TorsoAnim.Play(HarvestDirection.ToString() + AnimName);

    }

    public void ResetAnimDirection() {

        OverrideAnimDirection = false;

        LegAnim.SetFloat("Direction", (int) MoveDirection);
        TorsoAnim.SetFloat("Direction", (int) MoveDirection);
    }

    int HarvestPower; int HarvestStrength; HarvestTool HarvestToolType; MoveDirection HarvestDirection;

    public void Harvest() {

        Collider2D Harvestable = new Collider2D();
        switch (HarvestDirection) {
            case MoveDirection.North :
                Harvestable = ClosestCollider(transform.position + new Vector3(0, 0.5f, 0), 0.4f, HarvestableMask);
                break;
            case MoveDirection.West :
                Harvestable = ClosestCollider(transform.position + new Vector3(-0.5f, 0, 0), 0.4f, HarvestableMask);
                break;
            case MoveDirection.South :
                Harvestable = ClosestCollider(transform.position + new Vector3(0, -0.5f, 0), 0.4f, HarvestableMask);
                break;
            case MoveDirection.East :
                Harvestable = ClosestCollider(transform.position + new Vector3(0.5f, 0, 0), 0.4f, HarvestableMask);
                break;
        }

        if (!Harvestable) return;
        if (!Harvestable.GetComponent<Harvestable>()) return;

        Harvestable HarvestableComponent = Harvestable.GetComponent<Harvestable>();

        HarvestableComponent.Harvest(HarvestToolType, HarvestPower, HarvestStrength);
    }

    public void RemoveFromInventory(List<CraftingItem> items) {
        foreach (CraftingItem Removal in items) {
            foreach (ItemStack stack in InventoryItems) {
                if (stack.Item == Removal.Item)
                    stack.RemoveItems(Removal.Amount);
            }
        }
    }
    
    public bool HasItem() {
        bool has = false;
        foreach (ItemStack ItemStack in InventoryItems) {
            if (ItemStack.Item == FinishObject)
                has = true;
        }
        return has;
    }

    public bool ItemPickUp(Item Item, int Amount = 1) {
        bool empty = false;
        int index = -1;
        foreach (ItemStack Stack in InventoryItems) {
            if (Stack.Item == Item && Stack.Item.Stackable) {
                Stack.Count += Amount;
                CC.RefreshRecipies();
                return true;
            } else if (Stack.Item == NoItem) {
                if (!empty) {                    
                    index = InventoryItems.IndexOf(Stack);
                    CC.RefreshRecipies();
                    empty = true;
                }
            }
        }
        if (empty) {
            InventoryItems[index].SetItem(Item);
            InventoryItems[index].Count = Amount;
            return true;
        }

        return false;
    }

    public void PlaceObject(Item Item) {
        Vector3 PlacePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlacePos.z = 0;

        if (Physics2D.OverlapCircle(PlacePos, 0.375f) || (transform.position - PlacePos).magnitude > Reach) {
            Audio.clip = FailPlace;
            Audio.Play();
            return;
        }

        List<CraftingItem> UsedItem = new List<CraftingItem>();

        UsedItem.Add(new CraftingItem(Item));

        RemoveFromInventory(UsedItem);

        Instantiate(Item.PlacedObject, PlacePos, new Quaternion());
        Audio.clip = Place;
        Audio.Play();
    }

    void Awake() {
        Canvas.SetActive(true);
    }

    void Start()
    {
        Mouse = Cursor.GetComponent<MouseStack>();
        Inventory.SetActive(false);
        Cursor.SetActive(false);
        Crafting.SetActive(false);
        CC = Crafting.GetComponent<CraftingController>();
        NoItem = Resources.Load("NoItem") as Item;
        Body = GetComponent<Rigidbody2D>();
        StartCoroutine(HungerDrain());
        InventoryItems = new List<ItemStack>(GetComponentsInChildren<ItemStack>());
        HotbarSlots = new List<HotbarSlot>(Hotbar.GetComponentsInChildren<HotbarSlot>());
        ItemActionLibrary = FindObjectOfType<ItemActionLibrary>();
        ItemUtility = FindObjectOfType<ItemUtility>();

        TorsoAnim = transform.GetChild(2).GetComponent<Animator>();
        LegAnim = transform.GetChild(3).GetComponent<Animator>();

        Audio = GetComponent<AudioSource>();

        foreach (Transform Menu in Menus) {
            Menu.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {
        
        if (OverrideAnimDirection && (TorsoAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || TorsoAnim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))) {
            OverrideAnimDirection = false;
        }

        if (DisableMovement || MenuOpen || ExternalMenu) {
            TorsoAnim.SetBool("Moving", false);
            LegAnim.SetBool("Moving", false);
            return;
        }

        Vector2 MovementVector = Vector2.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        if (MovementVector.magnitude > 0.3f) {

            TorsoAnim.SetFloat("Direction", (int) MoveDirection);

            if (OverrideAnimDirection)
                LegAnim.SetFloat("Direction", (int) HarvestDirection);
            else 
                LegAnim.SetFloat("Direction", (int) MoveDirection);

            TorsoAnim.SetBool("Moving", true);
            LegAnim.SetBool("Moving", true);
            if (Input.GetButton("Sprint"))
                LegAnim.SetBool("Running", true);
            else
                LegAnim.SetBool("Running", false);
        } else {
            TorsoAnim.SetBool("Moving", false);
            LegAnim.SetBool("Moving", false);
        }

        
        
        Vector2 AdjustedMovementVector = MovementVector * Time.fixedDeltaTime * Speed;
        Body.MovePosition(transform.position + (Vector3) (AdjustedMovementVector * ((Input.GetButton("Sprint")) ? SprintMultiplier : 1)));

        if (MovementVector.magnitude > TurnThreshold) {
            MoveDirection = DirectionBasedOnVector(MovementVector);
        }


    }

    void Update()
    {  
        MouseDirection = DirectionBasedOnVector(new Vector2(Input.mousePosition.x - (Camera.main.pixelWidth / 2), Input.mousePosition.y - (Camera.main.pixelHeight / 2)));

        if (Input.GetMouseButtonDown(1) && MenuOpen) {
            for (int i = 0; i < Mouse.Count; i++)
                ItemUtility.DropItemSpot(WorldPositionBasedOnDireciton(transform, MoveDirection), Mouse.Item);

            Mouse.SetItem(NoItem);
            Mouse.Count = 0;
        }

        if (Input.GetButtonDown("Inventory")) {
            if (MenuOpen || ExternalMenu) {
                for (int i = 0; i < Mouse.Count; i++)
                    ItemUtility.DropItemSpot(WorldPositionBasedOnDireciton(transform, MoveDirection), Mouse.Item);

                Mouse.SetItem(NoItem);
                Mouse.Count = 0;
                Cursor.SetActive(false);
                foreach (Transform Menu in Menus) {
                    Menu.gameObject.SetActive(false);
                }
                MenuOpen = false;
                ExternalMenu = false;
            }
            else {
                Inventory.SetActive(true);
                Cursor.SetActive(true);
                MenuOpen = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Pause.activeInHierarchy) {
                Time.timeScale = 1;
                Pause.SetActive(false);
            }
            else {
                Time.timeScale = 0;
                Pause.SetActive(true);
            }
        }

        if (Input.GetButtonDown("Craft")) {
            if (Crafting.activeInHierarchy) {
                Cursor.SetActive(false);
                foreach (Transform Menu in Menus) {
                    Menu.gameObject.SetActive(false);
                }
                MenuOpen = false;
                ExternalMenu = false;
            }
            else {
                if (ExternalMenu) {
                        Cursor.SetActive(false);
                    foreach (Transform Menu in Menus) {
                        Menu.gameObject.SetActive(false);
                    }
                    MenuOpen = false;
                    ExternalMenu = false;
                }
                Crafting.SetActive(true);
                Inventory.SetActive(true);
                Cursor.SetActive(true);
                MenuOpen = true;
            }
        }

        ScrollPos -= Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel") * MouseSensitivity );
        while (ScrollPos < 0) {
            ScrollPos += 5;
        }
        while (ScrollPos > 4) {
            ScrollPos -= 5;
        }
        foreach (HotbarSlot Hbs in HotbarSlots) {
            Hbs.SwapMainPlayerItem(ScrollPos);
        }

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject()) {

            if (OverrideAnimDirection) return;

            Item UseItem = InventoryItems[ScrollPos].Item;
            
            ItemActionLibrary.DoAction(UseItem);

            /*
            
            System.Type TrueUse = System.Type.GetType(UseItem.ItemUse + ",Assembly-CSharp");
            
            gameObject.AddComponent(TrueUse);
            GetComponent<ItemUse>().Use(UseItem.ExtraParams);
            Destroy((Object) GetComponent<ItemUse>());
            */
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (ExternalMenu) {
                Cursor.SetActive(false);
                foreach (Transform Menu in Menus) {
                    Menu.gameObject.SetActive(false);
                }
                MenuOpen = false;
                ExternalMenu = false;

                return;
            }
            if (MenuOpen) return;
            Collider2D Col = ClosestCollider(transform.position, 1.03f, InteractableMask);
            if (Col) {
                Interactable Inter = Col.GetComponentInParent<Interactable>();
                if (Inter.InteractType == InteractType.Collect) {
                    if (!ItemPickUp(Inter.Item, Inter.Count)) {
                        for (int i = 0; i < Inter.Count; i++) {
                            ItemUtility.DropItem(transform.position, Inter.Item);
                        }
                    }
                    Inter.Collected();
                }
                else if (Inter.InteractType == InteractType.UI) {
                    Cursor.SetActive(true);
                    Inventory.SetActive(true);
                    Menus.transform.Find(Inter.Menu).gameObject.SetActive(true);
                    MenuOpen = true;
                    ExternalMenu = true;
                }
            }
        }

        /*

        Vector2 MouseVector;

        float Angle = Vector2.SignedAngle(Vector2.up, MovementVector);

            if (Angle < 45 && Angle > -45)
                MoveDirection = MoveDirection.North;
            else if (Angle < -45 && Angle > -135)
                MoveDirection = MoveDirection.West;
            else if (Angle > 45 && Angle < 135)
                MoveDirection = MoveDirection.East;
            else
                MoveDirection = MoveDirection.South;

        */
    }

    public MoveDirection DirectionBasedOnVector(Vector2 Vector) {
        float Angle = Vector2.SignedAngle(Vector2.up, Vector);

            if (Angle <= 45 && Angle >= -45)
                return MoveDirection.North;
            else if (Angle < -45 && Angle > -135)
                return MoveDirection.East;
            else if (Angle > 45 && Angle < 135)
                return MoveDirection.West;
            else
                return MoveDirection.South;
    }

    public Vector2 VectorBasedOnDireciton(MoveDirection Direction) {
        switch (Direction) {
            case MoveDirection.North :
                return new Vector2(0, 1);

            case MoveDirection.East :
                return new Vector2(1, 0);

            case MoveDirection.South :
                return new Vector2(0, -1);

            case MoveDirection.West :
                return new Vector2(-1, 0);
            
            default :
                return new Vector2(0, 1);
        }
    }

    public Vector3 WorldPositionBasedOnDireciton(Transform Base, MoveDirection Direction) {
        return (Base.position + new Vector3(VectorBasedOnDireciton(Direction).x, VectorBasedOnDireciton(Direction).y));
    }

    public void Hurt(int Amount) {
        Health -= Amount;
        float Hung = Health;
        float MHung = MaxHealth;
        HealthBar.localScale = new Vector3(Hung / MHung, 1, 1);
        if (Health <= 0) {
            foreach (ItemStack Stack in InventoryItems) {
                for (int i = 0; i < Stack.Count; i++) {
                    ItemUtility.DropItem(transform.position, Stack.Item, 0.5f, 1f);
                }
                Stack.Item = NoItem;
                Stack.Count = 0;
            }

            FindObjectOfType<TeleHandler>().Die(SpawnPos);
            Heal(MaxHealth);
            Eat(MaxHunger);
        }
    }

    public void Heal(int Amount) {
        Health += Amount;
        float Hung = Health;
        float MHung = MaxHealth;
        HealthBar.localScale = new Vector3(Hung / MHung, 1, 1);
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public void Eat (int Calories) {
        Hunger += Calories;
        
        if (Hunger > MaxHunger)
            Hunger = MaxHunger;

        float Hung = Hunger;
        float MHung = MaxHunger;
        HungerBar.localScale = new Vector3(Hung / MHung, 1, 1);
        List<CraftingItem> Consumed = new List<CraftingItem>();
        Consumed.Add(new CraftingItem(InventoryItems[ScrollPos].Item, 1));
        RemoveFromInventory(Consumed);
    }

    IEnumerator HungerDrain() {
        while (true) {
            if (Hunger > 0) {
                float Hung = Hunger;
                float MHung = MaxHunger;
                HungerBar.localScale = new Vector3(Hung / MHung, 1, 1);
                //if (Hung / M)
                //Debug.Log();
                Hunger--;
                Heal(1);
            }
            else 
                Hurt(1);

            yield return new WaitForSeconds(1f);
        }

    }

    public Collider2D ClosestCollider(Vector2 Point, float Radius, LayerMask LayerMask) {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(Point, Radius, LayerMask);
        if (Colliders.Length == 0) return null;

        Collider2D Closest = new Collider2D();

        float Distance = Radius + 1;

        foreach (Collider2D Col in Colliders) {
            Vector2 RelativeVector = Point - new Vector2(Col.transform.position.x, Col.transform.position.y);
            if (RelativeVector.magnitude < Distance) {
                Closest = Col;
                Distance = RelativeVector.magnitude;
            }
        }

        return Closest;
    }
}


    public enum MoveDirection {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }