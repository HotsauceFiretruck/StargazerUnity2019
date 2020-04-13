using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public Entity player;
    public Health playerHealth;
    public Inventory playerInventory;

    private Text txtAmmo;
    private Text txtHealth;
    private Text txtInventory;
    private Text txtObjective;

    public void Start()
    {
        txtAmmo = GameObject.Find("PlayerUI/Canvas/AmmoCounter").GetComponent<Text>();
        txtInventory = GameObject.Find("PlayerUI/Canvas/Inventory").GetComponent<Text>();
        txtHealth = GameObject.Find("PlayerUI/Canvas/HealthCounter").GetComponent<Text>();
        txtObjective = GameObject.Find("PlayerUI/Canvas/Objective").GetComponent<Text>();

        txtObjective.text = GameManager.GetObjective(SceneManager.GetActiveScene().buildIndex);
        playerInventory.inventoryChangeCallback += OnInventoryChange;
    }

    public void WeaponDisplay()
    {
        Equipment weaponEquipment = player.equipments[EquipmentType.Weapon];
        if (weaponEquipment != null)
        {
            RangeWeapon rangeWeapon = weaponEquipment as RangeWeapon;
            if (rangeWeapon != null)
            {
                txtAmmo.text = "Ammo: " + rangeWeapon.GetAmmoCount();
            }
        }
        else
        {
            txtAmmo.text = "No range weapon selected";
        }
    }

    public void HealthDisplay()
    {
        if (playerHealth != null)
        {
            txtHealth.text = "Health: " + playerHealth.entityHealth;
        }
    }

    public void OnInventoryChange()
    {
        if (playerInventory != null)
        {
            string inventoryDisp = "";
            ItemData[] items = playerInventory.GetAllItemData();
            for (int i = 0; i < playerInventory.maxNumberOfSlots; i++)
            {
                if (items[i] != null)
                    inventoryDisp += "[" + (i + 1) + "] " + items[i].itemName + " ";
            }
            txtInventory.text = "Inventory: " + inventoryDisp;
        }
    }

    public void Update()
    {
        WeaponDisplay();
        HealthDisplay();
    }
}
