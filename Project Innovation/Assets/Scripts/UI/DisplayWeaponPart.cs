using ScriptableArchitecture.Data;
using UnityEngine;

public class DisplayWeaponPart : MonoBehaviour
{
    [SerializeField] private Transform _barrelpart;
    [SerializeField] private Transform _basepart;
    [SerializeField] private Transform _stockpart;

    public void AddPart(WeaponPartData part)
    {
        switch (part.PartType)
        {
            case WeaponPartType.Barrel:
                SetPart(_barrelpart, part.IconPrefab);
                break;
            case WeaponPartType.Base:
                SetPart(_basepart, part.IconPrefab);
                break;
            case WeaponPartType.Stock:
                SetPart(_stockpart, part.IconPrefab);
                break;
        }
    }

    public void RemoveParts() 
    {
        RemoveChildren(_barrelpart);
        RemoveChildren(_basepart);
        RemoveChildren(_stockpart);
    }

    public void SetPart(Transform parent, GameObject iconPrefab)
    {
        RemoveChildren(parent);

        Instantiate(iconPrefab, parent);
    }

    private void RemoveChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}