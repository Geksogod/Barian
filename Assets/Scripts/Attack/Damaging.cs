using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour
{

}

public interface ITakeDamage
{
    void TakeDamage(float damageValue);
}

public interface IAffordDammage
{
    float damageValue { get; set;}
    void Afforddamage();
}
