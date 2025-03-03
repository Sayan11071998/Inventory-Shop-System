using System;
public class EventController
{
    public event Action baseEvent;
    public void InvokeEvent() => baseEvent?.Invoke();
    public void AddListener(Action listener) => baseEvent += listener;
    public void RemoveListener(Action listener) => baseEvent -= listener;
}
public class EventController<T1, T2>
{
    public event Action<T1, T2> baseEvent;
    public void InvokeEvent(T1 type, T2 type2) => baseEvent?.Invoke(type, type2);
    public void AddListener(Action<T1, T2> listener) => baseEvent += listener;
    public void RemoveListener(Action<T1, T2> listener) => baseEvent -= listener;
}
public class EventController<T1>
{
    public event Action<T1> baseEvent;
    public void InvokeEvent(T1 type) => baseEvent?.Invoke(type);
    public void AddListener(Action<T1> listener) => baseEvent += listener;
    public void RemoveListener(Action<T1> listener) => baseEvent -= listener;
}
