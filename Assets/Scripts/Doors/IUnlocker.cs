using UnityEngine;

public interface IUnlocker
{
    void TryUnlock(DoorsOpening door, Transform interactor);
}
