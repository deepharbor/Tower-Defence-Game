using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildManager : MonoBehaviour
{
    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;

    private TurretData selectedTurretData;//表示当前UI选择的炮台（将要建造的炮台）

    [HideInInspector]
    public MapCube selectedMapCube;//表示当前3D中选择的炮台（场景中的游戏物体）

    public static BuildManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public Text moneyText;

    public Animator moneyAnimator;

    public float money = 200.0f;

    public GameObject upgradeCanvas;

    private Animator upgradeCanvasAnimator;

    public Button buttonUpgrade;
    
    public void ChangeMoney(float change = 0)
    {
        money += change;
        moneyText.text = "$" + money;
    }
    
    void Start()
    {
        moneyText.text = "$" + money;
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }
    
    void Update() // 检测鼠标点到了哪个cube
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                //开发炮台的建造
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>(); //得到点击的MapCube
                    if(mapCube.turretGo == null && selectedTurretData != null)
                    {
                        //可以创建
                        if(money >= selectedTurretData.cost)
                        {
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);
                        }
                        else
                        {
                            //提示钱不够
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if (mapCube.turretGo != null) //升级处理,显示升级UI
                    {
                        if(mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy) //判断选择的是同一个炮台，并且UI已经显示
                        {
                            StartCoroutine(HideUpgradeUI());
                        }
                        else
                        {
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
                }
            }
        }
    }
   
    //监听炮塔选择事件
    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }

    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade)
    {
        StopCoroutine("HideUpgradeUI()");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;
    }

    IEnumerator HideUpgradeUI()
    {
        upgradeCanvasAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.8f);
        upgradeCanvas.SetActive(false);  //直接禁用不播放hide动画
    }

    public void OnUpgradeButtonDown() //点击升级按钮
    {
        if(money >= selectedMapCube.turretData.costUpgraded)
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
        }
        else
        {
            moneyAnimator.SetTrigger("Flicker");
        }
        StartCoroutine(HideUpgradeUI());    
    }

    public void OnDestroyButtonDown() //点击拆按钮
    {
        selectedMapCube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());

    }
}
