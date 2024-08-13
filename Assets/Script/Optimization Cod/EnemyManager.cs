using System;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    void Start()
    {
        IEnemy zombie = EnemyFactory.CreateEnemy<Zombie>();
        zombie.Spawn();
    }
}
public interface IEnemy
{
    int ID { get; set; }
    void Spawn();
}
public class Zombie : MonoBehaviour, IEnemy
{
    public int ID
    {
        get { return 10;} 
        set { ıd = value; }
    }

    public int ıd;
    public void Spawn() {}
}
public class Vampire : MonoBehaviour, IEnemy
{
    public int ID { get; set; }
    
    public int ıd => ID;
    public void Spawn() {}
}
public class EnemyFactory
{
    public static T CreateEnemy<T>() where T: MonoBehaviour,IEnemy
    {
        return new GameObject("Zombie").AddComponent<T>();
    }
}