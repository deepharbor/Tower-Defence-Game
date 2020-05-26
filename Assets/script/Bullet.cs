using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50; //子弹伤害
    
    public float speed = 50; //子弹速度

    public GameObject explosionEffectPrefab; //爆炸特效

    private float distanceArriveTarget = 1.3f;

    private Transform target; //敌人目标，默认进入攻击范围的第一个
    
    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

    void Update() 
    {
        if(target == null) //FixBug null prt报错
        {
            Die();
            return;
        }
        
        transform.LookAt(target.position);  //实现子弹射向敌人
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 dir = target.position - transform.position;
        if(dir.magnitude < distanceArriveTarget)
        {
            target.GetComponent<Enemy>().TakeDamage(damage); //传递damage，使敌人掉血
            Die();
        }

        void Die()
        {
            GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation); //实例化爆炸特效
            Destroy(effect, 1);
            Destroy(this.gameObject); //销毁特效
        }
    }
}
