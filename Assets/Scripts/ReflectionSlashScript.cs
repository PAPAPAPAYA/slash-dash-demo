using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReflectionSlashScript : MonoBehaviour
{
    public List<GameObject> endOfPathes; // store all the end of paths in order, updates everyframe
    public List<Vector3> targetPoses; // store all the target poses, only update when mouse lifted
    public List<GameObject> currentlyActivePaths; // store all the active paths

    public int ReflectionTimes = 0;

    #region SINGLETON
    public static ReflectionSlashScript me;
    private void Awake()
    {
        me = this;
    }
    #endregion
}
