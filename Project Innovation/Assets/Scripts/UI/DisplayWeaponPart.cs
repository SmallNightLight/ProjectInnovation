using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

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

        SetAllState(false);
    }

    [ContextMenu("RemoveParts")]
    public void RemoveParts()
    {
        RemoveChildren(_barrelpart);
        RemoveChildren(_basepart);
        RemoveChildren(_stockpart);
        SetAllState(false);
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

    public void CombineWeapon()
    {
        SetAllState(true);
    }

    public void SetAllState(bool combined)
    {
        SetState(_barrelpart.gameObject, !combined);
        SetState(_basepart.gameObject, !combined);
        SetState(_stockpart.gameObject, !combined);
    }

    private void SetState(GameObject target, bool state)
    {
        if (target.TryGetComponent(out Image image))
        {
            image.enabled = state;
        }
    }
}