using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangable
{
    State CurrState { get; set; }
    void UpdateClearState();
    void UpdateNormalState();
}
