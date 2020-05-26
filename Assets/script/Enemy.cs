using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10; //敌人前进的默认速度
    public float hp = 200;

    public int addition = 0; //击杀奖励

    private float totalHp;
    public GameObject explosionEffect;
    private Slider hpSlider;
    private Transform[] positions; //储存敌人的位置
    private int index = 0; 


    // Start is called before the first frame update
    void Start()
    {
        positions = Waypoints.positions;
        totalHp = hp;
        hpSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() //敌人从第一个waypoint走到最后一个waypoint
    {
        if (index > positions.Length - 1) return;
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    void ReachDestination() //到达终点，销毁这个敌人
    {
        GameManager.Instance.Failed();
        GameObject.Destroy(this.gameObject);
    }

    void OnDestroy() //存活的敌人数目
    {
        EnemySpawner.CountEnemyAlive--;
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = hp / totalHp;
        if(hp <= 0)
        {
            Die();
        }
    }

    
    void Die() //敌人血量低于0后，处理后事
    {
        BuildManager.Instance.ChangeMoney(addition);
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1f);
        Destroy(this.gameObject);
    }

}
