using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour
{
    public const float MoveSpeed = 20;
    const float EnemyHp = 200;

    private int index = 0;

    public virtual void toMove() { } //虚函数，不同敌人的行进
    public virtual void toDie() { }  //虚函数，不同敌人的消灭
}

class TotalEnemy : BasicEnemy
{
    private float totalHp;
    private Transform[] positions; //储存敌人的位置
    private int index = 0;

    void toMove() //敌人从第一个waypoint走到最后一个waypoint
    {
        if (index > positions.Length - 1) return;
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * MoveSpeed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    void ReachDestination()
    {
        GameManager.Instance.Failed();
        GameObject.Destroy(this.gameObject);
    }

    void toDie() 
    {
        Destroy(this.gameObject);
    }
}