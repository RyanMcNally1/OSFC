using UnityEngine;

public class PlayerEquipment : MonoBehaviour {

    public enum EquipmentSlot {
        None,
        Rifle,
        Knife
    }

    [Header("Equipment Objects")]
    public GameObject rifleObject;
    public GameObject knifeObject;

    [Header("Weapon UI")]
    public GameObject crosshairObject;
    public GameObject ammoUIObject;

    [Header("Runtime")]
    [SerializeField] private EquipmentSlot currentEquipment = EquipmentSlot.None;

    void Start() {
        Equip(EquipmentSlot.None);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ToggleEquipment(EquipmentSlot.Rifle);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ToggleEquipment(EquipmentSlot.Knife);
        }
    }

    void ToggleEquipment(EquipmentSlot selectedEquipment) {
        if (currentEquipment == selectedEquipment) {
            Equip(EquipmentSlot.None);
        }
        else {
            Equip(selectedEquipment);
        }
    }

    void Equip(EquipmentSlot selectedEquipment) {
        currentEquipment = selectedEquipment;

        bool rifleEquipped =
            currentEquipment == EquipmentSlot.Rifle;

        bool knifeEquipped =
            currentEquipment == EquipmentSlot.Knife;

        if (rifleObject != null) {
            rifleObject.SetActive(rifleEquipped);
        }

        if (knifeObject != null) {
            knifeObject.SetActive(knifeEquipped);
        }

        if (crosshairObject != null) {
            crosshairObject.SetActive(rifleEquipped);
        }

        if (ammoUIObject != null) {
            ammoUIObject.SetActive(rifleEquipped);
        }
    }

    public bool IsRifleEquipped() {
        return currentEquipment == EquipmentSlot.Rifle;
    }

    public bool IsKnifeEquipped() {
        return currentEquipment == EquipmentSlot.Knife;
    }

    public EquipmentSlot GetCurrentEquipment() {
        return currentEquipment;
    }
}
