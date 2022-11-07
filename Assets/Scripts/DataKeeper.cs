using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper instance;

    public DifficultyLevel Difficulty { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
