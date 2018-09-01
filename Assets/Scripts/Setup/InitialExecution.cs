using UnityEngine;

public class InitialExecution : MonoBehaviour
{
    private static bool isInitialized;

    public static void Init()
    {
        if(isInitialized) { return; }

        new RaceBootstrap();

        isInitialized = true;
    }
}