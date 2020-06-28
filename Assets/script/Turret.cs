using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public virtual void AttackType() { }
    
    private List<GameObject> enemys = new List<GameObject>(); //List储存进入攻击范围的敌人序号
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject); //发现敌人，size + 1
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy") //敌人离开，size - 1
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 0.6f;//多少秒攻击一次
    private float timer = 0; //计时器

    public GameObject bulletPrefab; //子弹
    public Transform firePosition; //子弹发射前的位置
    public Transform head; //炮管的位置

    public bool useLaser = false; //判断攻击方式是否为激光

    public float damageRate = 100; //激光每秒的伤害
    public LineRenderer laserRenderer; //显示激光
    public GameObject laserEffect; //激光命中敌人的特效

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        if(enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }     
        if(useLaser == false)
        {
            timer += Time.deltaTime;
            if(timer >= attackRateTime && enemys.Count > 0)
            {
                timer = 0;
                Attack();
            }
        }
        else if(enemys.Count > 0)
        {
            if (laserRenderer.enabled == false)
                laserRenderer.enabled = true;
            laserEffect.SetActive(true);
            if(enemys[0] == null)
            {
                UpdateEnemys();
            }
            if(enemys.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[] { firePosition.position, enemys[0].transform.position });
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate * Time.deltaTime);
                laserEffect.transform.position = enemys[0].transform.position;
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            laserEffect.SetActive(false);
            laserRenderer.enabled = false;
        }
    }

    void Attack() //炮塔攻击第一个敌人
    {
        if(enemys[0] == null)
        {
            UpdateEnemys();
        }
        if(enemys.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;  //重置计时器
        }
        
    }

    void UpdateEnemys()  //更新enemys数组，取出空指针并将其remove
    {
        for(int i = enemys.Count - 1; i >= 0; i--)
        {
            if(enemys[i] == null)
            {
                enemys.RemoveAt(i);
            }
        }
    }

}
