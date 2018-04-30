using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Singleton

    public static EnemyManager instance;
    public List<Transform> hormigas = new List<Transform>();

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject enemy;
}
