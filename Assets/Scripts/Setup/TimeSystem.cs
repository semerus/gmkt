using System.Collections.Generic;
using UnityEngine;

public sealed class TimeSystem : MonoBehaviour
{

    private enum TimeState
    {
        Running,
        Slowed,
        Paused
    }

    // singleton
    private static TimeSystem instance;
    private TimeState timeState = TimeState.Running;
    private int slowCtr = 0;
    public int slowness = 5;

    // list of timers
    [SerializeField] // for debug easy
    private List<ITimeHandler> timers = new List<ITimeHandler>();
    private ITimeHandler[] nonSlowedTimers;

    public static TimeSystem GetTimeSystem()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(TimeSystem)) as TimeSystem;
            if (!instance)
                Debug.LogError("No active TimeSystem in the scene");
        }
        return instance;
    }

    void Update()
    {
        switch (timeState)
        {
            case TimeState.Running:
                RunTime();
                break;
            case TimeState.Paused:
                break;
            case TimeState.Slowed:
                RunSlowTime();
                break;
        }
    }

    private void RunTime()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            timers[i].RunTime();
        }
    }

    private void RunSlowTime()
    {
        int j = 0;
        //bool isSlowed = false;
        // slow all things
        // except target
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        for (int i = 0; i < timers.Count; i++)
        {
            timers[i].RunTime();
        }

        while (j < slowness)
        {
            for (int i = 0; i < nonSlowedTimers.Length; i++)
            {
                if (timers.Contains(nonSlowedTimers[i]))
                {
                    nonSlowedTimers[i].RunTime();
                }
            }
            j++;
        }
        /*
		if (slowCtr++ > slowness) {
			slowCtr = 0;
			for (int i = 0; i < timers.Count; i++) {
				timers [i].RunTime ();
			}
		} else {
			for (int i = 0; i < nonSlowedTimers.Length; i++) {
				if (timers.Contains (nonSlowedTimers [i])) {
					nonSlowedTimers [i].RunTime();
				}
			}
		}
        */
        //for (int i = 0; i < timers.Count; i++)
        //{
        //    for (int j = 0; j < nonSlowedTimers.Length; j++)
        //    {
        //        if (timers[i] == nonSlowedTimers[j])
        //        {
        //            Time.timeScale = 1f;
        //            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //            timers[i].RunTime();
        //            isSlowed = true;
        //            break;
        //        }
        //    }
        //    if(!isSlowed)
        //    {
        //        Time.timeScale = 0.01f;
        //        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //        timers[i].RunTime();
        //    }
        //    isSlowed = false;
        //}
    }

    public void AddTimer(ITimeHandler handler)
    {
        if (CheckTimer(handler))
            return;
        else
            timers.Add(handler);
    }

    public bool CheckTimer(ITimeHandler handler)
    {
        if (timers.Contains(handler))
            return true;
        else
            return false;
    }

    // change to RemoveTimer later
    public void DeleteTimer(ITimeHandler handler)
    {
        if (timers.Contains(handler))
        {
            timers.Remove(handler);
        }
    }

    public void PauseTime()
    {
        timeState = TimeState.Paused;
    }

    public void UnPauseTime()
    {
        timeState = TimeState.Running;
    }

    public void SlowMotion(ITimeHandler[] nonSlowed)
    {
        timeState = TimeState.Slowed;
        nonSlowedTimers = nonSlowed;
    }

    public void UnSlowMotion()
    {
        timeState = TimeState.Running;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}