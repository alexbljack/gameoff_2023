using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingType : ScriptableObject
{
    [SerializeField] Sprite image;
    [SerializeField] float timeToBuild = 1f;
    [SerializeField] List<ResourceCost> cost;
    [SerializeField] List<ResourceGenerator> resources;
    [SerializeField] int housing;

    public Sprite Image => image;
    public float TimeToBuild => timeToBuild;
    public List<ResourceCost> Cost => cost;
    public List<ResourceGenerator> Resources => resources;
    public int Housing => housing;
}
