using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHider : MonoBehaviour
{
    [SerializeField] private GameObject model;

    private SkinnedMeshRenderer smr;

    private void Awake()
    {
        smr = model.GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }

    public void Hide()
    {
        foreach(var mat in smr.materials)
        {
            Color n = mat.color;
            n.a = 0.1f;
            mat.color = n;
        }
    }

    public void Show()
    {
        foreach (var mat in smr.materials)
        {
            Color n = mat.color;
            n.a = 1f;
            mat.color = n;
        }
    }
}
