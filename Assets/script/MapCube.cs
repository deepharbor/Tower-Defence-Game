using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject turretGo; //保存当前cube上的炮台

    [HideInInspector]
    public TurretData turretData;

    [HideInInspector]

    //public static MapCube Instance;
    public bool isUpgraded = false;

    public GameObject buildEffect; //建造炮台的特效

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void BuildTurret(TurretData turretData) 
    {
        this.turretData = turretData;
        isUpgraded = false;
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    public void UpgradeTurret()
    {
        if (isUpgraded == true) return;
        Destroy(turretGo);
        isUpgraded = true;
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    public void DestroyTurret()
    {
        if (isUpgraded == false)
        {
            BuildManager.Instance.ChangeMoney(BuildManager.Instance.selectedMapCube.turretData.cost * 0.5f);
        }
        else
        {
            BuildManager.Instance.ChangeMoney((BuildManager.Instance.selectedMapCube.turretData.cost + BuildManager.Instance.selectedMapCube.turretData.costUpgraded) * 0.5f);
        }

        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    void OnMouseEnter() //鼠标移上Cube，Cube变红
    {
        if( turretGo == null && EventSystem.current.IsPointerOverGameObject() == false) //判断Cube上没有炮台，鼠标没有放在UI上
        {
            renderer.material.color = Color.red;
        }
    }

    void OnMouseExit()  //鼠标移出Cube，Cube恢复正常
    {
        renderer.material.color = Color.white;
    }
}
